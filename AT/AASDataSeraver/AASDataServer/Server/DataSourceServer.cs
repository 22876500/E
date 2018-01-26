using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AASDataServer.DataAdapter;
using AASDataServer.DataAdapter.TDF;
using DataModel;
using AASDataServer.Service;
using AASDataServer.Model.Setting;
using Microsoft.Practices.Unity;
using System.Threading;
using AASDataServer.Helper;
using AASDataServer.Manager;
using Ice;
using Exception = System.Exception;

namespace AASDataServer.Server
{
    public class DataSourceServer : IServer
    {
        private object sync = new object();
        private const string Title = "数据服务器";

        private IWindowLogger _logger;
        private IDataAdapter _dataAdapter;
        private IPubService _pubService;
        private IDataService _dataService;

        private IceDataServer _iceDataServer;
        private Thread _iceDataServerThread;

        private DateTime? _bootTime;

        public bool IsRunning
        {
            get {
                if (_dataAdapter != null && _pubService != null)
                {
                    return _dataAdapter.IsRunning && _pubService.IsRunning;
                }
                return false;
            }
        }

        public IDataService DataService
        {
            get { return _dataService; }
        }

        public IDataAdapter DataSource
        {
            get {
                return _dataAdapter;
            }
        }

        public IPubService PubService
        {
            get {
                return _pubService;
            }
        }

        public IceDataServer ClientServer
        {
            get {
                return _iceDataServer;
            }
        }

        public DateTime? BootTime
        {
            get {
                return _bootTime;
            }
        }

        public DataSourceServer()
        {

            IDataAdapter ds = new TDFDataAdapter();
            //IDataAdapter ds = new DataAdapter.TDB.TDBDataAdapter();
            ds.Setting = SettingManager.GetInstance.RealTimeAdapterSetting;

            _logger = UnityContainerHost.Container.Resolve<IWindowLogger>();
            _iceDataServer = new IceDataServer();

            Init(ds);
        }

        public void Init(IDataAdapter ds)
        {
            _dataAdapter = ds;
            _dataAdapter.NewSysEvent += _dataAdapter_NewSysEvent;

            _pubService = UnityContainerHost.Container.Resolve<IPubService>();
            _pubService.DataSource = _dataAdapter;

            _dataService = UnityContainerHost.Container.Resolve<IDataService>();
            _dataService.DataSource = _dataAdapter;
        }

        void _dataAdapter_NewSysEvent(string obj)
        {
            _logger.Warn("数据源服务", obj);
        }


        public int Start()
        {
            try
            {
                StartIceDataServer();
                //启动数据发布服务
                if (_pubService.Start() == 0)
                {
                    //启动数据源服务
                    if (StartDataSource() == 0)
                    {
                        _bootTime = DateTime.Now;
                        return 0;
                    }
                } 
            }
            catch (Exception ex)
            {
                _logger.Error(Title, "启动失败！请查看日志！", ex);
            }

            return 1;
        }

        public int Stop()
        {
            try
            {
                //停止数据源服务
                StopDataSource();
                //停止数据发布服务
                _pubService.Stop();

                StopIceDataServer();

                _bootTime = null;
            }
            catch (Exception ex)
            {
                _logger.Error(Title, "停止失败！请查看日志！", ex);
            }

            return 0;
        }

        public void RestoreDataSource()
        {
            if (_dataService != null && _dataAdapter != null && _dataAdapter.IsRunning)
            {
                if (_dataService.Codes.Count > _dataAdapter.Codes.Count)
                {
                    List<string> codes = _dataService.Codes.Keys.ToList<string>();
                    List<string> subcodes = codes.Except(_dataAdapter.Codes).ToList<string>();
                    if (_dataAdapter.IsRegistVip && !_dataAdapter.IsVIPRegisted)
                    {
                        _dataAdapter.IsVIPRegisted = true;
                        var vipCodes = StockCodeManager.GetInstance.VIPCodes.Except(_dataAdapter.Codes).Except(subcodes);
                        subcodes.AddRange(vipCodes);
                    }
                    if (StockCodeManager.GetInstance.SubCodesType != SubType.None)
                    {
                        var addList = StockCodeManager.GetInstance.GetAddList(StockCodeManager.GetInstance.SubCodesType);
                        subcodes.AddRange(addList.Except(StockCodeManager.GetInstance.VIPCodes).Except(subcodes));
                    }
                    _dataAdapter.RegisterCodes(subcodes);
                }
                else if (_dataAdapter.IsRegistVip && !_dataAdapter.IsVIPRegisted)
                {
                    lock (sync)
                    {
                        if (!_dataAdapter.IsVIPRegisted)
                        {
                            _dataAdapter.IsVIPRegisted = true;
                            List<string> regCodes = StockCodeManager.GetInstance.VIPCodes.Except(_dataAdapter.Codes).ToList();
                            if (StockCodeManager.GetInstance.SubCodesType != SubType.None)
                            {
                                var addList = StockCodeManager.GetInstance.GetAddList(StockCodeManager.GetInstance.SubCodesType);
                                regCodes.AddRange(addList.Except(StockCodeManager.GetInstance.VIPCodes).Except(regCodes));
                            }
                            _dataAdapter.RegisterCodes(regCodes);
                        }
                    }
                }
            }
        }

        public int StartDataSource()
        {
            _logger.Info(Title,  "开始连接数据源！");
            if (_dataAdapter.Start() == 0 && _dataAdapter.IsRunning)
            {
                _logger.Warn(Title, "数据源已连接！");
                _logger.Info(Title, "开始获取代码列表！");
                List<MarketCode> codes;
                int count = _dataAdapter.GetCodeTable(out codes);
                if (count > 0)
                {
                    StockCodeManager.GetInstance.UpdateMarketCodes(codes);
                    _logger.Warn(Title, "获取代码列表成功！");

                    RestoreDataSource();
                    return 0;
                }
                _logger.Error(Title, "代码列表获取失败！");
            }
            _logger.Error(Title, "数据源连接失败！");
            return 1;
        }

        public int StopDataSource()
        {
            if (_dataAdapter.IsRunning)
            {
                _logger.Info(Title, "开始停止数据源！");
                if (_dataAdapter.Stop() == 0)
                {
                    _dataService.ClearAll();
                    _logger.Warn(Title, "数据源停止成功！");
                    return 0;
                }
            }
            _logger.Error(Title, "数据源停止失败！");
            return 1;
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
            catch (Exception ex)
            {
                _logger.Error(Title, "订阅服务启动失败！", ex);
            }

            return false;
        }

        public bool StopIceDataServer()
        {
            try
            {
                if (_iceDataServer != null && _iceDataServer.IsRunning == true)
                {
                    if (_iceDataServer.Ic.isShutdown() == false)
                    {
                        _iceDataServer.Ic.shutdown();
                        Thread.Sleep(1000);
                        _iceDataServer.Ic.destroy();
                        _iceDataServerThread.Abort();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(Title, "订阅服务停止失败！", ex);
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
                    args[0] = "AASDataServer.exe";
                    args[1] = "";

                    var props = Util.createProperties();
                    props.setProperty("Ice.IPv6", "0");
                    var icData = new InitializationData();
                    icData.properties = props;
                    exit = server.main(args, icData);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(Title, "订阅服务运行出错！", ex);
            }
        }
    }
}
