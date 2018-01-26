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

namespace AASClient.UserCoins
{
    public partial class UserCoinEdit : Form
    {
        AASServiceReference.DbDataSet.可用资金Row rowEdit;

        public UserCoinEdit()
        {
            InitializeComponent();
            Load += UserCoinEdit_Load;
        }

        private void UserCoinEdit_Load(object sender, EventArgs e)
        {
            var users = Program.AASServiceClient.QueryJY();
            foreach (var item in users)
            {
                comboBox交易员.Items.Add(item.用户名);
            }


        }

        public void Init(AASServiceReference.DbDataSet.可用资金Row row)
        {
            rowEdit = row;
            comboBox交易员.SelectedValue = row.交易员;
            textBoxCoinType.Text = row.币种;
            numericUpDownQty.Value = row.可用数量;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rowEdit == null)
            {
                Program.AASServiceClient.Add可用资金(comboBox交易员.SelectedItem.ToString(), textBoxCoinType.Text.Trim(), numericUpDownQty.Value);
                this.Close();
            }
            else
            {
                Program.AASServiceClient.Update可用资金(comboBox交易员.SelectedItem.ToString(), textBoxCoinType.Text.Trim(), numericUpDownQty.Value);
            }
        }
    }
}
