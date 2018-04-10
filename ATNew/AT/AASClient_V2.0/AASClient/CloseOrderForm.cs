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
    public partial class CloseOrderForm : Form
    {
        AASClient.AASServiceReference.DbDataSet.订单Row 订单Row;
        public CloseOrderForm(AASClient.AASServiceReference.DbDataSet.订单Row 订单Row1)
        {
            InitializeComponent();

            this.订单Row = 订单Row1;
        }

        private void CloseOrderForm_Load(object sender, EventArgs e)
        {
            this.textBox交易员.Text = this.订单Row.交易员;
            this.textBox证券代码.Text = this.订单Row.证券代码;
            this.textBox证券名称.Text = this.订单Row.证券名称;
            this.comboBox开仓类别.SelectedIndex = this.订单Row.开仓类别;
            this.numericUpDown开仓价位.Value = this.订单Row.开仓价位;
            this.numericUpDown开仓数量.Value = this.订单Row.已开数量;
            this.numericUpDown平仓价位.Value = this.订单Row.开仓价位;
            this.numericUpDown平仓数量.Value = this.订单Row.已开数量;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.AASServiceClient.CloseOrder(this.订单Row.交易员,this.订单Row.组合号,this.订单Row.证券代码, this.numericUpDown平仓数量.Value, this.numericUpDown平仓价位.Value);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
