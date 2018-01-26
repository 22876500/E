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
using System.Windows.Threading;

namespace TranInfoManager.UC
{
    /// <summary>
    /// UCLoading.xaml 的交互逻辑
    /// </summary>
    public partial class UCLoading : UserControl
    {
        public Boolean IsShowShadow
        {
            set
            {
                if (value)
                {
                    this.bdShadow.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    this.bdShadow.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        public string Description
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this.txtDescription.Text = value;
                }
            }
        }

        public UCLoading()
        {
            InitializeComponent();
        }

        public void ShowLoading(string msg = "加载中，请耐心等待…")
        {
            
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => 
            {
                if (!string.IsNullOrEmpty(msg))
                    this.Description = msg;
                this.Visibility = Visibility.Visible; 
            }));
        }

        public void HideLoading() 
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => 
            { 
                this.Visibility = Visibility.Collapsed; 
            }));
            
        }
    }
}
