using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.ECoinAccount
{
    public partial class formECoinAccountList : Form
    {
        public formECoinAccountList()
        {
            InitializeComponent();
            this.Load += Loaded;
        }

        private void Loaded(object sender, EventArgs e)
        {
            var table = Program.AASServiceClient.QueryECoinQsAccount();
            bindingSource电子币帐户.DataSource = table;
            this.dataGridView电子币帐户.DataSource = bindingSource电子币帐户;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView电子币帐户.SelectedRows)
            {
                
                var dataItem = (item.DataBoundItem as DataRowView).Row as AASServiceReference.DbDataSet.电子币帐户Row;
                if (dataItem != null)
                {
                    Program.AASServiceClient.DeleteECoinQSAccount(dataItem.名称);
                }
            }
            bindingSource电子币帐户.DataSource = Program.AASServiceClient.QueryECoinQsAccount();
        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new ECoinAccount.AddECoinAccount();
            win.ShowDialog();
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new ECoinAccount.AddECoinAccount();
            win.ShowDialog();
        }
    }
}
