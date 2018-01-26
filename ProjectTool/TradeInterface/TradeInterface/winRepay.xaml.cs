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

namespace TradeInterface
{
    /// <summary>
    /// winRepay.xaml 的交互逻辑
    /// </summary>
    public partial class winRepay : Window
    {
        券商 GroupItem = null;

        public winRepay()
        {
            InitializeComponent();
        }

        public void Init(券商 group)
        {
            this.tbAccount.Text = group.名称;
            GroupItem = group;
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.GroupItem == null)
            {
                MessageBox.Show("初始化异常，请联系管理员");
                return;
            }

            var client = GroupServiceHelper.GetGroupClient(GroupItem.名称);
            if (client != null)
            {
                try
                {
                    var s = client.AccountRepay(tbAccount.Text, decimal.Parse(txtAmount.Text.Trim()));
                    MessageBox.Show(s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
