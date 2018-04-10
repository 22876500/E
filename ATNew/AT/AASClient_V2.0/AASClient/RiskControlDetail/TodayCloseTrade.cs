using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.RiskControlDetail
{
    public partial class TodayCloseTrade : Form
    {
        public TodayCloseTrade()
        {
            InitializeComponent();
            this.Load += TodayCloseTrade_Load;
        }

        private void TodayCloseTrade_Load(object sender, EventArgs e)
        {
            this.bindingSource当日统计.DataSource = Program.serverDb;
            this.bindingSource当日统计.DataMember = "已平仓订单";
            this.dataGridView当日平仓.DataSource = this.bindingSource当日统计;
            this.dataGridView当日平仓.Columns["组合号"].Visible = false;
            this.dataGridView当日平仓.Columns["平仓类别"].Visible = false;
            this.dataGridView当日平仓.Columns["开仓时间"].DefaultCellStyle.Format = "HH:mm:ss";
            this.dataGridView当日平仓.Columns["平仓时间"].DefaultCellStyle.Format = "HH:mm:ss";

            this.dataGridView当日平仓.Columns["已开数量"].DefaultCellStyle.Format = "f0";
            this.dataGridView当日平仓.Columns["已平数量"].DefaultCellStyle.Format = "f0";

            this.dataGridView当日平仓.Columns["毛利"].DefaultCellStyle.Format = "f2";
            this.dataGridView当日平仓.Columns["已开金额"].DefaultCellStyle.Format = "f2";
            this.dataGridView当日平仓.Columns["已平金额"].DefaultCellStyle.Format = "f2";
        }

        private void dataGridView当日平仓_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            decimal fdyk = (decimal)this.dataGridView当日平仓["毛利", e.RowIndex].Value;
            Color bg = fdyk > 0 ? System.Drawing.Color.Red : (fdyk < 0 ? System.Drawing.Color.Blue : Color.Black);
            this.dataGridView当日平仓.Rows[e.RowIndex].DefaultCellStyle.ForeColor = bg;
        }

        private void dataGridView当日平仓_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                return;
            }

            if (this.dataGridView当日平仓.Columns[e.ColumnIndex].Name == "开仓类别" || this.dataGridView当日平仓.Columns[e.ColumnIndex].Name == "平仓类别")
            {
                int int1 = (int)e.Value;
                switch (int1)
                {
                    case 0:
                        e.Value = "多";
                        break;
                    case 1:
                        e.Value = "空";
                        break;
                    case 2:
                        e.Value = "融资买入";
                        break;
                    case 3:
                        e.Value = "融券卖出";
                        break;
                    default:
                        e.Value = "X";
                        break;
                }
            }
        }

        private void dataGridView当日平仓_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Program.logger.LogInfo("dataGridView当日平仓_DataError: " + e.Exception.Message);
            e.ThrowException = false;
            e.Cancel = true;
        }
    }
}
