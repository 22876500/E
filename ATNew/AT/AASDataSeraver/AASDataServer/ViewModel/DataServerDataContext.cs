using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AASDataServer.Model;
using DataModel;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Unity;
using DataServerIce;
using AASDataServer.Server;
using AASDataServer.Helper;
using AASDataServer.Service;
using AASDataServer.Manager;


namespace AASDataServer.ViewModel
{
    public class DataServerDataContext : AbstractDataContext
    {
        #region 数据状态

        private int _dataType;
        private bool _serverIsRuning;
        private DateTime? _serverBootTime;
        private int _clientCount;
        private int _subCount;
        private long _recvDataCount;
        private long _sendDataCount;
        private long _cacheCount;
        private ObservableCollection<Run> _logs = new ObservableCollection<Run>();

        public int DataType
        {
            get {
                return _dataType;
            }
            set {
                _dataType = value;
                OnPropertyChanged("DataType");
            }
        }

        public Run ServerIsRunning
        {
            get {
                Run r = new Run("已运行");
                r.Foreground = Brushes.Green;
                Run s = new Run("停止运行");
                s.Foreground = Brushes.Red;
                return _serverIsRuning ? r : s; 
            }
        }

        public ImageSource ServerIsRunningImage
        {
            get {
                BitmapImage bi = new BitmapImage();
                // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                bi.BeginInit();
                if (_serverIsRuning == true)
                {
                    bi.UriSource = new Uri(@"/AASDataServer;component/Images/server_start.png", UriKind.RelativeOrAbsolute);
                }
                else {
                    bi.UriSource = new Uri(@"/AASDataServer;component/Images/server_stop.png", UriKind.RelativeOrAbsolute);
                }
                bi.EndInit();

                return bi;
            }
        }

        public string ServerBootTime
        {
            get {
                if (_serverBootTime == null)
                {
                    return "未启动";
                }

                return _serverBootTime.ToString(); 
            }
        }

        public int ClientCount
        {
            get {
                return _clientCount;
            }
            set {
                _clientCount = value;
                OnPropertyChanged("ClientCount");
            }
        }

        public int CodeCount
        {
            get {
                return StockCodeManager.GetInstance.CodeList.Count;
            }
        }

        public int SubCount
        {
            get {
                return _subCount;
            }
            set {
                _subCount = value;
                OnPropertyChanged("SubCount");
            }
        }

        public long RecvDataCount
        {
            get {
                return _recvDataCount;
            }
            set {
                _recvDataCount = value;
                OnPropertyChanged("RecvDataCount");
            }
        }

        public long SendDataCount
        {
            get {
                return _sendDataCount;
            }
            set {
                _sendDataCount = value;
                OnPropertyChanged("SendDataCount");
            }
        }

        public long CacheCount
        {
            get {
                return _cacheCount;
            }
            set {
                _cacheCount = value;
                OnPropertyChanged("CacheCount");
            }
        }

        public ObservableCollection<Run> Logs
        {
            get {
                return _logs;
            }
        }


        #endregion

        #region 订阅列表

        private string _subCodeInput;
        private ObservableCollection<SubCode> _subCodeList = new ObservableCollection<SubCode>();

        public string SubCodeInput
        {
            get {
                return _subCodeInput;
            }
            set {
                _subCodeInput = value;
                OnPropertyChanged("SubCodeInput");
            }
        }

        public ObservableCollection<SubCode> SubCodeList
        {
            get {
                return _subCodeList;
            }
            set {
                _subCodeList = value;
                OnPropertyChanged("SubCodeList");
            }
        }
        #endregion

        #region 股票列表

        private string _stockCodeInput;
        private ObservableCollection<StockCode> _marketCodeList = new ObservableCollection<StockCode>();

        public string StockCodeInput
        {
            get {
                return _stockCodeInput;
            }
            set {
                _stockCodeInput = value;
                OnPropertyChanged("StockCodeInput");
            }
        }

        public ObservableCollection<StockCode> MarketCodeList
        {
            get { return _marketCodeList; }
            set {
                _marketCodeList = value;
                OnPropertyChanged("MarketCodeList");
            }
        }

        #endregion

        #region 客户端列表

        private ObservableCollection<UserClient> _clientList = new ObservableCollection<UserClient>();

        public ObservableCollection<UserClient> ClientList
        {
            get { return _clientList; }
            set {
                _clientList = value;
                OnPropertyChanged("ClientList");
            }
        }

        #endregion

        public DataServerDataContext()
        {
            _dataType = 0;
            _serverIsRuning = false;
            _serverBootTime = null;

        }

        public void RefreshServer(DataSourceServer server)
        {
            _serverIsRuning = server.IsRunning;
            OnPropertyChanged("ServerIsRunning");
            OnPropertyChanged("ServerIsRunningImage");

            if (server.DataService != null)
            {
                ClientCount = server.DataService.Clients.Count;
                SubCount = server.DataService.Codes.Count;
                _serverBootTime = server.BootTime;
                OnPropertyChanged("ServerBootTime");
            }

            if (server.PubService != null)
            {
                SendDataCount = server.PubService.PubCount;
            }

            if (server.DataSource != null)
            {
                RecvDataCount = server.DataSource.RecvDataCount;
                CacheCount = server.DataSource.CacheCount;
            }
        }

        public void RefreshSubCode(DataSourceServer server)
        {
            if (server.DataService != null)
            {
                List<SubCode> codes = server.DataService.Codes.Values.ToList<SubCode>();
                SubCodeList = new ObservableCollection<SubCode>(codes);
            }
        }

        public void RefreshStockCode(DataSourceServer server)
        {
            if (MarketCodeList.Count == 0)
            {
                List<StockCode> codes = StockCodeManager.GetInstance.CodeList;
                MarketCodeList = new ObservableCollection<StockCode>(codes);
            }
        }

        public void RefreshClient(DataSourceServer server)
        { 
            if (server.DataService != null)
            {
                List<UserClient> clients = server.DataService.Clients.Values.ToList<UserClient>();
                ClientList = new ObservableCollection<UserClient>(clients);
            }
            
        }

        public void ServerStart()
        {
            _serverIsRuning = true;
            _serverBootTime = DateTime.Now;

            OnPropertyChanged("ServerIsRunning");
            OnPropertyChanged("ServerIsRunningImage");
            OnPropertyChanged("ServerBootTime");
        }

        public void ServerStop()
        {
            _serverIsRuning = false;
            _serverBootTime = null;

            OnPropertyChanged("ServerIsRunning");
            OnPropertyChanged("ServerIsRunningImage");
            OnPropertyChanged("ServerBootTime");
        }
    }
}
