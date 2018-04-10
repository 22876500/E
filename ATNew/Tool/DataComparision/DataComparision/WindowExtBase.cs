using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;
using System.Media;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace DataComparision
{
    public class WindowExtBase : Window
    {
        public WindowExtBase()
        {

            var wc = new WindowChrome();
            wc.ResizeBorderThickness = new Thickness(5);
            this.Resources.Add("WindowChromeKey", wc);

            this.FontFamily = new System.Windows.Media.FontFamily("Microsoft YaHei");
            this.AllowsTransparency = true;
            this.WindowStyle = System.Windows.WindowStyle.None;
            WindowChrome.SetWindowChrome(this, wc);

            var uri = new Uri("pack://application:,,,/Images/ThunderShield.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(uri);
        }

    }
}
