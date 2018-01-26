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
    public partial class AddHsAccountForm : Form
    {
        public AddHsAccountForm()
        {
            InitializeComponent();
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


        private void button添加_Click(object sender, EventArgs e)
        {


            Program.AASServiceClient.AddHsAccount(this.checkBox启用.Checked, this.textBox名称.Text, this.textBoxIP.Text, (short)this.numericUpDown端口.Value, this.textBox基金编码.Text, this.textBox资产单元编号.Text, this.textBox组合编号.Text, this.textBox操作员用户名.Text, this.textBox操作员密码.Text, this.textBox登录IP.Text, this.textBoxMAC.Text, this.textBox硬盘序列号.Text, (int)this.numericUpDown查询时间间隔.Value);



            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

       
    }
}
