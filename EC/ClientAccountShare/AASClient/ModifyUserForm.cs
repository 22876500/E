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
    public partial class ModifyUserForm : Form
    {
        AASClient.AASServiceReference.DbDataSet.平台用户Row 平台用户;
        public ModifyUserForm(AASClient.AASServiceReference.DbDataSet.平台用户Row AASUser1)
        {
            InitializeComponent();

            this.平台用户 = AASUser1;
        }

        private void ModifyUserForm_Load(object sender, EventArgs e)
        {
            this.comboBox角色.DataSource = System.Enum.GetNames(typeof(角色));



            this.comboBox角色.SelectedIndex = this.平台用户.角色;

            this.textBox用户名.Text = this.平台用户.用户名;



            this.numericUpDown仓位限制.Value = this.平台用户.仓位限制;
            this.numericUpDown亏损限制.Value = this.平台用户.亏损限制;
            this.numericUpDown手续费率.Value = this.平台用户.手续费率;

            this.checkBox允许删除碎股订单.Checked = this.平台用户.允许删除碎股订单;

            this.groupBox1.Enabled = this.平台用户.角色 == (int)角色.交易员;

            this.groupBox2.Enabled = this.平台用户.角色 == (int)角色.普通风控员;

         
            string[] region = System.Enum.GetNames(typeof(分组));

            if (this.平台用户.角色 == (int)角色.交易员)
            {
                this.comboBox分组.DataSource = region.Skip(1).ToList();
                this.comboBox分组.SelectedIndex = this.平台用户.分组 - 1;
            }
            else
            {
                this.comboBox分组.DataSource = region;
                this.comboBox分组.SelectedIndex = this.平台用户.分组;
            }
        }



        private void button确定_Click(object sender, EventArgs e)
        {
            分组 region = (分组)Enum.Parse(typeof(分组), this.comboBox分组.SelectedItem.ToString(), false);
            Program.AASServiceClient.UpdateUser(this.平台用户.用户名, this.numericUpDown仓位限制.Value, this.numericUpDown亏损限制.Value, this.numericUpDown手续费率.Value, this.checkBox允许删除碎股订单.Checked, region);


            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }

      
    }
}
