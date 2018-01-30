using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;
using System.Collections.Concurrent;
using AASTrader.Model.DataModel;
using DataServerIce;
using Ice;
using Exception = System.Exception;

namespace AASClient.TDFData
{
    public class DataCache
    {
        #region Info
        private const int THREAD_COUNT = 8;
        private const int FLUSH_INTERVAL = 60;

        public const int ServerPort = 38002;
        public const int ICEPort = 38000;
        public static string ServerIP = "";

        #endregion

        #region Members
        bool isMarketSubAll = false;
        bool isTransSubAll = false;
        private Ice.Communicator _ic;
        private DataServantPrx _dataServant;
        private Dictionary<string, StockCode> _stockCodes;

        Thread _checkThread = null;
        Thread _marketThread = null;
        Thread _transThread = null;
        Thread _updateMarketThread = null;
        Thread _updateTranThread = null;

        Thread _decryptMarketThread = null;
        Thread _decryptTranThread = null;

        ConcurrentQueue<ZMessage> QueueMarketMessage = null;
        ConcurrentQueue<ZMessage> QueueTransMessage = null;
        ConcurrentQueue<MarketData> QueueMarketData = null;
        ConcurrentQueue<MarketTransaction> QueueTrans = null;

        ConcurrentQueue<MarketData> QueueMonitorMarket = null;
        ConcurrentQueue<MarketTransaction> QueueMonitorTran = null;

        ConcurrentQueue<string> RequestMarketCodes = null;
        ConcurrentQueue<string> RequestTransCodes = null;
        ConcurrentQueue<string> RequestCodes = null;

        ConcurrentDictionary<string, DateTime> RequestTime = null;

        ConcurrentQueue<string> RemoveMarketeCodes = null;
        ConcurrentQueue<string> RemoveTransCodes = null;

        static object sync = new object();

        List<string> Codes = null;

        ZContext Context = null;

        public Action<MarketData> UpdateMarketData = null;

        public Action<MarketTransaction> UpdateTransaction = null;
        
        public ConcurrentDictionary <string, MarketData> MarketNewDict = null;

        DateTime LastFlushTime;
        #endregion

        #region Properties
        private List<MarketCode> _marketCodeList;
        public List<MarketCode> MarketCodeList
        {
            get
            {
                if (_marketCodeList == null)
                {
                    lock (sync)
                    {
                        if (_marketCodeList == null)
                        {
                            _marketCodeList = new List<MarketCode>();
                        }
                    }
                }
                return _marketCodeList;
            }
        }

        public bool IsConnected { get; set; }

        public bool IsShowPrewarning
        { 
            get 
            {
                return false;
                //return CommonUtils.SpecialTrader == Program.Current平台用户.用户名;
            }
        }

        public int PubDataType { get; set; }
        #endregion

        #region Instance
        private DataCache()
        {
            Codes = new List<string>();
            RequestTime = new ConcurrentDictionary<string, DateTime>();
            RequestMarketCodes = new ConcurrentQueue<string>();
            RequestTransCodes = new ConcurrentQueue<string>();
            RequestCodes = new ConcurrentQueue<string>();

            RemoveMarketeCodes = new ConcurrentQueue<string>();
            RemoveTransCodes = new ConcurrentQueue<string>();

            MarketNewDict = new  ConcurrentDictionary<string,MarketData>();
            _stockCodes = new Dictionary<string, StockCode>();
            Context = ZContext.Create();
            IsConnected = false;

            QueueMarketData = new ConcurrentQueue<MarketData>();
            QueueTrans = new ConcurrentQueue<MarketTransaction>();
            QueueMarketMessage = new ConcurrentQueue<ZMessage>();
            QueueTransMessage = new ConcurrentQueue<ZMessage>();

            QueueMonitorMarket = new ConcurrentQueue<MarketData>();
        }

        private static DataCache _instance;
        public static DataCache GetInstance()
        {
            if (_instance == null)
            {
                lock (sync)
                {
                    if (_instance == null)
                    {
                        _instance = new DataCache();
                    }
                }
            }

            return _instance;
        } 
        #endregion

        #region 订阅买卖盘信息
        private void StartSubMarket()
        {
            if (_marketThread == null)
            {
                lock (sync)
                {
                    if (_marketThread == null)
                    {
                        _marketThread = new Thread(new ThreadStart(MarketThreadMain)) { IsBackground = true };
                        _marketThread.Start();
                    }
                }
            }
        }

        private void MarketThreadMain()
        {
            using (ZSocket subSocket = new ZSocket(Context, ZSocketType.SUB))
            {
                subSocket.Connect(string.Format("tcp://{0}:{1}", ServerIP, ServerPort));//分频道订阅或全部订阅。
                if (isMarketSubAll)
                    subSocket.SubscribeAll();
                else
                {
                    var codeNeedSub = Codes.ToArray();
                    codeNeedSub.ToList().ForEach(item => subSocket.Subscribe(item));
                }
                Program.logger.LogRunning("买卖盘行情订阅开始，行情服务器ip {0}", ServerIP);

                while (true)
                {
                    if (Codes.Count == 0)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        try
                        {
                            if (!isMarketSubAll)
                            {
                                CheckAddMarketSub(subSocket);
                                CheckRemoveMarketCode(subSocket);
                            }
                            ZMessage msg = subSocket.ReceiveMessage();
                            if (msg != null) QueueMarketMessage.Enqueue(msg);
                            this.IsConnected = true;
                            DataSourceConfig.IsUseTDFData = true;
                        }
                        catch (Exception ex)
                        {
                            Program.logger.LogRunning("买卖盘行情接收出错！：{0}", ex.Message);
                        }
                    }
                }
            }
        }

        private void CheckAddMarketSub(ZSocket subSocket)
        {
            if (RequestMarketCodes.Count > 0)
            {
                var code = string.Empty;
                while (RequestMarketCodes.TryDequeue(out code))
                {
                    subSocket.Subscribe(code);
                }
            }
        }

        private void CheckRemoveMarketCode(ZSocket subSocket)
        {
            if (RemoveMarketeCodes.Count > 0)
            {
                var code = string.Empty;
                while (RemoveMarketeCodes.TryDequeue(out code))
                {
                    subSocket.Unsubscribe(code);
                }
            }
        }

        #endregion

        #region 订阅逐笔成交
        private void StartSubTrans()
        {
            if (_transThread == null)
            {
                lock (sync)
                {
                    if (_transThread == null)
                    {
                        _transThread = new Thread(TransThreadMain) { IsBackground = true };
                        _transThread.Start();
                    }
                }
            }
        }

        private void TransThreadMain()
        {
            using (ZSocket subSocket = new ZSocket(Context, ZSocketType.SUB))
            {
                subSocket.Connect(string.Format("tcp://{0}:{1}", ServerIP, ServerPort + 3));

                if (isTransSubAll)
                    subSocket.SubscribeAll();
                else
                {
                    var codesNeedSub = Codes.ToArray();
                    codesNeedSub.ToList().ForEach(item => subSocket.Subscribe(item));
                }

                Program.logger.LogRunning("买卖盘行情订阅开始，行情服务器ip {0}", ServerIP);
                while (true)
                {
                    if (Codes.Count == 0)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        try
                        {
                            if (!isTransSubAll)
                            {
                                CheckAddTransSub(subSocket);
                                CheckRemoveTransSub(subSocket);
                            }
                            ZMessage msg = subSocket.ReceiveMessage();
                            if (msg != null) QueueTransMessage.Enqueue(msg);
                        }
                        catch (Exception ex)
                        {
                            Program.logger.LogRunning("逐笔更新异常记录：{0}", ex.Message);
                        }
                    }
                }
            }
        }

        private void CheckAddTransSub(ZSocket subSocket)
        {
            if (RequestTransCodes.Count > 0)
            {
                var code = string.Empty;
                while (RequestTransCodes.TryDequeue(out code))
                {
                    subSocket.Subscribe(code);
                }
            }
        }

        private void CheckRemoveTransSub(ZSocket subSocket)
        {
            if (RemoveTransCodes.Count > 0)
            {
                var code = string.Empty;
                while (RemoveTransCodes.TryDequeue(out code))
                {
                    subSocket.Unsubscribe(code);
                }
            }
        }

        #endregion

        #region 订阅信息解码
        private void StartDecrypt()
        {
            if (_decryptMarketThread == null)
            {
                _decryptMarketThread = new Thread(() =>
                {
                    ZMessage msg = null;
                    while (true)
                    {
                        if (QueueMarketMessage.TryDequeue(out msg))
                        {
                            var marketItem = DeserializeMessage<MarketData>(msg);
                            if (Codes.Contains(marketItem.Code))
                            {
                                QueueMarketData.Enqueue(marketItem);
                            }

                            if (IsShowPrewarning)
                            {
                                QueueMonitorMarket.Enqueue(marketItem);
                            }
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }
                    }
                }) { IsBackground = true};
                _decryptMarketThread.Start();
            }
            if (_decryptTranThread == null)
            {
                _decryptTranThread = new Thread(new ThreadStart(() => 
                {
                    ZMessage msg = null;
                    while (true)
                    {
                        if (QueueTransMessage.TryDequeue(out msg))
                        {
                            var mt = DeserializeMessage<MarketTransaction>(msg);
                            
                            if (Codes.Contains(mt.Code))
                            {
                                QueueTrans.Enqueue(mt);
                            }

                            if (IsShowPrewarning && isTransSubAll)
                            {
                                QueueMonitorTran.Enqueue(mt);
                            }
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }
                    }
                })) { IsBackground = true };
                _decryptTranThread.Start();
            }
        }

        private T DeserializeMessage<T>(ZMessage msg) where T : class
        {
            T t = null;
            ByteArrayPool _bytePool = new ByteArrayPool();
            byte[] tmp = _bytePool.Malloc((int)(msg[1].Length));
            msg[1].Read(tmp, 0, (int)(msg[1].Length));
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(tmp))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter fmt = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                t = fmt.Deserialize(stream) as T;
            }
            _bytePool.Free(tmp);
            //try
            //{
            //    t = CommonUtils.FromJSON<T>(msg[1].ReadString());
            //}
            //catch (Exception ex)
            //{
            //    Program.logger.LogRunning("反序列化市场数据失败！\r\n  Message:{0}\r\n  StackTrace:{1}", ex.Message, ex.StackTrace);
            //}
            
            msg.Dispose();
            return t;
        }
        #endregion

        #region ICE 接口调用
        protected DataServantPrx GetDataServant()
        {
            try
            {
                lock (this)
                {
                    if (_dataServant != null)
                    {
                        _dataServant.ice_ping();
                        return _dataServant;
                    }

                    if (_ic == null)
                    {

                        InitializationData icData = new InitializationData();
                        Ice.Properties icProp = Util.createProperties();
                        icProp.setProperty("Ice.ACM.Client", "0");
                        icProp.setProperty("Ice.MessageSizeMax", "2097152");//2gb in kb
                        icData.properties = icProp;
                        Communicator ic = Util.initialize(icData);

                        if (ic != null)
                        {
                            _ic = ic;
                        }
                    }

                    if (_ic != null)
                    {
                        string endpoint = string.Format("AASDataServer/DataServant:tcp -h {0} -p {1}", ServerIP, ICEPort);
                        Ice.ObjectPrx obj = _ic.stringToProxy(endpoint);
                        DataServantPrx client = DataServantPrxHelper.checkedCast(obj);
                        if (client == null)
                        {
                            AASClient.Program.logger.LogRunning("DataServerClient：无法获得有效数据服务器接口");
                            return null;
                        }

                        client.ice_ping();
                        _dataServant = client;

                        return _dataServant;
                    }
                }
            }
            catch(Ice.ConnectionRefusedException)
            {
                //计算机积极拒绝，认为未部署鱼头，不进行记录。
                //Program.logger.LogRunning("DataServerClient：数据服务器连接失败\r\n  {0}", ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                _dataServant = null;
                Program.logger.LogInfo(string.Format("DataServerClient：数据服务器连接失败\r\n  Message:{0}\r\n  StackTrace:{1}", ex.Message, ex.StackTrace));
            }
            return null;
        }

        private int SubscribeCodes(string[] codelist)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null && codelist.Length > 0)
                {
                    int retval = prx.SubscribeCodes(AASClient.Program.Current平台用户.用户名, codelist);
                    return retval;
                }
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("DataServerClient：SubscribeCodes调用失败\n{0}", ex.Message);
            }
            return 0;
        }

        private int UnsubscribeCodes(string[] codelist)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    int retval = prx.UnsubscribeCodes(AASClient.Program.Current平台用户.用户名, codelist);
                    return retval;
                }
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("DataServerClient：UnsubscribeCodes调用失败\n{0}", ex.Message);
            }
            return 0;
        }

        private void FlushCodes()
        {
            if (Codes.Count > 0)
            {
                if ((DateTime.Now - this.LastFlushTime).TotalSeconds > FLUSH_INTERVAL)
                {
                    LastFlushTime = DateTime.Now;
                    try
                    {
                        DataServantPrx prx = GetDataServant();
                        if (prx != null)
                        {
                            if (Codes.Count > 0)
                            {
                                prx.FlushCodes(Program.Current平台用户.用户名, Codes.ToArray<string>());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogRunning("DataServerClient：FlushCodes调用失败\n{0}", ex.StackTrace);
                    }
                }
            }
        }

        private List<StockCode> GetStockCodes()
        {
            try
            {
                if (_stockCodes.Count > 0)
                {
                    return _stockCodes.Values.ToList<StockCode>();
                }

                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    DSIceStockCode[] arr;
                    var re = prx.GetStockCodes(out arr);
                    Program.logger.LogRunning("DataServerClient：获取全市场股票代码成功，股票总数：{0}", arr.Length);

                    foreach (var item in arr)
                    {
                        var stockCodeItem = new StockCode(item);

                        _stockCodes.Add(stockCodeItem.Code, stockCodeItem);
                    }

                    return _stockCodes.Values.ToList();
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("DataServerClient：GetStockCodes调用失败\r\n  Message:{0}.\r\n  StackTrace:{1}", ex.Message, ex.StackTrace);
            }
            return new List<StockCode>();
        }

        private List<string> GetVipCodes()
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    Program.logger.LogRunning("开始获取订阅列表,订阅服务器地址：{0},ICE端口：{1}", ServerIP, ICEPort);
                    var re = prx.GetVipCodes();
                    var codes = re.ListFromJSON<string>();
                    Program.logger.LogRunning("已获取订阅列表，股票数：" + codes.Count);
                    return codes;
                }
                else
                {
                    TDFData.DataSourceConfig.IsUseTDFData = false;
                }
            }
            catch (Exception ex)
            {
                TDFData.DataSourceConfig.IsUseTDFData = false;
                Program.logger.LogRunning("DataServerClient：GetStockCodes调用失败\n{0}", ex.StackTrace);
            }
            return null;
        }

        public bool SetSubType(int subType)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    var re = prx.SetSubType(Program.Current平台用户.用户名, subType);
                    Program.logger.LogRunning("DataServerClient：设置订阅模式成功！");
                    return re;
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("DataServerClient：设置订阅模式失败！异常信息：{0}", ex.Message);
            }
            return false;
        }

        public int GetSubType()
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    var re = prx.GetSubType();
                    Program.logger.LogRunning("DataServerClient：获取当前订阅模式成功！");
                    return re;
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("DataServerClient：获取当前订阅模式失败！异常信息：{0}", ex.Message);
            }
            return -1;
        }
        #endregion

        #region Check Add
        /// <summary>
        /// 增加订阅
        /// </summary>
        /// <param name="codes"></param>
        public void AddSub(string codes)
        {
            if (TDFData.DataSourceConfig.IsUseTDFData)
            {
                if (codes.Length > 6)
                {
                    var arrCode = CommonUtils.GetCodes(codes);
                    arrCode.ToList().ForEach(_ => AddSubCode(_));
                }
                else if (!Codes.Contains(codes))
                {
                    AddSubCode(codes);
                }
            }

        }

        List<string> AddCodesCache = new List<string>();
        private void AddSubCode(string item)
        {
            if (CommonUtils.IsCode(item) && !Codes.Contains(item))
            {
                if (DataSourceConfig.IsUseVipCodes && DataSourceConfig.VipCodes.Count == 0)
                {
                    if (!AddCodesCache.Contains(item))
                    {
                        AddCodesCache.Add(item);
                    }
                    return;
                }
                if (DataSourceConfig.IsUseVipCodes && !DataSourceConfig.VipCodes.Contains(item))
                    return;
                lock (Codes)
                {
                    Codes.Add(item);
                }

                RequestCodes.Enqueue(item);
                CheckCodes();
                UpdateData();
            }
        }

        public void CheckCodes()
        {
            if (_checkThread == null)
            {
                lock (sync)
                {
                    if (_checkThread == null)
                    {
                        _checkThread = new Thread(() =>
                        {
                            while (true)
                            {
                                try
                                {
                                    AddSubCheck();
                                    FlushCodes();
                                }
                                catch (Exception) { }

                                Thread.Sleep(300);
                            }


                        }) { IsBackground = true };
                        _checkThread.Start();
                    }
                }
            }
        }

        private void AddSubCheck()
        {
            if (RequestCodes.Count > 0)
            {
                List<string> lstCode = new List<string>();
                string code = null;
                foreach (var item in RequestCodes)
                {
                    if (RequestCodes.TryDequeue(out code))
                    {
                        lstCode.Add(code);
                        RequestMarketCodes.Enqueue(code);
                        RequestTransCodes.Enqueue(code);
                    }
                }

                SubscribeCodes(lstCode.ToArray());
            }
        }

        public void RefreshCodes(List<string> codesHqForm)
        {
            var needRemoveCodes = this.Codes.Except(codesHqForm).ToList();
            
            foreach (var item in needRemoveCodes)
            {
                RemoveMarketeCodes.Enqueue(item);
                RemoveTransCodes.Enqueue(item);
            }
            
            lock (Codes)
            {
                needRemoveCodes.ForEach(item => Codes.Remove(item));
            }

        }
        #endregion

        #region Update Data Invoke
        private void UpdateData()
        {
            if (_updateMarketThread == null)
            {
                lock (sync)
                {
                    if (_updateMarketThread == null)
                    {
                        _updateMarketThread = new Thread(new ThreadStart(UpdateMarketInvoke)) { IsBackground = true };
                        _updateMarketThread.Start();
                    }
                    if (_updateTranThread == null)
                    {
                        _updateTranThread = new Thread(new ThreadStart(UpdateTranInvoke)) { IsBackground = true };
                        _updateTranThread.Start();
                    }
                }
            }
        }

        private void UpdateTranInvoke()
        {
            MarketTransaction mt = null;
            while (true)
            {
                if (QueueTrans.TryDequeue(out mt))
                {
                    if (this.UpdateTransaction != null)
                    {
                        this.UpdateTransaction.Invoke(mt);
                    }
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }

        private void UpdateMarketInvoke()
        {
            while (true)
            {
                MarketData md = null;
                if (QueueMarketData.TryDequeue(out md))
                {
                    MarketNewDict[md.Code] = md;

                    if (this.UpdateMarketData != null)
                        this.UpdateMarketData.Invoke(md);
                }
                else
                {
                    Thread.Sleep(50);
                }
            }
        }
        #endregion

        public void Start(Action onStarted = null)
        {
            if (this.IsShowPrewarning)
            {
                this.isMarketSubAll = true;
                //this.isTransSubAll = true;
                var monitorMarketThread = new Thread[THREAD_COUNT];
                for (int i = 0; i < THREAD_COUNT; i++)
                {
                    monitorMarketThread[i] = new Thread(new ThreadStart(MonitorMarketMain)) { IsBackground = true };
                    monitorMarketThread[i].Start(i);

                    //monitorTransThread[i] = new Thread(new ParameterizedThreadStart(MonitorTransMain)) { IsBackground = true };
                    //monitorTransThread[i].Start();
                }
            }

            WaitCallback waitCallback = o => 
            {
                if (!this.IsConnected)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (SetVipCodes())
                        {

                            StartSubMarket();
                            StartSubTrans();
                            StartDecrypt();
                            GetStockCodes();

                            if (onStarted != null)
                                onStarted.Invoke();
                            break;
                        }
                        else
                        {
                            DataSourceConfig.IsUseTDFData = false;
                            Thread.Sleep(5000);
                        }
                    }
                }
            };
            ThreadPool.QueueUserWorkItem(waitCallback, null);
        }


        private void MonitorMarketMain()
        {
            //int index = (int)o;
            MarketData md = null;
            while (true)
            {
                try
                {
                    if (QueueMonitorMarket.TryDequeue(out md))
                    {
                        foreach (var item in Program.WarningFormulas)
                        {
                            item.Match(md);
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("市场数据预警进程出现异常，当前数据信息：{0}\r\n  Message:{1}\r\n  StackTrace:{2}",  md == null ? "" : md.ToJSON(), ex.Message, ex.StackTrace);
                }
                Thread.Sleep(20);
            }
        }

        //private void MonitorTransMain(object obj)
        //{
        //    int index = (int)obj;
        //    MarketTransaction mt = null;
        //    while (true)
        //    {
        //        try
        //        {
        //            if (QueueMonitorTran.TryDequeue(out mt))
        //            {
        //                //判断是否大单，是否满足同时吃掉三档以上的条件。
        //            }
        //            else
        //            {
        //                Thread.Sleep(50);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Program.logger.LogRunning("监控预警进程{0}出现异常，当前数据信息：{1}\r\n  Message:{2}\r\n  StackTrace:{3}", index, mt == null ? "" : mt.ToJSON(), ex.Message, ex.StackTrace);
        //        }
        //    }
        //}

        private bool SetVipCodes()
        {

            if (TDFData.DataSourceConfig.IsUseVipCodes)
            {
                List<string> vipCodes = GetVipCodes();
                if (vipCodes != null && vipCodes.Count > 0)
                {
                    DataSourceConfig.VipCodes.Clear();
                    DataSourceConfig.VipCodes.AddRange(vipCodes);
                    AddCodesCache.Where(_ => vipCodes.Contains(_)).ToList().ForEach(_ => AddSubCode(_));
                    AddCodesCache.Clear();
                    return true;
                }
            }
            else
            {
                return true;
            }
            
            return false;
        }
    }




}
