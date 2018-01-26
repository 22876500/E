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
    public partial class ClosedTradeForm : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public ClosedTradeForm()
        {
            InitializeComponent();
        }

        private void ClosedTradeForm_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = Program.serverDb;
            this.bindingSource1.DataMember = "已平仓订单";
            this.dataGridView已平仓订单.DataSource = this.bindingSource1;


            this.dataGridView已平仓订单.Columns["交易员"].Visible = false;
            this.dataGridView已平仓订单.Columns["组合号"].Visible = false;
            this.dataGridView已平仓订单.Columns["平仓类别"].Visible = false;


            this.dataGridView已平仓订单.Columns["开仓时间"].DefaultCellStyle.Format = "HH:mm:ss";
            this.dataGridView已平仓订单.Columns["平仓时间"].DefaultCellStyle.Format = "HH:mm:ss";

            this.dataGridView已平仓订单.Columns["已开数量"].DefaultCellStyle.Format = "f4";
            this.dataGridView已平仓订单.Columns["已平数量"].DefaultCellStyle.Format = "f4";

            this.dataGridView已平仓订单.Columns["毛利"].DefaultCellStyle.Format = "f8";
            this.dataGridView已平仓订单.Columns["已开金额"].DefaultCellStyle.Format = "f8";
            this.dataGridView已平仓订单.Columns["已平金额"].DefaultCellStyle.Format = "f8";
        }

        private void dataGridView已平仓订单_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            if (this.dataGridView已平仓订单.Columns[e.ColumnIndex].Name == "开仓类别" || this.dataGridView已平仓订单.Columns[e.ColumnIndex].Name == "平仓类别")
            {
                int int1 = (int)e.Value;
                switch (int1)
                {
                    case 0:
                    case 2:
                        e.Value = "多";
                        break;
                    case 1:
                    case 3:
                        e.Value = "空";
                        break;
                    default:
                        e.Value = "X";
                        break;
                }
            }
        }

        private void dataGridView已平仓订单_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            decimal fdyk = (decimal)this.dataGridView已平仓订单["毛利", e.RowIndex].Value;

            if (fdyk > 0)
            {
                this.dataGridView已平仓订单.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            }
            else if (fdyk < 0)
            {
                this.dataGridView已平仓订单.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
            }
            else
            {
                this.dataGridView已平仓订单.Rows[e.RowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void dataGridView已平仓订单_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Program.logger.LogInfo(string.Format("ClosedTradeForm DataError {0}", (e.Exception.InnerException ?? e.Exception).Message));
        }
    }
}
