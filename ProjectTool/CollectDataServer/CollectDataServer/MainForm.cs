using CollectDataServer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollectDataServer
{
    public partial class MainForm : Form
    {
        ServiceHost ServiceHost1;
        private string port = string.Empty;
        private string serverIP = string.Empty;
        public MainForm()
        {
            InitializeComponent();
            Program.logger.Init(this.listView1);
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;

            this.backgroundWorker1.WorkerSupportsCancellation = true;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            port = ConfigMain.GetConfigValue("ServicePort");
            if (string.IsNullOrEmpty(port))
            {
                port = "80";
            }
            serverIP = ConfigMain.GetConfigValue("ServiceIp");
            if (string.IsNullOrEmpty(serverIP))
            {
                serverIP = "localhost";
            }
            this.Text += string.Format(" [端口:{0}]", port);
            this.backgroundWorker1.RunWorkerAsync();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("业绩统计服务正在运行，确定要关闭吗！", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                e.Cancel = false;
                Program.logger.LogInfo("停止服务中...");

                this.backgroundWorker1.CancelAsync();
                while (this.backgroundWorker1.IsBusy)
                {
                    Application.DoEvents();
                    Thread.Sleep(100);
                }

                Program.logger.LogInfo("服务已停止");
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Program.logger.LogInfo("启动服务中...");
            #region 初始化服务
            InitService();
            Program.logger.LogInfo("服务已启动");
            #endregion
        }

        private void InitService()
        {
            #region 初始化服务
            //var port = ConfigMain.GetConfigValue("ServicePort");
            //if (string.IsNullOrEmpty(port))
            //{
            //    port = "80";
            //}

            //string ip = ConfigMain.GetConfigValue("bind_ip");
            //if (string.IsNullOrEmpty(ip))
            //{
            //    ip = "localhost";
            //}

            string uri = string.Format("http://{0}:{1}/", serverIP, port);
            ServiceHost1 = new ServiceHost(typeof(DataService), new Uri(uri));
            //CommonUtils.Log("服务初始化开始:uri {0}", uri);

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
                ServiceHost1.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CollectDataUserNamePasswordValidator();//用户名密码认证 
                #endregion

                #region Authorization
                ServiceHost1.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles;
                ServiceHost1.Authorization.RoleProvider = new CollectDataRoleProvider();
                ServiceHost1.Authorization.ServiceAuthorizationManager = new CollectDataServiceAuthorizationManager();
                #endregion
            }

            ServiceHost1.Open();
            //CommonUtils.Log("服务初始化完毕:uri {0}", uri);
            #endregion
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

            host.AddServiceEndpoint(typeof(DataService), Binding, string.Format("http://{0}:{1}/", ip, port));
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

            host.AddServiceEndpoint(typeof(DataService), Binding, "net.tcp://localhost/");
            return Binding;
        }
        #endregion
    }
}
