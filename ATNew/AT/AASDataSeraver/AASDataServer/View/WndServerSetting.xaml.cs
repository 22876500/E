using AASDataServer.ViewModel;
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

namespace AASDataServer.View
{
    /// <summary>
    /// WndServerSetting.xaml 的交互逻辑
    /// </summary>
    public partial class WndServerSetting : Window
    {
        ServerSettingDataContext _context;

        public WndServerSetting()
        {
            InitializeComponent();

            _context = new ServerSettingDataContext();
            this.DataContext = _context;
        }

        private void ViewServerSetting_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _context.Reload();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _context.Save();
            _context.Reload();
            MessageBox.Show("配置已保存！");
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _context.Reload();
            Close();
        }
    }
}
