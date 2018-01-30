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
    public partial class SetDefaultQuantityForm : Form
    {
        string zqdm;
        public SetDefaultQuantityForm(string Zqdm)
        {
            InitializeComponent();

            this.zqdm = Zqdm;
        }

        private void SetDefaultQuantityForm_Load(object sender, EventArgs e)
        {
            this.Text = "设置" + this.zqdm;

            this.numericUpDown默认委托股数.Value = decimal.Parse(Program.accountDataSet.参数.GetParaValue(this.zqdm + "默认股数", "0"));


            this.numericUpDown最大委托金额.Value = decimal.Parse(Program.accountDataSet.参数.GetParaValue(this.zqdm + "最大金额", "0"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.accountDataSet.参数.SetParaValue(this.zqdm + "默认股数", this.numericUpDown默认委托股数.Value.ToString());

            Program.accountDataSet.参数.SetParaValue(this.zqdm + "最大金额", this.numericUpDown最大委托金额.Value.ToString());

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void numericUpDown默认委托股数_Enter(object sender, EventArgs e)
        {
            this.numericUpDown最大委托金额.Select(0, this.numericUpDown最大委托金额.Value.ToString().Length);
        }

        private void numericUpDown最大委托金额_Enter(object sender, EventArgs e)
        {
            this.numericUpDown默认委托股数.Select(0, this.numericUpDown默认委托股数.Value.ToString().Length);
        }
    }
}
