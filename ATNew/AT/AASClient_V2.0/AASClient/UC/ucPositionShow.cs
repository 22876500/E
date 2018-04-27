using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace AASClient.UC
{
    public partial class ucPositionShow : System.Windows.Forms.UserControl
    {
        AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend[] Arr;
        //BindingList<AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend> lstFiltedBind = new BindingList<AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend>();
        DataTable dtBind = new DataTable();

        public ucPositionShow()
        {
            InitializeComponent();
            this.Load += UcPositionShow_Load;
        }

        private void UcPositionShow_Load(object sender, EventArgs e)
        {
            dtBind.Columns.Add("组合号");
            dtBind.Columns.Add("证券代码");
            dtBind.Columns.Add("证券名称");
            dtBind.Columns.Add("总仓位", typeof(System.Decimal));
            dtBind.Columns.Add("已用数量", typeof(System.Decimal));
            dtBind.Columns.Add("剩余数量", typeof(System.Decimal));
            dtBind.Columns.Add("剩余市值", typeof(System.Decimal));
            dtBind.Columns.Add("昨收", typeof(System.Decimal));


            dataGridView可用仓位.AutoGenerateColumns = false;
            dataGridView可用仓位.DataSource = bindingSource可用仓位;

            RefreshDataAsync();
            var thread = new Thread(RefreshPageData) { IsBackground = true };
            thread.Start();
        }

        private void RefreshPageData()
        {
            while (true)
            {
                if (!this.IsDisposed)
                {
                    RefreshDataAsync();
                    Thread.Sleep(10000);
                }
                else
                {
                    break;
                }
            }
        }

        private void buttonFilte_Click(object sender, EventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            List<AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend> lstFilted = null;
            if (string.IsNullOrEmpty(textBox券商账户过滤.Text) && string.IsNullOrEmpty(textBox证券代码过滤.Text))
            {
                 lstFilted = Arr.ToList();
            }
            else if (string.IsNullOrEmpty(textBox券商账户过滤.Text))
            {
                lstFilted = Arr.Where(_ => _.证券代码.Contains(textBox证券代码过滤.Text)).ToList();
            }
            else if (string.IsNullOrEmpty(textBox证券代码过滤.Text))
            {
                lstFilted = Arr.Where(_ => _.组合号.Contains(textBox券商账户过滤.Text)).ToList();
            }
            else
            {
                lstFilted = Arr.Where(_ => _.组合号.Contains(textBox券商账户过滤.Text) && _.证券代码.Contains(textBox证券代码过滤.Text)).ToList();
            }

            try
            {
                Stack<DataRow> stackNeedRemove = new Stack<DataRow>();
                foreach (DataRow item in dtBind.Rows)
                {
                    if (item != null)
                    {
                        string acc = item["组合号"] + "";
                        string stock = item["证券代码"] + "";
                        var existItem = lstFilted.FirstOrDefault(_ => _.组合号 == acc && _.证券代码 == stock);
                        if (existItem == null)
                        {
                            stackNeedRemove.Push(item);
                        }
                        else
                        {
                            if ((item["总仓位"] + "") != existItem.总仓位.ToString())
                            {
                                item["总仓位"] = existItem.总仓位;
                            }
                            if ((item["已用数量"] + "") != existItem.已用数量.ToString())
                            {
                                item["已用数量"] = existItem.已用数量;
                            }
                            if ((item["剩余数量"] + "") != existItem.剩余数量.ToString())
                            {
                                item["剩余数量"] = existItem.剩余数量;
                            }
                            if ((item["剩余市值"] + "") != existItem.剩余市值.ToString())
                            {
                                item["剩余市值"] = existItem.剩余市值;
                            }
                            lstFilted.Remove(existItem);
                        }
                    }
                }
                foreach (var item in stackNeedRemove)
                {
                    dtBind.Rows.Remove(item);
                }
                foreach (var item in lstFilted)
                {
                    dtBind.Rows.Add(item.组合号, item.证券代码, item.证券名称, item.已用数量, item.剩余数量, item.剩余市值, item.昨收);
                }
                if (bindingSource可用仓位.DataSource == null)
                {
                    bindingSource可用仓位.DataSource = dtBind;
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("ucPositionShow FilterData exeption:" + ex.Message);
            }
            
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeletePositions();

        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPosition();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditPosition();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            StockPosition.PositionImport win = new StockPosition.PositionImport();
            win.ShowDialog();
            RefreshDataAsync();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeletePositions();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditPosition();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddPosition();
        }

        private void dataGridView可用仓位_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridView可用仓位.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                    try
                    {
                        string account = dataGridView可用仓位.Rows[e.RowIndex].Cells["Column组合号"].Value +"";
                        string stock = dataGridView可用仓位.Rows[e.RowIndex].Cells["Column证券代码"].Value + "";
                        var o = Arr.First(_ => _.组合号 == account && _.证券代码 == stock);
                        var win = new StockPosition.PositionDetailList() { StartPosition = FormStartPosition.CenterParent };
                        win.Init(o);
                        win.Show(this);
                        RefreshDataAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void DeletePositions()
        {
            if (dataGridView可用仓位.SelectedRows != null && dataGridView可用仓位.SelectedRows.Count > 0)
            {
                var result=  MessageBox.Show(string.Format("确认删除{0}条记录？", dataGridView可用仓位.SelectedRows.Count), 
                    "删除公共券池确认", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                labelPositionLoading.Visible = true;
                labelPositionLoading.Text = "开始删除!";
                bool hasChanged = false;
                foreach (DataGridViewRow item in dataGridView可用仓位.SelectedRows)
                {
                    string account = item.Cells["Column组合号"].Value + "";
                    string stock = item.Cells["Column证券代码"].Value + "";
                    var dataItem = Arr.First(_ => _.组合号 == account && _.证券代码 == stock);

                    //删除可用仓位数据，接口未加入
                    if (dataItem != null)
                    {
                        Program.AASServiceClient.Delete可用仓位Async(dataItem.组合号, dataItem.证券代码);
                        labelPositionLoading.Text = string.Format("正在删除仓位 组合号{0}, 证券代码 {1}", dataItem.组合号, dataItem.证券代码);
                        hasChanged = true;
                    }
                }

                labelPositionLoading.Text = "删除完毕!";
                labelPositionLoading.Visible = false;
                labelPositionLoading.Text = "加载中";
                if (hasChanged)
                {
                    RefreshDataAsync();
                }
            }
        }

        private void EditPosition()
        {
            if (dataGridView可用仓位.SelectedRows != null && dataGridView可用仓位.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView可用仓位.SelectedRows[0];
                string account = row.Cells["Column组合号"].Value + "";
                string stock = row.Cells["Column证券代码"].Value + "";

                var updateItem = Arr.First(_ => _.组合号 == account && _.证券代码 == stock);
                var window = new AASClient.StockPosition.StockPositionEdit() { StartPosition = FormStartPosition.CenterParent };
                window.Init(updateItem);
                window.ShowDialog(this);
                RefreshDataAsync();
            }
            else
            {
                //MessageBox.Show("请选中一行再进行编辑!");
            }
        }

        private void AddPosition()
        {
            var window = new AASClient.StockPosition.StockPositionEdit() { StartPosition = FormStartPosition.CenterParent }; ;
            window.ShowDialog(this);
            RefreshDataAsync();
        }

        private void dataGridView可用仓位_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            EditPosition();
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataAsync();
        }

        private void RefreshDataAsync()
        {
            Task.Run(() =>
            {
                try
                {
                    Arr = Program.AASServiceClient.GetPositonLockedAll();
                    this.Invoke(new Action(FilterData));
                }
                catch (Exception) { }
            });
        }

        private void textBox券商账户过滤_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }

        private void textBox证券代码过滤_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }
    }
}
