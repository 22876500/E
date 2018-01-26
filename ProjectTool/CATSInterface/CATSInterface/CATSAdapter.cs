using Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CATSInterface
{
    public class CATSAdapter: INotifyPropertyChanged
    {
        private static object Sync = new object();

        #region Configs
        static string _accType = null;
        private static string Acc_Type
        {
            get
            {
                if (_accType == null)
                {
                    _accType = Utils.GetConfig("ACCT_TYPE");
                    if (string.IsNullOrEmpty(_accType))
                    {
                        _accType = "0";
                    }
                }
                return _accType;
            }
        }

        static string _catsPath = null;
        private static string CATS_PATH
        {
            get
            {
                if (_catsPath == null)
                {
                    _catsPath = Utils.GetConfig("CATS_PATH");
                    if (string.IsNullOrEmpty(_catsPath))
                    {
                        //_catsPath = "D:\\Program Files (x86)\\Wealth CATS\\scan_order";
                        Utils.logger.LogInfo("CATS_PATH 为空，请检查配置文件！");
                    }
                }
                return _catsPath;
            }
        } 

        #endregion

        #region 下单撤单命令表
        public ConcurrentDictionary<string, InstructionEntity> CancelInstructionDict = new ConcurrentDictionary<string, InstructionEntity>();
        public ConcurrentDictionary<string, InstructionEntity> AddInstructionDict = new ConcurrentDictionary<string, InstructionEntity>(); 
        #endregion

        #region 资金及持仓表
        DataTable FSetTable = null;
        DataTable PSetTable = null;
        #endregion

        #region 订单表
        public ConcurrentDictionary<string, bool> OrderChange = new ConcurrentDictionary<string, bool>();
        public ConcurrentDictionary<string, StandardOrderEntity> ExceptionOrderDict = new ConcurrentDictionary<string, StandardOrderEntity>();
        public ConcurrentDictionary<string, StandardOrderEntity> StandardOrderDict = new ConcurrentDictionary<string, StandardOrderEntity>(); 
        private ConcurrentDictionary<string, OrdersEntity> OrdersDict = new ConcurrentDictionary<string, OrdersEntity>();//key值为ClientID
        private ConcurrentDictionary<string, OrdersEntity> NoIDOrderDict = new ConcurrentDictionary<string, OrdersEntity>();//key值为OrderID
        private ConcurrentDictionary<string, DateTime> ClientIDTime = new ConcurrentDictionary<string, DateTime>();// key值为ClientID;
        
        
        private ConcurrentQueue<OrdersEntity> noLocalOrdersQueue = new ConcurrentQueue<OrdersEntity>();
        
        private List<string> noLocalOrderNo = new List<string>();

        public DataTable dtOrderNewst = null;
        bool needRefreshOrder = false;
        bool isUpdatingOrder = false;
        #endregion

        #region Properties

        private bool _isStarted = false;
        public bool IsStarted {
            get{
                return _isStarted;
            }
            set{
                _isStarted = value;
                NotifyPropertyChange("IsStarted");
            }
        }

        public Action OnOrderChange { get; set; }

        public Action OnAddInstructionChange { get; set; }

        public Action OnCancelInstructionChange { get; set; }


        private static CATSAdapter _instance;
        public static CATSAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new CATSAdapter();
                        }
                    }
                }
                return _instance;
            }
        }

        private static int _clientID = -1;
        private static string ClientID
        {
            get
            {
                return (_clientID++).ToString();
            }
        }

        private Thread OrderScanThread = null;

        private Thread ASSetScanThread = null;

        private Thread InstrunctionsThread = null;

        private Thread OrderStardThread = null;

        #endregion

        private CATSAdapter()
        { }

        public void Start()
        {

            if (CATS_PATH != null && Directory.Exists(CATS_PATH) && !IsStarted)
            {
                lock (Sync)
                {
                    if (!IsStarted)
                    {
                        Utils.logger.LogInfo("开启扫单线程!");
                        StandardOrderDict.Clear();

                        OrderScanThread = new Thread(OrderScanMain) { IsBackground = true };
                        OrderScanThread.Start();

                        ASSetScanThread = new Thread(new ThreadStart(ASSetScanMain)) { IsBackground = true };
                        ASSetScanThread.Start();

                        InstrunctionsThread = new Thread(new ThreadStart(InstructionMain)) { IsBackground = true };
                        InstrunctionsThread.Start();

                        OrderStardThread = new Thread(new ThreadStart(StandardOrderMain)) { IsBackground = true };
                        OrderStardThread.Start();

                        IsStarted = true;

                        CATSService.Instance.StartService();
                    }
                }
                
            }
            else
            {
                Utils.logger.LogInfo("路径{0}异常导致扫单工具无法正常开始!");
            }
        }

        public void Stop()
        {
            try
            {
                Thread.Sleep(200);
                
                OrderScanThread.Abort();
                ASSetScanThread.Abort();
                InstrunctionsThread.Abort();
                IsStarted = false;
            }
            catch (Exception ex)
            {
                Utils.logger.LogInfo("stop exception, info {0}" + ex.Message);
            }

        }



        public string ClearDataFile(bool isBakFile)
        {
            this.Stop();
            Thread.Sleep(300);
            string ErrorInfo = string.Empty;
            if (Directory.Exists(CATS_PATH))
            {
                KillWealth();

                ErrorInfo = DeleteDBF(isBakFile);
            }
            else
            {
                ErrorInfo = "CATS PATH 配置项对应路径不存在!";
            }
            return ErrorInfo;
        }

        private static string DeleteDBF(bool isBakFile)
        {
            string strReturn = string.Empty;
            Utils.logger.LogInfo("2.开始删除DFB文件");
            var fileNames = new string[] { "asset.dbf", "instructions.dbf", "order_updates.dbf" };

            try
            {
                var dirBak = CATS_PATH + "\\" + DateTime.Today.ToString("yyyyMMdd");
                if (!Directory.Exists(dirBak))
                {
                    var dir = Directory.GetDirectories(CATS_PATH);
                    foreach (var item in dir)
                    {
                        if (!item.Contains("empty_files"))
                        {
                            try
                            {
                                Directory.Delete(item, true);
                            }
                            catch (Exception) { }
                        }
                    }
                    Directory.CreateDirectory(dirBak);
                }

                foreach (var item in fileNames)
                {
                    if (isBakFile)
                    {
                        while (!File.Exists(dirBak + "\\" + item))
                        {
                            try
                            {
                                File.Copy(CATS_PATH + "\\" + item, dirBak + "\\" + item);
                            }
                            catch (Exception) { Thread.Sleep(100); }
                        }
                    }

                    while (File.Exists(CATS_PATH + "\\" + item))
                    {
                        try
                        {
                            File.Delete(CATS_PATH + "\\" + item);
                        }
                        catch (Exception) { Thread.Sleep(100); }
                    }

                    if (File.Exists(CATS_PATH + "\\empty_files\\" + item))
                    {
                        File.Copy(CATS_PATH + "\\empty_files\\" + item, CATS_PATH + "\\" + item);
                    }
                }
            }
            catch (Exception ex)
            {
                strReturn = string.Format("删除逻辑失败, {0}", ex.Message);
            }
           
            return strReturn;
        }

        private void KillWealth()
        {
            try
            {
                Utils.logger.LogInfo("1.开始关闭Wealth CATS");
                Process[] myproc = Process.GetProcesses();
                foreach (Process item in myproc)
                {
                    if (item.ProcessName == "WCATSCS")
                    {
                        item.Kill();
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.logger.LogInfo("wealth cats软件关闭异常！Message:{0}", ex.Message);
            }
        
        }

        #region Instruction Main
        private void InstructionMain()
        {
            int lasRowCount = 0;
            while (true)
            {
                try
                {

                    OledbAdapter o = new OledbAdapter();
                    var dt = OledbAdapter.GetInstruction("select * from instructions.dbf");
                    if (dt != null && dt.Rows.Count > lasRowCount)
                    {
                        bool needRefreshAdd = false;
                        bool needRefreshCancel = false;
                        
                        for (int i = lasRowCount; i < dt.Rows.Count; i++)
                        {
                            RefreshInstructionItem(dt.Rows[i], ref needRefreshAdd, ref needRefreshCancel);
                        }

                        if (_clientID == -1)
                        {
                            int addMax = -1;
                            int canMax = -1;
                            if (AddInstructionDict.Count > 0)
                            {
                                addMax = AddInstructionDict.Values.Max(_ => int.Parse(_.ClientId));
                            }
                            if (CancelInstructionDict.Count > 0)
                            {
                                canMax = CancelInstructionDict.Values.Max(_ => int.Parse(_.ClientId));
                            }
                            _clientID = Math.Max(addMax, canMax) + 1;
                        }
                        if (needRefreshAdd && OnAddInstructionChange != null)
                        {
                            OnAddInstructionChange.Invoke();
                        }
                        //if (needRefreshCancel && OnCancelInstructionChange != null)
                        //{
                        //    OnCancelInstructionChange.Invoke();
                        //}
                        lasRowCount = dt.Rows.Count;

                    }
                    else if (_clientID == -1)
                    {
                        _clientID = 0;
                    }
                }
                catch (Exception ex)
                {
                    Utils.logger.LogInfo("InstructionMain Exception, Message:{0}", ex.Message);
                }

                Thread.Sleep(200);
            }
        }

        private void RefreshInstructionItem(DataRow row ,ref bool needRefreshAdd, ref bool needRefreshCancel)
        {
            var type = row["inst_type"].ToString().Trim();
            var clientID = row["client_id"].ToString().Trim(); ;
            InstructionEntity order = null;
            if (type == "O")
            {
                if (AddInstructionDict.ContainsKey(clientID))
                {
                    order = AddInstructionDict[clientID];
                }
                else
                {
                    order = new InstructionEntity() { ClientId = clientID };
                    AddInstructionDict[clientID] = order;
                    needRefreshAdd = true;
                }
            }
            else if (type == "C")//撤单命令，以订单号为key
            {
                if (CancelInstructionDict.ContainsKey(clientID))
                {
                    order = CancelInstructionDict[clientID];
                }
                else
                {
                    order = new InstructionEntity() { ClientId = clientID };
                    CancelInstructionDict[clientID] = order;
                    needRefreshCancel = true;
                }
            }
            order.OrderNo = row["ord_no"].ToString();
            order.InstType = row["inst_type"].ToString();
            order.AcctType = row["acct_type"].ToString();
            order.Acct = row["acct"].ToString();
            order.Symbol = row["symbol"].ToString();
            order.TradeSide = row["tradeside"].ToString();

            order.OrdQty = int.Parse(row["ord_qty"].ToString());
            order.OrdPrice = decimal.Parse(row["ord_price"].ToString());
            order.OrdType = row["ord_type"].ToString();
            order.OrdParam = row["ord_param"].ToString();
        }

        public bool ExistDBFData()
        {
            var insTable = OledbAdapter.GetInstruction("select * from instructions.dbf");
            if (insTable != null && insTable.Rows.Count > 0)
            {
                return true;
            }
            var ordTable = OledbAdapter.GetDataTable("select * from order_updates.dbf");
            if (ordTable != null && ordTable.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region ASSet Main
        private void ASSetScanMain()
        {
            string table = @"asset.dbf";
            while (true)
            {
                try
                {
                    OledbAdapter o = new OledbAdapter();
                    var dt = OledbAdapter.GetDataTable("select * from " + table);
                    if (FSetTable == null)
                    {
                        FSetTable = dt.Copy();

                    }
                    if (PSetTable == null)
                    {
                        PSetTable = dt.Copy();

                    }

                    PSetTable.Clear();
                    FSetTable.Clear();

                    if (dt.Rows.Count > 0)
                    {
                        //将两种数据分别记录到两个表中
                        foreach (DataRow row in dt.Rows)
                        {
                            var type = row["a_type"].ToString();
                            if (type == "F")
                            {
                                FSetTable.ImportRow(row);
                            }
                            else if (type == "P")
                            {
                                PSetTable.ImportRow(row);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.logger.LogInfo("ASSetScanMain Exception, Message:{0}", ex.Message);
                }

                Thread.Sleep(1000);
            }
        } 
        #endregion

        #region Order Main
        private void OrderScanMain(object obj)
        {
            string table = @"order_updates.dbf";
            int lastOrderNum = 0;
            while (true)
            {
                try
                {
                    var dt = OledbAdapter.GetDataTable("select * from " + table);
                    if (dtOrderNewst ==null || dtOrderNewst.Rows.Count < dt.Rows.Count)
                    {
                        dtOrderNewst = dt;
                        needRefreshOrder = true;
                        isUpdatingOrder = true;
                    }
                    for (int i = lastOrderNum; i < dt.Rows.Count; i++)
                    {
                        UpdateOrderRow(dt.Rows[i]);
                    }
                    lastOrderNum = dt.Rows.Count;
                    isUpdatingOrder = false;
                }
                catch (Exception ex)
                {
                    Utils.logger.LogInfo("OrderScanMain Exception, Message:{0}", ex.Message);
                }

                Thread.Sleep(100);
            }
        }

        private void UpdateOrderRow(DataRow row)
        {
            decimal price = 0;
            decimal.TryParse(row["avg_px"] + "", out price);
            var clientID = row["client_id"].ToString().Trim();
            var qty = (row["ord_qty"] + "").Trim();
            var order_no = row["ord_no"] + "";


            OrdersEntity order = null;
            if (string.IsNullOrEmpty(clientID))
            {

                if (!NoIDOrderDict.ContainsKey(order_no))
                {
                    order = new OrdersEntity() { ord_no = order_no };
                    InitEntityInfo(row, order);
                    NoIDOrderDict[order_no] = order;
                }
                else
                {
                    order = NoIDOrderDict[order_no];
                    if ((string.IsNullOrEmpty(order.ord_qty) || int.Parse(order.ord_qty) == 0) ||
                      (int.Parse(order.ord_qty) > (int.Parse(order.filled_qty) + int.Parse(order.cxl_qty))
                       && (int.Parse(order.filled_qty) < int.Parse(row["filled_qty"].ToString())
                            || int.Parse(order.cxl_qty) < int.Parse(row["cxl_qty"].ToString()))))
                    {
                        InitEntityInfo(row, order);
                    }
                }
            }
            else
            {

                if (OrdersDict.ContainsKey(clientID))
                {
                    order = OrdersDict[clientID];
                    // 订单委托数大于成交数+撤单数 且 订单中成交数或撤单数比数据行中小
                    if ((string.IsNullOrEmpty(order.ord_qty) || int.Parse(order.ord_qty) == 0) ||
                           (int.Parse(order.ord_qty) > (int.Parse(order.filled_qty) + int.Parse(order.cxl_qty))
                            && (int.Parse(order.filled_qty) < int.Parse(row["filled_qty"].ToString())
                                 || int.Parse(order.cxl_qty) < int.Parse(row["cxl_qty"].ToString()))))
                    {
                        InitEntityInfo(row, order);
                    }
                }
                else
                {
                    order = new OrdersEntity() { client_id = clientID.Trim() };
                    OrdersDict[clientID] = order;
                    InitEntityInfo(row, order);
                }


            }
        }

        private static void InitEntityInfo(DataRow row, OrdersEntity order)
        {
            order.acct = row["acct"].ToString().Trim();
            order.acct_type = row["acct_type"].ToString().Trim();
            order.avg_px = row["avg_px"].ToString().Trim();
            order.cats_acct = row["cats_acct"].ToString().Trim();
            order.corr_id = row["corr_id"].ToString().Trim();
            order.corr_type = row["corr_type"].ToString().Trim();
            order.cxl_qty = row["cxl_qty"].ToString().Trim();
            order.err_msg = row["err_msg"].ToString().Trim();
            order.filled_qty = row["filled_qty"].ToString().Trim();
            order.ord_no = row["ord_no"].ToString().Trim();
            order.ord_param = row["ord_param"].ToString().Trim();
            order.ord_px = row["ord_px"].ToString().Trim();
            order.ord_qty = row["ord_qty"].ToString();
            order.ord_status = row["ord_status"].ToString();

            order.ord_type = row["ord_type"].ToString();
            order.symbol = row["symbol"].ToString();
            order.tradeside = row["tradeside"].ToString();
            string time = row["ord_time"].ToString().Trim();
            order.ord_time = string.IsNullOrEmpty(time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : row["ord_time"].ToString();
        } 

        private static void UpdateEntityInfo(DataRow row, OrdersEntity order)
        {
            order.avg_px = row["avg_px"].ToString().Trim();
            order.cxl_qty = row["cxl_qty"] + "";
            order.filled_qty = row["filled_qty"].ToString().Trim();
            order.err_msg = row["err_msg"].ToString().Trim();
            order.ord_status = row["ord_status"].ToString();
            string time = row["ord_time"].ToString().Trim();
            order.ord_time = string.IsNullOrEmpty(time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : row["ord_time"].ToString();
        }
        #endregion

        #region Standard Order Main
        private void StandardOrderMain()
        {
            while (true)
            {
                if (this.OrdersDict.Count > 0)
                {
                    foreach (var item in OrdersDict)
                    {
                        StandardOrderItem(item);
                    }

                    if (OnOrderChange != null && needRefreshOrder && !isUpdatingOrder)
                    {
                        OnOrderChange.Invoke();
                        needRefreshOrder = false;
                    }
                    
                }
                Thread.Sleep(100);
            }
        }

        private void StandardOrderItem(KeyValuePair<string, OrdersEntity> item)
        {
            if (string.IsNullOrWhiteSpace(item.Value.ord_no))
            {
                return;
            }
            try
            {
                if (StandardOrderDict.ContainsKey(item.Key))
                {
                    var so = StandardOrderDict[item.Key];
                    bool isChange = (so.CancelQty != int.Parse(item.Value.cxl_qty)
                        || so.FilledPrice != decimal.Parse(item.Value.avg_px)
                        || so.FilledQty != int.Parse(item.Value.filled_qty)
                        || so.OrderStatus != Entities.CATSTypeInfo.GetOrderStatusDescription(item.Value.ord_status.Trim())
                        || so.ErrMsg != item.Value.err_msg);
                    if (isChange)
                    {
                        so.CancelQty = int.Parse(item.Value.cxl_qty);
                        so.FilledPrice = decimal.Parse(item.Value.avg_px);
                        so.FilledQty = int.Parse(item.Value.filled_qty);
                        so.OrderStatus = Entities.CATSTypeInfo.GetOrderStatusDescription(item.Value.ord_status.Trim());
                        so.ErrMsg = item.Value.err_msg;
                        OrderChange[item.Key] = true;
                    }
                }
                else
                {
                    if (AddInstructionDict.ContainsKey(item.Value.client_id.Trim()))
                    {
                        var sendOrderInfo = AddInstructionDict[item.Value.client_id.Trim()];
                        var standardItem = new StandardOrderEntity()
                        {
                            //部分数据必须从下单信息中取。
                            CancelQty = int.Parse(item.Value.cxl_qty),
                            ClientID = item.Value.client_id.Trim(),
                            FilledPrice = decimal.Parse(item.Value.avg_px),
                            FilledQty = int.Parse(item.Value.filled_qty),
                            Market = Utils.GetMarket(item.Value.symbol),
                            OrderNo = item.Value.ord_no.Trim(),
                            OrderPrice = sendOrderInfo.OrdPrice,
                            OrderQty = sendOrderInfo.OrdQty,
                            OrderStatus = Entities.CATSTypeInfo.GetOrderStatusDescription(item.Value.ord_status.Trim()),
                            StockCode = Utils.GetStockID(sendOrderInfo.Symbol),

                            ErrMsg = item.Value.err_msg,
                            OrderTime = DateTime.Parse(item.Value.ord_time),
                            //TradeSide = int.Parse(item.Value.tradeside),
                            TradeSide = int.Parse(sendOrderInfo.TradeSide),
                            Account = sendOrderInfo.Acct.Trim(),

                        };
                        StandardOrderDict[item.Key] = standardItem;


                        OrderChange[item.Key] = true;
                    }
                    //else
                    //{
                    //    Utils.logger.LogInfo("下单命令中不包含该clientID" + item.Value.ToJson());
                    //}
                }
            }
            catch (Exception ex)
            {
                Utils.logger.LogInfo("CATSAdapter.StandardOrderMain Exception, Message:{0}", ex.Message);
            }
        } 
        #endregion

        public string AddOrder(string account, string market, string stockID, string price, string qty, string tradeSide, out string clientID)
        {
            clientID = ClientID;//应自动生成，根据帐号，每个帐号配置一个自动取的信息。
            try
            {
                ClientIDTime[clientID] = DateTime.Now;
                //string table = @"instructions.dbf";
                
                string symbol = Utils.GetSymbol( market, stockID);
                string ordertype ="0";
                string catsTradeSide = Entities.CATSTypeInfo.GetTradeSide(tradeSide);

                OledbAdapter o = new OledbAdapter();
                //string sql = string.Format("insert into {0} (inst_type, client_id, acct_type, acct, symbol, tradeside, ord_qty, ord_price, ord_type) values('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')", table, "O",
                //    clientID, accType, account, symbol, tradeSide, qty, price, ordertype);
                var count = OledbAdapter.SetAddInstruction(clientID, Acc_Type, account, symbol, catsTradeSide, qty.ToString(), price.ToString(), ordertype);
                //if (count <= 0)
                //{
                //    Utils.logger.LogInfo("下单返回影响行数: {0}", count);
                //}
                return clientID;
                
            }
            catch (Exception ex)
            {
                Utils.logger.LogInfo("InstructionMain Exception, Message:{0}", ex.Message);
                return null;
            }
        }

        public int CancelOrder(string account, string orderNo)
        {
            try
            {
                //string table = @"instructions.dbf";
                string clientID = ClientID;//应自动生成，根据帐号，每个帐号配置一个自动取的信息。

                OledbAdapter o = new OledbAdapter();
                //string sql = string.Format("insert into {0} (inst_type, client_id, acct_type, acct, order_no) values('{1}', '{2}', '{3}', '{4}', '{5}')", table, "1", clientID, Acc_Type, account, orderNo);
                var count = OledbAdapter.SetCancelInstruction(clientID, Acc_Type, account, orderNo);

                return count;
            }
            catch (Exception ex)
            {
                Utils.logger.LogInfo("InstructionMain Exception, Message:{0}", ex.Message);
            }
            return 0;
        }

        internal string QueryOrderItem(string orderNo)
        {
            return this.StandardOrderDict.FirstOrDefault(_=> _.Key == orderNo).ToJson();
        }

        internal string QueryOrdersByClientID(List<string>  lstClientID)
        {
            var item = this.StandardOrderDict.Values.Where(_ => lstClientID.Contains(_.ClientID) && _.OrderTime >= DateTime.Today).ToList();
            return item == null ? "" : item.ToJson();
        }

        internal IEnumerable<StandardOrderEntity> QueryOrdersByOrderNo(List<string> lstOrderNo)
        {
            return this.StandardOrderDict.Values.Where(_ => lstOrderNo.Contains(_.OrderNo)  && _.OrderTime >= DateTime.Today).ToList();
        }

        internal bool CheckOppositeOrder(string account, string stockID, string price, string tradeSide, out string errInfo)
        {
            //1.检查是否有n秒内相反方向的指令。
            //2.检查是否有相反方向的委托（本地委托及外地委托）。
            errInfo = string.Empty;
            int secondNeedCheck = 2;
            
            decimal decPrice = GetDecimal(price);
            var bsFlag = CATSTypeInfo.IsTDXBuySide(tradeSide);

            //1.本地委托
            var opOrder = OrdersDict.Values.FirstOrDefault(_=> _.acct == account
                && Utils.GetStockID(_.symbol) == stockID
                && bsFlag != CATSTypeInfo.IsCATSBuySide(_.tradeside)
                && GetDecimal(_.ord_qty) > 0
                && IsQtyMatch(_.ord_qty, _.filled_qty, _.cxl_qty)
                && IsOppoPriceMatch(tradeSide, decPrice, GetDecimal(_.ord_px))
                ) ;
            if (opOrder != null)
            {
                errInfo = string.Format("已有本地可自成交委托，Client_ID {0}, 委托编号{1}, 买卖方向 {2}, 委托价格{3}, 委托时间{4}", opOrder.client_id, opOrder.ord_no, CATSTypeInfo.GetTradeSideDes(opOrder.tradeside), opOrder.ord_px, opOrder.ord_time);
                return true;
            }

            //2.外地委托
            var NoLocalOrder = NoIDOrderDict.Values.FirstOrDefault(_ => _.acct == account
                && _.symbol.Contains(stockID)
                && bsFlag != CATSTypeInfo.IsCATSBuySide(_.tradeside)
                && IsQtyMatch(_.ord_qty, _.filled_qty, _.cxl_qty)
                && IsOppoPriceMatch(tradeSide, decPrice, GetDecimal(_.ord_px))
                && !ExceptionOrderDict.ContainsKey(_.ord_no.Trim())
                );
            if (NoLocalOrder != null)
            {
                errInfo = string.Format("已有外地可自成交委托， 委托编号{0}, 买卖方向 {1}, 委托价格{2}, 委托数量{3}, 委托时间{4}, 证券代码{5}", NoLocalOrder.ord_no, CATSTypeInfo.GetTradeSideDes(NoLocalOrder.tradeside), NoLocalOrder.ord_px, NoLocalOrder.ord_qty, NoLocalOrder.ord_time, NoLocalOrder.symbol);
                return true;
            }

            //3.本地短期命令，防止命令冲突
            DateTime dtNow = DateTime.Now;
            var nearInstrumentInfo = ClientIDTime.Where(_ => (dtNow - _.Value).TotalSeconds < secondNeedCheck);
            if (nearInstrumentInfo.Count() > 0)
            {
                var insuctionHis = nearInstrumentInfo.Select(_=> _.Key).ToList();
                foreach (var item in insuctionHis)
                {
                    if (AddInstructionDict.ContainsKey(item))
                    {
                        var insMatch = AddInstructionDict[item];
                        if (insMatch.Symbol.Contains(stockID) && CATSTypeInfo.IsCATSBuySide(insMatch.TradeSide) != bsFlag && IsOppoPriceMatch(tradeSide, decPrice, insMatch.OrdPrice))
                        {
                            errInfo = string.Format("已有{0}秒内可自成交的下单命令{1}！", secondNeedCheck, insMatch.ClientId);
                            return true;
                        }
                    }   
                }
            }
     
            return false;
        }

        internal void SetExceptionOrder(StandardOrderEntity exceptionOrder)
        {
            if (!string.IsNullOrWhiteSpace(exceptionOrder.OrderNo))
            {
                ExceptionOrderDict[exceptionOrder.OrderNo] = exceptionOrder;
            }
            else
            {
                Utils.logger.LogInfo("设置异常委托失败, 委托编号为空!, 委托信息 {0}", exceptionOrder);
            }
        }

        /// <summary>
        /// 价格匹配，欲下买入单价格大于已存在卖委托价格，或欲下卖单价格小于已存在买委托价格
        /// </summary>
        /// <param name="tradeSide"></param>
        /// <param name="priceWantAdd"></param>
        /// <param name="priceExist"></param>
        /// <returns></returns>
        private static bool IsOppoPriceMatch(string tradeSide, decimal priceWantAdd, decimal priceExist)
        {
            if (priceExist<= 0)
                return false;

            int bs = int.Parse(tradeSide) % 2;
            return bs == 0 ? priceWantAdd >= priceExist : priceWantAdd <= priceExist;
        }

        private static bool IsQtyMatch(string ord_qty, string ord_fill_qty, string ord_can_qty)
        {
            return GetDecimal(ord_qty)> (GetDecimal(ord_fill_qty) + GetDecimal(ord_can_qty));
        }

        static decimal GetDecimal(string s)
        {
            decimal d;
            decimal.TryParse(s, out d);
            return d;
        }

        #region NotifyPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //根据PropertyChanged事件的委托类，实现PropertyChanged事件：
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
