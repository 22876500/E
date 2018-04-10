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
    public partial class OrderLogDetail : Form
    {
        public OrderLogDetail()
        {
            InitializeComponent();
            this.Load += OrderLogDetail_Load;
        }

        private void OrderLogDetail_Load(object sender, EventArgs e)
        {
            //this.bindingSource委托记录.DataSource = Program.jyDataSet;
            //this.bindingSource委托记录.DataMember = "委托";
            bindingSource委托记录.DataSource = Program.jyDataSet.委托;
            this.dataGridView委托列表.DataSource = bindingSource委托记录;
        }

        private void dataGridView委托记录_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                int mmfx = (int)this.dataGridView委托列表["买卖方向", e.RowIndex].Value;
                Color bg = mmfx == 0 ? System.Drawing.Color.Red : (mmfx == 1 ? System.Drawing.Color.Blue : Color.Black);
                this.dataGridView委托列表.Rows[e.RowIndex].DefaultCellStyle.ForeColor = bg;
            }
            catch (Exception) { }
        }

        private void dataGridView委托记录_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Program.logger.LogInfo("dataGridView委托记录_DataError: " + e.Exception.Message);
            e.ThrowException = false;
            e.Cancel = true;
        }
    }
}
