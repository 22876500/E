using AASClient.AASServiceReference;
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
    public partial class ShareLimitStockItemEdit : Form
    {
        StockLimitItem limitItem;
        string shareGroupName;

        public ShareLimitStockItemEdit()
        {
            InitializeComponent();
        }

        public ShareLimitStockItemEdit(string group, StockLimitItem item)
        {
            InitializeComponent();
            Init(group, item);
        }

        public void Init(string group, StockLimitItem item)
        {
            limitItem = item;
            shareGroupName = group;

            this.textBoxStockCode.Text = item.StockID;
            this.textBoxStockName.Text = item.StockName;
            numericUpDownLimit.Value = decimal.Parse(item.LimitCount);
            numericUpDownCommission.Value = decimal.Parse(item.Commission);
            
            var groups = Program.AASServiceClient.QueryQsAccount();
            foreach (DataRow qs in groups.Rows)
            {
                comboBoxGroups.Items.Add(qs["名称"]);
            }
            comboBoxGroups.SelectedItem = item.GroupAccount;
            comboBoxBuy.SelectedIndex = int.Parse(item.BuyType);
            comboBoxSale.SelectedIndex = int.Parse(item.SaleType);
        }

        private void button1_Click(object sender, EventArgs e)
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
            string result = Program.AASServiceClient.UpdateStock(shareGroupName, stock);
            if (result.StartsWith("0|"))
            {
                textBoxStockCode.Text = string.Empty;
                textBoxStockName.Text = string.Empty;
                numericUpDownLimit.Value = 0;
                comboBoxGroups.SelectedIndex = -1;
                numericUpDownCommission.Value = 0;
                comboBoxBuy.SelectedIndex = 0;
                comboBoxSale.SelectedIndex = 0;
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Substring(2));
            }
        }
    }
}
