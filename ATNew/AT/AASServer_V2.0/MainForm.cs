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

namespace AASServer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            
            InitializeComponent();

            Program.logger.Init(this.listView2);
            CatchLostOrder();

            TimerSendYjData();
        }
        private void TimerSendYjData()
        {
            var thread = new Thread(YjThreadMain) { IsBackground = true };
            thread.Start();
        }
        void YjThreadMain()
        {

            //业绩统计服务初始化
            if (Tool.InitService() == false)
            {
                Program.logger.LogInfo("定时服务初始化失败");
                return;
            }
            string startTime = CommonUtils.GetConfig("SendDataTime");

            int iHour = 15;
            int iMinute = 10;
            try
            {
                if (!string.IsNullOrEmpty(startTime))
                {
                    iHour = int.Parse(startTime.Split(':')[0]);
                    iMinute = int.Parse(startTime.Split(':')[1]);
                }
            }
            catch (Exception)
            {
                iHour = 15;
                iMinute = 10;
            }
            while (true)
            {
                if (DateTime.Now.Hour == iHour && DateTime.Now.Minute == iMinute)
                {
                    //定时发送业绩数据
                    Tool.SendYJDataToServer();
                    //定时发送委托数据
                    Tool.SendWTDataToServer();
                    //定时发送额度分配数据
                    Tool.SendQuotaDataToServer();

                }
                Thread.Sleep(60*1000);
            }

        }
        private void CatchLostOrder()
        {
            var thread = new Thread(LogOrderThreadMain) { IsBackground = true };
            thread.Start();
        }

        void LogOrderThreadMain()
        {
            while (true)
            {
                //删除当天缓存
                if (DateTime.Now.Hour > dateTimePicker2.Value.Hour)
                {
                    while (true)
                    {
                        OrderCacheEntity entity = null;

                        if (CommonUtils.OrderCacheQueue.Count == 0 || !CommonUtils.OrderCacheQueue.TryDequeue(out entity))
                        {
                            break;
                        }
                        
                    }
                    Thread.Sleep(60000);
                }
                Thread.Sleep(100);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var port = CommonUtils.GetConfig("ServiceConnectPort");
            if (string.IsNullOrEmpty(port))
            {
                port = "808";
            }
            this.Text = string.Format("AAS服务端 [版本号: {0}, 端口:{1}, 启动时间: {2}]", Program.Version, port, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            this.toolStripStatusLabelHostName.Text = Dns.GetHostName();

            this.dataGridView1.DataSource = Program.UIdataSet.交易帐户;







            this.dateTimePicker1.Value = DateTime.Parse(Program.appConfig.GetValue("开始查询时间", "8:15"));
            this.dateTimePicker2.Value = DateTime.Parse(Program.appConfig.GetValue("结束查询时间", "15:30"));

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

            foreach (var item in Program.db.平台用户)
            {
                if (item.角色 == (int)角色.交易员 && !Program.SendOrderObjDict.ContainsKey(item.用户名))
                {
                    Program.SendOrderObjDict[item.用户名] = new object();
                }
            }


            //AyersMessageAdapter.Instance.Load();

            //AyersMessageAdapter.Instance.Start();

            LimitManageService.Instance.Start();

            if (!Program.db.平台用户.ExistsRole(角色.超级管理员))
            {
                this.backgroundWorker1.ReportProgress(0);
            }


            #region 初始化服务
            ServiceHost TradeServiceHost = RunService();
            

            ServiceHost GroupServiceHost = InitGroupService();
            
            #endregion

            

            while (!this.backgroundWorker1.CancellationPending)
            {
                this.backgroundWorker1.ReportProgress(1);

                try
                {
                    Program.db.券商帐户.Start();

                    Program.db.恒生帐户.Start();

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
                            Program.logger.LogInfo("刷新交易员数据异常：{0},交易员：{1}", ex.Message, 交易员);
                        }

                    }
                    #endregion

                    AddNewestConsignment();
                    Tool.SendNotifyToClient();  //通知交易客户端和风控客户端
                    RemoveAddedConsignment();
                    Program.IsServiceUpdating = false;

                    Thread.Sleep(100);

                }
                catch(Exception ex)
                {
                    Program.logger.LogInfo("服务器线程异常:{0} {1}", ex.Message, ex.StackTrace);

                    Thread.Sleep(1000);
                }
            }

            Program.db.券商帐户.Stop();

            Program.db.恒生帐户.Stop();

            AyersMessageAdapter.Instance.Stop();

            //TdxHqApi.Instance.Stop();

            LimitManageService.Instance.Stop();

            if (GroupServiceHost != null)
            {
                GroupServiceHost.Close();
            }
            

            TradeServiceHost.Close();

            Program.logger.LogInfo("服务已关闭");
            
        }

        List<已发委托> lstOrderSending = new List<已发委托>();
        List<已发委托> lstOrderSended = new List<已发委托>();
        private void AddNewestConsignment()
        {
            try
            {
                if (Program.QueueConsignmentCache.Count > 0)
                {
                    已发委托 wt;
                    while (true)
                    {
                        if (Program.QueueConsignmentCache.Count > 0 && Program.QueueConsignmentCache.TryDequeue(out wt) && wt != null)
                        {
                            lstOrderSending.Add(wt);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (lstOrderSending.Count > 0)
                {
                    DateTime dt = DateTime.Now;
                    List<已发委托> lstOutTimeOrder = new List<已发委托>();
                    foreach (var item in lstOrderSending)
                    {
                        if (item == null)
                        {
                            continue;
                        }
                        if ((dt - item.日期).TotalSeconds < 50)
                        {
                            if (Program.交易员委托DataTable.ContainsKey(item.交易员))
                            {
                                if (Program.交易员委托DataTable[item.交易员] == null)
                                {
                                    Program.交易员委托DataTable[item.交易员] = new JyDataSet.委托DataTable();
                                    Program.交易员委托DataTable[item.交易员].Add委托Row(item.交易员, item.组合号, item.证券代码, item.证券名称, item.买卖方向, item.委托价格, item.委托数量, item.成交价格, item.成交数量, item.撤单数量, item.状态说明, item.委托编号, item.市场代码, item.日期.ToString("yyyy-MM-dd HH:mm:ss"));
                                    if (!lstOrderSended.Contains(item)) Program.委托表Changed[item.交易员] = true;
                                }
                                else if (Program.交易员委托DataTable[item.交易员].FirstOrDefault(_ => _.组合号 == item.组合号 && _.委托编号 == item.委托编号) == null)
                                {
                                    Program.交易员委托DataTable[item.交易员].Add委托Row(item.交易员, item.组合号, item.证券代码, item.证券名称, item.买卖方向, item.委托价格, item.委托数量, item.成交价格, item.成交数量, item.撤单数量, item.状态说明, item.委托编号, item.市场代码, item.日期.ToString("yyyy-MM-dd HH:mm:ss"));
                                    if (!lstOrderSended.Contains(item)) Program.委托表Changed[item.交易员] = true;
                                }
                                else
                                {
                                    lstOutTimeOrder.Add(item);
                                }
                                if (!lstOrderSended.Contains(item) && !lstOutTimeOrder.Contains(item)) lstOrderSended.Add(item);
                            }
                            else
                            {
                                Program.交易员委托DataTable[item.交易员] = new JyDataSet.委托DataTable();
                                Program.交易员委托DataTable[item.交易员].Add委托Row(item.交易员, item.组合号, item.证券代码, item.证券名称, item.买卖方向, item.委托价格, item.委托数量, item.成交价格, item.成交数量, item.撤单数量, item.状态说明, item.委托编号, item.市场代码, item.日期.ToString("yyyy-MM-dd HH:mm:ss"));
                                Program.委托表Changed[item.交易员] = true;
                            }
                        }
                        else
                        {
                            lstOutTimeOrder.Add(item);
                        }
                    }
                    lstOrderSending = lstOrderSending.Except(lstOutTimeOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("委托缓存修正异常：{0}", ex.Message, ex.StackTrace);
            }
            
        }

        private void RemoveAddedConsignment()
        {
            if (lstOrderSending.Count > 0)
            {
                foreach (var item in lstOrderSending)
                {
                    if (item == null)
                    {
                        continue;
                    }
                    try
                    {
                        if (Program.交易员委托DataTable.ContainsKey(item.交易员))
                        {
                            var wt = Program.交易员委托DataTable[item.交易员].FirstOrDefault(_ => _.组合号 == item.组合号 && _.委托编号 == item.委托编号);
                            if (wt != null)
                            {
                                Program.交易员委托DataTable[item.交易员].Remove委托Row(wt);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfoDetail("移除委托缓存对象出错, {0}", ex.Message);
                    }
                }
            }
        }

        private ServiceHost RunService()
        {
            string servicePort = CommonUtils.GetConfig("ServiceConnectPort");
            string host = "http://localhost:81/";

            if (servicePort != "808")
            {
                host = "http://localhost:82/";
            }

            ServiceHost ServiceHost1 = new ServiceHost(typeof(AASService), new Uri(host));

            //Program.logger.LogInfo("1.Binding设置开始");
            #region Binding
            NetTcpBinding NetTcpBinding1 = new NetTcpBinding(SecurityMode.Message);
            NetTcpBinding1.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            NetTcpBinding1.MaxBufferSize = 2147483647;//在Binding里指定了最大缓存字节数和最大接受字节数，相当于2G的大小
            NetTcpBinding1.MaxBufferPoolSize = 2147483647;
            NetTcpBinding1.MaxReceivedMessageSize = 2147483647;
            NetTcpBinding1.SendTimeout = new TimeSpan(0, 0, 8);
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
            ServiceHost1.Credentials.ServiceCertificate.Certificate = new X509Certificate2("AAS.pfx", "mima1234");
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
            Program.logger.LogInfo("交易端服务已启动，Port {0}。", servicePort);
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
                Exception ex = e.Error.InnerException == null ? e.Error : e.Error.InnerException;

                MessageBox.Show("服务器线程异常停止:" + ex.Message + ex.StackTrace);
            }
            
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

           

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            Program.appConfig.SetValue("开始查询时间", this.dateTimePicker1.Value.ToString("HH:mm:ss"));
          
        }


        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

            Program.appConfig.SetValue("结束查询时间", this.dateTimePicker2.Value.ToString("HH:mm:ss"));
           
        }

        private void 组合号配置重置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CommonUtils.ResetGroupConfig();
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
                        var qsAll = Program.db.券商帐户.QueryQsAccount();
                        string groupName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        if (!string.IsNullOrEmpty(groupName))
                        {
                            var groupItem = qsAll.FirstOrDefault(_ => _.名称 == groupName);
                            if (!string.IsNullOrEmpty(groupName) && groupItem != null)
                            {
                                if (groupItem.启用)
                                {
                                    cell.Value = "启用";
                                }
                                else
                                {
                                    cell.Value = "停用";
                                }
                                Program.db.券商帐户.EnableQSAccount(groupName, !groupItem.启用);                               
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

        private ServiceHost InitGroupService()
        {
            if (ConfigCache.OpenGroupService != "1")
            {
                return null;
            }
            #region 初始化服务
            string port = Program.appConfig.GetValue("GroupServicePort", "80");
            string ip = Program.appConfig.GetValue("GroupServiceBindIp", "localhost");
            string uri = string.Format("http://{0}:{1}/", ip, port);

            var GroupServiceHost = new ServiceHost(typeof(GroupClient.GroupService), new Uri(uri));

            System.ServiceModel.Channels.Binding binding = GeWStHttpBinding(GroupServiceHost, ip, port);

            #region Behavior
            ServiceThrottlingBehavior ServiceThrottlingBehavior1 = new ServiceThrottlingBehavior();
            ServiceThrottlingBehavior1.MaxConcurrentInstances = 1;//如果 InstanceContextMode 为 PerSession，则结果值将为总会话数。 如果 InstanceContextMode 为 PerCall，则结果值将为并发调用的数量。默认值Int32.MaxValue
            ServiceThrottlingBehavior1.MaxConcurrentCalls = 1000;//获取或设置一个值，该值指定整个 ServiceHost 中正在处理的最多消息数。 最大并发调用数目，默认16。 这个属于服务限流行为。
            ServiceThrottlingBehavior1.MaxConcurrentSessions = 1000;//限制连接到一个服务的并发会话最大数量 默认值10
            GroupServiceHost.Description.Behaviors.Add(ServiceThrottlingBehavior1);

            // Enable metadata publishing.
            ServiceMetadataBehavior ServiceMetadataBehavior1 = new ServiceMetadataBehavior();
            ServiceMetadataBehavior1.HttpGetEnabled = true;
            //ServiceMetadataBehavior1.HttpGetUrl = new Uri(string.Format("http://localhost:{0}/GroupAPI", port));
            GroupServiceHost.Description.Behaviors.Add(ServiceMetadataBehavior1);

            ServiceDebugBehavior ServiceDebugBehavior1 = GroupServiceHost.Description.Behaviors[typeof(ServiceDebugBehavior)] as ServiceDebugBehavior;
            ServiceDebugBehavior1.IncludeExceptionDetailInFaults = true;
            #endregion

            GroupServiceHost.Open();
            Program.logger.LogInfo("发送端服务已启动，Port {0}。", port);
            #endregion
            return GroupServiceHost;
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

            host.AddServiceEndpoint(typeof(GroupClient.GroupService), Binding, string.Format("http://{0}:{1}/", ip, port));
            return Binding;
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
    }   
}
