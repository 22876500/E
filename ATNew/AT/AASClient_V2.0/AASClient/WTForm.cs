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
    public partial class WTForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {


        public WTForm()
        {
            InitializeComponent();

        }

        private void WTForm_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = Program.jyDataSet;
            this.bindingSource1.DataMember = "委托";



            this.dataGridView委托.DataSource = this.bindingSource1;

            this.dataGridView委托.Columns["交易员"].Visible = false;
            this.dataGridView委托.Columns["市场代码"].Visible = false;
            this.dataGridView委托.Columns["组合号"].Visible = false;
            this.dataGridView委托.Columns["委托编号"].Visible = false;

            this.dataGridView委托.Columns["委托数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView委托.Columns["成交数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView委托.Columns["撤单数量"].DefaultCellStyle.Format = "f0";
        }

        private void dataGridView委托_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int mmfx = (int)this.dataGridView委托["买卖方向", e.RowIndex].Value;
            if (mmfx == 0)
            {
                this.dataGridView委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
            else if (mmfx == 1)
            {
                this.dataGridView委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.dataGridView委托.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void dataGridView委托_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "买卖方向")
            {
                int int1 = (int)e.Value;

                switch (int1)
                {
                    case 0:
                        e.Value = "买入";
                        break;
                    case 1:
                        e.Value = "卖出";
                        break;
                    case 2:
                        e.Value = "融资买入";
                        break;
                    case 3:
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;

                }

            }
        }


        private void dataGridView委托_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string orderID = this.dataGridView委托.Rows[e.RowIndex].Cells["委托编号"].Value + "";
                string code = this.dataGridView委托.Rows[e.RowIndex].Cells["证券代码"].Value + "";
                decimal price = decimal.Parse(this.dataGridView委托.Rows[e.RowIndex].Cells["委托价格"].Value + "");
                decimal qty = decimal.Parse(this.dataGridView委托.Rows[e.RowIndex].Cells["委托数量"].Value + "");
                if (code.Length == 5)
                {
                    var winUpdate = new UpdateOrderForm();
                    winUpdate.Init(orderID, code, price, qty);
                    winUpdate.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("WTForm.dataGridView委托_CellDoubleClick 获取订单信息时发生异常," + ex.Message);
            }
            
            
        }
    }
}
