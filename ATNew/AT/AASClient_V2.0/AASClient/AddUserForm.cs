using AASClient.AASServiceReference;
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
    public partial class AddUserForm : Form
    {
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {

            if (Program.Current平台用户.角色 == (int)角色.普通管理员)
            {
                string[] Data = System.Enum.GetNames(typeof(角色));
                if (Program.Current平台用户.分组 == (int)分组.ALL)
                {
                    this.comboBox角色.DataSource = new string[] { Data[1], Data[3], Data[4] };
                }
                else
                {
                    this.comboBox角色.DataSource = new string[] { Data[3], Data[4] };
                }
            }
            else
            {
                this.comboBox角色.DataSource = System.Enum.GetNames(typeof(角色));
            }

            string[] region = System.Enum.GetNames(typeof(分组));
            if (Program.Current平台用户.分组 != (int)分组.ALL)
            {
                this.comboBox分组.DataSource = new string[] { region[Program.Current平台用户.分组] };
            }
            else
            {
                if (Program.Current平台用户.角色 == (int)角色.普通管理员)
                {
                    this.comboBox分组.DataSource = region.Skip(1).ToList();
                }
                else
                {
                    this.comboBox分组.DataSource = region;
                }
            }

        }

        private void AddUserForm_Activated(object sender, EventArgs e)
        {
            this.textBox用户名.Focus();
        }

        private void comboBox角色_SelectedIndexChanged(object sender, EventArgs e)
        {
            角色 角色1 = (角色)Enum.Parse(typeof(角色), this.comboBox角色.SelectedItem.ToString(), false);


            this.groupBox1.Enabled = 角色1 == 角色.交易员;

            this.groupBox2.Enabled = 角色1 == 角色.普通风控员;

            if (角色1 == 角色.超级管理员)
            {
                this.comboBox分组.DataSource = System.Enum.GetNames(typeof(分组));
                this.comboBox分组.Text = 分组.ALL.ToString();
                this.comboBox分组.Enabled = false;
            }
            else if (角色1 == 角色.交易员)
            {
                if (this.comboBox分组.Enabled == false) this.comboBox分组.Enabled = true;
                this.comboBox分组.DataSource = System.Enum.GetNames(typeof(分组)).Skip(1).ToList();
            }
            else
            {
                if (this.comboBox分组.Enabled == false) this.comboBox分组.Enabled = true;
                this.comboBox分组.DataSource = System.Enum.GetNames(typeof(分组));
            }

        }

        private void button确定_Click(object sender, EventArgs e)
        {
            if (this.textBox密码.Text != this.textBox确认密码.Text)
            {
                MessageBox.Show("确认密码与密码不同");
                return;
            }




            角色 角色1 = (角色)Enum.Parse(typeof(角色), this.comboBox角色.SelectedItem.ToString(), false);

            分组 region = (分组)Enum.Parse(typeof(分组), this.comboBox分组.SelectedItem.ToString(), false);

            Program.AASServiceClient.AddUser(this.textBox用户名.Text, this.textBox密码.Text, 角色1, this.numericUpDown仓位限制.Value, this.numericUpDown亏损限制.Value, this.numericUpDown手续费率.Value, this.checkBox允许删除碎股订单.Checked, region);


            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }

    }
}
