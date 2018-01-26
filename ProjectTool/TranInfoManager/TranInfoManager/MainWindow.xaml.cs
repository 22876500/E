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

namespace TranInfoManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Label _selectedLable;
        Label SelectedLable {
            get {
                return _selectedLable;
            }
            set 
            {
                _selectedLable = value;
                foreach (var item in gdHeader.Children)
                    LableMouseLeave(item as Label);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Header Lable Events
        public void lblLink_MouseMove(object sender, RoutedEventArgs e)
        {
            var lbl = sender as Label;
            LableMouseMove(lbl);
        }

        public void lblLink_MouseLeave(object sender, RoutedEventArgs e)
        {
            var lbl = sender as Label;
            LableMouseLeave(lbl);

        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectedLable = sender as Label;
            foreach (UserControl item in this.gridContent.Children)
            {
                if (item != null) item.Visibility = Visibility.Collapsed;
            }

            if (sender == this.navCompare)
            {
                this.Comparer.Visibility = Visibility.Visible;
                this.Comparer.Init();
            }
            else if (sender == this.navImport)
            {
                this.Importer.Visibility = Visibility.Visible;
            }
            else if (sender == this.navSelect)
            {
                this.SelecteResult.Visibility = Visibility.Visible;
                this.SelecteResult.Init();
            }
        } 
        #endregion

        private void LableMouseMove(Label lbl)
        {
            lbl.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x99, 0x00));
            lbl.Cursor = lbl == SelectedLable ? Cursors.Arrow : Cursors.Hand;
            if (lbl != SelectedLable)
                lbl.Background = new SolidColorBrush(Colors.Cyan);//F0F8FF
        }

        private void LableMouseLeave(Label lbl)
        {
            lbl.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2B, 0x86, 0xCA));
            if (lbl == SelectedLable)
                lbl.Background = new SolidColorBrush(Colors.SkyBlue);
            else
                lbl.Background = new SolidColorBrush(Colors.AliceBlue);
        }

        

    }
}
