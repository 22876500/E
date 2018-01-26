using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuotaShareClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DataTable dtQuato = null;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.WindowState = System.Windows.WindowState.Maximized;

            InitListView();
        }
        private void InitListView()
        {
            for (int i = 0; i < 100; i++)
            {
                //listBook.Add(new Book(0,123456, "testBook"+i, "Math", "qiaobus", "shanghai", "just a book", "none"));  
                listView1.Items.Add(new Quota() { Trader = "", Code = "600271", Group = "C02", Market = 1, CodeName = "航天信息", PinYin = "HTXX", BuyType = 0, SellType = 0, TradeQuota = 50000, Rate = 0.0002M });

            }  
        }
        private void OpenImport_Click(object sender, RoutedEventArgs e)
        {
            Import import = new Import();
            import.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            import.ShowDialog();
        }
    }
}
