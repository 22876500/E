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
    /// WinHeader.xaml 的交互逻辑
    /// </summary>
    public partial class WinHeader : UserControl
    {

        public string Header
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.txtTitle.Content = value;
                }
            }
        }

        public WinHeader()
        {
            InitializeComponent();
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
