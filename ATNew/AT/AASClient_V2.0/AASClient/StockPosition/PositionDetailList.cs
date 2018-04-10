using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.StockPosition
{
    public partial class PositionDetailList : Form
    {
        AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend PositionLocked;

        public PositionDetailList()
        {
            InitializeComponent();
        }


        public void Init(AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend position)
        {
            PositionLocked = position;
            RefreshData(position);
        }

        private void RefreshData(AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend position)
        {
            //Task.Run(()=> {
            //    var dt = Program.AASServiceClient.GetPositionLockedDetail(position.组合号, position.证券代码);
            //});
            var task = Program.AASServiceClient.GetPositionLockedDetailAsync(position.组合号, position.证券代码);
            //dataGridView仓位占用详情.DataSource = dt.DefaultView;
            if (task.Result.GetPositionLockedDetailResult != null)
            {
                dataGridView仓位占用详情.DataSource = task.Result.GetPositionLockedDetailResult;
            } 

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (dataGridView仓位占用详情.SelectedRows != null && dataGridView仓位占用详情.SelectedRows.Count > 0)
            {
                string messageShow = string.Format("确认删除当前选定的{0}条额度分配记录吗？", dataGridView仓位占用详情.SelectedRows.Count);
                DialogResult result = MessageBox.Show(messageShow, "删除提示", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    bool hasChanged = false;
                    foreach (DataGridViewRow item in dataGridView仓位占用详情.SelectedRows)
                    {
                        var dataItem = ((System.Data.DataRowView)item.DataBoundItem).Row as AASServiceReference.DbDataSet.额度分配Row;
                        if (dataItem != null)
                        {
                            Program.AASServiceClient.DeleteTradeLimit(dataItem.交易员, dataItem.证券代码, dataItem.组合号);
                            hasChanged = true;
                        }
                    }

                    if (hasChanged)
                    {
                        RefreshData(PositionLocked);
                    }
                }
            }
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView仓位占用详情.SelectedRows != null && dataGridView仓位占用详情.SelectedRows.Count > 0)
            {
                bool hasChanged = false;
                foreach (DataGridViewRow item in dataGridView仓位占用详情.SelectedRows)
                {
                    var dataItem = ((System.Data.DataRowView)item.DataBoundItem).Row as AASServiceReference.DbDataSet.额度分配Row;
                    if (dataItem != null)
                    {
                        //修改需要加入界面
                        var win = new ModifyTradeLimitForm(dataItem);
                        win.ShowDialog();
                        RefreshData(PositionLocked);
                    }
                }

                if (hasChanged)
                {
                    RefreshData(PositionLocked);
                }
            }
        }
    }
}
