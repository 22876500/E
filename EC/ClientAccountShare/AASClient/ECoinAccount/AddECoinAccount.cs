using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.ECoinAccount
{
    public partial class AddECoinAccount : Form
    {
        AASServiceReference.DbDataSet.电子币帐户Row Account;


        public AddECoinAccount()
        {
            InitializeComponent();
        }

        public void Init(AASServiceReference.DbDataSet.电子币帐户Row row)
        {
            this.Text = "电子币账户编辑";
            Account = row;
            textBox名称.Enabled = false;
            textBox交易平台.Text = row.交易平台;
            textBox名称.Text = row.名称;
            textBox登录帐号.Text = row.登录帐号;
            textBoxApiKey.Text = row.ApiKey;
            textBoxSecretKey.Text = row.SecretKey;
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //需要进行验证吗？
            try
            {
                if (Account == null)
                {
                    Program.AASServiceClient.AddECoinQSAccount(checkBox启用.Checked, textBox名称.Text, textBox交易平台.Text, textBox登录帐号.Text, textBoxApiKey.Text, textBoxSecretKey.Text);
                    MessageBox.Show("电子币帐户添加完毕!");
                }
                else
                {
                    Program.AASServiceClient.UpdateECoinQSAccount(textBox名称.Text, textBox交易平台.Text, textBox登录帐号.Text, textBoxApiKey.Text, textBoxSecretKey.Text);
                    MessageBox.Show("电子币帐户修改完毕!");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("电子币帐户添加异常!{0}{1}", Environment.NewLine, ex.Message));
            }
            
            
        }
    }
}
