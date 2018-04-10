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
    public partial class ModifyPasswordForm : Form
    {
        public ModifyPasswordForm()
        {
            InitializeComponent();
        }

        private void button修改密码_Click(object sender, EventArgs e)
        {
            //if (this.textBox密码.Text == string.Empty)
            //{
            //    MessageBox.Show("密码不能为空");
            //    return;
            //}

            //if (this.textBox密码.Text != this.textBox确认密码.Text)
            //{
            //    MessageBox.Show("确认密码与密码不同");
            //    return;
            //}


            //string Msg = Program.AASServiceClient.ModifyPassword(this.textBox原密码.Text, this.textBox密码.Text);
            //if (Msg!=string.Empty)
            //{
            //    MessageBox.Show(Msg);
            //    return;
            //}


            //MessageBox.Show("密码修改成功");

            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
