using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GroupClient
{
    public enum RoleType
    {
        超级管理员 = 0,
        普通管理员 = 1,

        超级风控员 = 10,
        普通风控员 = 11,
        初级风控员 =12,

        交易员 = 20,
    }

    public class 券商 : INotifyPropertyChanged
    {
        static Regex regexOrder = new Regex("(?<=[0-9]{2}:[0-9]{2}:[0-9]{2}\t)[0-9]+");

        public 券商() { }

        public 券商(GroupInfo o) {
            this.IP = o.IP;
            this.Port = o.Port;
            this.版本号 = o.版本号;
            this.查询间隔时间 = o.查询间隔时间;
            this.登录帐号 = o.登录帐号;
            this.交易服务器 = o.交易服务器;
            this.交易密码 = o.交易密码;
            this.交易帐号 = o.交易帐号;
            this.名称 = o.名称;
            this.启用 = o.启用;
            this.通讯密码 = o.通讯密码;
            this.营业部代码 = o.营业部代码;
            this.Multithreading = o.Multithreading;
            this.IsIMSAccount = o.IsIMSAccount;
            this.产品信息 = o.产品信息;
            this.资产单元 = o.资产单元;
            this.投资组合 = o.投资组合;
        }

        public string 名称 { get; set; }

        public bool 启用 { get; set; }

        public string 交易服务器 { get; set; }

        public string IP { get; set; }

        public short Port { get; set; }

        public string 版本号 { get; set; }

        public short 营业部代码 { get; set; }

        public string 登录帐号 { get; set; }

        public string 交易帐号 { get; set; }

        public string 交易密码 { get; set; }

        public int 查询间隔时间 { get; set; }

        public string 通讯密码 { get; set; }

        public bool? Multithreading { get; set; }

        #region IMS 帐号属性
        /// <summary>
        /// 是否IMS接口
        /// </summary>
        public bool IsIMSAccount { get; set; }

        public string 产品信息 { get; set; }

        public string 资产单元 { get; set; }

        public string 投资组合 { get; set; } 
        #endregion

        string _status = "未登录";
        public string Status 
        { 
            get 
            {
                return _status; 
            }
            set
            {
                _status = value;
                NotifyPropertyChange("Status");
            }
        }


        object sync = new object();
        public bool Safe启用
        {
            get
            {
                lock (sync)
                {
                    return this.启用;
                }
            }
        }


        BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        public void Start()
        {
            if (!this.backgroundWorker1.WorkerSupportsCancellation)
            {


                this.backgroundWorker1.WorkerSupportsCancellation = true;
                this.backgroundWorker1.WorkerReportsProgress = true;
                this.backgroundWorker1.DoWork += backgroundWorker1_DoWork;
                this.backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
                this.backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
                this.backgroundWorker1.RunWorkerAsync();
            }
            if (this.查询间隔时间 < 200)
            {
                this.查询间隔时间 = 200;
            }
        }

        private void ImsNotify(string obj)
        {
            //throw new NotImplementedException();
            CommonUtils.Log("ImsNotify: ", obj);
        }

        public void Stop()
        {
            try
            {
                this.backgroundWorker1.CancelAsync();
            }
            catch (Exception)
            {
                
            }
            
            while (this.backgroundWorker1.IsBusy)
            {
                Thread.Sleep(100);
            }
        }

        public static object LoginObject = new object();

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //int i = 0;
            while (!this.backgroundWorker1.CancellationPending)
            {
                if (this.Safe启用)
                {

                    if (DateTime.Parse(CommonUtils.GetConfig("开始查询时间", "8:15")) <= DateTime.Now && DateTime.Now <= DateTime.Parse(CommonUtils.GetConfig("结束查询时间", "15:30")))
                    {
                        #region 交易时段
                        if (this.ClientID == -1)
                        {
                            #region 登录
                            this.backgroundWorker1.ReportProgress(0, "登录中...");
                            string Msg;
                            lock (LoginObject)
                            {
                                Msg = this.Logon();
                            }
                            this.backgroundWorker1.ReportProgress(0, Msg);
                            #endregion

                            Thread.Sleep(1000);
                        }
                        else
                        {
                            DateTime DateTime1 = DateTime.Now;
                            this.QueryData();
                            this.QueryInfo.QueryTime = DateTime.Now - DateTime1;
                            this.backgroundWorker1.ReportProgress(0, QueryInfo.QueryTime.TotalSeconds.ToString());
                        }
                        #endregion
                    }
                    else
                    {
                        if (isLogonInterim)
                        {
                            return;
                        }

                        #region 非交易时段
                        if (this.ClientID != -1)
                        {
                            this.backgroundWorker1.ReportProgress(0, "注销中...");

                            this.Logoff();

                            this.backgroundWorker1.ReportProgress(0, "已注销");
                        }

                        this.backgroundWorker1.ReportProgress(0, "非交易时段");

                        if (DateTime.Now.Hour > 18)
                        {
                            ClearCache();
                        }
                        //this.backgroundWorker1.ReportProgress(0, "非交易时段" + i++);

                        Thread.Sleep(300);

                        #endregion
                    }
                }
                else
                {
                    #region 未启用
                    if (this.ClientID != -1)
                    {
                        this.backgroundWorker1.ReportProgress(0, "注销中...");

                        this.Logoff();

                        this.backgroundWorker1.ReportProgress(0, "已注销");
                    }

                    this.backgroundWorker1.ReportProgress(0, "未启用");

                    Thread.Sleep(1000);

                    #endregion
                }
            }
        }

        #region Members
        //1.所有要存储的订单号列表
        public ConcurrentBag<string> lstOrderID = new ConcurrentBag<string>();

        //2.每个服务端Mac 对应的订单列表
        ConcurrentDictionary<string, ConcurrentBag<string>> dictMacOrder = new ConcurrentDictionary<string, ConcurrentBag<string>>();

        //3.原始查询结果
        ConcurrentQueue<string> queueWt = new ConcurrentQueue<string>();
        ConcurrentQueue<string> queueCj = new ConcurrentQueue<string>();

        //4.原始数据的索引
        Dictionary<string, int> dictWtIndex = new Dictionary<string, int>();
        Dictionary<string, int> dictCjIndex = new Dictionary<string, int>();

        //5.过滤后根据订单号索引的字典
        ConcurrentDictionary<string, string> dictWt = new ConcurrentDictionary<string, string>();
        ConcurrentDictionary<string, string> dictCj = new ConcurrentDictionary<string, string>();

        //6.记录下单提示超时或连接交易服务器失败的信息
        ConcurrentQueue<OrderInfoCache> queueMayLostOrder = new ConcurrentQueue<OrderInfoCache>();

        Thread FilteCjDataThread = null;
        Thread FilteWtDataThread = null;
        Thread QeuryCJThread = null;

        public string strTitleWt { get; set; }

        public string strTitleCj { get; set; }
        #endregion

        #region Data Query Main
        private void QueryData()
        {
            
            if (QueryInfo == null)
            {
                QueryInfo = new GroupService.QueryDataObj();
            }

            if (this.IsIMSAccount)
            {
                QueryImsData();
                return;
            }

            if (this.Multithreading == true)
            {
                QueryFilterData();
            }
            else
            {
                StringBuilder result = new StringBuilder(1024 * 1024);
                StringBuilder errInfo = new StringBuilder(256);

                TdxApi.QueryData(this.ClientID, 3, result, errInfo);
                QueryInfo.SearchTradeResult = result.ToString();
                QueryInfo.SearchTradeErrInfo = errInfo.ToString();
                
                Thread.Sleep(查询间隔时间 / 2);

                TdxApi.QueryData(this.ClientID, 2, result, errInfo);
                QueryInfo.SearchOperatorResult = result.ToString();
                QueryInfo.SearchOperatorError = errInfo.ToString();

                if (QueryInfo.SearchTradeErrInfo.Contains("请尝试其它交易服务器") && QueryInfo.SearchOperatorError.Contains("请尝试其它交易服务器"))
                {
                    CommonUtils.Log("检测到{0}连接交易服务器失败信息，尝试注销登录后重新登录",this.名称);
                    this.Logoff();
                    this.Logon();
                }
                Thread.Sleep(查询间隔时间 / 2);
            }
        }


        string lastOrderUpdateTime = string.Empty;
        int serialNumm = 0;
        private void QueryImsData()
        {
            try
            {
                StringBuilder Result = new StringBuilder(1024 * 1024);
                StringBuilder ErrInfo = new StringBuilder(1024);
                ImsApi.ImsPbClient_QueryProductOrder(lastOrderUpdateTime, Result, ErrInfo);
                QueryInfo.SearchOperatorError = ErrInfo.ToString();
                QueryInfo.SearchOperatorResult = Result.ToString();
                if (QueryInfo.SearchOperatorError.Contains("网络已中断, 请重新连接"))
                {
                    this.ClientID = -1;
                    this.Logoff();
                    return;
                }
                //if (!string.IsNullOrEmpty(QueryInfo.SearchOperatorResult))
                //{
                //    var tableWT = Tool.ChangeDataStringToTable(Result.ToString());
                //    if (tableWT.Rows.Count > 0)
                //    {
                //        lastOrderUpdateTime = tableWT.Rows[0]["lastupdatetime"].ToString();
                //    }
                //}
               

                Thread.Sleep(查询间隔时间 / 2);

                ImsApi.ImsPbClient_QueryProductKnock(serialNumm, Result, ErrInfo);
                QueryInfo.SearchTradeErrInfo = ErrInfo.ToString();
                QueryInfo.SearchTradeResult = Result.ToString();
                if (QueryInfo.SearchTradeErrInfo.Contains("网络已中断, 请重新连接"))
                {
                    this.ClientID = -1;
                    this.Logoff();
                    return;
                }
                //if (!string.IsNullOrEmpty(QueryInfo.SearchTradeResult))
                //{
                //    var tableCJ = Tool.ChangeDataStringToTable(Result.ToString());
                //    if (tableCJ.Rows.Count > 0)
                //    {
                //        serialNumm = (int)tableCJ.Rows[0]["serialNum"];
                //    }
                //}

                Thread.Sleep(查询间隔时间 / 2);
            }
            catch (Exception ex)
            {
                CommonUtils.Log("QueryImsData Exception:", ex);
            }

            
            
            //如何计算 serialNumm
            


            
        }

        private void QueryFilterData()
        {
            StringBuilder resultWt = new StringBuilder(1024 * 1024);
            StringBuilder errInfoWt = new StringBuilder(256);
            DateTime dt1 = DateTime.Now;
            TdxApi.QueryData(this.ClientID, 2, resultWt, errInfoWt);
            var span = DateTime.Now - dt1;
            var costsSeconds = span.TotalSeconds;
            //CommonUtils.Log("{0}委托查询单次耗时{1}", this.名称, costsSeconds.ToString());
            queueWt.Enqueue(resultWt.ToString());
            QueryInfo.SearchOperatorError = errInfoWt.ToString();
            Thread.Sleep(查询间隔时间);

            #region 其他线程
            if (QeuryCJThread == null)
            {
                QeuryCJThread = new Thread(new ThreadStart(QueryCJMain)) { IsBackground = true };
                QeuryCJThread.Start();
            }

            if (FilteWtDataThread == null)
            {
                FilteWtDataThread = new Thread(new ThreadStart(FilterWtDataMain)) { IsBackground = true };
                FilteWtDataThread.Start();
            }

            if (FilteCjDataThread == null)
            {
                FilteCjDataThread = new Thread(new ThreadStart(FilterCjDataMain)) { IsBackground = true };
                FilteCjDataThread.Start();
            }
            #endregion
        }

        private void QueryCJMain()
        {
            StringBuilder resultCJ = new StringBuilder(1024 * 1024);
            StringBuilder errInfoCJ = new StringBuilder(256);
            while (true)
            {
                DateTime dt = DateTime.Now;
                if ((DateTime.Parse(CommonUtils.GetConfig("开始查询时间", "8:15")) <= DateTime.Now
                    && DateTime.Now <= DateTime.Parse(CommonUtils.GetConfig("结束查询时间", "15:30"))))
                {
                    try
                    {
                        TdxApi.QueryData(ClientID, 3, resultCJ, errInfoCJ);
                        queueCj.Enqueue(resultCJ.ToString());
                        QueryInfo.SearchTradeErrInfo = errInfoCJ.ToString();
                    }
                    catch (Exception ex) {
                        CommonUtils.Log("成交查询异常", ex);
                    }
                    Thread.Sleep(查询间隔时间);
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
        } 
        #endregion

        #region Data Filter Main
        private void FilterWtDataMain()
        {
            string last = string.Empty;
            while (true)
            {
                try
                {
                    string strWtResult = string.Empty;
                    //DateTime dt1 = DateTime.Now;
                    if (queueWt.Count > 0 && queueWt.TryDequeue(out strWtResult))
                    {
                        if (!string.IsNullOrEmpty(strWtResult) && (last != strWtResult || dictWt.Count != lstOrderID.Count))
                        {
                            string[] arr = strWtResult.ToString().Split('\n');
                            MatchData(dictWtIndex, arr, dictWt);
                            //MatchData(dictWt, arr);
                            last = strWtResult;
                            foreach (var item in dictOrderSuccessTime)
                            {
                                if (item.Value != DateTime.MinValue && dictWtIndex.ContainsKey(item.Key))
                                {
                                    dictOrderSuccessTime[item.Key] = DateTime.MinValue;
                                }
                            }
                            if (string.IsNullOrEmpty(strTitleWt) && arr.Length > 0)
                                strTitleWt = arr[0];
                            //CommonUtils.Log("过滤委托数据总耗时：" + (DateTime.Now - dt1).TotalSeconds);
                        }
                        
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                    
                }
                catch (Exception ex)
                {
                    CommonUtils.Log("Wt FilteDataMain, Main Exception:" + ex.Message);
                    Thread.Sleep(1000);
                }
            }
        } 

        private void FilterCjDataMain()
        {
            string last = string.Empty;
            while (true)
            {
                try
                {
                    DateTime dt1 = DateTime.Now;
                    string strCjResult;
                    if (queueCj.Count > 0 && queueCj.TryDequeue(out strCjResult))
                    {
                        if (!string.IsNullOrEmpty(strCjResult) && last != strCjResult || dictCj.Count != lstOrderID.Count)
                        {
                            string[] arr = strCjResult.ToString().Split('\n');
                            MatchData(dictCjIndex, arr, dictCj);
                            //MatchData(dictCj, arr);
                            last = strCjResult;

                            if (string.IsNullOrEmpty(strTitleCj) && arr.Length > 0)
                                strTitleCj = arr[0];

                            CommonUtils.Log("过滤成交数据总耗时：" + (DateTime.Now - dt1).TotalSeconds);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                    
                }
                catch (Exception ex)
                {
                    CommonUtils.Log("Cj FilteDataMain, Main Exception:" + ex.Message);
                    Thread.Sleep(500);
                }
            }
        } 

        private void MatchData(Dictionary<string, int> dictIndex, string[] arrPrimitive, ConcurrentDictionary<string, string> dict)
        {
            try
            {
                List<string> lstNotMatchedID = new List<string>();

                #region 根据ID找匹配项。
                foreach (var id in lstOrderID)
                {
                    if (string.IsNullOrEmpty(id)) continue;

                    if (dictIndex.ContainsKey(id))
                    {
                        if (arrPrimitive.Length > dictIndex[id])
                        {
                            var itemPrimitive = arrPrimitive[dictIndex[id]];
                            var itemMatchedID = regexOrder.Match(itemPrimitive);
                            if (itemMatchedID.Success && itemMatchedID.Value == id)
                            {
                                dict[id] = itemPrimitive;
                                continue;
                            }
                        }
                       
                    }
                    lstNotMatchedID.Add(id);
                }
                #endregion

                #region 循环所有查询结果，查到未匹配的项对应的index，更新Index
                if (lstNotMatchedID.Count > 0)
                {
                    for (int i = 0; i < arrPrimitive.Length; i++)
                    {
                        var matchItem = regexOrder.Match(arrPrimitive[i]);
                        if (matchItem.Success && lstNotMatchedID.Contains(matchItem.Value))
                        {
                            dict[matchItem.Value] = arrPrimitive[i];
                            dictIndex[matchItem.Value] = i;
                            lstNotMatchedID.Remove(matchItem.Value);
                            if (lstNotMatchedID.Count == 0)
                            {
                                break;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                CommonUtils.Log("MatchData Exception", ex);
            }
        }
        #endregion

        #region Get Order
        public string GetOrder(string OrderID)
        {
            try
            {
                if (dictWt.ContainsKey(OrderID))
                {
                    return dictWt[OrderID];
                }
                else if (!lstOrderID.Contains(OrderID))
                {
                    lstOrderID.Add(OrderID);
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("GetOrder Exception", ex);
            }
            
            return null;
        }

        public string[] GetOrdersDetail(string mac)
        {
            if (dictMacOrder.ContainsKey(mac))
            {
                var listOrderID = dictMacOrder[mac];
                var listOrderDetail = new List<string>();
                foreach (var id in listOrderID)
                {
                    if (dictWt.ContainsKey(id))
                        listOrderDetail.Add(dictWt[id]);
                    else if (!lstOrderID.Contains(id))
                        lstOrderID.Add(id);
                }
                return listOrderDetail.ToArray();
            }
            return null;
        }

        public string[] GetTradesDetail(string mac)
        {
            if (dictMacOrder.ContainsKey(mac))
            {
                var listOrderID = dictMacOrder[mac];
                var listTradeDetail = new List<string>();
                foreach (var id in listOrderID)
                {
                    if (dictCj.ContainsKey(id))
                        listTradeDetail.Add(dictCj[id]);
                    else if (!lstOrderID.Contains(id))
                        lstOrderID.Add(id);
                }
                return listTradeDetail.ToArray();
            }
            return null;
        }

        public string[] UpdateOrderID(string mac, string[] arrID)
        {
            if (dictMacOrder.ContainsKey(mac))
            {
                var list = dictMacOrder[mac];
                foreach (var id in arrID)
                {
                    if (!list.Contains(id))
                    {
                        list.Add(id);

                        if (!lstOrderID.Contains(id))
                            lstOrderID.Add(id);
                    }
                }
            }
            else
            {
                dictMacOrder[mac] = new ConcurrentBag<string>(arrID);
            }
            return lstOrderID.ToArray();
        }
        #endregion


        #region Filter Data
        public string[] FilterDataByOrder(string result, string mac)
        {
            string[] lstQuery = null;
            StringBuilder strWhere = new StringBuilder();
            try
            {
                string orderColName = string.Empty;
                //CommonUtils.Log(result);
                if (dictMacOrder.ContainsKey(mac))
                {
                    DataTable dtResult = Tool.ChangeDataStringToTable(result);
                    if (dtResult == null || dtResult.Rows.Count == 0) return null;

                    if (dtResult.Columns.Contains("合同编号"))
                        orderColName = "合同编号";
                    else if (dtResult.Columns.Contains("委托编号"))
                        orderColName = "委托编号";
                    //else if (dtResult.Columns.Contains("contractNum"))
                    //    orderColName = "contractNum";

                    ConcurrentBag<string> order = dictMacOrder[mac];
                    if (string.IsNullOrEmpty(orderColName) || order == null || order.Count == 0) 
                    {
                        if (dtResult.Columns.Contains("撤单标志"))
                            strWhere.AppendFormat(" 撤单标志 <> '1'");
                        lstQuery = Tool.QueryDataTableByWhere(dtResult, strWhere.ToString());
                    }
                    else
                    {
                        OrderInfoCache mayLostOrd;
                        if (queueMayLostOrder.Count > 0 && queueMayLostOrder.TryPeek(out mayLostOrd))
                        {
                            if ((DateTime.Now - mayLostOrd.SendTime).TotalSeconds > 30)
                            {
                                queueMayLostOrder.TryDequeue(out mayLostOrd);
                            }
                            else if ( dtResult.Columns.Contains("证券代码") && dtResult.Columns.Contains("买卖标志") && dtResult.Columns.Contains("委托价格") && dtResult.Columns.Contains("委托数量"))
                            {
                                //验证是否可以进行补漏行为
                                for (int i = Math.Max(0, mayLostOrd.CheckedIndex); i < dtResult.Rows.Count; i++)
                                {
                                    
                                    DataRow row = dtResult.Rows[i];
                                    if (!order.Contains(row[orderColName].ToString()))
                                    {
                                        bool isDataSame = mayLostOrd.StockID == row["证券代码"].ToString()
                                        && CommonUtils.IsTdxBuy(mayLostOrd.Category) == CommonUtils.IsTdxBuy(int.Parse(row["买卖标志"] + ""))
                                        && (mayLostOrd.SendPrice == float.Parse(row["委托价格"] + ""))
                                        && (mayLostOrd.SendQty == decimal.Parse(row["委托数量"] + ""));
                                        if (isDataSame)
                                        {
                                            order.Add(row[orderColName].ToString());

                                            CommonUtils.Log("匹配到可能漏单数据{0}，已加入返回信息", mayLostOrd.ToJson());
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        StringBuilder sbOrder = new StringBuilder();
                        sbOrder.Append("'");
                        sbOrder.Append(string.Join("','", order.ToArray()));
                        sbOrder.Append("'");
                        //foreach (var item in order)
                        //{
                        //    sbOrder.AppendFormat(string.Format("'{0}',"), item);
                        //}
                        strWhere.AppendFormat("{0} in ({1})", orderColName, sbOrder.ToString());

                        if (dtResult.Columns.Contains("撤单标志"))
                            strWhere.AppendFormat(" and 撤单标志 <> '1'");
                        lstQuery = Tool.QueryDataTableByWhere(dtResult, strWhere.ToString());
                    }
                    //CommonUtils.Log("查询条件strWhere={0}，查询结果{1}", strWhere.ToString(), dtQuery.Rows.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("数据过滤异常-FilterDataByOrder:{0},查询条件strWhere={1}", ex.StackTrace, strWhere.ToString());
            }

            return lstQuery;
        }
        #endregion
        public GroupClient.GroupService.QueryDataObj QueryInfo { get; set; }

        //public GroupClient.GroupService.QueryDataObj QueryInfoHK { get; set; }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 0:
                    var status = e.UserState as string;
                    var item = Adapter.GroupLogonList.FirstOrDefault(r => r.Name == this.名称);
                    if (item == null)
                    {
                        item = new GroupLogonInfo();
                        item.Name = this.名称;
                        item.Times = status;
                        item.CanUse = this.启用;
                        Adapter.GroupLogonList.Add(item);
                    }
                    else
                    {
                        item.CanUse = this.启用;
                        item.Times = status;
                    }
                    if (Regex.IsMatch(status, "[\u4e00-\u9fa5]+"))
                    {
                        Status = status;
                    }
                    break;
                default:
                    break;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Error == null)
            {
                var item = Adapter.GroupLogonList.FirstOrDefault(r => r.Name == this.名称);
                if (item != null)
                {
                    Adapter.GroupLogonList.Remove(item);
                }
            }
            else
            {
                var item = Adapter.GroupLogonList.FirstOrDefault(r => r.Name == this.名称);
                if (item == null)
                {
                    item = new GroupLogonInfo() { Name = this.名称 };
                    item.Times = e.Error.Message;
                    Adapter.GroupLogonList.Add(item);
                }
                else
                {
                    item.Times = e.Error.Message;
                }
            }
        }

        public int ClientID = -1;

        public string Logon()
        {
            StringBuilder ErrInfo = new StringBuilder(256);
            if (IsIMSAccount)
            {
                string Msg;
                ImsApi.Init(ImsNotify);
                bool isLogin = ImsApi.Login(this.交易帐号, Cryptor.MD5Decrypt(this.交易密码), this.版本号, out Msg);
                if (isLogin)
                {
                    this.ClientID = 0;
                    return "登录成功";
                }
                else
                {
                    return Msg;
                }
            }
            else
            {
                try
                {
                    this.ClientID = TdxApi.Logon(this.IP, this.Port, this.版本号, this.营业部代码, this.登录帐号, this.交易帐号, Cryptor.MD5Decrypt(this.交易密码), Cryptor.MD5Decrypt(this.通讯密码), ErrInfo);
                }
                catch (Exception ex)
                {
                    CommonUtils.Log("组合号登录异常", ex);
                }

                string strError = ErrInfo.ToString();
                if (!string.IsNullOrEmpty(strError))
                {
                    //交易中心(-10016):您使用的版本太低已因为安全原因停止使用,请到官网上下载最新版本重新安装。
                    if (strError.Contains("密码") || strError.Contains("您使用的版本太低已因为安全原因停止使用"))
                    {
                        //ErrInfo.Append("登录异常，将自动禁用，请检查组合号信息！");
                        CommonUtils.Log("登录密码错误或版本过低导致登录失败，将自动停用，错误信息：{0}", strError);
                        this.启用 = false;
                    }
                    return "登录失败:" + ErrInfo.ToString();
                }
                else
                {
                    return "登录成功";
                }
            }
           
        }

        public void Logoff()
        {
            try
            {
                if (this.IsIMSAccount)
                {
                    ImsApi.ImsPbClient_Disconnect();
                }
                else
                {
                    TdxApi.Logoff(this.ClientID);
                }
            }
            catch{ }

            this.ClientID = -1;
        }

        private void ClearCache()
        {
            if (QeuryCJThread != null)
            {
                QeuryCJThread.Abort();
                FilteCjDataThread.Abort();
                FilteWtDataThread.Abort();
                lstOrderID = new ConcurrentBag<string>();
                dictWt.Clear();
                dictCj.Clear();
            }

            QeuryCJThread = null;
            FilteCjDataThread = null;
            FilteWtDataThread = null;
            dictOrderSuccessTime = new ConcurrentDictionary<string, DateTime>();//测试订单从收到委托编号，到查询结果里有这条数据的耗时。
        }

        #region Send Order
        /// <summary>
        /// A股下单逻辑
        /// </summary>
        public GroupService.Queryinfo SendOrderNormal(int Category, int PriceType, string Gddm, string Zqdm, float Price, int Quantity, string mac, StringBuilder result, StringBuilder errInfo)
        {
            DateTime st = DateTime.Now;

            StringBuilder sbResult = new StringBuilder(1024 * 1024);
            StringBuilder sbError = new StringBuilder(256);
            GroupService.Queryinfo response = new GroupService.Queryinfo();
            try
            {
                sbResult.Append(this.strTitleWt + "\n" + string.Join("\n", dictWt.Values));
                sbError.Append(this.QueryInfo.SearchOperatorError);

                var 下单前ErrInfo = sbError.ToString();
                if (下单前ErrInfo != string.Empty)
                {
                    response.Result = sbResult.ToString();
                    response.Error = sbError.ToString();
                    CommonUtils.Log("下单前查询异常，组合号{0}，证券代码{1},错误信息{2},耗时{3}", this.名称, Zqdm, response.Error, (DateTime.Now - st).TotalSeconds.ToString());
                }
                else
                {
                    response.Other = result.ToString();
                    TdxApi.SendOrder(ClientID, Category, PriceType, Gddm, Zqdm, Price, Quantity, sbResult, sbError);
                    string ErrInfo = sbError.ToString();
                    if (ErrInfo == string.Empty)
                    {
                        DataTable DataTable1 = CommonUtils.ChangeDataStringToTable(sbResult.ToString());

                        string id = null;
                        if (DataTable1.Columns.Contains("委托编号"))
                        {
                            id = DataTable1.Rows[0]["委托编号"] as string;
                        }
                        else if (DataTable1.Columns.Contains("合同编号"))
                        {
                            id = DataTable1.Rows[0]["合同编号"] as string;
                        }
                        if (id != null && !lstOrderID.Contains(id))
                        {
                            lstOrderID.Add(id);
                            dictOrderSuccessTime[id] = DateTime.Now;
                        }
                    }
                    else
                    {
                        CommonUtils.Log("下单异常，组合号{0}，证券代码{1},错误信息{2}", this.名称, Zqdm, ErrInfo);
                        if (ErrInfo.Contains("尝试其他交易服务器") || ErrInfo.Contains("超时"))
                        {
                            CommonUtils.Log("开始记录可能存在漏单数据");
                            var mayLostOrder = new OrderInfoCache()
                            {
                                Category = Category,
                                GroupName = this.名称,
                                SendPrice = Price,
                                SendQty = Quantity,
                                StockID = Zqdm,
                                SendTime = DateTime.Now,
                                SendMac = mac,
                            };
                            CommonUtils.Log("可能存在漏单数据，请求数据{0}", mayLostOrder.ToJson());
                            var table = Tool.ChangeDataStringToTable(this.QueryInfo.SearchOperatorResult);
                            if (table != null && table.Rows.Count > 0)
                            {
                                mayLostOrder.CheckedIndex = table.Rows.Count;
                            }
                            queueMayLostOrder.Enqueue(mayLostOrder);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("下单异常,组合号{0}, 股票代码{1},异常信息{2}", this.名称, Zqdm, ex.Message);
            }
            response.Result = sbResult.ToString();
            response.Error = sbError.ToString();
            return response;
        }

        public GroupService.Queryinfo SendOrderHK(int Category, int PriceType, string Gddm, string Zqdm, float Price, int Quantity, StringBuilder result, StringBuilder errInfo)
        {
            GroupClient.GroupService.Queryinfo response = new GroupService.Queryinfo();
            try
            {

                TdxApi.QueryHKData(ClientID, 2, result, errInfo);
                var 下单前ErrInfo = errInfo.ToString();
                if (下单前ErrInfo != string.Empty)
                {
                    response.Result = result.ToString();
                    response.Error = errInfo.ToString();

                }
                else
                {
                    response.Other = result.ToString();
                    TdxApi.SendHKOrder(ClientID, Category, 2, 0, Gddm, Zqdm, Price.ToString(), Quantity.ToString(), result, errInfo);
                }
            }
            catch (Exception ex)
            {
                response.Error += "下单异常:" + ex.Message;
            }
            
            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BSFlag">B 买， S 卖</param>
        /// <param name="Market">上海0    深圳1    港股H    沪港通0H      沪股通H0      深港通1H         深股通H1</param>
        /// <param name="Zqdm"></param>
        /// <param name="Price"></param>
        /// <param name="Quantity"></param>
        /// <param name="result"></param>
        /// <param name="errInfo"></param>
        public void SendOrderIMS(string BSFlag, string Market, string Zqdm, float Price, int Quantity, StringBuilder result, StringBuilder errInfo)
        {
            try
            {

                ImsApi.ImsPbClient_RequestNormalOrder(this.产品信息, this.资产单元, this.投资组合, Market, Zqdm, BSFlag, Quantity, Convert.ToDouble(Price.ToString()), "LIMIT", string.Empty, "我的备注", result, errInfo);
                CommonUtils.Log("SendOrderIMS Result:{0}, ErrorInfo{1}", result.ToString(), errInfo.ToString());
            }
            catch (Exception ex)
            {
                errInfo.Append("SendOrderIMS Exception:" + ex.Message);
            }
            
        }
        #endregion

        #region Cancel Order
        public void CancelOrder(string ExchangeID, string hth, StringBuilder resultCancel, StringBuilder errCancelInfo)
        {
            if (this.ClientID > -1)
            {
                DateTime st = DateTime.Now;
                try
                {
                    //TdxApi.QueryData(ClientID, 5, resultCancel, errCancelInfo);
                    TdxApi.CancelOrder(ClientID, ExchangeID, hth, resultCancel, errCancelInfo);
                    CommonUtils.Log("{0}执行撤单完成, Result:{1}, Error:{2}, 耗时{3}", this.名称, resultCancel.ToString(), errCancelInfo.ToString(), (DateTime.Now - st).TotalSeconds.ToString());
                }
                catch (Exception ex)
                {
                    CommonUtils.Log(string.Format("{0}执行撤单异常,订单号{1}, 耗时{2}， Exception{3}", this.名称,hth, (DateTime.Now - st).TotalSeconds.ToString(), ex.Message));
                }
            }
            else
            {
                errCancelInfo.AppendFormat("撤单失败，组合号{0} ClientID为-1", this.名称);
            }
        }

        public void CancelHKOrder(string hth, StringBuilder resultCancel, StringBuilder errCancelInfo)
        {
            if (this.ClientID > -1)
            {
                try
                {
                    StringBuilder result = new StringBuilder(1024 * 1024);
                    StringBuilder errInfo = new StringBuilder(256);
                    TdxApi.QueryHKData(ClientID, 0, result, errInfo);
                    result.Clear();
                    errInfo.Clear();
                    TdxApi.CancelHKOrder(ClientID, hth, result, errInfo);
                }
                catch (Exception ex)
                {
                    CommonUtils.Log("CancelHKOrder Exception", ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="market">上海0    深圳1</param>
        /// <param name="zqdm"></param>
        /// <param name="orderID"></param>
        /// <param name="Result"></param>
        /// <param name="ErrInfo"></param>
        public void CancelImsOrder(string market, string zqdm, string orderID, StringBuilder Result, StringBuilder ErrInfo)
        {
            ImsApi.ImsPbClient_CancelNormalOrder(this.资产单元, this.投资组合, market, zqdm, orderID, string.Empty, Result, ErrInfo);
        
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType">表示查询信息的种类，0资金  1股份   2当日委托  3当日成交     4可撤单   5股东代码  6融资余额   7融券余额  8可融证券</param>
        /// <returns></returns>
        internal void QueryAccountData(int dataType, GroupClient.GroupService.Queryinfo QueryResult)
        {
            StringBuilder result = new StringBuilder(1024 * 1024);
            StringBuilder errInfo = new StringBuilder(256);

            try
            {
                if (this.ClientID == -1)
                {
                    this.ClientID = TdxApi.Logon(this.IP, this.Port, this.版本号, this.营业部代码, this.登录帐号, this.交易帐号, Cryptor.MD5Decrypt(this.交易密码), Cryptor.MD5Decrypt(this.通讯密码), errInfo);
                    isLogonInterim = this.ClientID > -1;
                }
                if (this.ClientID > -1)
                {
                    TdxApi.QueryData(this.ClientID, dataType, result, errInfo);
                }
            }
            catch (Exception ex) 
            {
                errInfo.AppendFormat("查询接口异常:{0}", ex.Message);
            }

            QueryResult.Error = errInfo.ToString();
            QueryResult.Result = result.ToString();

            if (isLogonInterim)
            {
                isLogonInterim = false;
            }
        }
        
        /// <summary>
        /// 临时登录标记，用于Repay，仓位查询等接口
        /// </summary>
        bool isLogonInterim = false;
        public void Repay(string amount, StringBuilder Result, StringBuilder ErrInfo)
        {

            try
            {
                if (this.ClientID == -1)
                {
                    this.ClientID = TdxApi.Logon(this.IP, this.Port, this.版本号, this.营业部代码, this.登录帐号, this.交易帐号, Cryptor.MD5Decrypt(this.交易密码), Cryptor.MD5Decrypt(this.通讯密码), ErrInfo);
                    isLogonInterim = this.ClientID > -1;
                }
                if (this.ClientID > -1)
                {
                    TdxApi.Repay(this.ClientID, amount, Result, ErrInfo);
                }
            }
            catch (Exception ex)
            {
                ErrInfo.AppendFormat("Repay异常:{0}", ex.Message);
            }

            if (isLogonInterim)
            {
                isLogonInterim = false;
            }
        }

        public void SetStatus(string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                this.Status = status;
            }
        }

        ConcurrentDictionary<string, DateTime> dictOrderSuccessTime = new ConcurrentDictionary<string, DateTime>();

        #region NotifyPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        } 
        #endregion


    }

    public class GroupLogonInfo: INotifyPropertyChanged
    {
        public string Name { get; set; }

        string _times;
        public string Times { 
            get { return _times; }
            set 
            {
                _times = value;
                NotifyPropertyChange("Times");
            }
        }

        bool _canUse;
        public bool CanUse {
            get
            {
                return _canUse;
            }
            set
            {
                _canUse = value;
                NotifyPropertyChange("CanUse");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        } 
    }

    //用于保存到配置文件中，避免配置文件过大。
    public class GroupInfo
    {
        public GroupInfo()
        { }

        public GroupInfo(券商 o)
        {
            this.IP = o.IP;
            this.Port = o.Port;
            this.版本号 = o.版本号;
            this.查询间隔时间 = o.查询间隔时间;
            this.登录帐号 = o.登录帐号;
            this.交易服务器 = o.交易服务器;
            this.交易密码 = o.交易密码;
            this.交易帐号 = o.交易帐号;
            this.名称 = o.名称;
            this.启用 = o.启用;
            this.通讯密码 = o.通讯密码;
            this.营业部代码 = o.营业部代码;
            this.Multithreading = o.Multithreading;
            this.IsIMSAccount = o.IsIMSAccount;
            this.产品信息 = o.产品信息;
            this.资产单元 = o.资产单元;
            this.投资组合 = o.投资组合;
        }


        public string 名称 { get; set; }

        public bool 启用 { get; set; }


        public string 交易服务器 { get; set; }

        public string IP { get; set; }

        public short Port { get; set; }

        public string 版本号 { get; set; }

        public short 营业部代码 { get; set; }

        public string 登录帐号 { get; set; }

        public string 交易帐号 { get; set; }

        public string 交易密码 { get; set; }

        public int 查询间隔时间 { get; set; }

        public string 通讯密码 { get; set; }

        /// <summary>
        /// 使用多线程
        /// </summary>
        public bool? Multithreading { get; set; }

        /// <summary>
        /// 是否IMS接口
        /// </summary>
        public bool IsIMSAccount { get; set; }

        public string 产品信息 { get; set; }

        public string 资产单元 { get; set; }

        public string 投资组合 { get; set; } 
    }

    public class OrderInfoCache
    {
        public string OrderID { get; set; }

        public string GroupName { get; set; }

        public string StockID { get; set; }

        public decimal SendQty { get; set; }

        public float SendPrice { get; set; }

        public DateTime SendTime { get; set; }

        public int Category { get; set; }

        public string SendMac { get; set; }

        public int CheckedIndex { get; set; }

    }
}
