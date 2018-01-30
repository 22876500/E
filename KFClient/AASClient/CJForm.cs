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
    public partial class CJForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {

        public CJForm()
        {
            InitializeComponent();


        }

        private void CJForm_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = Program.jyDataSet;
            this.bindingSource1.DataMember = "成交";
            this.dataGridView成交.DataSource = this.bindingSource1;


            this.dataGridView成交.Columns["交易员"].Visible = false;
            this.dataGridView成交.Columns["市场代码"].Visible = false;
            this.dataGridView成交.Columns["组合号"].Visible = false;
            this.dataGridView成交.Columns["成交编号"].Visible = false;
            this.dataGridView成交.Columns["委托编号"].Visible = false;


            this.dataGridView成交.Columns["成交数量"].DefaultCellStyle.Format = "f0";
        }

        private void dataGridView成交_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int mmfx = (int)this.dataGridView成交["买卖方向", e.RowIndex].Value;
            if (mmfx == 0)
            {
                this.dataGridView成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
            else if (mmfx == 1)
            {
                this.dataGridView成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.dataGridView成交.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
        }

        private void dataGridView成交_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
                    default:
                        break;

                }

            }
        }
    }
}
