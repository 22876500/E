using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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

namespace GroupClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string VersionInfo = "201711130001";

        ServiceHost ServiceHost1;
        Adapter adapter = null;

        BackgroundWorker worker = new BackgroundWorker() { WorkerSupportsCancellation = true };

        public MainWindow()
        {
            InitializeComponent();

            adapter = new Adapter();

            this.Loaded += MainWindow_Loaded;
            
            worker.DoWork += worker_DoWork;
            
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            try
            {
                worker.CancelAsync();
                while (this.worker.IsBusy)
                {
                    try
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));
                    }
                    catch { }

                    Thread.Sleep(100);
                }
            }
            catch { }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            status.Text = "版本号：" + VersionInfo;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CommonUtils.Log("启动服务中…");

            TdxApi.OpenTdx();

            InitService();

            adapter.Start();

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.dgState.ItemsSource = Adapter.GroupLogonList;
            }));
            

            while (!this.worker.CancellationPending)
            {
                //try
                //{
                //    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                //    {
                //        this.dgState.Items.Refresh();
                //    }));
                //}
                //catch (Exception) { }
                Thread.Sleep(1000);
            }
            
            CommonUtils.Log("服务关闭中…");
            adapter.Stop();

            TdxApi.CloseTdx();

            ImsApi.Close();

            if (this.ServiceHost1.State != CommunicationState.Faulted)
            {
                this.ServiceHost1.Close();    
            }

            CommonUtils.Log("服务已关闭！");
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                StringBuilder sb = new StringBuilder("服务器线程异常停止:" + e.Error.Message + e.Error.StackTrace + "\r\n 异常类型:" + e.Error.GetType() + "\r\n");

                try
                {
                    if (e.Error.InnerException != null)
                    {
                        Exception ex = e.Error.InnerException;
                        sb.AppendLine("内部异常: " + ex.Message + ex.StackTrace + System.Environment.NewLine + ex.GetType());
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                            sb.AppendLine("内部异常: " + ex.Message + ex.StackTrace + System.Environment.NewLine + ex.GetType());
                        }
                    }
                    MessageBox.Show(sb.ToString());
                }
                catch (Exception ex1)
                {
                    MessageBox.Show("循环输入内部异常时发生异常:" + ex1.Message);
                }
            }
        }

        private void InitService()
        {
            
            #region 初始化服务
            var port = CommonUtils.GetConfig("ServicePort");
            if (string.IsNullOrEmpty(port))
            {
                port = "80";
            }

            string ip = CommonUtils.GetConfig("bind_ip");
            if (string.IsNullOrEmpty(ip))
            {
                ip = "localhost";
            }

            string uri = string.Format("http://{0}:{1}/", ip, port);
            ServiceHost1 = new ServiceHost(typeof(GroupService), new Uri(uri));
            CommonUtils.Log("服务初始化开始:uri {0}", uri);

            System.ServiceModel.Channels.Binding binding = GeWStHttpBinding(ServiceHost1, ip, port);

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
                ServiceHost1.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new GroupUserNamePasswordValidator();//用户名密码认证 
                #endregion

                #region Authorization
                ServiceHost1.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles;
                ServiceHost1.Authorization.RoleProvider = new GroupRoleProvider();
                ServiceHost1.Authorization.ServiceAuthorizationManager = new GroupServiceAuthorizationManager();
                #endregion
            }

           

            ServiceHost1.Open();
            CommonUtils.Log("服务初始化完毕:uri {0}", uri);
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

            host.AddServiceEndpoint(typeof(GroupService), Binding, string.Format("http://{0}:{1}/", ip, port));
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
            Binding.SendTimeout = new TimeSpan(0, 0, 5);
            Binding.ReceiveTimeout = new TimeSpan(0, 0, 10);
            Binding.ReliableSession.InactivityTimeout = TimeSpan.FromHours(4);


            Binding.ReaderQuotas.MaxStringContentLength = 0xfffffff;
            Binding.ReaderQuotas.MaxArrayLength = 0xfffffff;
            Binding.ReaderQuotas.MaxBytesPerRead = 0xfffffff;
            Binding.ReaderQuotas.MaxDepth = 0xfffffff;
            #endregion

            host.AddServiceEndpoint(typeof(GroupService), Binding, "net.tcp://localhost/");
            return Binding;
        } 
        #endregion

        private void MenuItem_AddGroup_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddGroup();
            win.OnAddComplete += (券商 o) => 
            {
                //this.dgMain.ItemsSource = Adapter.GroupsDict.Values.ToList();
            };
            var isAdd = win.ShowDialog();
            if (isAdd == true)
            {
                this.dgState.ItemsSource = Adapter.GroupLogonList;
                //this.dgMain.ItemsSource = Adapter.GroupsDict.Values.ToList();
            }
        }

        private void Button_Logon_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is GroupLogonInfo)
            {
                try
                {
                    var logonInfo = (sender as Button).DataContext as GroupLogonInfo;
                    var o = Adapter.GroupsDict[logonInfo.Name];
                    o.启用 = true;
                    Adapter.UpdateGroup(o);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("组合号启用异常！" + ex.Message);
                }
            }
        }

        private void Button_LogOff_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is GroupLogonInfo)
            {
                try
                {
                    var logonInfo = (sender as Button).DataContext as GroupLogonInfo;
                    var o = Adapter.GroupsDict[logonInfo.Name];
                    o.启用 = false;
                    Adapter.UpdateGroup(o);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("组合号停用异常" + ex.Message);
                }
            }
        }

        private void MenuItem_ConfigEdit_Click(object sender, RoutedEventArgs e)
        {
            ConfigEditor editor = new ConfigEditor();
            editor.ShowDialog();
        }

        private void MenuItem_GroupListConfig_Click(object sender, RoutedEventArgs e)
        {
            var winVerification = new winIdentityVerification();
            if (winVerification.ShowDialog() == true)
            {
                var win = new winGroupList();
                win.ShowDialog();
            }
        }

        private void MenuItem_AdminEdit_Click(object sender, RoutedEventArgs e)
        {
            var winVerification = new winIdentityVerification();
            if (winVerification.ShowDialog() == true)
            {
                var win = new AccountEdit();
                win.ShowDialog();
            }
        }

        private void MenuItem_IMSAccountAdd_Click(object sender, RoutedEventArgs e)
        {
            var win = new AddImsGroup();
            win.ShowDialog();
        }

        private void ComboBox_ServerList_Loaded(object sender, RoutedEventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb != null && cmb.DataContext is GroupLogonInfo)
            {
                var logonInfo = cmb.DataContext as GroupLogonInfo;
                var o = Adapter.GroupsDict[logonInfo.Name];
                
                if (File.Exists(CommonUtils.CurrentPath + "交易服务器.txt"))
                {
                    var arr = File.ReadAllLines(CommonUtils.CurrentPath + "交易服务器.txt", Encoding.Default);
                    cmb.ItemsSource = arr;
                    for (int i = 0; i < cmb.Items.Count; i++)
                    {
                        if (cmb.Items[i].ToString().Contains( o.交易服务器))
                        {
                            cmb.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void ComboBox_Server_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb != null && cmb.DataContext is GroupLogonInfo)
            {
                try
                {
                    var logonInfo = cmb.DataContext as GroupLogonInfo;
                    var o = Adapter.GroupsDict[logonInfo.Name];
                    o.交易服务器 = cmb.SelectedItem.ToString();
                    var serverInfoArr = o.交易服务器.Split(':');
                    o.IP = serverInfoArr[1];
                    o.Port = short.Parse(serverInfoArr[2]);

                    Adapter.UpdateGroup(o);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("交易服务器修改异常 " + ex.Message);
                }

            }
        }

   
        
    }


}
