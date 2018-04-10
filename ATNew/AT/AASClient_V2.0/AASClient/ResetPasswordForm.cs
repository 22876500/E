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
    public partial class ResetPasswordForm : Form
    {
        AASClient.AASServiceReference.DbDataSet.平台用户Row userDataRow;
        public ResetPasswordForm(AASClient.AASServiceReference.DbDataSet.平台用户Row UserDataRow)
        {
            InitializeComponent();

            this.userDataRow = UserDataRow;
        }

        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            this.textBox用户名.Text = this.userDataRow.用户名;
        }

        private void button修改密码_Click(object sender, EventArgs e)
        {
            if (this.textBox密码.Text != this.textBox确认密码.Text)
            {
                MessageBox.Show("确认密码与密码不同");
                return;
            }


            Program.AASServiceClient.ResetPassword(this.userDataRow.用户名, this.textBox密码.Text);



            MessageBox.Show("重置密码成功");

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

       
    }
}
