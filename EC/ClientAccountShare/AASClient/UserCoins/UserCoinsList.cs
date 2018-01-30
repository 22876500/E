using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.UserCoins
{
    public partial class UserCoinsList : Form
    {
        AASServiceReference.DbDataSet.可用资金DataTable CoinsAll;

        public UserCoinsList()
        {
            InitializeComponent();

            this.Load += UserCoinsList_Load;    
        }

        private void UserCoinsList_Load(object sender, EventArgs e)
        {
            CoinsAll = Program.AASServiceClient.QueryAll可用资金();
            this.bindingSourceUserCoin.DataSource = CoinsAll;
            this.dataGridView1.DataSource = bindingSourceUserCoin;

            var coinsType = CoinsAll.Select(_ => _.币种).Distinct();
            comboBoxCoin.Items.Add("--All--");
            foreach (var item in coinsType)
            {
                comboBoxCoin.Items.Add(item);   
            }

            var user = Program.AASServiceClient.QueryJY();
            comboBoxUser.Items.Add("--All--");
            foreach (var item in user)
            {
                comboBoxUser.Items.Add(item.用户名);
            }


        }

        private void 新增ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new UserCoinEdit();
            win.FormClosed += Win_FormClosed;
            win.Show();

            
        }

        private void Win_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshCoinInfo();
        }

        private void RefreshCoinInfo()
        {
            CoinsAll = Program.AASServiceClient.QueryAll可用资金();
            this.bindingSourceUserCoin.DataSource = CoinsAll;

            var coinsType = CoinsAll.Select(_ => _.币种).Distinct();
            foreach (var item in coinsType)
            {
                comboBoxCoin.Items.Add(item);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定删除选中行吗？", "删除确认", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                var deleteItems = dataGridView1.SelectedRows;
                foreach (DataGridViewRow item in deleteItems)
                {
                    var row = (item.DataBoundItem as DataRowView).Row as AASServiceReference.DbDataSet.可用资金Row;
                    Program.AASServiceClient.Delete可用资金(row.交易员, row.币种);
                    CoinsAll.Remove可用资金Row(row);
                }
            }
        }

        private void comboBoxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddFilter();
        }

        private void AddFilter()
        {
            string filter = string.Empty;

            if (comboBoxUser.SelectedIndex > 0)
            {
                filter += string.Format("交易员 = '{0}'", comboBoxUser.SelectedItem.ToString());
            }

            if (comboBoxCoin.SelectedIndex > 0)
            {
                if (filter.Length > 0)
                {
                    filter += " AND ";
                }
                filter += string.Format("币种 = '{0}'", comboBoxCoin.SelectedItem.ToString());
            }
            bindingSourceUserCoin.Filter = filter;
        }

        private void comboBoxCoinType_SelectedIndexChange(object sender, EventArgs e)
        {
            AddFilter();
        }
    }
}
