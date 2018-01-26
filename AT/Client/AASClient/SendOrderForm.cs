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
    public partial class SendOrderForm : Form
    {
        //AASClient.AASServiceReference.DbDataSet.额度分配Row dataRow;
        public SendOrderForm(string 交易员, string 证券代码, string 证券名称)
        {
            InitializeComponent();

            this.textBox交易员.Text = 交易员;
            this.textBox证券代码.Text = 证券代码;
            this.label证券名称.Text = 证券名称;
            this.comboBox交易方向.SelectedIndex = 0;


            Program.TempZqdm = 证券代码;
        }

        private void SendOrderForm_Load(object sender, EventArgs e)
        {
           
           
        }

        private void SendOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.TempZqdm = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Program.HqDataTable.ContainsKey(this.textBox证券代码.Text))
            {
                if (this.numericUpDown价格.Value == 0)
                {
                    DataTable DataTable1 = Program.HqDataTable[this.textBox证券代码.Text];
                    DataRow DataRow1 = DataTable1.Rows[0];
                    this.numericUpDown价格.Value = decimal.Parse((DataRow1["现价"] as string));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal 委托价格 = Math.Round(this.numericUpDown价格.Value, L2Api.Get精度(this.textBox证券代码.Text), MidpointRounding.AwayFromZero);
            decimal 委托数量 = Math.Round(this.numericUpDown数量.Value, 0, MidpointRounding.AwayFromZero);


            string Ret = Program.AASServiceClient.SendOrder(this.textBox交易员.Text, this.textBox证券代码.Text, this.comboBox交易方向.SelectedIndex, 委托数量, 委托价格);
            string[] Data = Ret.Split('|');
            if (Data[1] != string.Empty)
            {
                MessageBox.Show(Data[1], "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



          


            MessageBox.Show("下单成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
           
        }

       
    }
}
