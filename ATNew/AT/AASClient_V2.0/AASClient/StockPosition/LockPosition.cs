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
    public partial class LockPosition : Form
    {
        public LockPosition()
        {
            InitializeComponent();
            this.Activated += LockPosition_Activated;
        }

        private void LockPosition_Activated(object sender, EventArgs e)
        {
            numericUpDown请求数量.Focus();
        }

        public void Init(string Name, string stockID, string stockName, decimal qtyTotal, decimal qtyLocked)
        {
            textBox组合号.Text = Name;
            textBox证券代码.Text = stockID;
            textBox总仓位.Text = qtyTotal.ToString();
            textBox证券名称.Text = stockName;
            textBox已用数量.Text = qtyLocked.ToString();
            textBoxSelfLockQty.Text = "0";
            numericUpDown请求数量.Value = qtyTotal - qtyLocked;
            if ((stockName??"").Length > 0)
            {
                textBox缩写.Text = Regex.Replace(PinYin.PinYinConvertor.GetFirstPinyin(stockName), "[^a-zA-Z]", "");
            }
            if (qtyLocked > 0)
            {
                var existLimit = Program.serverDb.额度分配.FirstOrDefault(_ => _.组合号 == Name && _.证券代码 == stockID && _.交易员 == Program.Current平台用户.用户名);
                if (existLimit != null)
                {
                    numericUpDown请求数量.Value += existLimit.交易额度;
                    textBoxSelfLockQty.Text = existLimit.交易额度 + "";
                }
            }
        }


        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            SendLockRequest();
        }

        private void SendLockRequest()
        {
            decimal qtyTotal = decimal.Parse(textBox总仓位.Text);
            decimal qtyLocked = decimal.Parse(textBox已用数量.Text);
            decimal qtySelfLock = decimal.Parse(textBoxSelfLockQty.Text);

            if (numericUpDown请求数量.Value <= (qtyTotal - qtyLocked))
            {
                string acc = textBox组合号.Text;
                string stockID = textBox证券代码.Text;
                decimal qtyAdd = numericUpDown请求数量.Value;
                string shortName = textBox缩写.Text;
                string trader = Program.Current平台用户.用户名;


                try
                {
                    var msg = Program.AASServiceClient.LockPositionAsync(acc, stockID, qtySelfLock + qtyAdd, shortName, trader);
                    if (msg.Result.LockPositionResult != null)
                    {
                        if (msg.Result.LockPositionResult.Contains("成功"))
                        {
                            var dt = Program.AASServiceClient.QueryTraderLimits(trader);
                            Tool.RefreshDrcjDataTable(Program.serverDb.额度分配, dt, new string[] { "组合号", "证券代码", "交易员" });
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("锁定异常:" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show(string.Format("请求锁定数量过多, 总数{0}, 已锁定{1}, 剩余数量{2} 小于请求数量{3}", qtyTotal, qtyLocked, (qtyTotal - qtyLocked), numericUpDown请求数量.Value));
            }
        }

        private void numericUpDown请求数量_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendLockRequest();
            }
        }

        private void textBox缩写_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendLockRequest();
            }
        }
    }
}
