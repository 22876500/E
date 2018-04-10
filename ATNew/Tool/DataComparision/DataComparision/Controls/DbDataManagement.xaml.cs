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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataComparision.Controls
{
    /// <summary>
    /// DbDataManagement.xaml 的交互逻辑
    /// </summary>
    public partial class DbDataManagement : UserControl
    {
        public DbDataManagement()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            //var 
        }

        private void Button_Soft_Search_Click(object sender, RoutedEventArgs e)
        {
            List<Entity.软件委托> lstSoft = new List<Entity.软件委托>();
            using (var db = new DataComparisionDataset())
            {
                lstSoft = db.软件委托ds.ToList();
            }
        }

        private void Button_Soft_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Deli_Search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Deli_Delete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
