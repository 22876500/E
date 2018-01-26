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
    /// Loading.xaml 的交互逻辑
    /// </summary>
    public partial class Loading : UserControl
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

        public Loading()
        {
            InitializeComponent();
        }

        public void ShowLoading(string loadingDescription = "加载中，请耐心等待…")
        {
            if (!string.IsNullOrEmpty(loadingDescription))
            {
                Description = loadingDescription;
            }
            this.Visibility = Visibility.Visible;

        }

        public void HideLoading()
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
