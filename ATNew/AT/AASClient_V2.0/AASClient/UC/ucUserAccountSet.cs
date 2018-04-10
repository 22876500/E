using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient.UC
{
    public partial class ucUserAccountSet : System.Windows.Forms.UserControl
    {
        //List<AccountSelected> AccountList = new List<AccountSelected>();
        AASServiceReference.DbDataSet.组合号交易员关联DataTable dt;
        string[] userNames;
        public ucUserAccountSet()
        {
            InitializeComponent();

            this.Load += UcUserAccountSet_Load;
        }

        private void UcUserAccountSet_Load(object sender, EventArgs e)
        {
            labelLoading.Visible = true;
            dataGridViewNotSelected.AutoGenerateColumns = false;
            dataGridViewSelected.AutoGenerateColumns = false;
            Task.Run(()=> 
            {
                dt = Program.AASServiceClient.QueryAllTraderAccounts();
                userNames = Program.AASServiceClient.QueryTraderNameList();
                var accountList = Program.AASServiceClient.QueryQSNameList();

                listBoxAccounts.BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i < accountList.Length; i++)
                    {
                        if (listBoxAccounts.Items.Count > i)
                        {
                            listBoxAccounts.Items[i] = accountList[i];
                        }
                        else
                        {
                            listBoxAccounts.Items.Add(accountList[i]);
                        }
                    }
                    if (listBoxAccounts.Items.Count > accountList.Length)
                    {
                        for (int i = listBoxAccounts.Items.Count -1; i > accountList.Length; i--)
                        {
                            listBoxAccounts.Items.RemoveAt(i);
                        }
                    }
                    labelLoading.Visible = false;
                }));
            });
        }

        private void listBoxTraders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAccounts.SelectedIndex > -1)
            {
                RefreshDataGridView();
            }
        }

        private void RefreshDataGridView()
        {
            var accountName = listBoxAccounts.SelectedItem.ToString();
            var selectedTraders = dt.Where(_ => _.组合号 == accountName).Select(_ => _.交易员).ToList();

            dataGridViewSelected.Rows.Clear();
            dataGridViewNotSelected.Rows.Clear();
            foreach (var item in userNames)
            {
                if (selectedTraders.Contains(item))
                {
                    dataGridViewSelected.Rows.Add(item);
                }
                else
                {
                    dataGridViewNotSelected.Rows.Add(item);
                }
            }
        }

        private void dataGridViewNotSelected_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridViewNotSelected.Columns[e.ColumnIndex];
                if (column is DataGridViewCheckBoxColumn)
                {
                    string userName = dataGridViewNotSelected.Rows[e.RowIndex].Cells[0].Value + "";
                    AddTraderAccountRelation(userName);
                }
            }
        }

        private void dataGridViewSelected_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewColumn column = dataGridViewSelected.Columns[e.ColumnIndex];
                if (column is DataGridViewCheckBoxColumn)
                {
                    string userName = dataGridViewSelected.Rows[e.RowIndex].Cells[0].Value + "";
                    DeleteTraderAccountRelation(userName);
                }
            }
        }

        private void DeleteTraderAccountRelation(string userName)
        {
            string account = listBoxAccounts.SelectedItem.ToString();
            Task.Run(() =>
            {
                try
                {
                    Program.AASServiceClient.DeleteTraderAccountRelation(account, userName);
                    dt = Program.AASServiceClient.QueryAllTraderAccounts();
                    this.Invoke(new Action(RefreshDataGridView));
                }
                catch (Exception ex)
                {
                    this.BeginInvoke(new Action(() => { MessageBox.Show("交易员分配券商账户异常:" + ex.Message); }));
                }
            });
        }

        private void AddTraderAccountRelation(string userName)
        {
            string account = listBoxAccounts.SelectedItem.ToString();
            Task.Run(() =>
            {
                try
                {
                    var result = Program.AASServiceClient.AddTraderAccount(userName, account);
                    var arrResult = result.Split('|');
                    if (arrResult[0] == "0")
                    {
                        this.Invoke(new Action(() => { MessageBox.Show(arrResult[1]); }));
                    }
                    else
                    {
                        Program.logger.LogInfo("交易员分配券商账户成功:" + result);
                        dt = Program.AASServiceClient.QueryAllTraderAccounts();
                        this.Invoke(new Action(RefreshDataGridView));
                    }
                }
                catch (Exception ex)
                {   
                    this.BeginInvoke(new Action(() => { MessageBox.Show("交易员分配券商账户异常: " + ex.Message); }));
                }
            });
        }
    }
}
