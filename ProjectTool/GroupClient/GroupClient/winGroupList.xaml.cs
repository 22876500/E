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

namespace GroupClient
{
    /// <summary>
    /// winGroupList.xaml 的交互逻辑
    /// </summary>
    public partial class winGroupList : Window
    {
        public winGroupList()
        {
            InitializeComponent();

            this.Loaded += winGroupList_Loaded;
        }

        void winGroupList_Loaded(object sender, RoutedEventArgs e)
        {
            this.dgMain.ItemsSource = Adapter.GroupsDict.Values.ToList();
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is 券商)
            {
                var o = (sender as Button).DataContext as 券商;
                if (CommonUtils.ConfigurationInstance.AppSettings.Settings.AllKeys.Contains(o.名称))
                {
                    var logonItem = Adapter.GroupLogonList.First(_ => _.Name == o.名称);
                    Adapter.GroupLogonList.Remove(logonItem);
                    Adapter.GroupsDict.Remove(o.名称);

                    CommonUtils.ConfigurationInstance.AppSettings.Settings.Remove(o.名称);
                    CommonUtils.ConfigurationInstance.Save();

                    o.Stop();

                    dgMain.ItemsSource = Adapter.GroupsDict.Values.ToList();

                    MessageBox.Show("删除组合号成功！");
                }
                else
                {
                    MessageBox.Show("删除组合号失败！");
                }
            }

        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is 券商)
            {
                var o = (sender as Button).DataContext as 券商;
                if (o.IsIMSAccount)
                {
                    var win = new AddImsGroup(o);
                    win.ShowDialog();
                }
                else
                {
                    var win = new AddGroup();
                    win.Init(o);
                    win.ShowDialog();
                }
                
                this.dgMain.ItemsSource = Adapter.GroupsDict.Values.ToList();
            }
        }
    }
}
