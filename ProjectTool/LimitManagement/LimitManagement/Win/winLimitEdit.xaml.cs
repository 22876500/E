using LimitManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LimitManagement.Win
{
    /// <summary>
    /// winLimitEdit.xaml 的交互逻辑
    /// </summary>
    public partial class winLimitEdit : Window
    {
        private ServerLimitRow Limit;

        public winLimitEdit()
        {
            InitializeComponent();
        }

        public void Init(ServerLimitRow row)
        {
            this.DataContext = row;
            txtLimitQty.Text = row.交易额度.ToString();
            Limit = row;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            decimal qty = 0;
            if (!decimal.TryParse(txtLimitQty.Text.Trim(), out qty))
            {
                MessageBox.Show("请填写分配额度数值！");
                return;
            }
            if (Limit != null)
            {
                string errMsg = string.Empty;

                var updatedRow = Limit.Update(qty, out errMsg);
                if (updatedRow == null)
                {
                    MessageBox.Show(errMsg);
                }
                else
                {
                    //Limit.交易额度 = updatedRow.交易额度;
                    Limit.RefreshData(updatedRow);
                    MessageBox.Show("修改成功！");
                }
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
