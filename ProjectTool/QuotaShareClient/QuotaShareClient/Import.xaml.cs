using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace QuotaShareClient
{
    /// <summary>
    /// Import.xaml 的交互逻辑
    /// </summary>
    public partial class Import : Window
    {
        public Import()
        {
            InitializeComponent();
            this.Loaded += Import_Loaded;
        }
        private void Import_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow.dtQuato = new DataTable();
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xls file(*.xls)|*.xls|xlsx file(*.xlsx)|*.xlsx|csv file(.csv)|*.csv";
            if (ofd.ShowDialog() == true)
            {
                //得到上传文件的完整名
                string loadFullName = ofd.FileName.ToString();

                this.txtFilePath.Text = loadFullName;

                if (string.IsNullOrEmpty(loadFullName))
                {
                    MessageBox.Show("请选择导入文件");
                    return;
                }

                if (loadFullName.EndsWith(".xls") || loadFullName.EndsWith(".xlsx"))
                {
                    MainWindow.dtQuato = ExcelHelper.ImportExcel(loadFullName, 0);
                }
                else if (loadFullName.EndsWith(".csv"))
                {
                    MainWindow.dtQuato = CsvHelper.OpenCSV(this.txtFilePath.Text.Trim());
                }
                else
                {
                    MessageBox.Show("无法识别的文件格式，请选择excel或者csv文件");
                    return;
                }

                if (MainWindow.dtQuato == null || MainWindow.dtQuato.Rows.Count == 0)
                {
                    MessageBox.Show("已导入数据为0条");
                    return;
                }
                MessageBox.Show(string.Format("导入成功,共{0}条", MainWindow.dtQuato.Rows.Count));
                
            }
        }
    }
}
