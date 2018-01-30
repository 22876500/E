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
    public partial class ModifyShortcutForm : Form
    {
        AASClient.AccountDataSet.快捷键Row 快捷键Row;
        public ModifyShortcutForm(AASClient.AccountDataSet.快捷键Row 快捷键Row1)
        {
            InitializeComponent();

            this.快捷键Row = 快捷键Row1;
        }

        private void ModifyShortcutForm_Load(object sender, EventArgs e)
        {
            this.comboBox方向.DataSource = System.Enum.GetNames(typeof(交易方向));
            this.comboBox价格.DataSource = System.Enum.GetNames(typeof(价格模式));
            this.comboBox价差模式.DataSource = System.Enum.GetNames(typeof(价差模式));
            this.comboBox股数模式.DataSource = System.Enum.GetNames(typeof(股数模式));


            this.textBox键名.Text = this.快捷键Row.键名;
            this.comboBox方向.SelectedIndex = (int)this.快捷键Row.方向;
            this.comboBox价格.SelectedIndex = (int)this.快捷键Row.价格模式;
            this.comboBox价差模式.SelectedIndex = (int)this.快捷键Row.价差模式;
            this.numericUpDown价差数值.Value = this.快捷键Row.价差数值;
            this.comboBox股数模式.SelectedIndex = (int)this.快捷键Row.股数模式;
            this.numericUpDown股数数值.Value = this.快捷键Row.股数数值;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AASClient.AccountDataSet.快捷键Row 快捷键Row1 = Program.accountDataSet.快捷键.First(r => r.键名 == this.textBox键名.Text);
            快捷键Row1.方向 = this.comboBox方向.SelectedIndex;
            快捷键Row1.价格模式 = this.comboBox价格.SelectedIndex;
            快捷键Row1.价差模式 = this.comboBox价差模式.SelectedIndex;
            快捷键Row1.价差数值 = this.numericUpDown价差数值.Value;
            快捷键Row1.股数模式 = this.comboBox股数模式.SelectedIndex;
            快捷键Row1.股数数值 = this.numericUpDown股数数值.Value;


            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void comboBox股数模式_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox股数模式.SelectedIndex > -1)
            {
                if (this.comboBox股数模式.SelectedItem.ToString() == System.Enum.GetName(typeof(股数模式), 股数模式.股数数值))
                {
                    this.numericUpDown股数数值.Maximum = decimal.MaxValue;
                }
                else
                {
                    this.numericUpDown股数数值.Maximum = 100;
                }
            }
        }

      
    }
}
