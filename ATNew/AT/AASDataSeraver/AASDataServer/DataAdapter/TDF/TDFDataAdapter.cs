using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using DataModel;
using TDFAPI;
using AASDataServer.Service;
using System.Threading;
using AASDataServer.Helper;
using AASDataServer.Model.Setting;
using AASDataServer.Manager;
using System.Configuration;

namespace AASDataServer.DataAdapter.TDF
{
    public class TDFDataAdapter : IDataAdapter
    {
        private object sync = new object();
        const int THREAD_COUNT = 24;

        private TDFSourceImp _source;
        private long _recvDataCount;
        private List<MarketCode> _codes;
        private List<string> _subCodes;
        private bool _isRunning;
        private HHDataAdapterSetting _config;

        private Thread[] _dataThread;

        public TDFSourceImp TDFSource
        {
            get {
                return _source;
            }
        }


        Boolean? _isAllCodes = null;
        public bool IsAllCodes 
        {
            get 
            {
                if (_isAllCodes == null)
                {
                    lock (sync)
                    {
                        if (_isAllCodes== null)
                        {

                            _isAllCodes = string.Equals(AppSettingsHelper.getString("IsAllCodes"), "true", StringComparison.CurrentCultureIgnoreCase);
                        }
                    }
                }
                return _isAllCodes == true;
            }
        }

        Boolean? _isRegistVip = null;
        public bool IsRegistVip 
        {
            get 
            {
                if (_isRegistVip == null)
                {
                    lock (sync)
                    {
                        if (_isRegistVip == null)
                        {
                            _isRegistVip = string.Equals(AppSettingsHelper.getString("IsRegistVIP"), "true", StringComparison.CurrentCultureIgnoreCase);
                        }
                    }
                }
                return _isRegistVip == true;
            }
        }

        public bool IsVIPRegisted { get; set; }

        public bool IsRunning
        {
            get {
                return _isRunning;
            }
        }

        public long CacheCount
        {
            get
            {
                if (_source != null)
                {
                    return _source.MarketDataCache.Count +
                        _source.OrderCache.Count +
                        _source.OrderQueueCache.Count +
                        _source.TransactionCache.Count;
                }

                return 0;
            }
        }

        public long RecvDataCount
        {
            get { return _recvDataCount; }
        }

        public List<string> Codes
        {
            get { return _subCodes; }
        }

        public HHDataAdapterSetting Setting
        {
            get { return _config; }
            set {
                _config = value;
            }
        }

        public event Action<string> NewSysEvent;

        public event Action<MarketData[]> NewMarketData;

        public event Action<MarketTransaction[]> NewMarketTransction;

        public event Action<MarketOrder[]> NewMarketOrder;

        public event Action<MarketOrderQueue[]> NewMarketOrderQueue;

        public TDFDataAdapter()
        {
            _config = new HHDataAdapterSetting();
            _recvDataCount = 0;
            _subCodes = new List<string>();
            _codes = new List<MarketCode>();

            _dataThread = new Thread[THREAD_COUNT];
            for (int i = 0; i < THREAD_COUNT; i++)
            {
                _dataThread[i] = new Thread(new ParameterizedThreadStart(MarketDataRoutine));
                _dataThread[i].IsBackground = true;
                _dataThread[i].Start(i);
            }

            _isRunning = false;
        }

        private void MarketDataRoutine(object obj)
        {
            int index = (int)obj;
            while (true)
            {
                if (_isRunning == true && _source != null)
                {
                    if (_source.MarketDataCache.Count > 0)
                    {
                        TDFMarketData[] tmd;
                        if (_source.MarketDataCache.TryDequeue(out tmd))
                        {
                            MarketData[] md = new MarketData[tmd.Length];
                            for (int i = 0; i < tmd.Length; i++)
                            {
                                md[i] = new MarketData();
                                md[i].WindCode = tmd[i].WindCode;
                                md[i].Code = tmd[i].Code;
                                md[i].ActionDay = tmd[i].ActionDay;
                                md[i].Time = tmd[i].Time;
                                md[i].Status = tmd[i].Status;
                                md[i].PreClose = tmd[i].PreClose;
                                md[i].Open = tmd[i].Open;
                                md[i].High = tmd[i].High;
                                md[i].Low = tmd[i].Low;
                                md[i].Match = tmd[i].Match;
                                for (int j = 0; j < 10; j++)
                                {
                                    md[i].AskPrice[j] = tmd[i].AskPrice[j];
                                    md[i].AskVol[j] = (int)tmd[i].AskVol[j];
                                    md[i].BidPrice[j] = tmd[i].BidPrice[j];
                                    md[i].BidVol[j] = (int)tmd[i].BidVol[j];
                                }
                                md[i].NumTrades = (int)(tmd[i].NumTrades);
                                md[i].Volume = (int)(tmd[i].Volume);
                                md[i].Turnover = tmd[i].Turnover;
                                md[i].TotalAskVol = (int)(tmd[i].TotalAskVol);
                                md[i].TotalBidVol = (int)(tmd[i].TotalBidVol);
                                md[i].WeightedAvgAskPrice = tmd[i].WeightedAvgAskPrice;
                                md[i].WeightedAvgBidPrice = tmd[i].WeightedAvgBidPrice;
                                md[i].IOPV = tmd[i].IOPV;
                                md[i].YieldToMaturity = tmd[i].YieldToMaturity;
                                md[i].HighLimited = tmd[i].HighLimited;
                                md[i].LowLimited = tmd[i].LowLimited;
                                tmd[i].Prefix.CopyTo(md[i].Prefix, 0);
                                md[i].Syl1 = tmd[i].Syl1;
                                md[i].Syl2 = tmd[i].Syl2;
                                md[i].SD2 = tmd[i].SD2;
                                
                            }
                            lock (this)
                            {
                                _recvDataCount += tmd.Length;
                            }
                            if (NewMarketData != null)
                            {
                                NewMarketData(md);
                            }
                        }
                    }
                    else if (_source.OrderCache.Count > 0)
                    {
                        TDFOrder[] oc;
                        if (_source.OrderCache.TryDequeue(out oc))
                        {
                            MarketOrder[] mo = new MarketOrder[oc.Length];
                            for (int i = 0; i < mo.Length; i++)
                            {
                                mo[i] = new MarketOrder();
                                mo[i].WindCode = oc[i].WindCode;
                                mo[i].Code = oc[i].Code;
                                mo[i].ActionDay = oc[i].ActionDay;
                                mo[i].Time = oc[i].Time;
                                mo[i].Order = oc[i].Order;
                                mo[i].Price = oc[i].Price;
                                mo[i].Volume = oc[i].Volume;
                                mo[i].OrderKind = oc[i].OrderKind;
                                mo[i].FunctionCode = oc[i].FunctionCode;
                                
                            }
                            lock (this)
                            {
                                _recvDataCount += oc.Length;
                            }
                            if (NewMarketOrder != null)
                            {
                                NewMarketOrder(mo);
                            }
                        }
                    }
                    else if (_source.TransactionCache.Count > 0)
                    {
                        TDFTransaction[] t;
                        if (_source.TransactionCache.TryDequeue(out t))
                        {
                            MarketTransaction[] mt = new MarketTransaction[t.Length];
                            for (int i = 0; i < t.Length; i++)
                            {
                                mt[i] = new MarketTransaction();
                                mt[i].WindCode = t[i].WindCode;
                                mt[i].Code = t[i].Code;
                                mt[i].ActionDay = t[i].ActionDay;
                                mt[i].Time = t[i].Time;
                                mt[i].Index = t[i].Index;
                                mt[i].Price = t[i].Price;
                                mt[i].Volume = t[i].Volume;
                                mt[i].Turnover = t[i].Turnover;
                                mt[i].Flag = t[i].BSFlag;
                                mt[i].OrderKind = t[i].OrderKind;
                                mt[i].FunctionCode = t[i].FunctionCode;
                                mt[i].AskOrder = t[i].AskOrder;
                                mt[i].BidOrder = t[i].BidOrder;
                            }
                            lock (this)
                            {
                                _recvDataCount += t.Length;
                            }
                            if (NewMarketTransction != null)
                            {
                                NewMarketTransction(mt);
                            }
                        }
                    }
                    else if (_source.OrderQueueCache.Count > 0)
                    {
                        TDFOrderQueue[] oq;
                        if (_source.OrderQueueCache.TryDequeue(out oq))
                        {
                            MarketOrderQueue[] moq = new MarketOrderQueue[oq.Length];
                            for (int i = 0; i < oq.Length; i++)
                            {
                                moq[i] = new MarketOrderQueue();
                                moq[i].WindCode = oq[i].WindCode;
                                moq[i].Code = oq[i].Code;
                                moq[i].ActionDay = oq[i].ActionDay;
                                moq[i].Time = oq[i].Time;
                                moq[i].Side = oq[i].Side;
                                moq[i].Price = oq[i].Price;
                                moq[i].Orders = oq[i].Orders;
                                moq[i].Items = oq[i].ABItems;
                                oq[i].ABVolume.CopyTo(moq[i].Volume, 0);
                            }
                            lock (this)
                            {
                                _recvDataCount += oq.Length;
                            }
                            if (NewMarketOrderQueue != null)
                            {
                                NewMarketOrderQueue(moq);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                else {
                    Thread.Sleep(1000);
                }
            }
        }

        public int RegisterCodes(List<string> codes)
        {
            if (IsAllCodes)
            {
                return 0;
            }
            if (_isRunning == true && _source != null)
            {
                StringBuilder substring = new StringBuilder();
                List<string> _codes = new List<string>();
                foreach (string code in codes)
                {
                    string wind = StockCodeManager.GetInstance.ToWindCode(code);
                    if (!_subCodes.Contains(wind))
                    {
                        _subCodes.Add(wind);
                        _codes.Add(code);
                        substring.Append(wind + ";");
                    }
                }
                if (substring.Length > 0)
                {
                    string substr = substring.ToString().TrimEnd(';');
                    _source.SetSubscription(substr, SubscriptionType.SUBSCRIPTION_ADD);
                    App.Logger.Info("TDF数据服务：添加订阅:" + substr);
                    return _codes.Count;
                }
            }
            else
            {
                App.Logger.Error("TDF数据服务：未启动！");
            }
            return 0;
        }

        public int DeRegisterCodes(List<string> codes)
        {
            if (IsAllCodes)
            {
                return 0;
            }
            if (_isRunning == true && _source != null)
            {
                StringBuilder desubstring = new StringBuilder();
                List<string> _codes = new List<string>();
                foreach (string code in codes)
                {
                    string wind = StockCodeManager.GetInstance.ToWindCode(code);
                    if (_subCodes.Contains(wind))
                    {
                        _subCodes.Remove(wind);
                        _codes.Add(code);
                        desubstring.Append(wind + ";");
                    }
                }
                if (desubstring.Length > 0)
                {
                    string desubstr = desubstring.ToString().TrimEnd(';');
                    _source.SetSubscription(desubstr, SubscriptionType.SUBSCRIPTION_DEL);
                    App.Logger.Info("TDF数据服务：取消订阅" + desubstr);
                    return _codes.Count;
                }
            }
            else
            {
                App.Logger.Error("TDF数据服务：未启动！");
            }
            return 0;
        }

        public void DeRegisterAll()
        {
            if (_isRunning == true && _source != null)
            {
                StringBuilder desubstring = new StringBuilder();
                foreach (string code in _subCodes)
                {
                    desubstring.Append(code + ";");
                }
                if (desubstring.Length > 0)
                {
                    _source.SetSubscription(desubstring.ToString().TrimEnd(';'), SubscriptionType.SUBSCRIPTION_DEL);
                    App.Logger.Info("TDF数据服务：取消全部订阅！");
                }
            }
            else
            {
                App.Logger.Error("TDF数据服务：未启动！");
            }
        }

        public int GetCodeTable(out List<MarketCode> codes)
        {
            if (_isRunning == true && _source != null)
            {
                int retry = 0;
                while (_source.Codes == null || _source.Codes.Length == 0 || retry < 10)
                {
                    Thread.Sleep(1000);
                    retry++;
                }

                if (_source.Codes != null && _source.Codes.Length > 0)
                {
                    _codes.Clear();
                    for (int i = 0; i < _source.Codes.Length; i++)
                    {
                        TDFCode code = _source.Codes[i];
                        //List<int> allow = new List<int>() { 0x10, 0x11, 0x12};
                        //if (allow.Contains(code.Type))
                        //{
                            MarketCode mc = new MarketCode()
                            {
                                WindCode = code.WindCode,
                                Market = code.Market,
                                Code = code.Code,
                                EnName = code.ENName,
                                CnName = code.CNName,
                                Type = code.Type
                            };
                            _codes.Add(mc);
                        //}
                    }
                    if (NewSysEvent != null)
                    {
                        NewSysEvent(string.Format("获取到{0}个股票代码。", _codes.Count));
                    }
                    App.Logger.Info(string.Format("TDF数据服务：获取到{0}个股票信息。", _codes.Count));
                    codes = _codes;
                    return _codes.Count;
                }
                App.Logger.Error("TDF数据服务：获取股票列表失败！");
            }
            else {
                App.Logger.Error("TDF数据服务：未启动！");
            }
            codes = new List<MarketCode>();
            return 0;
        }

        public void ClearCache()
        {
            if (_source != null)
            {
                _source.ClearCache();
                App.Logger.Info("TDF数据服务：清除缓存成功！");
            }
        }

        public int Start()
        {
            if (_isRunning == true)
            {
                App.Logger.Info("TDF数据服务：已启动！");
                return 1;
            }

            try
            {
                TDFServerInfo[] theServers = new TDFServerInfo[4];
                
                theServers[0] = new TDFServerInfo()
                {
                    Ip = _config.Ip,                                    //服务器Ip
                    Port = _config.Port.ToString(),                     //服务器端口
                    Username = _config.Username,                        //服务器用户名
                    Password = _config.Password,                        //服务器密码
                };
                //theServers[0].Username = "TD1038491003";
                //theServers[0].Password = "37442745";
                //Setting.IsHistory = true;
                theServers[1] = new TDFServerInfo();
                if (_config.Ip == "114.80.154.34")
                {
                    bool isSpec = false;
                    if (_config.Port == 6221)
                    {
                        isSpec = true;
                        theServers[1].Port = "6231";
                    }
                    else if (_config.Port == 6231)
                    {
                        isSpec = true;
                        theServers[1].Port = "6221";
                    }
                    if (isSpec)
                    {
                        theServers[1].Ip = _config.Ip;
                        theServers[1].Username = _config.Username;
                        theServers[1].Password = _config.Password;
                    }
                }
                
                theServers[2] = new TDFServerInfo();
                theServers[3] = new TDFServerInfo();
                
                TDFOpenSetting_EXT openSetting = new TDFOpenSetting_EXT()
                {
                    Servers = theServers,
                    ServerNum = 1,
                    Subscriptions = "600000.sh;000001.sz",              //订阅列表，以 ; 分割的代码列表，例如:if1406.cf;if1403.cf；如果置为空，则全市场订阅
                    Markets = "sh-2;sz-2;",                          //市场列表，以 ; 分割，例如: sh;sz;cf;shf;czc;dce
                    ConnectionID = 1,    //连接ID，标识某个Open调用，跟回调消息中TDFMSG结构nConnectionID字段相同
                    Time = Setting.IsHistory ? -1 : 0,               //请求的时间，格式HHMMSS，为0则请求实时行情，为(uint)-1从头请求
                    TypeFlags = unchecked((uint)(DataTypeFlag.DATA_TYPE_ORDER | DataTypeFlag.DATA_TYPE_TRANSACTION | DataTypeFlag.DATA_TYPE_ORDERQUEUE))   //为0请求所有品种，或者取值为DataTypeFlag中多种类别，比如DATA_TYPE_MARKET | DATA_TYPE_TRANSACTION
                };

                if (IsAllCodes)
                {
                    openSetting.Subscriptions = string.Empty;
                    openSetting.Markets = "SH-2;SZ-2";
                }
                _source = new TDFSourceImp(openSetting);

                _source.NewSysEvent += _source_NewSysEvent;
                TDFERRNO nOpenRet = _source.Open();
                if (nOpenRet == TDFERRNO.TDF_ERR_SUCCESS)
                {
                    App.Logger.Info("TDF数据服务：登陆成功！");
                    _isRunning = true;
                    if (_subCodes.Count > 0)
                    { 
                        //重新注册
                        StringBuilder s = new StringBuilder();
                        foreach (string wind in _subCodes)
                        {
                            s.Append(wind);
                            s.Append(";");
                        }
                        string substring = s.ToString().TrimEnd(new char[] { ';' });
                        _source.SetSubscription(substring, SubscriptionType.SUBSCRIPTION_ADD);
                    }
                }
                else
                {
                    App.Logger.Error(string.Format("TDF数据服务：连接失败，错误代码{0}", nOpenRet));
                    return 2;
                }
            } 
            catch (Exception ex)
            {
                App.Logger.Error(ex);
                App.Logger.Error("TDF数据服务：初始化错误！" + ex.Message);
                return 100;
            }

            return 0;
        }

        void _source_NewSysEvent(string obj)
        {
            if (NewSysEvent != null)
            {
                NewSysEvent(obj);
            }
        }

        public int Stop()
        {
            if (_source != null)
            {
                _source.Close();
                _source.ClearCache();
                _subCodes.Clear();
                App.Logger.Warn("TDF数据服务：断开连接！");
                _isRunning = false;
                _recvDataCount = 0;
            }
            return 0;
        }

    }
}
