using AASClient.AASServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class LoginForm : Form
    {
        public static Dictionary<string, string> ServerIPDict = new Dictionary<string, string>();

        bool isSelectServer = false;
        Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        readonly string serverIP = "211.149.152.92:808";//"211.149.152.92:808";
        const string pubServerName = "分发服务器";
        string pubServerIPCache = string.Empty;


        string MAC;
        public LoginForm()
        {
            InitializeComponent();
            //ServerIPDict.Add("云服务器", "云服务器 101.132.154.219:40808");
            //ServerIPDict.Add("65", "65 192.168.1.65:40808");
            ServerIPDict.Add("测试环境", "127.0.0.1:808");
            ServerIPDict.Add("公共机房", "公共机房 211.149.152.92:808");
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (Program.IsSinglePoint)
            {
                this.Text = string.Format("登录 [版本号: {0}]", Program.Version);
            }
            else
            {
                this.Text = string.Format("登录 [版本号: {0}_M]", Program.Version);
            }

            List<string> userName = new List<string>();
            foreach (var user in this.AppConfig.AppSettings.Settings.AllKeys)
            {
                if (!user.Equals(pubServerName))
                    userName.Add(user);
                else
                    pubServerIPCache = Cryptor.MD5Decrypt(this.AppConfig.AppSettings.Settings[pubServerName].Value);
            }
            this.comboBox用户名.DataSource = userName;

            this.MAC = CommonUtils.GetMacAddress();

            if (!string.IsNullOrEmpty(serverIP))
            {
                this.label3.Visible = false;
                this.textBox服务器.Visible = false;
                this.textBox服务器.ReadOnly = true;
                this.textBox服务器.Text = serverIP;
                foreach (var item in ServerIPDict)
                {
                    if (item.Value.Contains(serverIP))
                    {
                        if (Program.IsSinglePoint)
                        {
                            this.Text = string.Format("{0} [版本号: {1}]", item.Key, Program.Version);
                        }
                        else
                        {
                            this.Text = string.Format("{0} [版本号: {1}_M]", item.Key, Program.Version);
                        }
                    }
                }
            }

            if (isSelectServer)
            {
                foreach (var item in ServerIPDict.Values)
                {
                    comboBoxServer.Items.Add(item);
                }
                comboBoxServer.Left = textBox服务器.Left;
                comboBoxServer.Width = textBox服务器.Width;
                SetServerSelectIndex();
                //textBox服务器.Visible = false;
            }
            else
            {
                comboBoxServer.Visible = false;
            }

        }

        private void SetServerSelectIndex()
        {
            comboBoxServer.SelectedIndex = -1;
            for (int i = 0; i < comboBoxServer.Items.Count; i++)
            {
                if (comboBoxServer.Items[i].ToString().Contains(textBox服务器.Text.Trim()))
                {
                    comboBoxServer.SelectedIndex = i;
                    break;
                }
            }
            //if (comboBoxServer.SelectedIndex == -1)
            //{
            //    textBox服务器.Text = "";
            //}
        }

        private void comboBox用户名_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.AppConfig.AppSettings.Settings.AllKeys.Contains(this.comboBox用户名.Text))
            {
                string string1 = this.AppConfig.AppSettings.Settings[this.comboBox用户名.Text].Value;
                int k = string1.IndexOf(",");
                if (k < 0)
                    return;

                if (!textBox服务器.ReadOnly)
                    this.textBox服务器.Text = string1.Substring(0, k);

                this.textBox密码.Text = Cryptor.MD5Decrypt(string1.Substring(k + 1));

            }
            else
            {
                this.textBox服务器.Text = string.Empty;
                this.textBox密码.Text = string.Empty;
            }
            if (isSelectServer)
            {
                SetServerSelectIndex();
            }
        }


        private void LoginForm_Activated(object sender, EventArgs e)
        {

            if (textBox服务器.Visible)
            {
                this.textBox服务器.Focus();
            }
            else
            {
                this.comboBox用户名.Focus();
            }
        }

        private void button登录_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            if (textBox服务器.Text.Length == 0)
            {
                MessageBox.Show("请输入登陆服务器IP！");
                return;
            }
            button登录.Text = "登录中……";
            button登录.Enabled = false;
            string serverIP = textBox服务器.Text;
            string user = this.comboBox用户名.Text;
            string psw = this.textBox密码.Text + "\t" + this.MAC;
            string uri = string.Format("net.tcp://{0}/", serverIP.Contains(":") ? serverIP : (serverIP + ":" + Program.Port));


            Task.Run(() =>
            {
                try
                {
                    Program.EndpointAddress = new EndpointAddress(new Uri(uri), EndpointIdentity.CreateDnsIdentity("localhost"));
                    Program.AASServiceClient = new AASServiceClient(Program.callbackInstance, Program.NetTcpBinding, Program.EndpointAddress);
                    Program.AASServiceClient.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
                    Program.AASServiceClient.ClientCredentials.UserName.UserName = user;
                    Program.AASServiceClient.ClientCredentials.UserName.Password = psw;
                    var result = Program.AASServiceClient.QuerySingleUser(Program.Version);

                    Program.Current平台用户 = result[0];

                    Program.LoginInfoCache(serverIP, user, psw);

                    HqServerInit();


                    ICommunicationObject ICommunicationObject1 = Program.AASServiceClient as ICommunicationObject;
                    ICommunicationObject1.Faulted += ICommunicationObject1_Faulted;
                    ICommunicationObject1.Closed += ICommunicationObject1_Closed;

                    if (this.checkBox记住密码.Checked)
                    {
                        if (this.AppConfig.AppSettings.Settings.AllKeys.Contains(user))
                        {
                            this.AppConfig.AppSettings.Settings[user].Value = serverIP + "," + Cryptor.MD5Encrypt(psw);
                        }
                        else
                        {
                            this.AppConfig.AppSettings.Settings.Add(user, serverIP + "," + Cryptor.MD5Encrypt(psw));
                        }
                        this.AppConfig.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("appSettings");
                    }
                    else
                    {
                        if (this.AppConfig.AppSettings.Settings.AllKeys.Contains(user))
                        {
                            this.AppConfig.AppSettings.Settings.Remove(user);
                            this.AppConfig.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("appSettings");
                        }
                    }

                    Directory.CreateDirectory(Program.Current平台用户.用户名);

                    this.DialogResult = DialogResult.OK;
                }
                catch (MessageSecurityException ex)
                {
                    if (ex.InnerException == null)
                    {
                        this.Invoke(new Action(() => { MessageBox.Show(ex.Message, "MessageSecurityException - InnerException"); }));

                    }
                    else
                    {
                        this.Invoke(new Action(() => { MessageBox.Show(ex.InnerException.Message, "MessageSecurityException"); }));
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => { MessageBox.Show(ex.Message, ex.GetType().Name); }));
                    
                }

                button登录.BeginInvoke(new Action(() =>
                {
                    button登录.Enabled = true;
                    button登录.Text = "登录";
                }));

            });

        }

        private void HqServerInit()
        {
            if (pubServerIPCache != string.Empty && CommonUtils.TelnetPort(pubServerIPCache, AASClient.TDFData.DataCache.ServerPort))
            {
                AASClient.TDFData.DataCache.ServerIP = pubServerIPCache;
            }
            else
            {
                string ip = Program.AASServiceClient.GetDataServerIP();
                if (string.IsNullOrEmpty(ip))
                {
                    TDFData.DataSourceConfig.IsUseTDFData = false;
                }
                else
                {
                    if (ip.Contains(System.Environment.NewLine))
                    {
                        string[] arr = ip.Split(new[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in arr)
                        {
                            if (CommonUtils.TelnetPort(item, AASClient.TDFData.DataCache.ServerPort))
                            {
                                AASClient.TDFData.DataCache.ServerIP = item;
                                if (this.AppConfig.AppSettings.Settings.AllKeys.Contains(pubServerName))
                                {
                                    if (item.Equals(Cryptor.MD5Decrypt(this.AppConfig.AppSettings.Settings[pubServerName].Value))) break;
                                    this.AppConfig.AppSettings.Settings[pubServerName].Value = Cryptor.MD5Encrypt(item);
                                }
                                else
                                {
                                    this.AppConfig.AppSettings.Settings.Add(pubServerName, Cryptor.MD5Encrypt(item));
                                }

                                this.AppConfig.Save(ConfigurationSaveMode.Modified);
                                ConfigurationManager.RefreshSection("appSettings");
                                break;
                            }
                            Thread.Sleep(100);
                        }
                    }
                    else
                    {
                        if (CommonUtils.TelnetPort(ip, AASClient.TDFData.DataCache.ServerPort))
                        {
                            AASClient.TDFData.DataCache.ServerIP = ip;
                        }
                    }
                }
            }
            if (AASClient.TDFData.DataCache.ServerIP == string.Empty)
            {
                TDFData.DataSourceConfig.IsUseTDFData = false;
            }
        }

        void ICommunicationObject1_Faulted(object sender, EventArgs e)
        {
            Program.AASServiceClient = new AASServiceReference.AASServiceClient(Program.callbackInstance, Program.NetTcpBinding, Program.EndpointAddress);

            Program.AASServiceClient.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
            Program.AASServiceClient.ClientCredentials.UserName.UserName = this.comboBox用户名.Text;
            Program.AASServiceClient.ClientCredentials.UserName.Password = this.textBox密码.Text + "\t" + this.MAC;



            ICommunicationObject ICommunicationObject1 = Program.AASServiceClient as ICommunicationObject;
            ICommunicationObject1.Faulted += ICommunicationObject1_Faulted;
            ICommunicationObject1.Closed += ICommunicationObject1_Closed;
        }
        void ICommunicationObject1_Closed(object sender, EventArgs e)
        {
            //MessageBox.Show("到服务器的连接已关闭");
        }

        private void checkBox记住密码_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBox记住密码.Checked)
            {
                if (this.AppConfig.AppSettings.Settings.AllKeys.Contains(this.comboBox用户名.Text))
                {
                    this.AppConfig.AppSettings.Settings.Remove(this.comboBox用户名.Text);
                    this.AppConfig.Save(ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");

                    this.comboBox用户名.DataSource = null;
                    List<string> userName = new List<string>();
                    foreach (var user in this.AppConfig.AppSettings.Settings.AllKeys)
                    {
                        if (!user.Equals(pubServerName))
                            userName.Add(user);
                    }
                    this.comboBox用户名.DataSource = userName;
                }
            }
        }

        private void textBox_KeyEnterDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.button登录_Click(this.button登录, new EventArgs());
            }
        }

        private void comboBoxServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxServer.SelectedIndex > -1)
            {
                var ipInfo = comboBoxServer.SelectedItem.ToString();
                var matchItem = Regex.Match(ipInfo, "[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}");
                if (matchItem.Success)
                {
                    textBox服务器.Text = matchItem.Value;
                    if (ipInfo.Contains(":"))
                    {
                        var port = ipInfo.Split(':')[1];
                        Program.Port = port;
                    }
                }
            }
        }


    }
}
