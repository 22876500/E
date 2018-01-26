using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using YjDataClient.Common;
using YJDataClient.Common;
using System.ServiceModel;
using YJDataClient.ServiceReference1;

namespace YJDataClient
{

    public partial class FormLogin : Form
    {
        private static string servicePort = string.Empty;
        private static string serviceIp = string.Empty;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

        /// <summary>
        /// 登录判断
        /// </summary>
        private void Login()
        {
            string error = string.Empty;
            if (this.txtUserName.Text.Trim() != string.Empty && this.txtUserPwd.Text.Trim() != string.Empty)
            {
                bool result = true;//Program.DataServiceClient.LoginUser(this.txtUserName.Text.Trim(), this.txtUserPwd.Text.Trim(), out error);
                if (result == true)
                {
                    ConfigMain.SetConfigValue("userName", this.txtUserName.Text.Trim());
                    if (chkBoxPwd.Checked)
                    {
                        ConfigMain.SetConfigValue("userPwd", CommonUtils.EncryptDES(this.txtUserPwd.Text.Trim(), CommonUtils.encryptKey));
                    }
                    else
                    {
                        //ConfigMain.SetConfigValue("userPwd", "");
                        ConfigMain.removeItem("userPwd");
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(error, "Error");
                }
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            InitService();
            string userName = ConfigMain.GetConfigValue("userName");
            string userPwd = ConfigMain.GetConfigValue("userPwd");
            if (userName != string.Empty && userPwd != string.Empty)
            {
                this.txtUserName.Text = userName;
                this.txtUserPwd.Text = CommonUtils.DecryptDES(userPwd, CommonUtils.encryptKey);
                this.chkBoxPwd.Checked = true;
            }
            else
            {
                chkBoxPwd.Checked = false;
            }
        }

        private bool InitService()
        {
            if (Program.DataServiceClient != null) return true;

            servicePort = ConfigMain.GetConfigValue("ServicePort");
            if (string.IsNullOrEmpty(servicePort))
            {
                MessageBox.Show("通讯连接端口为空", "Error");
            }
            serviceIp = ConfigMain.GetConfigValue("ServiceIp");
            if (string.IsNullOrEmpty(serviceIp))
            {
                MessageBox.Show("通讯连接IP为空", "Error");
            }
            string endPoint = string.Format("http://{0}:{1}/", serviceIp, servicePort);

            var httpBinding = new WSHttpBinding(SecurityMode.None);
            httpBinding.MaxReceivedMessageSize = 2147483647;
            httpBinding.ReceiveTimeout = new TimeSpan(0, 0, 10);
            httpBinding.UseDefaultWebProxy = false;

            EndpointIdentity ei = null;
            ei = EndpointIdentity.CreateDnsIdentity(serviceIp);
            var endpointAddress = new EndpointAddress(new Uri(endPoint), ei);
            Program.DataServiceClient = new DataServiceClient(httpBinding, endpointAddress);
            if (Program.DataServiceClient == null) return false;
            return true;
        }
    }
}
