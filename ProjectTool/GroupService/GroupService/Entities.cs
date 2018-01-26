using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupService
{
    public enum RoleType
    {
        超级管理员 = 0,
        普通管理员 = 1,

        超级风控员 = 10,
        普通风控员 = 11,
        初级风控员 = 12,

        交易员 = 20,
    }

    public class 券商
    {
        static Regex regexOrder = new Regex("(?<=[0-9]{2}:[0-9]{2}:[0-9]{2}\t)[0-9]+");

        public 券商() { }

        public 券商(GroupInfo o)
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

        string _status = "未登录";
        public string Status { get { return _status; } }


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
            if (this.查询间隔时间 < 500)
            {
                this.查询间隔时间 = 500;
            }
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
                            //if (this.登录帐号 == "260500043078" && this.交易帐号 == "260500043078")
                            //{
                            //    this.QueryDataHK();
                            //}
                            this.QueryInfo.QueryTime = DateTime.Now - DateTime1;
                            this.backgroundWorker1.ReportProgress(0, this.QueryInfo.QueryTime.TotalSeconds.ToString());
                        }
                        #endregion
                    }
                    else
                    {
                        #region 非交易时段
                        if (this.ClientID != -1)
                        {
                            this.backgroundWorker1.ReportProgress(0, "注销中...");

                            this.Logoff();

                            this.backgroundWorker1.ReportProgress(0, "已注销");
                        }

                        this.backgroundWorker1.ReportProgress(0, "非交易时段");

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

        Thread FilteCjDataThread = null;
        Thread FilteWtDataThread = null;
        Thread QeuryCJThread = null;

        public string strTitleWt { get; set; }

        public string strTitleCj { get; set; }
        #endregion

        #region Data Query Main
        private void QueryData()
        {
            StringBuilder result = new StringBuilder(1024 * 1024);
            StringBuilder errInfo = new StringBuilder(256);
            if (QueryInfo == null)
            {
                QueryInfo = new GroupService.QueryDataObj();
            }
            if (this.Multithreading == true)
            {
                QueryFilterData();
            }
            else
            {
                TdxApi.QueryData(this.ClientID, 3, result, errInfo);
                QueryInfo.SearchTradeResult = result.ToString();
                QueryInfo.SearchTradeErrInfo = errInfo.ToString();

                Thread.Sleep(查询间隔时间 / 2);

                TdxApi.QueryData(this.ClientID, 2, result, errInfo);
                QueryInfo.SearchOperatorResult = result.ToString();
                QueryInfo.SearchOperatorError = errInfo.ToString();
                Thread.Sleep(查询间隔时间 / 2);
            }
        }

        private void QueryFilterData()
        {
            StringBuilder resultWt = new StringBuilder(1024 * 1024);
            StringBuilder errInfoWt = new StringBuilder(256);

            DateTime dt1 = DateTime.Now;
            TdxApi.QueryData(this.ClientID, 2, resultWt, errInfoWt);

            queueWt.Enqueue(resultWt.ToString());
            QueryInfo.SearchOperatorError = errInfoWt.ToString();
            Thread.Sleep(查询间隔时间 / 2);

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
                    catch (Exception ex)
                    {
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
                    DateTime dt1 = DateTime.Now;
                    if (queueWt.Count > 0 && queueWt.TryDequeue(out strWtResult))
                    {
                        if (!string.IsNullOrEmpty(strWtResult) && (last != strWtResult || dictWt.Count != lstOrderID.Count))
                        {
                            string[] arr = strWtResult.ToString().Split('\n');
                            MatchData(dictWtIndex, arr, dictWt);
                            //MatchData(dictWt, arr);
                            last = strWtResult;

                            if (string.IsNullOrEmpty(strTitleWt) && arr.Length > 0)
                                strTitleWt = arr[0];
                            CommonUtils.Log("过滤委托数据总耗时：" + (DateTime.Now - dt1).TotalSeconds);
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
                        var itemPrimitive = arrPrimitive[dictIndex[id]];
                        var itemMatchedID = regexOrder.Match(itemPrimitive);
                        if (itemMatchedID.Success && itemMatchedID.Value == id)
                        {
                            dict[id] = itemPrimitive;
                            continue;
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

        int wtOrderIndex = -1;
        //public string GetWtOrderID(string strLine)
        //{
        //    if (wtOrderIndex < 0)
        //    {
        //        var arr = strTitleWt.Split('\n');
        //        for (int i = 0; i < arr.Length; i++)
        //        {
        //            if (arr[i] == "委托编号" || arr[i] == "合同编号")
        //            {
        //                wtOrderIndex = i;
        //            }
        //        }
        //    }
        //    if (wtOrderIndex < -1)
        //    {

        //    }
        //}

        //public string GetDataRow委托编号(DataRow DataRow0)
        //{
        //    if (this.券商 == "银河证券" && this.类型 == "信用")
        //    {
        //        return DataRow0["合同编号"] as string;
        //    }
        //    else
        //    {
        //        if (DataRow0.Table.Columns.Contains("委托编号"))
        //        {
        //            return DataRow0["委托编号"] as string;
        //        }
        //        else
        //        {
        //            return "0";
        //        }
        //    }
        //}
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

        public string GetTrade(string OrderID)
        {
            if (dictCj.ContainsKey(OrderID))
            {
                return dictCj[OrderID];
            }
            else if (!lstOrderID.Contains(OrderID))
            {
                lstOrderID.Add(OrderID);
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

        public GroupService.TradeInterface.QueryDataObj QueryInfo { get; set; }

        public GroupService.QueryDataObj QueryInfoHK { get; set; }

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
                        Adapter.GroupLogonList.Add(item);
                    }
                    else
                    {
                        item.Times = status;
                    }
                    if (Regex.IsMatch(status, "[\u4e00-\u9fa5]+"))
                    {
                        this._status = status;
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
            try
            {
                this.ClientID = TdxApi.Logon(this.IP, this.Port, this.版本号, this.营业部代码, this.登录帐号, this.交易帐号, Cryptor.MD5Decrypt(this.交易密码), Cryptor.MD5Decrypt(this.通讯密码), ErrInfo);
            }
            catch (Exception ex)
            {
                this.ClientID = -1;
                ErrInfo.Append("登录异常，将自动禁用，请检查组合号信息！");
                this.启用 = false;
                CommonUtils.Log("组合号登录异常", ex);
            }

            if (ErrInfo.ToString() != string.Empty)
            {
                if (this.ClientID != -1)
                {
                    this.ClientID = -1;
                }
                return "登录失败:" + ErrInfo.ToString();
            }
            else
            {
                return "登录成功";
            }
        }

        public void Logoff()
        {
            try
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
                TdxApi.Logoff(this.ClientID);
                this.ClientID = -1;
                QeuryCJThread = null;
                FilteCjDataThread = null;
                FilteWtDataThread = null;
            }
            catch (Exception) { }


        }

        #region Send Order
        /// <summary>
        /// A股下单逻辑
        /// </summary>
        public GroupService.Queryinfo SendOrderNormal(int Category, int PriceType, string Gddm, string Zqdm, float Price, int Quantity, StringBuilder result, StringBuilder errInfo)
        {
            StringBuilder sbResult = new StringBuilder(1024 * 1024);
            StringBuilder sbError = new StringBuilder(256);
            GroupService.Queryinfo response = new GroupService.Queryinfo();
            try
            {
                sbResult.Append(this.strTitleWt + "\n" + string.Join("\n", dictWt.Values));
                sbError.Append(this.QueryInfo.SearchOperatorError);
                //if (this.Multithreading == true)
                //{
                //    TdxApi.QueryData(ClientID, 0, sbResult, sbError);
                //    sbResult.Clear();
                //    sbError.Clear();
                //    sbResult.Append(this.strTitleWt + "\n" + string.Join("\n", dictWt.Values));
                //    sbError.Append(this.QueryInfo.SearchOperatorError);
                //}
                //else
                //{
                //    TdxApi.QueryData(ClientID, 2, sbResult, sbError);
                //}

                var 下单前ErrInfo = sbError.ToString();
                if (下单前ErrInfo != string.Empty)
                {
                    response.Result = sbResult.ToString();
                    response.Error = sbError.ToString();
                    CommonUtils.Log("A股下单前查询异常，证券代码" + Zqdm + "：" + response.Error);
                }
                else
                {
                    response.Other = result.ToString();
                    TdxApi.SendOrder(ClientID, Category, PriceType, Gddm, Zqdm, Price, Quantity, sbResult, sbError);

                    string ErrInfo = sbError.ToString();
                    if (ErrInfo == string.Empty)
                    {
                        DataTable DataTable1 = CommonUtils.ChangeDataStringToTable(sbResult.ToString());
                        string id = DataTable1.Rows[0]["委托编号"] as string;
                        if (Regex.IsMatch(id, "^[0-9]+$") && !lstOrderID.Contains(id))
                        {
                            lstOrderID.Add(id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("下单异常", ex);
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
        #endregion

        #region Cancel Order
        public void CancelOrder(string ExchangeID, string hth, StringBuilder resultCancel, StringBuilder errCancelInfo)
        {
            if (this.ClientID > -1)
            {
                try
                {
                    DateTime dtStart = DateTime.Now;
                    //if (this.Multithreading == true)
                    //{
                    //    CommonUtils.Log(string.Format("{0} 开始撤单前查询", this.名称));
                    //}

                    //TdxApi.QueryData(this.ClientID, 0, resultCancel, errCancelInfo);

                    //if (this.Multithreading == true)
                    //{
                    //    //CommonUtils.Log(string.Format("{0}1.开始撤单前查询", this.名称));
                    //    CommonUtils.Log(string.Format("{0} 查询完毕，开始执行撤单,ExchangeID:{1}, hth:{2}", this.名称, ExchangeID, hth));
                    //}
                    TdxApi.CancelOrder(ClientID, ExchangeID, hth, resultCancel, errCancelInfo);

                    if (this.Multithreading == true)
                    {
                        CommonUtils.Log("{0}执行撤单完成, Result:{1}, Error:{2}, 耗时{3}", this.名称, resultCancel.ToString(), errCancelInfo.ToString(), (DateTime.Now - dtStart).TotalSeconds.ToString());
                    }
                }
                catch (Exception ex)
                {
                    CommonUtils.Log(string.Format("{0}执行撤单异常,订单号{1}， Exception{2}", this.名称, hth, ex.Message));
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
        #endregion

        public void SetStatus(string status)
        {
            if (!string.IsNullOrEmpty(status))
            {
                this._status = status;
            }
        }
    }

    public class GroupLogonInfo
    {
        public string Name { get; set; }

        public string Times { get; set; }

        public bool CanUse { get; set; }
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
    }
}