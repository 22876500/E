using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using log4net;
using ZeroMQ;
using ProtoBuf;
using Microsoft.Practices.Unity;
using DataModel;
using AASDataServer.DataAdapter;
using AASDataServer.Helper;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using AASDataServer.Manager;
using System.Threading;
using System.Collections.Concurrent;
using System.IO.Compression;

namespace AASDataServer.Service
{
    public class PubService : IPubService
    {
        private const string SENDER = "数据发布服务";

        private ZContext _context;
        private string _pubAddress;
        private int _pubPort;
        

        private long _dataCount;
        private long _orderCount;
        private long _orderQueueCount;
        private long _transactionCount;

        /// <summary>
        /// 为客户端提供普通数据流服务
        /// </summary>
        private Thread _dataThread;
        private Thread _orderThread;
        private Thread _orderQueueThread;
        private Thread _transactionThread;
        //private Thread _replyThread;

        private ConcurrentQueue<ZMessage> _dataCache;
        private ConcurrentQueue<ZMessage> _orderCache;
        private ConcurrentQueue<ZMessage> _orderQueueCache; 
        private ConcurrentQueue<ZMessage> _transactionCache;

        /// <summary>
        /// 为数据转发服务器提供压缩的数据流
        /// </summary>
        private Thread _dataThreadGZip;
        private Thread _orderThreadGZip;
        private Thread _orderQueueThreadGZip;
        private Thread _transactionThreadGZip;

        private ConcurrentQueue<ZMessage> _dataCacheGZip;
        private ConcurrentQueue<ZMessage> _orderCacheGZip;
        private ConcurrentQueue<ZMessage> _orderQueueCacheGZip; 
        private ConcurrentQueue<ZMessage> _transactionCacheGZip;

        /// <summary>
        /// 是否转发
        /// </summary>
        private bool _isTransport;

        /// <summary>
        /// 数据是否压缩
        /// </summary>
        private bool _isCompress;

        private bool _isRunning;

        private IDataAdapter _dataAdapter;
        private IWindowLogger _logger;

        private BinaryFormatter _fmt;

        public long DataCount
        {
            get { return _dataCount; }
        }

        public long OrderCount
        {
            get { return _orderCount; }
        }

        public long OrderQueueCount
        {
            get { return _orderQueueCount; }
        }

        public long TransactionCount
        {
            get { return _transactionCount; }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        public long PubCount
        {
            get { return DataCount + OrderCount + TransactionCount + OrderQueueCount; }
        }

        public IDataAdapter DataSource
        {
            get { return _dataAdapter; }
            set {
                _dataAdapter = value;
            }
        }

        public PubService()
        {
            _logger = UnityContainerHost.Container.Resolve<IWindowLogger>();
            _context = new ZContext();
         
            _dataCount = 0;
            _orderCount = 0;
            _orderQueueCount = 0;
            _transactionCount = 0;

            _dataCache = new ConcurrentQueue<ZMessage>();
            _orderCache = new ConcurrentQueue<ZMessage>();
            _orderQueueCache = new ConcurrentQueue<ZMessage>();
            _transactionCache = new ConcurrentQueue<ZMessage>();

            _dataCacheGZip = new ConcurrentQueue<ZMessage>();
            _orderCacheGZip = new ConcurrentQueue<ZMessage>();
            _orderQueueCacheGZip = new ConcurrentQueue<ZMessage>();
            _transactionCacheGZip = new ConcurrentQueue<ZMessage>();

            _fmt = new BinaryFormatter();

            _isRunning = false;
        }

        public int Start()
        {
            try
            {
                if (_dataAdapter == null)
                {
                    _logger.Error(SENDER,"未绑定数据源！");
                    return 1;
                }

                if (_isRunning == true)
                {
                    _logger.Error(SENDER, "不能重复启动！");
                    return 2;
                }

                _dataCount = 0;
                _orderCount = 0;
                _orderQueueCount = 0;
                _transactionCount = 0;

                _pubAddress = SettingManager.GetInstance.DataServer.PubAddress;
                _pubPort = SettingManager.GetInstance.DataServer.PubPort;
                _isTransport = SettingManager.GetInstance.DataServer.IsTransport;
                _isCompress = SettingManager.GetInstance.DataServer.IsCompress;
                

                _dataAdapter.NewMarketData += PubMarketData;
                _dataAdapter.NewMarketOrder += PubMarketOrder;
                _dataAdapter.NewMarketOrderQueue += PubMarketOrderQueue;
                _dataAdapter.NewMarketTransction += PubMarketTransction;


                //如果开启转发则不能进行本地分发，尽量减小cpu负担
                if (_isTransport == true)
                {
                    _dataThreadGZip = new Thread(new ThreadStart(DataPubRoutineGZip)) {IsBackground = true};
                    _dataThreadGZip.Start();

                    _orderThreadGZip = new Thread(new ThreadStart(OrderPubRoutineGZip)) {IsBackground = true};
                    _orderThreadGZip.Start();

                    _orderQueueThreadGZip = new Thread(new ThreadStart(OrderQueuePubRoutineGZip)) {IsBackground = true};
                    _orderQueueThreadGZip.Start();

                    _transactionThreadGZip = new Thread(new ThreadStart(TransactionPubRoutineGZip)) {IsBackground = true};
                    _transactionThreadGZip.Start();
                }
                else
                {
                    _dataThread = new Thread(new ThreadStart(DataPubRoutine)) {IsBackground = true};
                    _dataThread.Start();

                    _orderThread = new Thread(new ThreadStart(OrderPubRoutine)) {IsBackground = true};
                    _orderThread.Start();

                    _orderQueueThread = new Thread(new ThreadStart(OrderQueuePubRoutine)) { IsBackground = true };
                    _orderQueueThread.Start();

                    _transactionThread = new Thread(new ThreadStart(TransactionPubRoutine)) {IsBackground = true};
                    _transactionThread.Start();

                    //_replyThread = new Thread(new ThreadStart(ReplyInfo)) { IsBackground = true };
                    //_replyThread.Start();
                }

                _isRunning = true;
                _logger.Info(SENDER, "已启动！");
                return 0;
            }
            catch (Exception ex)
            {
                _logger.Error(SENDER, "启动失败！", ex);
                return 2;
            }
        }

        public int Stop()
        {
            try
            {
                if (_dataAdapter != null && _isRunning == true)
                {
                    _isRunning = false;

                    _dataCount = 0;
                    _orderCount = 0;
                    _orderQueueCount = 0;
                    _transactionCount = 0;

                    _dataAdapter.NewMarketData -= PubMarketData;
                    _dataAdapter.NewMarketOrder -= PubMarketOrder;
                    _dataAdapter.NewMarketOrderQueue -= PubMarketOrderQueue;
                    _dataAdapter.NewMarketTransction -= PubMarketTransction;

                    if (_isTransport == true)
                    {
                        _dataThreadGZip.Abort();
                        _orderThreadGZip.Abort();
                        _orderQueueThreadGZip.Abort();
                        _transactionThreadGZip.Abort();
                    }
                    else
                    {
                        _dataThread.Abort();
                        _orderThread.Abort();
                        _orderQueueThread.Abort();
                        _transactionThread.Abort();
                        //_replyThread.Abort();
                    }

                    _logger.Warn(SENDER, "已停止！");
                }
                else {
                    _logger.Warn(SENDER, "未运行！");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(SENDER, "停止失败！", ex);
                return 1;
            }

            return 0;
        }

        public void PubMarketData(MarketData[] datas)
        {
            if (datas != null && _isRunning == true)
            {
                if (_isTransport == true)
                {
                    ZMessage messageGZip = new ZMessage();
                    messageGZip.Add(new ZFrame("data"));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                        {
                            _fmt.Serialize(gzip, datas);
                            byte[] tmp = stream.ToArray();
                            messageGZip.Add(new ZFrame(tmp));
                        }
                    }
                    _dataCacheGZip.Enqueue(messageGZip);
                }
                else
                {
                    foreach (MarketData md in datas)
                    {
                        ZMessage message = new ZMessage();
                        using (MemoryStream stream = new MemoryStream())
                        {
                            if (_isCompress == true)
                            {
                                message.Add(new ZFrame(md.Code + ".gzip"));
                                using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                                {
                                    _fmt.Serialize(gzip, md);
                                    byte[] tmp = stream.ToArray();
                                    message.Add(new ZFrame(tmp));
                                }
                            }
                            else
                            {
                                message.Add(new ZFrame(md.Code + ".raw"));
                                _fmt.Serialize(stream, md);
                                byte[] tmp = stream.ToArray();
                                //var tmp = md.ToJSON();
                                message.Add(new ZFrame(tmp));
                            }
                        }
                        _dataCache.Enqueue(message);
                    }
                } 
            }
            
        }

        public void PubMarketOrder(MarketOrder[] orders)
        {
            if (orders != null && _isRunning == true)
            {
                if (_isTransport == true)
                {
                    ZMessage message = new ZMessage();
                    message.Add(new ZFrame("order"));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                        {
                            _fmt.Serialize(gzip, orders);
                            byte[] tmp = stream.ToArray();
                            message.Add(new ZFrame(tmp));
                        }
                    }
                    _orderCacheGZip.Enqueue(message);
                }
                else
                {
                    foreach (MarketOrder mo in orders)
                    {
                        ZMessage message = new ZMessage();
                        using (MemoryStream stream = new MemoryStream())
                        {
                            if (_isCompress == true)
                            {
                                message.Add(new ZFrame(mo.Code + ".gzip"));
                                using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                                {
                                    _fmt.Serialize(gzip, mo);
                                    byte[] tmp = stream.ToArray();
                                    message.Add(new ZFrame(tmp));
                                }
                            }
                            else
                            {
                                message.Add(new ZFrame(mo.Code + ".raw"));
                                _fmt.Serialize(stream, mo);
                                byte[] tmp = stream.ToArray();
                                message.Add(new ZFrame(tmp));
                            }
                        }
                        _orderCache.Enqueue(message);
                    }
                }
            }
        }

        private void PubMarketOrderQueue(MarketOrderQueue[] orderQueues)
        {
            if (orderQueues != null && _isRunning == true)
            {
                if (_isTransport == true)
                {
                    ZMessage message = new ZMessage();
                    message.Add(new ZFrame("orderqueue"));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                        {
                            _fmt.Serialize(gzip, orderQueues);
                            byte[] tmp = stream.ToArray();
                            message.Add(new ZFrame(tmp));
                        }
                    }
                    _orderCacheGZip.Enqueue(message);
                }
                else
                {
                    foreach (MarketOrderQueue moq in orderQueues)
                    {
                        ZMessage message = new ZMessage();
                        using (MemoryStream stream = new MemoryStream())
                        {
                            if (_isCompress == true)
                            {
                                message.Add(new ZFrame(moq.Code + ".gzip"));
                                using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                                {
                                    _fmt.Serialize(gzip, moq);
                                    byte[] tmp = stream.ToArray();
                                    message.Add(new ZFrame(tmp));
                                }
                            }
                            else
                            {
                                message.Add(new ZFrame(moq.Code + ".raw"));
                                _fmt.Serialize(stream, moq);
                                byte[] tmp = stream.ToArray();
                                message.Add(new ZFrame(tmp));
                            }
                        }
                        _orderQueueCache.Enqueue(message);
                    }
                }
            }
        }

        public void PubMarketTransction(MarketTransaction[] transactions)
        {
            if (transactions != null && _isRunning == true)
            {
                if (_isTransport == true)
                {
                    ZMessage message = new ZMessage();
                    message.Add(new ZFrame("transaction"));
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                        {
                            _fmt.Serialize(gzip, transactions);
                            byte[] tmp = stream.ToArray();
                            message.Add(new ZFrame(tmp));
                        }
                    }
                    _transactionCacheGZip.Enqueue(message);
                }
                else
                {
                    foreach (MarketTransaction mt in transactions)
                    {
                        if (mt.Price <= 0 || mt.Volume <= 0)
                        {
                            continue;
                        }
                        ZMessage message = new ZMessage();
                        
                        using (MemoryStream stream = new MemoryStream())
                        {
                            if (_isCompress == true)
                            {
                                message.Add(new ZFrame(mt.Code + ".gzip"));
                                using (GZipStream gzip = new GZipStream(stream, CompressionMode.Compress))
                                {
                                    _fmt.Serialize(gzip, mt);
                                    byte[] tmp = stream.ToArray();
                                    message.Add(new ZFrame(tmp));
                                }
                            }
                            else
                            {
                                message.Add(new ZFrame(mt.Code + ".raw"));
                                _fmt.Serialize(stream, mt);
                                byte[] tmp = stream.ToArray();
                                message.Add(new ZFrame(tmp));
                            }
                        }
                        _transactionCache.Enqueue(message);
                    }
                }
            }
        }

        private void DataPubRoutine()
        {
            using (ZSocket socket = new ZSocket(_context, ZSocketType.PUB))
            {
                socket.Bind(string.Format("tcp://*:{0}", _pubPort));
                while (true)
                {
                    ZMessage msg;
                    if (_dataCache.Count > 0 && _dataCache.TryDequeue(out msg))
                    {
                        socket.Send(msg);
                        _dataCount++;
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        private void OrderPubRoutine()
        {
            using (ZSocket socket = new ZSocket(_context, ZSocketType.PUB))
            {
                socket.Bind(string.Format("tcp://*:{0}", _pubPort + 1));
                while (true)
                {
                    ZMessage msg;
                    if (_orderCache.Count > 0 && _orderCache.TryDequeue(out msg))
                    {
                        socket.Send(msg);
                        _orderCount++;
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        private void OrderQueuePubRoutine()
        {
            using (ZSocket socket = new ZSocket(_context, ZSocketType.PUB))
            {
                socket.Bind(string.Format("tcp://*:{0}", _pubPort + 2));
                while (true)
                {
                    ZMessage msg;
                    if (_orderQueueCache.Count > 0 && _orderQueueCache.TryDequeue(out msg))
                    {
                        socket.Send(msg);
                        _orderQueueCount++;
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        private void TransactionPubRoutine()
        {
            using (ZSocket socket = new ZSocket(_context, ZSocketType.PUB))
            {
                socket.Bind(string.Format("tcp://*:{0}", _pubPort + 3));
                while (true)
                {
                    ZMessage msg;
                    if (_transactionCache.Count > 0 && _transactionCache.TryDequeue(out msg))
                    {
                        socket.Send(msg);
                        _transactionCount++;
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        private void DataPubRoutineGZip()
        {
            using (ZSocket socket = new ZSocket(_context, ZSocketType.PUB))
            {
                socket.Bind(string.Format("tcp://*:{0}", _pubPort + 4));
                while (true)
                {
                    ZMessage msg;
                    if (_dataCacheGZip.Count > 0 && _dataCacheGZip.TryDequeue(out msg))
                    {
                        socket.Send(msg);
                        _dataCount++;
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        private void OrderPubRoutineGZip()
        {
            using (ZSocket socket = new ZSocket(_context, ZSocketType.PUB))
            {
                socket.Bind(string.Format("tcp://*:{0}", _pubPort + 5));
                while (true)
                {
                    ZMessage msg;
                    if (_orderCacheGZip.Count > 0 && _orderCacheGZip.TryDequeue(out msg))
                    {
                        socket.Send(msg);
                        _orderCount++;
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        private void OrderQueuePubRoutineGZip()
        {
            using (ZSocket socket = new ZSocket(_context, ZSocketType.PUB))
            {
                socket.Bind(string.Format("tcp://*:{0}", _pubPort + 6));
                while (true)
                {
                    ZMessage msg;
                    if (_orderQueueCacheGZip.Count > 0 && _orderQueueCacheGZip.TryDequeue(out msg))
                    {
                        socket.Send(msg);
                        _orderQueueCount++;
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        private void TransactionPubRoutineGZip()
        {
            using (ZSocket socket = new ZSocket(_context, ZSocketType.PUB))
            {
                socket.Bind(string.Format("tcp://*:{0}", _pubPort + 7));
                while (true)
                {
                    ZMessage msg;
                    if (_transactionCacheGZip.Count > 0 && _transactionCacheGZip.TryDequeue(out msg))
                    {
                        socket.Send(msg);
                        _transactionCount++;
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
        }

        //private void ReplyInfo()
        //{
        //    using (ZSocket socket = new ZSocket(_context, ZSocketType.REP))
        //    {
        //        socket.Bind(string.Format("tcp://*:{0}", _pubPort + 8));
        //        while (true)
        //        {
        //            using (var msg = socket.ReceiveMessage())
        //            {
        //                ZMessage zMsgReply = new ZMessage();
        //                if (msg != null)
        //                {
        //                    var message = msg[0].ReadString();
        //                    if (message == "get:vipcodes")
        //                    {
        //                        var vipcodes = StockCodeManager.GetInstance.VIPCodes;
        //                        zMsgReply.Add(new ZFrame(string.Join(",", vipcodes)));
        //                    }
        //                    else if (message == "get:stocks")
        //                    {
        //                        var stock = StockCodeManager.GetInstance.CodeList;
        //                        zMsgReply.Add(new ZFrame("Invalid Message:" + message + " " + DateTime.Now.ToString()));
        //                    }
        //                    else
        //                    {
        //                        zMsgReply.Add(new ZFrame("Invalid Message:" + message + " " + DateTime.Now.ToString()));
        //                    }
        //                    msg.Dispose();
        //                }
        //                else
        //                {
        //                    zMsgReply.Add(new ZFrame("无效：消息为空!"));
        //                }

        //                socket.Send(zMsgReply);
        //                zMsgReply.Dispose();
        //            }
        //        }
        //    }
        //}
    }
}
