using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.StockPosition
{
    public partial class PositionList : Form
    {
        AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend[] info = null;
        

        public PositionList()
        {
            InitializeComponent();

            this.Load += PositionList_Load;
            var thread = new Thread(RefreshData) { IsBackground = true };
            thread.Start();
        }

        private void PositionList_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void RefreshData()
        {
            while (true)
            {
                try
                {
                    if (!this.IsDisposed)
                    {
                        RefreshPositionList();
                        Thread.Sleep(3000);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                }
            }
        }

        private void PositionList_Load(object sender, EventArgs e)
        {
            labelLoading.Visible = true;
            dataGridView可用仓位.AutoGenerateColumns = false;
            Task.Run(() =>
            {
                info = Program.AASServiceClient.GetPositonLockedAll();

                dataGridView可用仓位.BeginInvoke(new Action(() =>
                {
                    //foreach (var item in info)
                    //{
                    //    dataGridView可用仓位.Rows.Add(item.组合号, item.证券代码, item.证券名称, item.剩余数量, item.剩余市值, item.昨收);
                    //}
                    BindFiltedData(info);
                    labelLoading.Visible = false;
                }));
            });

        }

        private void dataGridView可用仓位_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridView可用仓位.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                    //var o = dataGridView可用仓位.Rows[e.RowIndex].DataBoundItem as AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend;
                    var row = dataGridView可用仓位.Rows[e.RowIndex];
                    string accName = row.Cells[0].Value.ToString();
                    string stock = row.Cells[1].Value.ToString();

                    var infoItem = info.First(_=> _.组合号 == accName && _.证券代码 == stock);
                    var win = new StockPosition.LockPosition() { StartPosition = FormStartPosition.CenterParent };
                    win.Init(infoItem.组合号, infoItem.证券代码, infoItem.证券名称, infoItem.总仓位, infoItem.已用数量);

                    var result = win.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        RefreshPositionList();
                    }
                }
            }
        }

        private void RefreshPositionList()
        {
            Task.Run(() =>
            {
                try
                {
                    info = Program.AASServiceClient.GetPositonLockedAll();
                    var arr = info.ToArray();
                    dataGridView可用仓位.BeginInvoke(new Action(() =>
                    {
                        BindFiltedData(arr);
                        labelLoading.Visible = false;
                    }));
                }
                catch (Exception ex)
                {
                    if (labelLoading != null && labelLoading.IsAccessible)
                    {
                        labelLoading.BeginInvoke(new Action(() => { labelLoading.Visible = false; }));
                    }
                    Program.logger.LogInfo("刷新可用仓位异常!" + ex.Message);
                }
            });
        }

        private void BindFiltedData(AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend[] arr)
        {
            if (arr == null)
            {
                return;
            }

            if (textBox证券代码.Text.Length > 0)
            {
                arr = arr.Where(_ => _.证券代码.StartsWith(textBox证券代码.Text)).ToArray();
            }
            if (textBox证券名称.Text.Length > 0)
            {

                string strFL = textBox证券名称.Text;
                if (Regex.IsMatch(strFL, "^[A-Za-z]+$"))
                {
                    arr = arr.Where(_ => PinYin.PinYinConvertor.GetFirstPinyin(_.证券名称).StartsWith(strFL)).ToArray();
                }
                else
                {
                    arr = arr.Where(_ => _.证券名称.StartsWith(textBox证券名称.Text)).ToArray();
                }
            }

            for (int i = 0; i < arr.Length; i++)
            {
                var item = arr[i];
                if (dataGridView可用仓位.Rows.Count > i)
                {
                    dataGridView可用仓位.Rows[i].SetValues(item.组合号, item.证券代码, item.证券名称, item.剩余数量, item.剩余市值, item.昨收);
                }
                else
                {
                    dataGridView可用仓位.Rows.Add(item.组合号, item.证券代码, item.证券名称, item.剩余数量, item.剩余市值, item.昨收);
                }
            }
            while (dataGridView可用仓位.Rows.Count > arr.Length)
            {
                dataGridView可用仓位.Rows.RemoveAt(arr.Length);
            }
        }

        private void textBox证券代码_TextChanged(object sender, EventArgs e)
        {
            BindFiltedData(info);
        }

        private void textBox证券名称_TextChanged(object sender, EventArgs e)
        {
            BindFiltedData(info);
        }

        private void button刷新_Click(object sender, EventArgs e)
        {
            RefreshPositionList();
        }


        private void dataGridView可用仓位_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            var colName = e.Column.Name;
            if (colName.Contains("剩余仓位") || colName.Contains("剩余数量") || colName.Contains("市值") || colName.Contains("昨收"))
            {
                e.SortResult = (Convert.ToDouble(e.CellValue1) - Convert.ToDouble(e.CellValue2) > 0) ? 1 : (Convert.ToDouble(e.CellValue1) - Convert.ToDouble(e.CellValue2) < 0) ? -1 : 0;
            }
            else
            {
                e.SortResult = System.String.Compare(Convert.ToString(e.CellValue1), Convert.ToString(e.CellValue2));
            }
            e.Handled = true;
        }

        private void dataGridView可用仓位_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridView可用仓位.Columns[e.ColumnIndex];
                if (!(column is DataGridViewButtonColumn))
                {
                    var row = dataGridView可用仓位.Rows[e.RowIndex];
                    string stock = row.Cells[1].Value.ToString();
                    var parent = this.Owner as TradeMainForm;
                    if (parent != null)
                    {
                        parent.AddHqForm(stock);
                    }

                }
            }
        }
    }
}
