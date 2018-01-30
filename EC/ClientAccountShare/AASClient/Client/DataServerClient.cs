using AASClient.TDFData;
using AASTrader.Model.DataModel;
using DataModel;
using DataServerIce;
using Ice;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Timers;
using ZeroMQ;
using Exception = System.Exception;
using Timer = System.Timers.Timer;

namespace AASTrader.Client
{
    public class DataServerClient : IClient
    {
        #region 数据客户端
        const int ThreadCount = 4;

        private ConcurrentQueue<byte[]> _marketDataTmp;
        private ConcurrentQueue<byte[]> _marketOrderTmp;
        private ConcurrentQueue<byte[]> _marketOrderQueueTmp;
        private ConcurrentQueue<byte[]> _marketTransactionTmp;

        private Thread _subDataThread;
        //private Thread _subOrderThread;
        //private Thread _subOrderQueueThread;
        private Thread _subTransactionThread;

        private List<string> _subDataCodes;
        private ConcurrentQueue<string> _subAddDataCodes;
        private ConcurrentQueue<string> _subDeleteDataCodes;

        private List<string> _subOrderCodes;
        private ConcurrentQueue<string> _subAddOrderCodes;
        private ConcurrentQueue<string> _subDeleteOrderCodes;

        private List<string> _subOrderQueueCodes;
        private ConcurrentQueue<string> _subAddOrderQueueCodes;
        private ConcurrentQueue<string> _subDeleteOrderQueueCodes;

        private List<string> _subTransactionCodes;
        private ConcurrentQueue<string> _subAddTransactionCodes;
        private ConcurrentQueue<string> _subDeleteTransactionCodes;

        private ByteArrayPool _bytePool;

        private Thread[] _pubThread;

        private ZContext _pubContext;
        private ZSocket _pubSocket;

        private BinaryFormatter _fmt;

        #endregion

        #region 订阅客户端

        private Communicator _ic;
        private DataServantPrx _dataServant;

        private Dictionary<string, List<string>> _subCodes;

        public List<string> SubCodes
        {
            get { return _subCodes.Keys.ToList(); }
        }

        private Dictionary<string, StockCode> _stockCodes;
        private Timer _timer;

        #endregion

        #region 设置及状态

        private string _serverIp;
        private int _dataPort;
        private int _icePort;

        private bool _isConnected;
        private Thread _testConnectedThread;

        public bool IsConnected
        {
            get { return _isConnected; }
        }

        #endregion

        #region 当前本地数据

        private ConcurrentDictionary<string, MarketData> _currentMarketDatas;

        public ConcurrentDictionary<string, MarketData> CurrentMarketDatas
        {
            get { return _currentMarketDatas; }
        }

        private ConcurrentDictionary<string, MarketTransaction> _lastTransactions;

        public ConcurrentDictionary<string, MarketTransaction> LasTransactions
        {
            get { return _lastTransactions; }
        }

        #endregion

        public DataServerClient()
        {
            string ip = AASClient.TDFData.DataCache.ServerIP;
            int icePort = AASClient.TDFData.DataCache.ICEPort;
            int dataPort = AASClient.TDFData.DataCache.ServerPort;

            Init(ip, dataPort, icePort);
        }

        public DataServerClient(string ip, int dataPort, int icePort)
        {
            Init(ip, dataPort, icePort);
        }

        private void Init(string ip, int dataPort, int icePort)
        {
            _bytePool = new ByteArrayPool();

            _marketDataTmp = new ConcurrentQueue<byte[]>();
            _marketOrderTmp = new ConcurrentQueue<byte[]>();
            _marketOrderQueueTmp = new ConcurrentQueue<byte[]>();
            _marketTransactionTmp = new ConcurrentQueue<byte[]>();

            _subCodes = new Dictionary<string, List<string>>();
            _stockCodes = new Dictionary<string, StockCode>();

            _subDataCodes = new List<string>();
            _subOrderCodes = new List<string>();
            _subOrderQueueCodes = new List<string>();
            _subTransactionCodes = new List<string>();

            _subAddDataCodes = new ConcurrentQueue<string>();
            _subAddOrderCodes = new ConcurrentQueue<string>();
            _subAddOrderQueueCodes = new ConcurrentQueue<string>();
            _subAddTransactionCodes = new ConcurrentQueue<string>();

            _subDeleteDataCodes = new ConcurrentQueue<string>();
            _subDeleteOrderCodes = new ConcurrentQueue<string>();
            _subDeleteOrderQueueCodes = new ConcurrentQueue<string>();
            _subDeleteTransactionCodes = new ConcurrentQueue<string>();

            _currentMarketDatas = new ConcurrentDictionary<string, MarketData>();
            _lastTransactions = new ConcurrentDictionary<string, MarketTransaction>();

            _fmt = new BinaryFormatter();

            _isConnected = false;
            _serverIp = ip;
            _dataPort = dataPort;
            _icePort = icePort;
        }

        public int Connect()
        {
            try
            {
                if (_ic == null)
                {

                    InitializationData icData = new InitializationData();
                    Ice.Properties icProp = Util.createProperties();
                    icProp.setProperty("Ice.ACM.Client", "0");
                    icData.properties = icProp;
                    Communicator ic = Util.initialize(icData);

                    if (ic != null)
                    {
                        _ic = ic;
                    }
                }

                _timer = new Timer(5000);
                _timer.Elapsed += Timer_Elapsed;
                _timer.Enabled = true;

                //启动数据接收线程
                _subDataThread = new Thread(new ThreadStart(SubDataRoutine)) { IsBackground = true };
                _subDataThread.Start();

                //_subOrderThread = new Thread(new ThreadStart(SubOrderRoutine)) {IsBackground = true};
                //_subOrderThread.Start();

                //_subOrderQueueThread = new Thread(new ThreadStart(SubOrderQueueRoutine)) { IsBackground = true };
                //_subOrderQueueThread.Start();

                _subTransactionThread = new Thread(new ThreadStart(SubTransactionRoutine)) { IsBackground = true };
                _subTransactionThread.Start();

                _testConnectedThread = new Thread(new ThreadStart(TestConnectedRoutine)) { IsBackground = true };
                _testConnectedThread.Start();

                _pubContext = new ZContext();

                _pubSocket = new ZSocket(_pubContext, ZSocketType.PUB) { SendHighWatermark = 10000 };
                _pubSocket.Bind("tcp://127.0.0.1:1800");

                _pubThread = new Thread[ThreadCount];
                for (int i = 0; i < ThreadCount; i++)
                {
                    _pubThread[i] = new Thread(new ThreadStart(PubRoutine)) { IsBackground = true };
                    _pubThread[i].Start();
                }

                return 0;
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("DataServerClient：数据服务器连接初始化失败！{0}", ex.Message);
            }

            return 1;
        }

        private void TestConnectedRoutine()
        {
            while (true)
            {
                try
                {
                    DataServantPrx prx = GetDataServant();
                    if (prx != null)
                    {
                        prx.ice_ping();
                        _isConnected = true;
                    }
                    else
                    {
                        _isConnected = false;
                    }
                }
                catch (Exception ex)
                {
                    AASClient.Program.logger.LogRunning("ICE通讯测试错误：\n{0}", ex.Message);
                    _isConnected = false;
                }

                Thread.Sleep(3000);
            }
        }

        public int Disconnect()
        {
            return 0;
        }

        private void SubDataRoutine()
        {
            using (ZContext context = new ZContext())
            {
                ZSocket subSocket = new ZSocket(context, ZSocketType.SUB);
                subSocket.Connect(string.Format("tcp://{0}:{1}", _serverIp, _dataPort));
                foreach (string code in _subDataCodes)
                {
                    subSocket.Subscribe(code);
                }
                while (true)
                {
                    if (_subDataCodes.Count > 0)
                    {
                        try
                        {
                            using (ZMessage msg = subSocket.ReceiveMessage())
                            {
                                string code = msg[0].ReadString();
                                byte[] tmp = _bytePool.Malloc((int)(msg[1].Length));
                                msg[1].Read(tmp, 0, (int)(msg[1].Length));
                                _marketDataTmp.Enqueue(tmp);
                            }
                        }
                        catch (Exception ex)
                        {
                            AASClient.Program.logger.LogRunning("DataServerClient：市场数据接收错误！{0}", ex.Message);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }

                    if (_subAddDataCodes.Count > 0)
                    {
                        string code;
                        while (_subAddDataCodes.TryDequeue(out code))
                        {
                            subSocket.Subscribe(code);
                            if (_subDataCodes.Contains(code) == false)
                            {
                                _subDataCodes.Add(code);
                            }
                        }
                    }
                    if (_subDeleteDataCodes.Count > 0)
                    {
                        string code;
                        while (_subDeleteDataCodes.TryDequeue(out code))
                        {
                            subSocket.Unsubscribe(code);
                            if (_subDataCodes.Contains(code))
                            {
                                _subDataCodes.Remove(code);
                            }
                        }
                    }
                }
            }
        }

        private void SubOrderRoutine()
        {
            using (ZContext context = new ZContext())
            {
                ZSocket subSocket = new ZSocket(context, ZSocketType.SUB);
                subSocket.Connect(string.Format("tcp://{0}:{1}", _serverIp, _dataPort + 1));
                //subSocket.ReceiveTimeout = new TimeSpan(0, 1, 0);
                foreach (string code in _subOrderCodes)
                {
                    subSocket.Subscribe(code);
                }
                while (true)
                {
                    if (_subOrderCodes.Count > 0)
                    {
                        try
                        {
                            using (ZMessage msg = subSocket.ReceiveMessage())
                            {
                                string code = msg[0].ReadString();
                                byte[] tmp = _bytePool.Malloc((int)(msg[1].Length));
                                msg[1].Read(tmp, 0, (int)(msg[1].Length));
                                _marketOrderTmp.Enqueue(tmp);
                            }
                        }
                        catch (Exception ex)
                        {
                            AASClient.Program.logger.LogRunning("DataServerClient：委托数据接收错误！{0}", ex.Message);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }

                    if (_subAddOrderCodes.Count > 0)
                    {
                        string code;
                        while (_subAddOrderCodes.TryDequeue(out code))
                        {
                            subSocket.Subscribe(code);
                            if (_subOrderCodes.Contains(code) == false)
                            {
                                _subOrderCodes.Add(code);
                            }
                        }
                    }

                    if (_subDeleteOrderCodes.Count > 0)
                    {
                        string code;
                        while (_subDeleteOrderCodes.TryDequeue(out code))
                        {
                            subSocket.Unsubscribe(code);
                            if (_subOrderCodes.Contains(code))
                            {
                                _subOrderCodes.Remove(code);
                            }
                        }
                    }
                }
            }
        }

        private void SubOrderQueueRoutine()
        {
            using (ZContext context = new ZContext())
            {
                ZSocket subSocket = new ZSocket(context, ZSocketType.SUB);
                subSocket.Connect(string.Format("tcp://{0}:{1}", _serverIp, _dataPort + 2));
                //subSocket.ReceiveTimeout = new TimeSpan(0, 1, 0);
                foreach (string code in _subOrderQueueCodes)
                {
                    subSocket.Subscribe(code);
                }
                while (true)
                {
                    if (_subOrderQueueCodes.Count > 0)
                    {
                        try
                        {
                            using (ZMessage msg = subSocket.ReceiveMessage())
                            {
                                if (msg != null)
                                {
                                    string code = msg[0].ReadString();
                                    byte[] tmp = _bytePool.Malloc((int)(msg[1].Length));
                                    msg[1].Read(tmp, 0, (int)(msg[1].Length));
                                    _marketOrderQueueTmp.Enqueue(tmp);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AASClient.Program.logger.LogRunning("DataServerClient：委托队列接收错误！{0}", ex.Message);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }

                    if (_subAddOrderQueueCodes.Count > 0)
                    {
                        string code;
                        while (_subAddOrderQueueCodes.TryDequeue(out code))
                        {
                            subSocket.Subscribe(code);
                            if (_subOrderQueueCodes.Contains(code) == false)
                            {
                                _subOrderQueueCodes.Add(code);
                            }
                        }
                    }

                    if (_subDeleteOrderQueueCodes.Count > 0)
                    {
                        string code;
                        while (_subDeleteOrderQueueCodes.TryDequeue(out code))
                        {
                            subSocket.Unsubscribe(code);
                            if (_subOrderCodes.Contains(code))
                            {
                                _subOrderCodes.Remove(code);
                            }
                        }
                    }
                }
            }
        }


        private void SubTransactionRoutine()
        {
            using (ZContext context = new ZContext())
            {
                ZSocket subSocket = new ZSocket(context, ZSocketType.SUB);
                subSocket.Connect(string.Format("tcp://{0}:{1}", _serverIp, _dataPort + 3));
                //subSocket.ReceiveTimeout = new TimeSpan(0, 1, 0);
                foreach (string code in _subTransactionCodes)
                {
                    subSocket.Subscribe(code);
                }
                while (true)
                {
                    if (_subTransactionCodes.Count > 0)
                    {
                        try
                        {
                            using (ZMessage msg = subSocket.ReceiveMessage())
                            {
                                if (msg != null)
                                {
                                    string code = msg[0].ReadString();
                                    byte[] tmp = _bytePool.Malloc((int)(msg[1].Length));
                                    msg[1].Read(tmp, 0, (int)(msg[1].Length));
                                    _marketTransactionTmp.Enqueue(tmp);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            AASClient.Program.logger.LogRunning("DataServerClient：成交数据接收错误！{0}", ex.Message);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }


                    if (_subAddTransactionCodes.Count > 0)
                    {
                        string code;
                        while (_subAddTransactionCodes.TryDequeue(out code))
                        {
                            subSocket.Subscribe(code);
                            if (_subTransactionCodes.Contains(code) == false)
                            {
                                _subTransactionCodes.Add(code);
                            }
                        }
                    }

                    if (_subDeleteTransactionCodes.Count > 0)
                    {
                        string code;
                        while (_subDeleteTransactionCodes.TryDequeue(out code))
                        {
                            subSocket.Unsubscribe(code);
                            if (_subTransactionCodes.Contains(code))
                            {
                                _subTransactionCodes.Remove(code);
                            }
                        }
                    }
                }
            }
        }


        private void PubRoutine()
        {
            if (_pubSocket != null)
            {
                while (true)
                {
                    try
                    {
                        byte[] tmp;
                        if (_marketDataTmp.Count > 0 && _marketDataTmp.TryDequeue(out tmp))
                        {
                            using (MemoryStream stream = new MemoryStream(tmp))
                            {
                                MarketData data = _fmt.Deserialize(stream) as MarketData;
                                if (data != null)
                                {
                                    //更新当前市场数据
                                    if (_currentMarketDatas.ContainsKey(data.Code))
                                    {
                                        _currentMarketDatas[data.Code] = data;
                                    }
                                    else
                                    {
                                        _currentMarketDatas.TryAdd(data.Code, data);
                                    }
                                    //发布市场数据
                                    if (_subCodes.ContainsKey(data.Code))
                                    {
                                        using (MemoryStream stream1 = new MemoryStream())
                                        {
                                            _fmt.Serialize(stream1, data);
                                            byte[] td = stream1.ToArray();
                                            using (ZMessage msg = new ZMessage())
                                            {
                                                msg.Add(new ZFrame(data.Code + ":" + "MarketData"));
                                                msg.Add(new ZFrame(td));
                                                lock (_pubSocket)
                                                {
                                                    _pubSocket.Send(msg);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            _bytePool.Free(tmp);
                        }
                        else if (_marketTransactionTmp.Count > 0 && _marketTransactionTmp.TryDequeue(out tmp))
                        {
                            using (MemoryStream stream = new MemoryStream(tmp))
                            {
                                MarketTransaction trans = _fmt.Deserialize(stream) as MarketTransaction;
                                if (trans != null)
                                {
                                    //更新最近成交
                                    if (_lastTransactions.ContainsKey(trans.Code))
                                    {
                                        _lastTransactions[trans.Code] = trans;
                                    }
                                    else
                                    {
                                        _lastTransactions.TryAdd(trans.Code, trans);
                                    }
                                    //发布最新成交
                                    if (_subCodes.ContainsKey(trans.Code))
                                    {
                                        using (MemoryStream stream1 = new MemoryStream())
                                        {
                                            _fmt.Serialize(stream1, trans);
                                            byte[] td = stream1.ToArray();
                                            using (ZMessage msg = new ZMessage())
                                            {
                                                msg.Add(new ZFrame(trans.Code + ":" + "MarketTransaction"));
                                                msg.Add(new ZFrame(td));
                                                lock (_pubSocket)
                                                {
                                                    _pubSocket.Send(msg);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            _bytePool.Free(tmp);
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }
                    }
                    catch (Exception ex)
                    {
                        AASClient.Program.logger.LogRunning("DataServerClient：数据发送错误！{0}", ex.Message);
                    }
                }
            }
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_subCodes.Count > 0)
            {
                try
                {
                    DataServantPrx prx = GetDataServant();
                    if (prx != null)
                    {
                        if (_subCodes.Count > 0)
                        {
                            prx.FlushCodes(AASClient.Program.Current平台用户.用户名, _subCodes.Keys.ToArray<string>());
                        }
                    }
                }
                catch (Exception ex)
                {
                    AASClient.Program.logger.LogRunning("DataServerClient：FlushCodes调用失败", ex.Message);
                }
            }
        }

        public bool IsConnectedTest()
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    prx.ice_ping();
                    return true;
                }
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("DataServerClient：连接测试失败{0}", ex.Message);
            }

            return false;
        }

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

                    if (_ic != null)
                    {
                        string endpoint = string.Format("ZeroMqConnector/DataServant:tcp -h {0} -p {1}", _serverIp, _icePort);
                        ObjectPrx obj = _ic.stringToProxy(endpoint);
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
            catch (Exception ex)
            {
                _dataServant = null;
                AASClient.Program.logger.LogRunning("DataServerClient：数据服务器连接失败\n{0}", ex.Message);
            }

            return null;
        }

        public int SubscribeCodes(string name, string[] codelist)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {

                    List<string> codes = new List<string>();
                    //寻找未注册的code
                    foreach (string code in codelist)
                    {
                        lock (_subCodes)
                        {
                            if (_subCodes.ContainsKey(code) || codes.Contains(code))
                            {
                                continue;
                            }
                        }
                        codes.Add(code);
                    }
                    if (codes.Count > 0)
                    {
                        //添加股票订阅
                        int retval = prx.SubscribeCodes(AASClient.Program.Current平台用户.用户名, codes.ToArray<string>());
                        foreach (string code in codes)
                        {
                            _subAddDataCodes.Enqueue(code);
                            _subAddOrderCodes.Enqueue(code);
                            _subAddOrderQueueCodes.Enqueue(code);
                            _subAddTransactionCodes.Enqueue(code);
                        }

                        //添加索引
                        foreach (string code in codelist)
                        {
                            if (_subCodes.ContainsKey(code))
                            {
                                if (_subCodes[code].Contains(name) == false)
                                {
                                    _subCodes[code].Add(name);
                                }
                            }
                            else
                            {
                                _subCodes.Add(code, new List<string> { name });
                            }
                        }
                        return retval;
                    }
                }
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("DataServerClient：SubscribeCodes调用失败\n{0}", ex.Message);
            }
            return 0;
        }

        public int UnsubscribeCodes(string name, string[] codelist)
        {
            try
            {
                DataServantPrx prx = GetDataServant();
                if (prx != null)
                {
                    List<string> codes = new List<string>();

                    //清除订阅索引
                    foreach (string code in codelist)
                    {
                        if (_subCodes.ContainsKey(code))
                        {
                            if (_subCodes[code].Contains(name))
                            {
                                if (_subCodes[code].Count == 1)
                                {
                                    _subCodes.Remove(code);
                                    codes.Add(code);
                                }
                                else
                                {
                                    _subCodes[code].Remove(name);
                                }
                            }

                        }
                    }

                    //移除股票订阅

                    int retval = prx.UnsubscribeCodes(AASClient.Program.Current平台用户.用户名, codes.ToArray<string>());
                    foreach (string code in codes)
                    {
                        _subDeleteDataCodes.Enqueue(code);
                        _subDeleteOrderCodes.Enqueue(code);
                        _subDeleteOrderQueueCodes.Enqueue(code);
                        _subDeleteTransactionCodes.Enqueue(code);
                    }
                    return retval;
                }
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("DataServerClient：UnsubscribeCodes调用失败\n{0}", ex.Message);
            }
            return 0;
        }

        public List<StockCode> GetStockCodes()
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
                    DSIceStockCode[] codes;
                    int retval = prx.GetStockCodes(out codes);
                    if (retval > 0 && codes != null)
                    {
                        foreach (DSIceStockCode code in codes)
                        {
                            StockCode sc = new StockCode(code);
                            if (_stockCodes.ContainsKey(sc.Code) == false)
                            {
                                _stockCodes.Add(sc.Code, sc);
                            }
                        }
                    }

                    return _stockCodes.Values.ToList<StockCode>();
                }
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("DataServerClient：GetStockCodes调用失败\n{0}", ex.Message);
            }
            return new List<StockCode>();
        }
    }
}
