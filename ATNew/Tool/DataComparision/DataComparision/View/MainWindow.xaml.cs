using DataComparision.Controls;
using DataComparision.DataAdapter;
using DataComparision.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DataComparision.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow //: WindowExtBase
    {
        Dictionary<string, string> GroupDict { get; set; }

        public MainWindow()
        {
            var logon = new Logon();
            logon.ShowDialog();

            if (!Logon.IsLogon)
            {
                this.Close();
                return;
            }

            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                //var client = new ServiceReference.DataWebServiceSoapClient();
                //var isLoginSuccess = client.CompareToolLogin("W7txy", "W5waD6Kk");
                //if (isLoginSuccess)
                //{
                //    CommonUtils.UserName = "W7txy";
                //}
                //else
                //{
                //    MessageBox.Show("连接失败，未能自动登录!");
                //}

                tiEncrypt.Visibility = System.Windows.Visibility.Visible;
                tiGroupManage.Visibility = System.Windows.Visibility.Visible;
                tiInterface.Visibility = System.Windows.Visibility.Visible;

                this.Title += CommonUtils.Version;

                importTool.OnDBChangeComplete += new Action(RefreshComparePage);

                groupTool.OnGroupChanged += new Action(() => { exportTool.Init(); });
            };
            TdxApi.OpenTdx();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            TdxApi.CloseTdx();
            base.OnClosing(e);
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshComparePage()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                ctrlCompare.RefreshWindow(false);
                ctrlCompare.RefreshCombInfo();
            }));
        }



        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.C && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
            //{
            //    MessageBox.Show("CTRL + SHIFT + TAB trapped");
            //}

            //if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            //{
            //    //MessageBox.Show("CTRL + TAB trapped");
            //    var o = sender as DataGrid;
            //    if (o  != null)
            //    {

            //    }
            //}
        }

        private void Button_Encrypt_Click(object sender, RoutedEventArgs e)
        {
            this.txtEncryptResult.Text = Cryptor.MD5Encrypt(txtEncrypt.Text);
        }




    }


}
