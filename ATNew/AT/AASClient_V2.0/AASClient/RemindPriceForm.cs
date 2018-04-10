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
    public partial class RemindPriceForm : Form
    {
        public RemindPriceForm()
        {
            InitializeComponent();
        }

        private void RemindPriceForm_Load(object sender, EventArgs e)
        {
            this.bindingSource1.DataSource = Program.accountDataSet.价格提示;
            this.dataGridView1.DataSource = this.bindingSource1;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "提示类型")
            {
                提示类型 提示类型1 = (提示类型)e.Value;


                e.Value = 提示类型1.ToString();
            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "提示等级")
            {
                提示等级 提示等级1 = (提示等级)e.Value;


                e.Value = 提示等级1.ToString();
            }
           
           
        }

        private void 新建提示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRemindForm AddRemindForm1 = new AddRemindForm();
            AddRemindForm1.ShowDialog();
            //this.bindingSource1.ResetBindings(false);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AASClient.AccountDataSet.价格提示Row 价格提示Row1 = (this.bindingSource1.Current as DataRowView).Row as AASClient.AccountDataSet.价格提示Row;
            Program.accountDataSet.价格提示.Remove价格提示Row(价格提示Row1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((sender as DataGridView).Columns[e.ColumnIndex].Name == "启用")
            {
                AASClient.AccountDataSet.价格提示Row 价格提示Row1 = (this.bindingSource1.Current as DataRowView).Row as AASClient.AccountDataSet.价格提示Row;

                价格提示Row1.启用 = !价格提示Row1.启用;
            }
        }

       
    }
}
