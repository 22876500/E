using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASServer
{
    public partial class AddUserForm : Form
    {
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void button确定_Click(object sender, EventArgs e)
        {
            if (this.textBox密码.Text != this.textBox确认密码.Text)
            {
                MessageBox.Show("确认密码与密码不同");
                return;
            }



            Program.db.平台用户.AddUser(this.textBox用户名.Text, this.textBox密码.Text, (int)角色.超级管理员, 0m, 0m, 0m, false, 分组.ALL);
          
              


            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }
    }
}
