using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using AASDataServer.Service;
using System.ComponentModel;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using AASDataServer.ViewModel;
using AASDataServer.Server;
using AASDataServer.DataAdapter.TDF;
using AASDataServer.DataAdapter;
using AASDataServer.Model;
using AASDataServer.Manager;

namespace AASDataServer.View
{
    /// <summary>
    /// WndMain.xaml 的交互逻辑
    /// </summary>
    public partial class WndMain : Window
    {
        private Mutex _runonce;
        private DispatcherTimer _timer;
        private DataServerDataContext _context;
        private DataSourceServer _server;
        private WindowLogger _logger;

        private BackgroundWorker _dataSourceWorker;
        private BackgroundWorker _serverWorker;

        private bool _isStartServer;
        private bool _isStartDataSource;

        public WndMain()
        {
            InitializeComponent();

            _context = new DataServerDataContext();
            this.DataContext = _context;

            _logger = UnityContainerHost.Container.Resolve<IWindowLogger>() as WindowLogger;
            if (_logger != null)
            {
                _logger.NewLogEvent += NewLogEvent;
                _logger.Info("数据服务器", "程序启动"); 
            }

            _dataSourceWorker = new BackgroundWorker();
            _dataSourceWorker.DoWork += _dataSourceWorker_DoWork;
            _dataSourceWorker.RunWorkerCompleted += _dataSourceWorker_RunWorkerCompleted;

            _serverWorker = new BackgroundWorker();
            _serverWorker.DoWork += _serverWorker_DoWork;
            _serverWorker.RunWorkerCompleted += _serverWorker_RunWorkerCompleted;

            _isStartDataSource = false;
            _isStartServer = false;
        }

        void NewLogEvent(LogMessage obj)
        {
            Action<WndMain, LogMessage> updateAction = new Action<WndMain, LogMessage>(UpdateLog);
            this.Dispatcher.BeginInvoke(updateAction, this, obj);  
        }

        void UpdateLog(WndMain main, LogMessage msg)
        {
            DataServerDataContext context = main.DataContext as DataServerDataContext;
            if (context != null)
            {
                Run log = new Run(msg.Time.ToShortTimeString() + " " + msg.Sender + " : " + msg.Message);
                switch(msg.Type)
                {
                    case LogMessageType.INFO:
                        log.Foreground = Brushes.Black;
                        break;
                    case LogMessageType.WARN:
                        log.Foreground = Brushes.Blue;
                        break;
                    case LogMessageType.ERROR:
                        log.Foreground = Brushes.Red;
                        break;
                }

                context.Logs.Insert(0, log);
                if (context.Logs.Count > 1000)
                {
                    context.Logs.RemoveAt(context.Logs.Count - 1);
                }
            }
        }

        void _serverWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_server != null)
            {
                _isStartServer = true;
                e.Result = _server.Start();
            }
            else {
                e.Result = 1;
            }
        }

        void _serverWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int rst = (int)e.Result;
            if (rst == 0)
            {
                cbServer_DataType.IsEnabled = false;
            }
            _isStartServer = false;
        }

        void _dataSourceWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_server != null)
            {
                _isStartDataSource = true;
                
                //if (_context.DataType == 0)
                //{
                //    IDataAdapter ds = new TDFDataAdapter();
                //    ds.Setting = SettingManager.GetInstance.RealTimeAdapterSetting;
                //    _server.Init(ds);
                //}
                //else if (_context.DataType == 1)
                //{
                //    IDataAdapter ds = new DataAdapter.TDB.TDBDataAdapter();
                //    ds.Setting = SettingManager.GetInstance.HistoryAdapterSetting;
                //    _server.Init(ds);
                //}
                
                e.Result = _server.StartDataSource();
            }
            else {
                e.Result = 1;
            }
        }

        void _dataSourceWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _isStartDataSource = false;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (_server != null)
            {
                if (tbServer.IsSelected)
                {
                    _context.RefreshServer(_server);
                }
                if (tbSubCode.IsSelected)
                {
                    _context.RefreshSubCode(_server);
                }
                if (tbStockCode.IsSelected)
                {
                    _context.RefreshStockCode(_server);
                }
                if (tbClient.IsSelected)
                {
                    _context.RefreshClient(_server);
                }
            }
        }

        private void ViewMain_Closed(object sender, EventArgs e)
        {
        }

        private void ViewMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void ViewMain_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _server = new DataSourceServer();
                
                //设置行情源
                cbServer_DataType.SelectedIndex = 0;
                IDataAdapter ds = new TDFDataAdapter();
                ds.Setting = SettingManager.GetInstance.RealTimeAdapterSetting;
                _server.Init(ds);

                _timer = new DispatcherTimer();
                _timer.Tick += new EventHandler(TimerTick);
                _timer.Interval = TimeSpan.FromMilliseconds(1000);
                _timer.Start();
            }
            catch (Exception ex)
            {
                _logger.Error("系统初始化失败！",ex);
                MessageBox.Show("系统初始化失败：" + ex.Message);
                Close();
            }
        }



        public bool RunOnceCreateAndCheck()
        {
            bool exit;
            _runonce = new Mutex(true, "AASDataServer", out exit);
            if (exit) {
                _runonce.ReleaseMutex();
                return true;
            }
            return false;
        }

        #region 服务器命令

        private void cmdStartServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_isStartServer == true)
            {
                e.CanExecute = false;
                return;
            }

            if (_server != null)
            {
                e.CanExecute = !_server.IsRunning;
            }
            else
            {
                e.CanExecute = false;
            }
            
        }

        private void cmdStartServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _serverWorker.RunWorkerAsync();
        }

        private void cmdStopServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_server != null)
            {
                e.CanExecute = _server.IsRunning;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void cmdStopServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_server != null)
            {
                _server.Stop();
                cbServer_DataType.IsEnabled = true;
            }
        }

        private void cmdClearCache_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_server != null && _server.IsRunning)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void cmdClearCache_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_server != null && _server.DataSource != null)
            {
                _server.DataSource.ClearCache();
            }
        }

        private void cmdStartIceDataServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_server != null && _server.ClientServer != null && _server.ClientServer.IsRunning == false)
            {
                e.CanExecute = true;
            }
            else {
                e.CanExecute = false;
            }
        }

        private void cmdStartIceDataServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_server != null)
            {
                _server.StartIceDataServer();
            }
        }

        private void cmdStopIceDataServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_server != null && _server.ClientServer != null && _server.ClientServer.IsRunning == true)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void cmdStopIceDataServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_server != null)
            {
                _server.StopIceDataServer();
            }
        }

        private void cmdStartPubServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_server != null && _server.PubService != null && _server.PubService.IsRunning == false)
            {
                e.CanExecute = true;
            }
            else {
                e.CanExecute = false;
            }
        }

        private void cmdStartPubServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_server != null && _server.PubService != null)
            {
                _server.PubService.Start();
            }
        }

        private void cmdStopPubServer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_server != null && _server.PubService != null && _server.PubService.IsRunning == true)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void cmdStopPubServer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_server != null && _server.PubService != null)
            {
                _server.PubService.Stop();
            }
        }

        private void cmdStartDataSource_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_isStartDataSource == true)
            {
                e.CanExecute = false;
                return;
            }
            if (_server != null && _server.DataSource != null && _server.DataSource.IsRunning == false)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void cmdStartDataSource_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _dataSourceWorker.RunWorkerAsync();
        }

        private void cmdStopDataSource_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_server != null && _server.DataSource != null && _server.DataSource.IsRunning == true)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void cmdStopDataSource_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (_server != null)
            {
                _server.StopDataSource();
            }
        }

        #endregion

        private void cbServer_DataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_server != null && _server.IsRunning == false)
            {
                if (cbServer_DataType.SelectedIndex == 0)
                {
                    IDataAdapter ds = new TDFDataAdapter();
                    ds.Setting = SettingManager.GetInstance.RealTimeAdapterSetting;
                    _server.Init(ds);
                }
                else if (cbServer_DataType.SelectedIndex == 1)
                {
                    IDataAdapter ds = new TDFDataAdapter();
                    ds.Setting = SettingManager.GetInstance.HistoryAdapterSetting;
                    _server.Init(ds);
                }
            }
        }

        private void mnServerSetting_Click(object sender, RoutedEventArgs e)
        {
            if (_server != null && _server.IsRunning == false)
            {
                WndServerSetting setting = new WndServerSetting();
                setting.Owner = this;
                setting.ShowDialog();
                //重新设置数据源
                if (cbServer_DataType.SelectedIndex == 0)
                {
                    IDataAdapter ds = new TDFDataAdapter();
                    ds.Setting = SettingManager.GetInstance.RealTimeAdapterSetting;
                    _server.Init(ds);
                }
                else if (cbServer_DataType.SelectedIndex == 1)
                {
                    IDataAdapter ds = new TDFDataAdapter();
                    ds.Setting = SettingManager.GetInstance.HistoryAdapterSetting;
                    _server.Init(ds);
                    //IDataAdapter ds = new DataAdapter.TDB.TDBDataAdapter();
                    //ds.Setting = SettingManager.GetInstance.HistoryAdapterSetting;
                    //_server.Init(ds);
                }
            }
            else
            {
                MessageBox.Show("请停止服务器后设置！");
            }
        }




    }
}
