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
    public partial class 业绩统计Form : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public 业绩统计Form()
        {
            InitializeComponent();
        }

        private void 交易统计Form_Load(object sender, EventArgs e)
        {
            this.dataGridView交易统计.AutoGenerateColumns = false;

            this.bindingSource1.DataSource = Program.jyDataSet;
            this.bindingSource1.DataMember = "业绩统计";
            this.dataGridView交易统计.DataSource = this.bindingSource1;

            this.dataGridView交易统计.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));


            this.dataGridView交易统计.Columns["买入数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView交易统计.Columns["卖出数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView交易统计.Columns["买入金额"].DefaultCellStyle.Format = "f2";
            this.dataGridView交易统计.Columns["卖出金额"].DefaultCellStyle.Format = "f2";

            this.dataGridView交易统计.Columns["毛利"].DefaultCellStyle.Format = "f2";
            this.dataGridView交易统计.Columns["交易费用"].DefaultCellStyle.Format = "f2";
            this.dataGridView交易统计.Columns["净利润"].DefaultCellStyle.Format = "f2";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }

        private void dataGridView交易统计_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            decimal fdyk = (decimal)this.dataGridView交易统计["净利润", e.RowIndex].Value;

            if (fdyk > 0)
            {
                this.dataGridView交易统计.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
            else if (fdyk < 0)
            {
                this.dataGridView交易统计.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.dataGridView交易统计.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }
    }
}
