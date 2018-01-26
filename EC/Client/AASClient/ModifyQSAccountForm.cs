using AASClient.AASServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class ModifyQSAccountForm : Form
    {
        AASClient.AASServiceReference.DbDataSet.券商帐户Row 券商帐户;
        public ModifyQSAccountForm(AASClient.AASServiceReference.DbDataSet.券商帐户Row QSAccount1)
        {
            InitializeComponent();

            this.券商帐户 = QSAccount1;
        }

        private void ModifyQSAccountForm_Load(object sender, EventArgs e)
        {
            this.textBox帐户名称.Text = this.券商帐户.名称;
            this.comboBox券商.Text = this.券商帐户.券商;
            this.comboBox类型.Text = this.券商帐户.类型;

            this.comboBox交易服务器.DataSource = Program.AASServiceClient.Get交易服务器(this.券商帐户.券商);
            this.comboBox交易服务器.Text = this.券商帐户.交易服务器;



            this.textBox版本号.Text = this.券商帐户.版本号;
            this.numericUpDown营业部代码.Value = this.券商帐户.营业部代码;
            this.textBox登录帐号.Text = this.券商帐户.登录帐号;
            this.textBox交易帐号.Text = this.券商帐户.交易帐号;
            this.textBox交易密码.Text = this.券商帐户.交易密码;
            this.textBox通讯密码.Text = this.券商帐户.通讯密码;

            this.textBox上海股东代码.Text = this.券商帐户.上海股东代码;
            this.textBox深圳股东代码.Text = this.券商帐户.深圳股东代码;

            this.numericUpDown查询时间间隔.Value = this.券商帐户.查询间隔时间;
          
        }

        private void comboBox券商_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }


        private void button测试帐号_Click(object sender, EventArgs e)
        {
            string[] JyServerInfo = this.comboBox交易服务器.Text.Split(new char[] { ':' });

            string Ret = Program.AASServiceClient.Test(JyServerInfo[1], short.Parse(JyServerInfo[2]), this.textBox版本号.Text, (short)this.numericUpDown营业部代码.Value, this.textBox登录帐号.Text, this.textBox交易帐号.Text, this.textBox交易密码.Text, this.textBox通讯密码.Text);
            string[] Data = Ret.Split('|');
            if (Data[1] != string.Empty)
            {
                MessageBox.Show(Data[1], "测试失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            this.dataGridView股东列表.DataSource = Tool.ChangeDataStringToTable(Data[0]);

        }

        private void button保存修改_Click(object sender, EventArgs e)
        {
            Program.AASServiceClient.UpdateQSAccount(this.textBox帐户名称.Text, this.comboBox交易服务器.Text, this.textBox版本号.Text, this.textBox交易密码.Text, this.textBox通讯密码.Text, (int)this.numericUpDown查询时间间隔.Value);



            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        
    }
}
