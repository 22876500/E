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
    public partial class AddMACForm : Form
    {
        string UserName;
        public AddMACForm(string 用户名1)
        {
            InitializeComponent();

            this.UserName = 用户名1;
        }

        private void AddMACForm_Load(object sender, EventArgs e)
        {
            this.textBox用户名.Text = this.UserName;
        }

        private void AddMACForm_Activated(object sender, EventArgs e)
        {
            this.textBoxMAC.Focus();
        }

        private void textBoxMAC_Leave(object sender, EventArgs e)
        {
            this.textBoxMAC.Text = this.textBoxMAC.Text.Replace("-", string.Empty).ToUpper();//00 21 5C 03 41 99
        }

        private void button添加_Click(object sender, EventArgs e)
        {
            Program.AASServiceClient.AddMAC(this.UserName, this.textBoxMAC.Text);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
