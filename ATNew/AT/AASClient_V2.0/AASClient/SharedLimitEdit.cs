using AASClient.AASServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    public partial class SharedLimitEdit : Form
    {
        ShareLimitGroupItem groupInfo;
        public SharedLimitEdit()
        {
            InitializeComponent();
        }

        public void Init(string groupName)
        {
            comboBoxBuy.SelectedIndex = 0;
            comboBoxSale.SelectedIndex = 0;
            var groups = Program.AASServiceClient.QueryQsAccount();
            foreach (DataRow item in groups.Rows)
            {
                comboBoxGroups.Items.Add(item["名称"]);
            }

            this.labelShareLimitGroup.Text = groupName;
            var group = Program.AASServiceClient.ShareGroupQuery().First(_ => _.GroupName == groupName);
            this.groupInfo = group;

            this.bindingSourceTraderNotIn.DataSource = Program.AASServiceClient.QueryNotGroupedTrader();
            this.bindingSourceTraderIn.DataSource = group.GroupTraderList;
            this.bindingSourceStocksIn.DataSource = group.GroupStockList.OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID).ToArray();

            this.dataGridViewGrouped.AutoGenerateColumns = false;
            this.dataGridViewNotGrouped.AutoGenerateColumns = false;
            this.dataGridViewStocks.AutoGenerateColumns = false;

            this.dataGridViewNotGrouped.DataSource = bindingSourceTraderNotIn;
            this.dataGridViewGrouped.DataSource = bindingSourceTraderIn;
            this.dataGridViewStocks.DataSource = bindingSourceStocksIn;

        }

        private void pictureBoxAdd_Click(object sender, EventArgs e)
        {
            var selectedItem = dataGridViewNotGrouped.SelectedRows;
            if (selectedItem.Count > 0)
            {
                foreach (DataGridViewRow item in selectedItem)
                {
                    var name = item.Cells[0].Value.ToString();
                    Program.AASServiceClient.AddTrader(this.labelShareLimitGroup.Text, name);
                }
            }
            else if (dataGridViewNotGrouped.CurrentRow != null)
            {
                var name = dataGridViewNotGrouped.CurrentRow.Cells[0].Value.ToString();
                Program.AASServiceClient.AddTrader(this.labelShareLimitGroup.Text, name);
            }
            this.bindingSourceTraderNotIn.DataSource = Program.AASServiceClient.QueryNotGroupedTrader();
            this.bindingSourceTraderIn.DataSource = Program.AASServiceClient.ShareGroupQuery().First(_ => _.GroupName == this.labelShareLimitGroup.Text).GroupTraderList;
        }

        private void pictureBoxRemove_Click(object sender, EventArgs e)
        {
            var selectedRows = dataGridViewGrouped.SelectedRows;
            if (selectedRows.Count > 0)
            {
                foreach (DataGridViewRow item in selectedRows)
                {
                    var name = item.Cells[0].Value.ToString();
                    Program.AASServiceClient.RemoveGroupTrader(this.labelShareLimitGroup.Text, name);
                }
            }
            else if (dataGridViewGrouped.CurrentRow != null)
            {
                var name = dataGridViewGrouped.CurrentRow.Cells[0].Value.ToString();
                Program.AASServiceClient.RemoveGroupTrader(this.labelShareLimitGroup.Text, name);

            }

            this.bindingSourceTraderNotIn.DataSource = Program.AASServiceClient.QueryNotGroupedTrader();
            this.bindingSourceTraderIn.DataSource = Program.AASServiceClient.ShareGroupQuery().First(_ => _.GroupName == this.labelShareLimitGroup.Text).GroupTraderList;
        }

        private void buttonAddStock_Click_1(object sender, EventArgs e)
        {
            var buyType = comboBoxBuy.SelectedIndex == 0 ? "0" : "2";
            var saleType = comboBoxSale.SelectedIndex == 0 ? "1" : "3";
            var stock = new StockLimitItem()
            {
                StockID = textBoxStockCode.Text,
                StockName = textBoxStockName.Text,
                LimitCount = numericUpDownLimit.Value.ToString(),
                GroupAccount = comboBoxGroups.SelectedItem.ToString(),
                Commission = numericUpDownCommission.Value.ToString(),
                BuyType = buyType,
                SaleType = saleType
            };
            string result = Program.AASServiceClient.AddStock(labelShareLimitGroup.Text, stock);
            if (result.StartsWith("0|"))
            {
                var group = Program.AASServiceClient.ShareGroupQuery().First(_ => _.GroupName == labelShareLimitGroup.Text);
                this.bindingSourceStocksIn.DataSource = group.GroupStockList.OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID);

                textBoxStockCode.Text = string.Empty;
                textBoxStockName.Text = string.Empty;
                numericUpDownLimit.Value = 0;
                comboBoxGroups.SelectedIndex = -1;
                numericUpDownCommission.Value = 0;
                comboBoxBuy.SelectedIndex = 0;
                comboBoxSale.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(result.Substring(2));
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedRows = dataGridViewStocks.SelectedRows;
                if (selectedRows.Count > 0)
                {
                    foreach (DataGridViewRow rowItem in selectedRows)
                    {
                        var result = Program.AASServiceClient.RemoveStock(labelShareLimitGroup.Text, rowItem.Cells["StockID"].Value.ToString());
                    }
                    var group = Program.AASServiceClient.ShareGroupQuery().First(_ => _.GroupName == labelShareLimitGroup.Text);
                    this.bindingSourceStocksIn.DataSource = group.GroupStockList;
                }
                else if (dataGridViewStocks.CurrentRow != null)
                {
                    var StockID = dataGridViewStocks.CurrentRow.Cells["StockID"].Value.ToString();
                    var result = Program.AASServiceClient.RemoveStock(labelShareLimitGroup.Text, StockID);
                    if (result.StartsWith("0|"))
                    {
                        //MessageBox.Show("删除成功");
                        var group = Program.AASServiceClient.ShareGroupQuery().First(_ => _.GroupName == labelShareLimitGroup.Text);
                        this.bindingSourceStocksIn.DataSource = group.GroupStockList;
                    }
                    else
                    {
                        MessageBox.Show(result.Substring(2));
                    }
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("删除异常" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            
            string[] FileContent = File.ReadAllLines(dialog.FileName, Encoding.Default);
            ImportToServer(FileContent);
        }

        private void ImportToServer(string[] FileContent)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in FileContent)
            {
                if (item.Contains("组合号"))
                {
                    continue;
                }
                try
                {
                    var members = Regex.Split(item, "[,，]+");
                    买模式 买模式1 = (买模式)Enum.Parse(typeof(买模式), members[6], false);
                    卖模式 卖模式1 = (卖模式)Enum.Parse(typeof(卖模式), members[7], false);
                    var stock = new StockLimitItem()
                    {
                        StockID = members[1],
                        GroupAccount = members[2],
                        //市场代码自动计算，不导入。
                        StockName = members[4],
                        //拼音缩写不导入
                        BuyType = ((int)买模式1).ToString(),
                        SaleType = ((int)卖模式1).ToString(),
                        LimitCount = members[8],
                        Commission = members[9]
                    };
                    string res = Program.AASServiceClient.AddStock(labelShareLimitGroup.Text, stock);
                    if (!res.StartsWith("0|"))
                    {
                        sb.Append("导入项错误，待保存数据 ").Append(item).Append(", 返回值 ").Append(res).Append(Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                    sb.Append("导入项异常，待保存数据 ").Append(item).Append("，Exception Message ").Append(ex.Message).Append(Environment.NewLine);
                }
                i++;
            }
            if (sb.Length > 0)
            {
                Program.logger.LogInfo(sb.ToString());
                MessageBox.Show(string.Format("导入错误或异常项{0}条，详情请查看日志!", i));
            }
            var group = Program.AASServiceClient.ShareGroupQuery().First(_ => _.GroupName == labelShareLimitGroup.Text);
            this.bindingSourceStocksIn.DataSource = group.GroupStockList.OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID);
        }

        private void dataGridViewStocks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //弹出修改界面
            var source = this.bindingSourceStocksIn.DataSource as StockLimitItem[];
            var win = new ShareLimitStockItemEdit(labelShareLimitGroup.Text.Trim(), source[e.RowIndex]);
            win.Show();
        }

        private void dataGridViewStocks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null) return;

            if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "买入方式")
            {
                var buyType = e.Value.ToString();

                switch (buyType)
                {
                    case "0":
                        e.Value = "买入";
                        break;
                    case "1":
                        e.Value = "融资买入";
                        break;
                    default:
                        break;
                }

            }
            else if ((sender as DataGridView).Columns[e.ColumnIndex].HeaderText == "卖出方式")
            {
                var saleType = e.Value.ToString();

                switch (saleType)
                {
                    case "0":
                        e.Value = "卖出";
                        break;
                    case "1":
                        e.Value = "融券卖出";
                        break;
                    default:
                        break;
                }
            }
        }

        private void textBoxKeyWords_TextChanged(object sender, EventArgs e)
        {
            var keyWords = textBoxKeyWords.Text.Trim();
            if (keyWords.Length > 0)
            {
                if (Regex.IsMatch(keyWords, "^[0-9]+$"))
                {//说明是股票代码
                    this.bindingSourceStocksIn.DataSource = groupInfo.GroupStockList.Where(_ => _.StockID.Contains(keyWords)).OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID).ToArray();
                }
                else if (Regex.IsMatch(keyWords, "^[A-Za-z][0-9]*"))
                {//说明是组合号
                    this.bindingSourceStocksIn.DataSource = groupInfo.GroupStockList.Where(_ => _.GroupAccount.Contains(keyWords)).OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID).ToArray();
                }
                //else if (Regex.IsMatch(keyWords, "[\u4e00-\u9fa5]"))
                else
                {
                    this.bindingSourceStocksIn.DataSource = groupInfo.GroupStockList.Where(_ => _.StockName.Contains(keyWords)).OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID).ToArray();
                }
            }
            else
            {
                this.bindingSourceStocksIn.DataSource = groupInfo.GroupStockList.OrderBy(_ => _.GroupAccount).ThenBy(_ => _.StockID).ToArray();
            }
        }
    }
}
