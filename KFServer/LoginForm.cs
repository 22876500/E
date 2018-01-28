using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASServer
{
    public partial class LoginForm : Form
    {
        short YybID = 0;
        string AccountNo = "260508992633";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.comboBox交易服务器.DataSource = File.ReadAllLines("交易服务器.txt", Encoding.Default);

            if (Program.appConfig.Exists("凭据"))
            {
                this.groupBox1.Visible = false;
            }
        }

        private void LoginForm_Activated(object sender, EventArgs e)
        {
            this.textBox交易密码.Focus();
        }

        private void checkBox记住密码_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBox记住密码.Checked)
            {

                Program.appConfig.Delete("凭据");



                this.comboBox交易服务器.SelectedIndex = 0;
                this.textBox版本号.Text = "6.0";
                this.textBox交易密码.Text = string.Empty;


                this.groupBox1.Visible = true;

            }
        }

        private void button登录_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Program.appConfig.Exists("凭据"))
                {
                    string[] ServerInfo = this.comboBox交易服务器.Text.Split(new char[] { ':' });

                    StringBuilder ErrInfo = new StringBuilder(256);
                    int ClientID = TdxApi.Logon(ServerInfo[1], short.Parse(ServerInfo[2]), this.textBox版本号.Text, this.YybID, this.AccountNo, this.AccountNo, this.textBox交易密码.Text, string.Empty, ErrInfo);
                    //int ClientID = TdxApi.Logon(ServerInfo[1], short.Parse(ServerInfo[2]), this.textBox版本号.Text, YybID, 8, AccountNo, this.AccountNo, this.textBox交易密码.Text, string.Empty, ErrInfo);
                    if (ClientID == -1)
                    {
                        MessageBox.Show(ErrInfo.ToString());
                        return;
                    }

                    TdxApi.Logoff(ClientID);

                    if (this.checkBox记住密码.Checked)
                    {
                        string 凭据 = string.Format("{0}|{1}|{2}", this.comboBox交易服务器.Text, this.textBox版本号.Text, this.textBox交易密码.Text);
                        Program.appConfig.SetValue("凭据", Cryptor.MD5Encrypt(凭据));
                    }

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else
                {
                    string 凭据 = Cryptor.MD5Decrypt(Program.appConfig.GetValue("凭据", string.Empty));

                    string[] Data = 凭据.Split('|');

                    string[] ServerInfo = Data[0].Split(new char[] { ':' });


                    StringBuilder ErrInfo = new StringBuilder(256);
                    int ClientID = TdxApi.Logon(ServerInfo[1], short.Parse(ServerInfo[2]), Data[1], this.YybID, this.AccountNo, this.AccountNo, Data[2], string.Empty, ErrInfo);
                    if (ClientID == -1)
                    {
                        MessageBox.Show(ErrInfo.ToString());
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    }
                    else
                    {
                        TdxApi.Logoff(ClientID);
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        
    }
}
