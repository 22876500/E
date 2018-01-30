using AASClient.AASServiceReference;
using AASClient.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using System.Text;

namespace AASClient
{
    static class Program
    {
        public static bool IsSinglePoint = true;
        private static System.Threading.Mutex mutex;
        public static bool IsRestart { get; set; }

        public static string Version = "201801070003";//"201711100001";

        public static string Port = "40008";//普通用808,测试用30808
        public static NetTcpBinding NetTcpBinding = new NetTcpBinding(SecurityMode.Message);

        public static EndpointAddress EndpointAddress;
        public static AASServiceReference.AASServiceClient AASServiceClient;
        public static AASClient.AASServiceReference.DbDataSet.平台用户Row Current平台用户;
        public static InstanceContext callbackInstance = new InstanceContext(new ServerCallback());
        public static Logger logger = new Logger();
        public static UIDataSet uiDataSet = new UIDataSet();
        public static AccountDataSet accountDataSet = new AccountDataSet();//帐户配置文件, 用在在本地保存交易员配置

        public static Form mainForm;

        public static List<WarningFormulaOne> WarningFormulas = new List<WarningFormulaOne>();
        public static ConcurrentDictionary<string, ConcurrentQueue<WarningEntity>> Warnings = new ConcurrentDictionary<string, ConcurrentQueue<WarningEntity>>();
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, MarketDataExtend>> MatchedDataCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, MarketDataExtend>>();
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, int>> MatchedCountCache = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();

        #region 交易员变量
        

        public static ConcurrentDictionary<string, string> 证券名称 = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, int> 证券精度 = new ConcurrentDictionary<string, int>();


        public static ConcurrentQueue<Notify> 交易通知 = new ConcurrentQueue<Notify>();


        public static List<PrewarningShowForm> fmPreWarnings;
        #endregion


        #region 交易员风控员共用变量
        public static ShareLimitGroupItem[] ShareLimitGroups = null;
        public static ConcurrentQueue<成交DataTableChanged> 成交表通知 = new ConcurrentQueue<成交DataTableChanged>();
        public static ConcurrentQueue<委托DataTableChanged> 委托表通知 = new ConcurrentQueue<委托DataTableChanged>();
        public static ConcurrentQueue<平台用户DataTableChanged> 平台用户表通知 = new ConcurrentQueue<平台用户DataTableChanged>();
        public static ConcurrentQueue<额度分配DataTableChanged> 额度分配表通知 = new ConcurrentQueue<额度分配DataTableChanged>();
        public static ConcurrentQueue<订单DataTableChanged> 订单表通知 = new ConcurrentQueue<订单DataTableChanged>();
        public static ConcurrentQueue<已平仓订单DataTableChanged> 已平仓订单表通知 = new ConcurrentQueue<已平仓订单DataTableChanged>();
      



        public static string TempZqdm;
        public static AASClient.AASServiceReference.JyDataSet jyDataSet = new JyDataSet();//作为UI 内存数据库  成交 委托
        public static AASClient.AASServiceReference.DbDataSet serverDb = new AASServiceReference.DbDataSet();//作为UI 对应服务器 sql 数据库


        public static string[] HqServer;
        public static ConcurrentDictionary<string, DataTable> HqDataTable = new ConcurrentDictionary<string, DataTable>();//保存行情数据
        public static ConcurrentDictionary<string, DataTable> TransactionDataTable = new ConcurrentDictionary<string, DataTable>();//保存行情数据
        #endregion




        #region 风控员变量
        public static AASClient.AASServiceReference.DbDataSet.订单DataTable 组合订单 = new AASServiceReference.DbDataSet.订单DataTable();


        public static ConcurrentQueue<风控分配DataTableChanged> 风控分配通知 = new ConcurrentQueue<风控分配DataTableChanged>();



      
        public static ConcurrentDictionary<string, JyDataSet.成交DataTable> 交易员成交DataTable = new ConcurrentDictionary<string, JyDataSet.成交DataTable>();
        public static ConcurrentDictionary<string, JyDataSet.委托DataTable> 交易员委托DataTable = new ConcurrentDictionary<string, JyDataSet.委托DataTable>();
        public static ConcurrentDictionary<string, AASClient.AASServiceReference.DbDataSet.平台用户DataTable> 交易员平台用户DataTable = new ConcurrentDictionary<string, AASClient.AASServiceReference.DbDataSet.平台用户DataTable>();
        public static ConcurrentDictionary<string, AASClient.AASServiceReference.DbDataSet.额度分配DataTable> 交易员额度分配DataTable = new ConcurrentDictionary<string, AASClient.AASServiceReference.DbDataSet.额度分配DataTable>();
        public static ConcurrentDictionary<string, AASClient.AASServiceReference.DbDataSet.订单DataTable> 交易员订单DataTable = new ConcurrentDictionary<string, AASClient.AASServiceReference.DbDataSet.订单DataTable>();
        public static ConcurrentDictionary<string, AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable> 交易员已平仓订单DataTable = new ConcurrentDictionary<string, AASClient.AASServiceReference.DbDataSet.已平仓订单DataTable>();
        #endregion
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException; //UI线程异常
            //处理未捕获的异常   
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理非UI线程异常   
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


            NetTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            NetTcpBinding.MaxBufferSize = 2147483647;//在Binding里指定了最大缓存字节数和最大接受字节数，相当于2G的大小
            NetTcpBinding.MaxBufferPoolSize = 2147483647;
            NetTcpBinding.MaxReceivedMessageSize = 2147483647;
            NetTcpBinding.SendTimeout = new TimeSpan(0, 0, 4);
            NetTcpBinding.ReceiveTimeout = new TimeSpan(4, 0, 0);

            NetTcpBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            NetTcpBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
            NetTcpBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            
            NetTcpBinding.ReaderQuotas.MaxStringContentLength = 0xfffffff;
            NetTcpBinding.ReaderQuotas.MaxArrayLength = 0xfffffff;
            NetTcpBinding.ReaderQuotas.MaxBytesPerRead = 0xfffffff;
            NetTcpBinding.ReaderQuotas.MaxDepth = 0xfffffff;
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (IsSinglePoint)
            {
                mutex = new System.Threading.Mutex(true, "AASClient");
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                    return;
                }
            }

            LoginForm LoginForm1 = new LoginForm();
            DialogResult DialogResult1 = LoginForm1.ShowDialog();
            if (DialogResult1!= DialogResult.OK)
            {
                return;
            }

            Program.logger.Init();

            Program.accountDataSet.Load();
            
            StringBuilder ErrInfo = new StringBuilder();


            try
            {
                switch ((角色)Program.Current平台用户.角色)
                {
                    case 角色.超级管理员:
                    case 角色.普通管理员:
                        mainForm = new AdminMainForm();
                        Application.Run(mainForm);
                        break;
                    case 角色.超级风控员:
                    case 角色.普通风控员:
                        mainForm = new RiskControlMainForm();
                        Application.Run(mainForm);
                        break;
                    case 角色.交易员:
                        mainForm = new TradeMainForm();
                        Application.Run(mainForm);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("主界面运行错误：{0}", ex.Message);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                if (mainForm == null)
                {
                    MessageBox.Show(ex.Message + ex.StackTrace);
                }
                else
                {
                    Program.logger.LogRunning(ex.Message + ex.StackTrace);
                }
            }
            else
            {
                Program.logger.LogRunning(e.ExceptionObject.ToString());
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception.InnerException == null ? e.Exception : e.Exception.InnerException;

            if (mainForm == null)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
            else
            {
                Program.logger.LogRunning(ex.Message + ex.StackTrace);
            }
        }

        static string serverIPCache;
        static string userNameCache;
        static string passwordCache;

        public static void LoginInfoCache(string serverIP, string userName, string password)
        {
            serverIPCache = serverIP;
            userNameCache = userName;
            passwordCache = password;
        }

        public static bool AutoReLogin()
        {
            try
            {
                if (Program.AASServiceClient != null)
                {
                    Program.AASServiceClient.Close();
                }
                Program.EndpointAddress = new EndpointAddress(new Uri(string.Format("net.tcp://{0}:{1}/", serverIPCache, Program.Port)), EndpointIdentity.CreateDnsIdentity("localhost"));
                
                Program.AASServiceClient = new AASServiceReference.AASServiceClient(Program.callbackInstance, Program.NetTcpBinding, Program.EndpointAddress);
                Program.AASServiceClient.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                Program.AASServiceClient.ClientCredentials.UserName.UserName = userNameCache;
                Program.AASServiceClient.ClientCredentials.UserName.Password = passwordCache + "\t" + CommonUtils.GetMacAddress();
                Program.Current平台用户 = Program.AASServiceClient.QuerySingleUser(Program.Version)[0];
                return true;
            }
            catch (System.ServiceModel.Security.MessageSecurityException)
            {
                //if (ex.InnerException == null)
                //{
                //    Program.logger.LogRunning("自动登录失败:{0}", ex.Message);
                //}
                //else
                //{
                //    Program.logger.LogRunning("自动登录失败:{0}", ex.InnerException.Message);
                //}
            }
            catch (Exception)
            {
                //Program.logger.LogRunning("自动登录失败:{0}", ex.Message);
            }
            return false;
        }
    }
}
