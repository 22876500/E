using QuotaShareServer.Common;
using QuotaShareServer.Logger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QuotaShareServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ObservableCollection<Loger> ocLog;
        
        ServiceHost ServiceHost1;
        private string port = string.Empty;
        private string serverIP = string.Empty;
        
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            Logger.LogHelper.Instance.Info("启动。。。");
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.dgLogs.ItemsSource = ocLog;
            port = ConfigMain.GetConfigValue("ServicePort", "80");
            serverIP = ConfigMain.GetConfigValue("ServiceIp", "localhost");
            this.Title += string.Format(" [端口:{0}]", port);

            Task task = new Task(InitService);
            task.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var result = MessageBox.Show("额度共享服务端确定要关闭吗? 将无法使用共享券池的额度。", "关闭提示", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
            try
            {
                
                base.OnClosing(e);
            }
            catch { }
        }

        private void InitService()
        {
            LogInfo("启动服务中...");
            
            string uri = string.Format("http://{0}:{1}/", serverIP, port);
            ServiceHost1 = new ServiceHost(typeof(QSDataService), new Uri(uri));
            System.ServiceModel.Channels.Binding binding = GeWStHttpBinding(ServiceHost1, serverIP, port);

            #region Behavior
            ServiceThrottlingBehavior ServiceThrottlingBehavior1 = new ServiceThrottlingBehavior();
            ServiceThrottlingBehavior1.MaxConcurrentInstances = 1;//如果 InstanceContextMode 为 PerSession，则结果值将为总会话数。 如果 InstanceContextMode 为 PerCall，则结果值将为并发调用的数量。默认值Int32.MaxValue
            ServiceThrottlingBehavior1.MaxConcurrentCalls = 1000;//获取或设置一个值，该值指定整个 ServiceHost 中正在处理的最多消息数。 最大并发调用数目，默认16。 这个属于服务限流行为。
            ServiceThrottlingBehavior1.MaxConcurrentSessions = 1000;//限制连接到一个服务的并发会话最大数量 默认值10
            ServiceHost1.Description.Behaviors.Add(ServiceThrottlingBehavior1);

            // Enable metadata publishing.
            ServiceMetadataBehavior ServiceMetadataBehavior1 = new ServiceMetadataBehavior();
            ServiceMetadataBehavior1.HttpGetEnabled = true;
            //ServiceMetadataBehavior1.HttpGetUrl = new Uri(string.Format("http://localhost:{0}/GroupAPI", port));
            ServiceHost1.Description.Behaviors.Add(ServiceMetadataBehavior1);

            ServiceDebugBehavior ServiceDebugBehavior1 = ServiceHost1.Description.Behaviors[typeof(ServiceDebugBehavior)] as ServiceDebugBehavior;
            ServiceDebugBehavior1.IncludeExceptionDetailInFaults = true;
            #endregion

            if (binding is NetTcpBinding)
            {

                #region Credentials
                ServiceHost1.Credentials.ServiceCertificate.Certificate = new X509Certificate2("Group.pfx", "mima1234");
                ServiceHost1.Credentials.ClientCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;

                ////用户名密码认证认证  角色授权
                ServiceHost1.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = System.ServiceModel.Security.UserNamePasswordValidationMode.Custom;
                ServiceHost1.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new QuotaShareDataUserNamePasswordValidator();//用户名密码认证 
                #endregion

                #region Authorization
                ServiceHost1.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles;
                ServiceHost1.Authorization.RoleProvider = new QuotaShareDataRoleProvider();
                ServiceHost1.Authorization.ServiceAuthorizationManager = new QuotaShareDataServiceAuthorizationManager();
                #endregion
            }
            ServiceHost1.Open();
            LogInfo("服务已启动");
        }

        #region Init Bind
        private System.ServiceModel.Channels.Binding GetBasicHttpBind(ServiceHost host)
        {
            BasicHttpBinding Binding = new BasicHttpBinding();
            Binding.MaxBufferPoolSize = 2147483647;
            Binding.MaxReceivedMessageSize = 2147483647;
            Binding.OpenTimeout = new TimeSpan(0, 0, 30);
            Binding.SendTimeout = new TimeSpan(0, 0, 30);
            Binding.ReceiveTimeout = new TimeSpan(4, 0, 0);


            Binding.ReaderQuotas.MaxStringContentLength = 0xfffffff;
            Binding.ReaderQuotas.MaxArrayLength = 0xfffffff;
            Binding.ReaderQuotas.MaxBytesPerRead = 0xfffffff;
            Binding.ReaderQuotas.MaxDepth = 0xfffffff;

            return Binding;
        }

        private System.ServiceModel.Channels.Binding GeWStHttpBinding(ServiceHost host, string ip, string port)
        {
            WSHttpBinding Binding = new WSHttpBinding(SecurityMode.None);

            #region Binding Main
            Binding.MaxBufferPoolSize = 2147483647;
            Binding.MaxReceivedMessageSize = 2147483647;
            Binding.OpenTimeout = new TimeSpan(0, 0, 30);
            Binding.SendTimeout = new TimeSpan(0, 0, 30);
            Binding.ReceiveTimeout = new TimeSpan(4, 0, 0);
            Binding.ReliableSession.InactivityTimeout = TimeSpan.FromHours(4);


            Binding.ReaderQuotas.MaxStringContentLength = 0xfffffff;
            Binding.ReaderQuotas.MaxArrayLength = 0xfffffff;
            Binding.ReaderQuotas.MaxBytesPerRead = 0xfffffff;
            Binding.ReaderQuotas.MaxDepth = 0xfffffff;
            #endregion

            host.AddServiceEndpoint(typeof(QSDataService), Binding, string.Format("http://{0}:{1}/", ip, port));
            return Binding;
        }

        private static System.ServiceModel.Channels.Binding GetTcpBinding(ServiceHost host)
        {
            NetTcpBinding Binding = new NetTcpBinding(SecurityMode.Message);

            Binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;

            #region Binding Main
            Binding.MaxBufferSize = 2147483647;
            Binding.MaxBufferPoolSize = 2147483647;
            Binding.MaxReceivedMessageSize = 2147483647;
            Binding.SendTimeout = new TimeSpan(0, 0, 8);
            Binding.ReceiveTimeout = new TimeSpan(4, 0, 0);
            Binding.ReliableSession.InactivityTimeout = TimeSpan.FromHours(4);


            Binding.ReaderQuotas.MaxStringContentLength = 0xfffffff;
            Binding.ReaderQuotas.MaxArrayLength = 0xfffffff;
            Binding.ReaderQuotas.MaxBytesPerRead = 0xfffffff;
            Binding.ReaderQuotas.MaxDepth = 0xfffffff;
            #endregion

            host.AddServiceEndpoint(typeof(QSDataService), Binding, "net.tcp://localhost/");
            return Binding;
        }
        #endregion

        public void LogInfo(string strLog)
        {
            Dispatcher.Invoke(() =>
            {
                ocLog.Add(new Loger(strLog));
            });
           
        }
    }
}
