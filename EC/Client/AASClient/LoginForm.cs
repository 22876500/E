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
        Dictionary<string, string> ServerIPDict = new Dictionary<string, string>();

        bool isSelectServer = false;
        Configuration AppConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        
        //110.86.28.229 西安登录的固定ip, 210.13.212.206上海登录的固定ip 
        readonly string serverIP = "";
        const string pubServerName = "分发服务器";
        string pubServerIPCache = string.Empty;

        string MAC;
        public LoginForm()
        {
            InitializeComponent();
            ServerIPDict.Add("上海虚拟机", "上海虚拟机 192.168.1.249");
            //ServerIPDict.Add("252", "厦门252 110.86.28.227");
            //ServerIPDict.Add("65", "65 110.86.28.229");
            //ServerIPDict.Add("策略正式", "策略正式 110.86.28.230");
            //ServerIPDict.Add("策略测试", "策略测试 110.86.28.228");
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
            //this.textBox服务器.ReadOnly = true;
            //this.textBox服务器.Text = "210.13.212.206";//上海交易使用

            if (isSelectServer)
            {
                //comboBoxServer.Items.Add("厦门252 110.86.28.227");
                //comboBoxServer.Items.Add("65 110.86.28.229");
                //comboBoxServer.Items.Add("策略正式 110.86.28.230");
                //comboBoxServer.Items.Add("策略测试 110.86.28.228");
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
            //this.comboBox用户名.Focus();
            this.textBox服务器.Focus();
        }

        private void button登录_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox服务器.Text.Length == 0)
                {
                    MessageBox.Show("请输入登陆服务器IP！");
                    return;
                }
                
                //string server = string.IsNullOrEmpty(serverIP) ? textBox服务器.Text : serverIP;

                Program.EndpointAddress = new EndpointAddress(new Uri(string.Format("net.tcp://{0}:{1}/", textBox服务器.Text, Program.Port)), EndpointIdentity.CreateDnsIdentity("localhost"));
                //if (Program.Port == "808")
                //{
                    
                //}
                //else
                //{
                //    Program.EndpointAddress = new EndpointAddress(new Uri(string.Format("net.tcp://{0}/", textBox服务器.Text)), EndpointIdentity.CreateDnsIdentity("localhost"));
                //}
                
                
                Program.AASServiceClient = new AASServiceReference.AASServiceClient(Program.callbackInstance, Program.NetTcpBinding, Program.EndpointAddress);

                //Program.EndpointAddress = new EndpointAddress(new Uri(string.Format("http://{0}/", server)), EndpointIdentity.CreateDnsIdentity("localhost"));
                //Program.AASServiceClient = new AASServiceReference.AASServiceClient(Program.callbackInstance, Program.WSDualHttpBinding, Program.EndpointAddress);


                Program.AASServiceClient.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                Program.AASServiceClient.ClientCredentials.UserName.UserName = this.comboBox用户名.Text;
                Program.AASServiceClient.ClientCredentials.UserName.Password = this.textBox密码.Text + "\t" + this.MAC;


                Program.Current平台用户 = Program.AASServiceClient.QuerySingleUser(Program.Version)[0];
                Program.LoginInfoCache(textBox服务器.Text, this.comboBox用户名.Text, this.textBox密码.Text);
               
                ICommunicationObject ICommunicationObject1 = Program.AASServiceClient as ICommunicationObject;
                ICommunicationObject1.Faulted += ICommunicationObject1_Faulted;
                ICommunicationObject1.Closed += ICommunicationObject1_Closed;
                
                if (this.checkBox记住密码.Checked)
                {
                    if (this.AppConfig.AppSettings.Settings.AllKeys.Contains(this.comboBox用户名.Text))
                    {
                        this.AppConfig.AppSettings.Settings[this.comboBox用户名.Text].Value = this.textBox服务器.Text + "," + Cryptor.MD5Encrypt(this.textBox密码.Text);
                    }
                    else
                    {
                        this.AppConfig.AppSettings.Settings.Add(this.comboBox用户名.Text, this.textBox服务器.Text + "," + Cryptor.MD5Encrypt(this.textBox密码.Text));
                    }
                    this.AppConfig.Save(ConfigurationSaveMode.Modified);
                    System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                }
                else
                {
                    if (this.AppConfig.AppSettings.Settings.AllKeys.Contains(this.comboBox用户名.Text))
                    {
                        this.AppConfig.AppSettings.Settings.Remove(this.comboBox用户名.Text);
                        this.AppConfig.Save(ConfigurationSaveMode.Modified);
                        System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                    }
                }

                Directory.CreateDirectory(Program.Current平台用户.用户名);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (MessageSecurityException ex)
            {

                if (ex.InnerException == null)
                {
                    MessageBox.Show(ex.Message, "MessageSecurityException - InnerException");
                }
                else
                {
                    MessageBox.Show(ex.InnerException.Message, "MessageSecurityException");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name);
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
                    //else
                    //{
                    //    Program.Port = "40808";
                    //}
                }
            }
        }


    }
}
