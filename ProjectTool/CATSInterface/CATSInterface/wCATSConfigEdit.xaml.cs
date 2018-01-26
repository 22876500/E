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

namespace CATSInterface
{
    /// <summary>
    /// wCATSConfigEdit.xaml 的交互逻辑
    /// </summary>
    public partial class wCATSConfigEdit : Window
    {
        public wCATSConfigEdit()
        {
            InitializeComponent();
            this.Loaded += wCATSConfigEdit_Loaded;
        }

        void wCATSConfigEdit_Loaded(object sender, RoutedEventArgs e)
        {
            var path = Utils.GetConfig("CATS_PATH");
            if (!string.IsNullOrEmpty(path))
            {
                this.txtPath.Text = path;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Utils.SetConfig("CATS_PATH", txtPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
