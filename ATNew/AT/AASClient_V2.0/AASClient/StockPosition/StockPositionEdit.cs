using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.StockPosition
{
    public partial class StockPositionEdit : Form
    {
        AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend updateItem = null;

        public StockPositionEdit()
        {
            InitializeComponent();
            this.Load += StockPositionEdit_Load;
        }

        private void StockPositionEdit_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox组合号.Items.Clear();
                var accounts = Program.AASServiceClient.QueryQSNameListAsync();
                if (accounts != null && accounts.Result.QueryQSNameListResult != null)
                {
                    foreach (var item in accounts.Result.QueryQSNameListResult)
                    {
                        comboBox组合号.Items.Add(item);
                    }
                    
                    if (updateItem != null)
                    {
                        comboBox组合号.SelectedItem = updateItem.组合号;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load异常: " + ex.Message);
            }
        }

        public void Init(AASServiceReference.DbDataSet可用仓位DataTable可用仓位Extend item)
        {
            numericUpDown总仓位.Value = item.总仓位;
            textBoxStockID.Text = item.证券代码;
            textBoxStockName.Text = item.证券名称;
            textBoxStockID.Enabled = false;
            comboBox组合号.Enabled = false;
            updateItem = item;
            this.Text = "可用仓位修改";
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                if (updateItem == null)
                {
                    var result = Program.AASServiceClient.Add可用仓位Async(comboBox组合号.Text, textBoxStockID.Text, textBoxStockName.Text, numericUpDown总仓位.Value);
                    if (!result.Result.Add可用仓位Result)
                    {
                        MessageBox.Show(string.Format("新增失败, 请检查组合号{0},证券代码{1}是否已存在.", comboBox组合号.Text, textBoxStockID.Text));
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    var result = Program.AASServiceClient.Update可用仓位Async(updateItem.组合号, textBoxStockID.Text, textBoxStockName.Text, numericUpDown总仓位.Value);
                    MessageBox.Show(result.Result.Update可用仓位Result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void textBoxStockID_Leave(object sender, EventArgs e)
        {
            if (updateItem == null && Regex.IsMatch(textBoxStockID.Text, "[0-9]{6}"))
            {
                string stockID = textBoxStockID.Text;
                string stockName;
                int market;

                Task.Run(()=> 
                {
                    bool result = StockInfoUtils.GetStockNameMarket(stockID, out market, out stockName);
                    if (result)
                    {
                        textBoxStockName.Invoke(new Action(()=> 
                        {
                            textBoxStockName.Text = stockName;
                        }));
                    }
                });
                
            }
            
        }
    }
}
