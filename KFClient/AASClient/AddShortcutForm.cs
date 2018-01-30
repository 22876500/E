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
    enum 交易方向 { 买入,卖出,撤单,全撤,取消, 修改默认, 券池融资买入, 券池融券卖出, 券池现金买入 }
    public enum 价格模式 { 卖五价, 卖四价, 卖三价, 卖二价, 卖一价, 买一价, 买二价, 买三价, 买四价, 买五价, 现价, 涨停价, 跌停价, 不处理 }
    enum 价差模式 { 百分之, 数值}
    enum 股数模式 { 可用股数的百分之, 仓位的百分之, 默认值的百分之, 股数数值, 不处理}
    public partial class AddShortcutForm : Form
    {
        public AddShortcutForm()
        {
            InitializeComponent();
        }

        private void AddShortcutForm_Load(object sender, EventArgs e)
        {
            this.comboBox方向.DataSource = System.Enum.GetNames(typeof(交易方向));
            this.comboBox价格.DataSource = System.Enum.GetNames(typeof(价格模式));
            this.comboBox价差模式.DataSource = System.Enum.GetNames(typeof(价差模式));
            this.comboBox股数模式.DataSource = System.Enum.GetNames(typeof(股数模式));
        }

        private void textBox键名_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;//表示取消输入
        }

        private void textBox键名_KeyDown(object sender, KeyEventArgs e)
        {
            this.textBox键名.Text = e.KeyCode.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox键名.Text==string.Empty)
            {
                MessageBox.Show("键名不能为空");
                return;
            }
            if (Program.accountDataSet.快捷键.Any(r => r.键名 == this.textBox键名.Text))
            {
                MessageBox.Show("不能重复添加相同快捷键");
                return;
            }


            AASClient.AccountDataSet.快捷键Row 快捷键Row1 = Program.accountDataSet.快捷键.New快捷键Row();
            快捷键Row1.键名 = this.textBox键名.Text;
            快捷键Row1.方向 = this.comboBox方向.SelectedIndex;
            快捷键Row1.价格模式 = this.comboBox价格.SelectedIndex;
            快捷键Row1.价差模式 = this.comboBox价差模式.SelectedIndex;
            快捷键Row1.价差数值 = this.numericUpDown价差数值.Value;
            快捷键Row1.股数模式 = this.comboBox股数模式.SelectedIndex;
            快捷键Row1.股数数值 = this.numericUpDown股数数值.Value;
            Program.accountDataSet.快捷键.Add快捷键Row(快捷键Row1);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
