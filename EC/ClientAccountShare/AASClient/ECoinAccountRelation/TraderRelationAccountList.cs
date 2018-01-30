using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.ECoinAccountRelation
{
    public partial class TraderRelationAccountList : Form
    {
        AASServiceReference.DbDataSet.交易账户关联DataTable TableRelated = null;

        public TraderRelationAccountList()
        {
            InitializeComponent();
            Load += Form_Load;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            TableRelated = Program.AASServiceClient.QueryAccountRelation();
            bindingSource交易员关联帐户.DataSource = TableRelated;
            dataGridView交易员关联帐户.DataSource = bindingSource交易员关联帐户;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView交易员关联帐户.SelectedRows)
            {
                var dataItem = (item.DataBoundItem as DataRowView).Row as AASServiceReference.DbDataSet.交易账户关联Row;
                if (dataItem != null)
                {
                    Program.AASServiceClient.DeleteAccountRelation(dataItem.交易员);
                }
            }

            bindingSource交易员关联帐户.DataSource = Program.AASServiceClient.QueryAccountRelation();
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new AddTradeAccountRelation();
            win.ShowDialog();
        }
    }
}
