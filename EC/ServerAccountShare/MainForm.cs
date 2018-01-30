using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IdentityModel.Claims;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Server
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            
            InitializeComponent();

            Program.logger.Init(this.listView2);

            startTime = DateTime.Now;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var port = ConfigCache.ConnectPort;
            this.Text = string.Format("ECoinServer [版本:{0}, 端口:{1}, 启动时间:{2}]", Program.Version, port, DateTime.Now.ToString("yy/MM/dd HH:mm:ss"));

            this.toolStripStatusLabelHostName.Text = Dns.GetHostName();

            this.dataGridView1.DataSource = Program.UIdataSet.交易帐户;

            this.backgroundWorker1.RunWorkerAsync();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WaitClose();
        }

        private void WaitClose()
        {
            Program.logger.LogInfo("停止服务中...");

            this.backgroundWorker1.CancelAsync();
            while (this.backgroundWorker1.IsBusy)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }

            Program.logger.LogInfo("服务已停止");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Program.logger.LogInfo("启动服务中...");

            Program.db.Load();

            if (AutoOrderService.Instance != null)
            {
                AutoOrderService.Instance.Start();
            }

            if (!Program.db.平台用户.ExistsRole(角色.超级管理员))
            {
                this.backgroundWorker1.ReportProgress(0);
            }

            #region 初始化服务
            ServiceHost ServiceHost1 = RunService();
            Program.logger.LogInfo("服务已启动");
            #endregion

            while (!this.backgroundWorker1.CancellationPending)
            {
                this.backgroundWorker1.ReportProgress(1);
                
                try
                {
                    Program.db.电子币帐户.Start();
                    
                    Program.IsServiceUpdating = true;
                    #region 由 帐户DataTable 转为 交易员DataTable
                    Dictionary<string, JyDataSet.成交DataTable> 交易员成交DataTable = new Dictionary<string, JyDataSet.成交DataTable>();
                    Dictionary<string, JyDataSet.委托DataTable> 交易员委托DataTable = new Dictionary<string, JyDataSet.委托DataTable>();

                    #region 成交
                    foreach (JyDataSet.成交DataTable 帐户成交DataTable in Program.帐户成交DataTable.Values)
                    {
                        foreach (JyDataSet.成交Row 成交Row1 in 帐户成交DataTable)
                        {
                            if (!交易员成交DataTable.ContainsKey(成交Row1.交易员))
                            {
                                交易员成交DataTable[成交Row1.交易员] = new JyDataSet.成交DataTable();
                            }

                            交易员成交DataTable[成交Row1.交易员].ImportRow(成交Row1);
                        }
                    }
                    #endregion

                    #region 委托
                    foreach (JyDataSet.委托DataTable 帐户委托DataTable in Program.帐户委托DataTable.Values)
                    {
                        foreach (JyDataSet.委托Row 委托Row1 in 帐户委托DataTable)
                        {
                            if (!交易员委托DataTable.ContainsKey(委托Row1.交易员))
                            {
                                交易员委托DataTable[委托Row1.交易员] = new JyDataSet.委托DataTable();
                            }

                            交易员委托DataTable[委托Row1.交易员].ImportRow(委托Row1);
                        }
                    }
                    #endregion

                    #region 风控平仓
                    JyDataSet.委托DataTable 风控平仓委托DataTable = Program.db.已发委托.Get风控平仓委托DataTable();
                    foreach (JyDataSet.委托Row 委托Row1 in 风控平仓委托DataTable)
                    {
                        if (!交易员委托DataTable.ContainsKey(委托Row1.交易员))
                        {
                            交易员委托DataTable[委托Row1.交易员] = new JyDataSet.委托DataTable();
                        }

                        交易员委托DataTable[委托Row1.交易员].ImportRow(委托Row1);
                    }
                    #endregion
                    #endregion

                    #region 比较Key是否一致
                    foreach (string 交易员 in 交易员成交DataTable.Keys)
                    {
                        if (!Program.交易员成交DataTable.ContainsKey(交易员))
                        {
                            Program.交易员成交DataTable[交易员] = new JyDataSet.成交DataTable();
                        }
                    }
                    foreach (string 交易员 in Program.交易员成交DataTable.Keys)
                    {
                        if (!交易员成交DataTable.ContainsKey(交易员))
                        {
                            交易员成交DataTable[交易员] = new JyDataSet.成交DataTable();
                        }
                    }
                    foreach (string 交易员 in 交易员委托DataTable.Keys)
                    {
                        if (!Program.交易员委托DataTable.ContainsKey(交易员))
                        {
                            Program.交易员委托DataTable[交易员] = new JyDataSet.委托DataTable();
                        }
                    }
                    foreach (string 交易员 in Program.交易员委托DataTable.Keys)
                    {
                        if (!交易员委托DataTable.ContainsKey(交易员))
                        {
                            交易员委托DataTable[交易员] = new JyDataSet.委托DataTable();
                        }
                    }
                    #endregion

                    #region 刷新交易员数据表
                    foreach (string 交易员 in 交易员成交DataTable.Keys)
                    {
                        bool bool1 = Tool.RefreshDrcjDataTable(Program.交易员成交DataTable[交易员], 交易员成交DataTable[交易员], new string[] { "组合号", "委托编号", "成交编号" });
                        if (bool1)
                        {
                            Program.成交表Changed[交易员] = true;
                        }
                    }
                    foreach (string 交易员 in 交易员委托DataTable.Keys)
                    {
                        try
                        {
                            bool bool1 = Tool.RefreshDrcjDataTable(Program.交易员委托DataTable[交易员], 交易员委托DataTable[交易员], new string[] { "组合号", "委托编号" });
                            if (bool1)
                            {
                                Program.委托表Changed[交易员] = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.logger.LogInfo("刷新交易员数据异常：{0} 交易员：{1}", ex.Message, 交易员);
                        }

                    }
                    #endregion
                    
                    Tool.SendNotifyToClient();  //通知交易客户端和风控客户端
                    Program.IsServiceUpdating = false;

                    Thread.Sleep(100);
                }
                catch(Exception ex)
                {
                    Program.logger.LogInfo("服务器线程异常:{0} {1}", ex.Message, ex.StackTrace);

                    Thread.Sleep(1000);
                }
            }


            Program.db.电子币帐户.Stop();
            if (ServiceHost1 != null && ServiceHost1.State != CommunicationState.Faulted)
            {
                ServiceHost1.Close();
            }

            Program.logger.LogInfo("服务已关闭");
        }

        private ServiceHost RunService()
        {
            string servicePort = ConfigCache.ConnectPort;
            string host = "http://localhost:91/";

            if (servicePort != "40808")
            {
                host = "http://localhost:92/";
            }

            ServiceHost ServiceHost1 = new ServiceHost(typeof(AASService), new Uri(host));

            //Program.logger.LogInfo("1.Binding设置开始");
            #region Binding
            NetTcpBinding NetTcpBinding1 = new NetTcpBinding(SecurityMode.Message);
            NetTcpBinding1.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            NetTcpBinding1.MaxBufferSize = 2147483647;//在Binding里指定了最大缓存字节数和最大接受字节数，相当于2G的大小
            NetTcpBinding1.MaxBufferPoolSize = 2147483647;
            NetTcpBinding1.MaxReceivedMessageSize = 2147483647;
            NetTcpBinding1.SendTimeout = new TimeSpan(0, 0, 5);
            NetTcpBinding1.ReceiveTimeout = new TimeSpan(4, 0, 0);
            //NetTcpBinding1.ReliableSession.InactivityTimeout = new TimeSpan(0, 0, 5);
            //NetTcpBinding1.ReliableSession.Enabled = true;
            //NetTcpBinding1.ReliableSession.InactivityTimeout = TimeSpan.FromHours(4);
            NetTcpBinding1.ReaderQuotas.MaxStringContentLength = 0xfffffff;
            NetTcpBinding1.ReaderQuotas.MaxArrayLength = 0xfffffff;
            NetTcpBinding1.ReaderQuotas.MaxBytesPerRead = 0xfffffff;
            NetTcpBinding1.ReaderQuotas.MaxDepth = 0xfffffff;

            
            string address = string.IsNullOrEmpty(servicePort) ? "net.tcp://localhost/" : string.Format("net.tcp://localhost:{0}/", servicePort);

            ServiceHost1.AddServiceEndpoint(typeof(AASService), NetTcpBinding1, address);

            //WSDualHttpBinding WSDualHttpBinding1 = new WSDualHttpBinding(WSDualHttpSecurityMode.Message);
            //WSDualHttpBinding1.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            ////WSDualHttpBinding1.MaxBufferSize = 2147483647;//在Binding里指定了最大缓存字节数和最大接受字节数，相当于2G的大小
            //WSDualHttpBinding1.MaxBufferPoolSize = 2147483647;
            //WSDualHttpBinding1.MaxReceivedMessageSize = 2147483647;
            //WSDualHttpBinding1.OpenTimeout = new TimeSpan(0, 0, 30);
            //WSDualHttpBinding1.SendTimeout = new TimeSpan(0, 0, 30);
            //WSDualHttpBinding1.ReceiveTimeout = new TimeSpan(4, 0, 0);
            ////WSDualHttpBinding1.ReliableSession.Enabled = true;
            //WSDualHttpBinding1.ReliableSession.InactivityTimeout = TimeSpan.FromHours(4);
            //WSDualHttpBinding1.ReaderQuotas.MaxStringContentLength = 0xfffffff;
            //WSDualHttpBinding1.ReaderQuotas.MaxArrayLength = 0xfffffff;
            //WSDualHttpBinding1.ReaderQuotas.MaxBytesPerRead = 0xfffffff;
            //WSDualHttpBinding1.ReaderQuotas.MaxDepth = 0xfffffff;
            //ServiceHost1.AddServiceEndpoint(typeof(AASService), WSDualHttpBinding1, "http://localhost/");

            //TcpTransportBindingElement TcpTransportBindingElement1 = new TcpTransportBindingElement();  
            //TcpTransportBindingElement1.ConnectionPoolSettings.MaxOutboundConnectionsPerEndpoint = 30;
            //string count = TcpTransportBindingElement1.ConnectionPoolSettings.MaxOutboundConnectionsPerEndpoint.ToString();
            //Console.Write("当前连接池中的最大连接数量为:" + count);   
            #endregion
            //Program.logger.LogInfo("2.ServiceBehavior设置开始");
            #region Service Behavior
            ServiceThrottlingBehavior ServiceThrottlingBehavior1 = new ServiceThrottlingBehavior();
            ServiceThrottlingBehavior1.MaxConcurrentInstances = 1;//如果 InstanceContextMode 为 PerSession，则结果值将为总会话数。 如果 InstanceContextMode 为 PerCall，则结果值将为并发调用的数量。默认值Int32.MaxValue
            ServiceThrottlingBehavior1.MaxConcurrentCalls = 1000;//获取或设置一个值，该值指定整个 ServiceHost 中正在处理的最多消息数。 最大并发调用数目，默认16。 这个属于服务限流行为。
            ServiceThrottlingBehavior1.MaxConcurrentSessions = 1000;//限制连接到一个服务的并发会话最大数量 默认值10
            ServiceHost1.Description.Behaviors.Add(ServiceThrottlingBehavior1);

            // Enable metadata publishing.
            ServiceMetadataBehavior ServiceMetadataBehavior1 = new ServiceMetadataBehavior();
            ServiceMetadataBehavior1.HttpGetEnabled = true;
            ServiceHost1.Description.Behaviors.Add(ServiceMetadataBehavior1);

            ServiceDebugBehavior ServiceDebugBehavior1 = ServiceHost1.Description.Behaviors[typeof(ServiceDebugBehavior)] as ServiceDebugBehavior;
            ServiceDebugBehavior1.IncludeExceptionDetailInFaults = true;
            #endregion

            //Program.logger.LogInfo("3.证书设置开始");
            #region Credentials
            ServiceHost1.Credentials.ServiceCertificate.Certificate = new X509Certificate2("AAS18.pfx", "mima1234");
            ServiceHost1.Credentials.ClientCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;

            //用户名密码认证认证  角色授权
            ServiceHost1.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = System.ServiceModel.Security.UserNamePasswordValidationMode.Custom;
            ServiceHost1.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new AASUserNamePasswordValidator();//用户名密码认证 
            #endregion

            //Program.logger.LogInfo("4.授权设置开始");
            #region Authorization
            ServiceHost1.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.UseAspNetRoles;
            ServiceHost1.Authorization.RoleProvider = new AASRoleProvider();
            ServiceHost1.Authorization.ServiceAuthorizationManager = new AASServiceAuthorizationManager();
            #endregion

            //自定义认证，授权,  哪些用户可以访问本服务器
            //ServiceHost1.Authentication.ServiceAuthenticationManager = new AASServiceAuthenticationManager();//自定义认证，哪些用户可以访问本服务器
            //自定义权限，注释掉表示使用默认权限
            //ServiceHost1.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            //ServiceHost1.Authorization.ExternalAuthorizationPolicies = new System.Collections.ObjectModel.ReadOnlyCollection<System.IdentityModel.Policy.IAuthorizationPolicy>(new[] { new AASAuthorizationPolicy() });//自定义授权
            //ServiceHost1.Authorization.ServiceAuthorizationManager = new AASServiceAuthorizationManager();//授权用户可以调用服务里的哪些函数

            //Program.logger.LogInfo("5.ServiceOpen开始");

            ServiceHost1.Open();

            return ServiceHost1;
        }


        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch(e.ProgressPercentage)
            {
                case 0:
                    AddUserForm AddUserForm1 = new AddUserForm();
                    AddUserForm1.ShowDialog();
                    break;
                case 1:
                    this.toolStripStatusLabelTime.Text = DateTime.Now.ToString("HH:mm:ss");
                    break;
                default:
                    break;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Exception ex = e.Error.InnerException ?? e.Error;

                MessageBox.Show("服务器线程异常停止:" + ex.Message + ex.StackTrace);
            }
            
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

           

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void 组合号设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GroupConfigForm frm = new GroupConfigForm();
            //frm.ShowDialog();
        }


        DateTime startTime = DateTime.Now;
        private void timerCheckClose_Tick(object sender, EventArgs e)
        {
            if (startTime.Date < DateTime.Today && DateTime.Now.Hour == 0)
            {
                timerCheckClose.Stop();
                WaitClose();
                Thread.Sleep(15000);
                Process.Start(Application.ExecutablePath, "");
                this.Close();
            }
        }

        private void 连接即时查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> connectionCount = new Dictionary<string, int>();
            foreach (var item in Program.ClientUserName)
            {
                if (connectionCount.ContainsKey(item.Value))
                {
                    connectionCount[item.Value] += 1;
                }
                else
                {
                    connectionCount[item.Value] = 1;
                }
            }

            MessageBox.Show(connectionCount.ToJson(), "连接数查看");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && dataGridView1.Columns[e.ColumnIndex] is DataGridViewColumn && e.RowIndex > -1)
            {
                var cell = (DataGridViewButtonCell)dataGridView1.CurrentCell;
                if (cell != null)
                {
                    try
                    {
                        string groupName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        if (!string.IsNullOrEmpty(groupName))
                        {
                            if (Program.db.券商帐户.Exists(groupName))
                            {
                                var groupItem = Program.db.券商帐户.QueryQsAccount().FirstOrDefault(_ => _.名称 == groupName);
                                Program.db.券商帐户.EnableQSAccount(groupName, !groupItem.启用);
                                cell.Value = groupItem.启用 ? "停用" : "启用";
                            }
                            else if(Program.db.电子币帐户.Exists(groupName))
                            {
                                var row = Program.db.电子币帐户.QueryQsAccount().FirstOrDefault(_ => _.名称 == groupName);
                                Program.db.电子币帐户.EnableQSAccount(groupName, !row.启用);
                                cell.Value = row.启用 ? "停用" : "启用";
                            }
                        }
                       
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfoDetail("dataGridView1_CellContentClick Exception: {0}", ex.Message);
                    }
                    
                }
            }
        }

        
    }   
}
