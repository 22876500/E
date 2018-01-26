using AASDataWService.Common;
using AASDataWService.DataModel;
using AASDataWService.Logger;
using AASDataWService.Server;
using Ice;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace AASDataWService
{
    public partial class Service1 : ServiceBase
    {
        private DateTime marketDt;
        private DateTime tranDt;

        private IceDataServer _iceDataServer;
        private Thread _iceDataServerThread;

        private Thread pubMarketThread;
        private Thread subMarketThread;
        private Thread pubTransThread;
        private Thread subTransThread;

        private Thread writeMarketThread;
        private Thread writeTranThread;

        private string ip;
        //private int port;
        private bool writeTxt = false;

        private long dataCount;
        private long tranCount;
        ConcurrentQueue<ZMessage> dataCache;
        ConcurrentQueue<ZMessage> tranCache;

        ConcurrentQueue<ZMessage> marketWriteCache;
        ConcurrentQueue< ZMessage> tranWriteCache;

        //private int subMarketErrCount = 0;
        //private int pubMarketErrCount = 0;
        //private int subTransErrCount = 0;
        //private int pubTransErrCount = 0;

        private ZContext contextPubMarket;
        private ZContext contextSubMarket;
        private ZContext contextSubTran;
        private ZContext contextPubTran;

        private ZSocket socketPubMarket;
        private ZSocket socketSubMarket;
        private ZSocket socketSubTran;
        private ZSocket socketPubTran;

        public long DataCount
        {
            get { return dataCount; }
        }
        public long TranCount
        {
            get { return tranCount; }
        }
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.Instance.Info("服务已启动");
            if (StartIceDataServer())
            {
                this.RunWork();
            }
            else
            {
                OnStop();
            }
        }
        public bool StartIceDataServer()
        {
            try
            {
                if ((_iceDataServer != null && _iceDataServer.IsRunning != true) || _iceDataServer == null)
                {
                    _iceDataServer = new IceDataServer();
                    _iceDataServer.IsRunning = true;  //防止再次启动

                    ParameterizedThreadStart server = new ParameterizedThreadStart(IceDataServerRoutine);
                    _iceDataServerThread = new Thread(server);
                    _iceDataServerThread.IsBackground = true;
                    _iceDataServerThread.Start(_iceDataServer);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("订阅服务启动失败" + ex);
            }

            return false;
        }
        protected void IceDataServerRoutine(object obj)
        {
            int exit = 0;
            try
            {
                IceDataServer server = obj as IceDataServer;
                if (server is IceDataServer)
                {
                    string[] args = new string[2];
                    args[0] = "AASDataWService.exe";
                    args[1] = "";

                    var props = Util.createProperties();
                    props.setProperty("Ice.IPv6", "0");
                    var icData = new InitializationData();
                    icData.properties = props;
                    exit = server.main(args, icData);
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("订阅服务运行出错" + ex);
            }
        }
        protected override void OnStop()
        {
            LogHelper.Instance.Info("服务已停止");
        }

        public void RunWork()
        {
            ip = ConfigMain.GetConfigValue(BaseCommon.CONFIGIP);
            bool outErr = false;
            writeTxt  = bool.TryParse(ConfigMain.GetConfigValue(BaseCommon.CONFIGWRITETXT),out outErr);
            tranCache = new ConcurrentQueue<ZMessage>();
            tranCount = 0;
            dataCache = new ConcurrentQueue<ZMessage>();
            dataCount = 0;

            if (Bind())
            {
                pubMarketThread = new Thread(new ThreadStart(PubMarket)) { IsBackground = true };
                pubMarketThread.Start();

                subMarketThread = new Thread(new ThreadStart(SubMarket)) { IsBackground = true };
                subMarketThread.Start();

                pubTransThread = new Thread(new ThreadStart(PubTransaction)) { IsBackground = true };
                pubTransThread.Start();

                subTransThread = new Thread(new ThreadStart(SubTransaction)) { IsBackground = true };
                subTransThread.Start();
                if (writeTxt)
                {
                    //marketWriteCache = new ConcurrentQueue<ZMessage>();
                    //tranWriteCache = new ConcurrentQueue<ZMessage>();

                    //writeMarketThread = new Thread(new ThreadStart(WriteMarket)) { IsBackground = true };
                    //writeMarketThread.Start();
                    //writeTranThread = new Thread(new ThreadStart(WriteTran)) { IsBackground = true };
                    //writeTranThread.Start();
                }
            }
        }

        private bool Bind()
        {
            try
            {
                contextPubMarket = new ZContext();
                contextSubMarket = new ZContext();
                contextPubTran = new ZContext();
                contextSubTran = new ZContext();

                socketPubMarket = new ZSocket(contextPubMarket, ZSocketType.PUB);
                socketSubMarket = new ZSocket(contextSubMarket, ZSocketType.SUB);
                socketPubTran = new ZSocket(contextPubTran, ZSocketType.PUB);
                socketSubTran = new ZSocket(contextSubTran, ZSocketType.SUB);

                socketPubMarket.Bind(string.Format("tcp://*:{0}", ConfigMain.GetConfigValue(BaseCommon.CONFIGMARKETPUBPORT)));
                LogHelper.Instance.Info("pubMarket数据端口：" + ConfigMain.GetConfigValue(BaseCommon.CONFIGMARKETPUBPORT));
                //socketPubMarket.SendTimeout = TimeSpan.FromSeconds(60);

                //socketSubMarket.ReceiveTimeout = TimeSpan.FromSeconds(60);
                //socketSubMarket.ReconnectInterval = TimeSpan.FromSeconds(60);
                socketSubMarket.Connect(string.Format("tcp://{0}:{1}", ip, ConfigMain.GetConfigValue(BaseCommon.CONFIGMARKETSUBPORT)));
                LogHelper.Instance.Info("SubMarket数据端口：" + ip + ":" + ConfigMain.GetConfigValue(BaseCommon.CONFIGMARKETSUBPORT));
                socketSubMarket.SubscribeAll();


                socketPubTran.Bind(string.Format("tcp://*:{0}", ConfigMain.GetConfigValue(BaseCommon.CONFIGTRANSPUBPORT)));
                LogHelper.Instance.Info("pubTransaction数据端口：" + ConfigMain.GetConfigValue(BaseCommon.CONFIGTRANSPUBPORT));
                //socketPubTran.SendTimeout = TimeSpan.FromSeconds(60);

                //socketSubTran.ReceiveTimeout = TimeSpan.FromSeconds(60);
                //socketSubTran.ReconnectInterval = TimeSpan.FromSeconds(60);
                socketSubTran.Connect(string.Format("tcp://{0}:{1}", ip, ConfigMain.GetConfigValue(BaseCommon.CONFIGTRANSSUBPORT)));
                LogHelper.Instance.Info("SubTransaction数据端口：" + ip + ":" + ConfigMain.GetConfigValue(BaseCommon.CONFIGTRANSSUBPORT));
                socketSubTran.SubscribeAll();

                ////socketSubMarket.TcpKeepAlive = TcpKeepaliveBehaviour.Enable;
                ////socketPubMarket.TcpKeepAliveInterval = 60 * 1000;

                ////socketPubTran.TcpKeepAlive = TcpKeepaliveBehaviour.Enable;
                ////socketPubTran.TcpKeepAliveInterval = 60 * 1000;
            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("异常：" + ex.ToString());
                return false;
            }


            return true;
        }
        private void PubMarket()
        {

            while (true)
            {
                try
                {
                    //LogHelper.Instance.Info("1");
                    ZMessage msg;
                    if (dataCache.Count > 0 && dataCache.TryDequeue(out msg))
                    {
                        //LogHelper.Instance.Info("1");
                        ZError error;
                        bool flag = socketPubMarket.Send(msg, out error);
                        if (!flag)
                        {
                            LogHelper.Instance.Info(error.ToString());
                        }
                        //else
                        //{
                        //    marketWriteCache.Enqueue(msg);
                        //}
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }

                catch (System.Exception ex)
                {
                    LogHelper.Instance.Info("异常：" + ex.ToString());
                    continue;
                }
            }

        }
        private void SubMarket()
        {

            while (true)
            {
                try
                {
                    //LogHelper.Instance.Info("行情数据...");
                    ZMessage recData = new ZMessage();
                    ZError error;
                    bool flag = socketSubMarket.ReceiveMessage(ref recData, out error);
                    if (flag)
                    {
                        if (recData.Count == 1)
                        {
                            LogHelper.Instance.Info("行情_心跳检测...");
                            continue;
                        }
                        dataCache.Enqueue(recData);
                    }
                    else
                    {
                        LogHelper.Instance.Info(error.ToString());
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.Instance.Info("异常：" + ex.ToString());
                    continue;
                }
            }
        }

        private void SubTransaction()
        {

            while (true)
            {
                try
                {
                    //LogHelper.Instance.Info("逐笔成交...");
                    ZMessage recData = new ZMessage();
                    ZError error;
                    bool flag = socketSubTran.ReceiveMessage(ref recData, out error);
                    if (flag)
                    {
                        if (recData.Count == 1)
                        {
                            LogHelper.Instance.Info("逐笔_心跳检测...");
                            continue;
                        }
                        tranCache.Enqueue(recData);
                    }
                    else
                    {
                        LogHelper.Instance.Info(error.ToString());
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.Instance.Info("异常：" + ex.ToString());
                    continue;
                }
            }
        }
        private void PubTransaction()
        {

            while (true)
            {
                try
                {
                    ZMessage msg;
                    //LogHelper.Instance.Info("4");
                    if (tranCache.Count > 0 && tranCache.TryDequeue(out msg))
                    {
                        ZError error;
                        bool flag = socketPubTran.Send(msg, out error);
                        if (!flag)
                        {
                            LogHelper.Instance.Info(error.ToString());
                        }
                        //else
                        //{
                        //    tranWriteCache.Enqueue(msg);
                        //}
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.Instance.Info("异常：" + ex.ToString());
                    continue;
                }
            }

        }
        private void WriteMarket()
        {
            ConcurrentDictionary<string, List<MarketData>> dicMarket = new ConcurrentDictionary<string,List<MarketData>>();
            const int MaxCacheCount = 500;
            int currentCacheCount = 0;
            while (true)
            {
                try
                {
                    //LogHelper.Instance.Info("1");
                    ZMessage msg;
                    if (marketWriteCache.Count > 0 && marketWriteCache.TryDequeue(out msg))
                    {
                        //LogHelper.Instance.Info("1");
                        if (msg == null || msg.Count != 2)
                        {
                            continue;
                        }
                        var marketItem = BaseCommon.DeserializeMessage<MarketData>(msg);
                        if (dicMarket.ContainsKey(marketItem.Code))
                        {
                            dicMarket[marketItem.Code].Add(marketItem);
                        }
                        else
                        {
                            dicMarket.TryAdd(marketItem.Code, new List<MarketData> { marketItem });
                        }
                        currentCacheCount = currentCacheCount + 1;
                        marketDt = DateTime.Now;

                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                    TimeSpan marketPlus = DateTime.Now - marketDt;
                    if (dicMarket.Count>0 && (currentCacheCount > MaxCacheCount || marketPlus.Minutes != 0 || marketPlus.Seconds > 30))
                    {
                        foreach (string code in dicMarket.Keys)
                        {
                            List<MarketData> lstMarket;
                            dicMarket.TryRemove(code, out lstMarket);
                            BaseCommon.WriteMarketTxt(code, lstMarket);
                        }
                    }
                }

                catch (System.Exception ex)
                {
                    LogHelper.Instance.Info("异常：" + ex.ToString());
                    continue;
                }
            }

        }
        private void WriteTran()
        {
            ConcurrentDictionary<string, List<MarketTransaction>> dicTran = new ConcurrentDictionary<string, List<MarketTransaction>>();
            const int MaxCacheCount = 500;
            int currentCacheCount = 0;

            while (true)
            {
                try
                {
                    ZMessage msg;
                    if (tranWriteCache.Count > 0 && tranWriteCache.TryDequeue(out msg))
                    {
                        //LogHelper.Instance.Info("1");
                        if (msg == null || msg.Count != 2)
                        {
                            continue;
                        }
                        var tranItem = BaseCommon.DeserializeMessage<MarketTransaction>(msg);
                        if (dicTran.ContainsKey(tranItem.Code))
                        {
                            dicTran[tranItem.Code].Add(tranItem);
                        }
                        else
                        {
                            dicTran.TryAdd(tranItem.Code, new List<MarketTransaction> { tranItem });
                        }
                        currentCacheCount = currentCacheCount + 1;
                        tranDt = DateTime.Now;

                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                    TimeSpan tranPlus = DateTime.Now - tranDt;
                    if (dicTran.Count > 0 && (currentCacheCount > MaxCacheCount || tranPlus.Minutes != 0 || tranPlus.Seconds > 30))
                    {
                        foreach (string code in dicTran.Keys)
                        {
                            List<MarketTransaction> lstTran;
                            dicTran.TryRemove(code, out lstTran);
                            BaseCommon.WriteTranTxt(code, lstTran);
                            
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.Instance.Info("异常：" + ex.ToString());
                    continue;
                }
            }

        }
    }
}
