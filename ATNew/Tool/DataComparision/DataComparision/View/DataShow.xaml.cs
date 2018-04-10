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

namespace DataComparision.View
{
    /// <summary>
    /// DeliveryListShow.xaml 的交互逻辑
    /// </summary>
    public partial class DataShow : UserControl
    {
        public DataShow()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

            using (var db = new DataComparisionDataset())
            {
                //if (true)
                //{
                    
                //}
                var lstData =  db.交割单ds.ToList();
                this.dgDeliveryList.ItemsSource = lstData;
            }
        }


    }
}
