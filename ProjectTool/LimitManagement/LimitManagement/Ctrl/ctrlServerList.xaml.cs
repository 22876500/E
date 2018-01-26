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

namespace LimitManagement.Ctrl
{
    /// <summary>
    /// ctrlServerList.xaml 的交互逻辑
    /// </summary>
    public partial class ctrlServerList : UserControl
    {
        public ctrlServerList()
        {
            InitializeComponent();
            this.Loaded += ctrlServerList_Loaded;
        }

        void ctrlServerList_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceConnectHelper.Instance.Start();
            dgServerList.ItemsSource = ServiceConnectHelper.Instance.ServerInfoList;
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            var server = (sender as Button).DataContext as LimitManagement.Entities.ServerInfo;
            if (server != null)
            {
                server.Stop();
                server.Start();
            }
        }

        private void Button_Stop_Click(object sender, RoutedEventArgs e)
        {
            var server = (sender as Button).DataContext as LimitManagement.Entities.ServerInfo;
            if (server != null)
            {
                server.Stop();
            }
        }
    }

    public class CanStopConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = false;
            bool bindingValue = (bool)value;
            result = !bindingValue;
            //if (bindingValue == true)
            //{
            //    result = false;
            //}
            //else if (bindingValue == true)
            //{
            //    result = true;
            //}
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
