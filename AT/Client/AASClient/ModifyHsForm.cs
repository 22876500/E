using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class ModifyHsForm : Form
    {
        AASClient.AASServiceReference.DbDataSet.恒生帐户Row 恒生帐户Row;


        public ModifyHsForm(AASClient.AASServiceReference.DbDataSet.恒生帐户Row DataRow1)
        {
            InitializeComponent();

            this.恒生帐户Row = DataRow1;
        }

        private void ModifyHsForm_Load(object sender, EventArgs e)
        {
            this.textBox名称.Text = this.恒生帐户Row.名称;
            this.textBoxIP.Text = this.恒生帐户Row.IP;
            this.numericUpDown端口.Value = this.恒生帐户Row.端口;
            this.textBox基金编码.Text = this.恒生帐户Row.基金编码;
            this.textBox资产单元编号.Text = this.恒生帐户Row.资产单元编号;
            this.textBox组合编号.Text = this.恒生帐户Row.组合编号;
            this.textBox操作员用户名.Text = this.恒生帐户Row.操作员用户名;
            this.textBox操作员密码.Text = this.恒生帐户Row.操作员密码;
            this.textBox登录IP.Text = this.恒生帐户Row.登录IP;
            this.textBoxMAC.Text = this.恒生帐户Row.MAC;
            this.textBox硬盘序列号.Text = this.恒生帐户Row.HDD;
            this.numericUpDown查询时间间隔.Value = this.恒生帐户Row.查询间隔时间;
        }

        private void button测试登录_Click(object sender, EventArgs e)
        {
            string Ret = Program.AASServiceClient.TestHsAccount(this.textBoxIP.Text, (short)this.numericUpDown端口.Value, this.textBox操作员用户名.Text, this.textBox操作员密码.Text, this.textBox登录IP.Text, this.textBoxMAC.Text, this.textBox硬盘序列号.Text);
            if (Ret != string.Empty)
            {
                MessageBox.Show(Ret, "测试失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            MessageBox.Show("成功登录到恒生服务器", "测试成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button修改_Click(object sender, EventArgs e)
        {
            Program.AASServiceClient.UpdateHsAccount(this.恒生帐户Row.名称, this.textBoxIP.Text, (short)this.numericUpDown端口.Value, this.textBox基金编码.Text, this.textBox资产单元编号.Text, this.textBox组合编号.Text, this.textBox操作员用户名.Text, this.textBox操作员密码.Text, this.textBox登录IP.Text, this.textBoxMAC.Text, this.textBox硬盘序列号.Text, (int)this.numericUpDown查询时间间隔.Value);



            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
