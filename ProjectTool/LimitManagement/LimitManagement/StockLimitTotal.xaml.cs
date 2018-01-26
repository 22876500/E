using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace LimitManagement
{
    /// <summary>
    /// StockLimitTotal.xaml 的交互逻辑
    /// </summary>
    public partial class StockLimitTotal : Window
    {
        public StockLimitTotal()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            ServiceConnectHelper.Instance.Start();
        }



        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            //转至 编辑总股数及自动分配额度界面。
            var win = new StockLimitDetail();
            win.Init("", "");
            win.ShowDialog();
            
        }

        private void cmbGroupFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtGroup_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtStock_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
