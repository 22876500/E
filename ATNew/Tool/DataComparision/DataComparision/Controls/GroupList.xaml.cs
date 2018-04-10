using DataComparision.Entity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// GroupList.xaml 的交互逻辑
    /// </summary>
    public partial class GroupList : UserControl
    {
        public Action OnGroupChanged;

        public GroupList()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //if (DataAdapter.DataHelper.IsExistsAAS)
            //{
            //    btnAASImport.IsEnabled = true;
            //}
            try
            {
                using (var db = new DataComparisionDataset())
                {
                    var list = db.券商ds.ToList();
                    this.dgGroup.ItemsSource = list;

                }
            }
            catch (Exception)
            {

            }
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            var o = (sender as Button).DataContext as 券商;
            var child = new View.GroupEdit(o);
            child.ShowDialog();
            Init();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var child = new View.GroupEdit();
            child.ShowDialog();
            Init();
        }

        private void Button_AASImport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = MessageBox.Show("是否覆盖原有相同组合号数据？" , "", MessageBoxButton.YesNoCancel);
                if (result != MessageBoxResult.Cancel)
                {
                    bool isSuccess = DataAdapter.DataHelper.ImportGroupFromAAS(result == MessageBoxResult.Yes);
                    if (isSuccess)
                    {
                        MessageBox.Show("导入成功");
                        if (OnGroupChanged != null)
                        {
                            OnGroupChanged.Invoke();
                        }
                    }
                    else
                    {
                        MessageBox.Show("导入失败！详细信息请查看日志。");
                    }
                }
                
                Init();
            }
            catch (Exception ex)
            {
                CommonUtils.ShowMsg(ex.Message);
            }
        }

        private void Button_FileImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { CheckFileExists = true, Multiselect = false, Filter = "文本文件|*.txt" };
            if (true == dialog.ShowDialog())
            {
                try
                {
                    
                    var arr = File.ReadAllLines(dialog.FileName);

                    List<Entity.券商> listImport = new List<券商>();
                    foreach (var item in arr)
                    {
                        if (item.StartsWith("名称") || string.IsNullOrWhiteSpace(item))
                        {
                            continue;
                        }
                        var info = item.Split(new[] {',', '\t'});
                        var o = new Entity.券商()
                        {
                            名称 = info[0],
                            启用 = bool.Parse(info[1]),
                            交易服务器 = info[2],
                            IP = info[3],
                            Port = short.Parse(info[4]),
                            版本号 = info[5],
                            营业部代码 = short.Parse(info[6]),
                            登录帐号 = info[7],
                            交易帐号 = info[8],
                            交易密码 = info[9],
                        };
                        if (info.Length>= 11)
                        {
                            o.通讯密码 = info[10];
                        }
                        listImport.Add(o);
                    }
                    if (listImport.Count > 0)
                    {
                        using (var db = new DataComparisionDataset())
                        {
                            var listName = listImport.Select(_ => _.名称);
                            var old = db.券商ds.Where(_ => listName.Contains(_.名称)).ToList();
                            if (old.Count > 0)
                            {
                                db.券商ds.RemoveRange(old);
                            }
                            db.券商ds.AddRange(listImport);
                            db.SaveChanges();
                            MessageBox.Show(string.Format("导入完成，导入{0}个券商信息，覆盖{1}个,新增{2}个！", listImport.Count, old.Count, listImport.Count - old.Count));
                        }
                        
                        Init();
                    }
                    else {
                        MessageBox.Show("导入失败！");
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private void Button_SqlImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog() { CheckFileExists = true, Multiselect = true, Filter = "sql文件|*.sql" };
            var result = dialog.ShowDialog();
            if (result == true && !string.IsNullOrEmpty(dialog.FileName))
            {
                var sqlArr = File.ReadAllLines(dialog.FileName);
                StringBuilder errMsg = new StringBuilder();
                int count = 0;
                int sucCount = 0;
                using (var conn = new SqlConnection(CommonUtils.DBConnection))
                {
                    conn.Open();
                    foreach (string sql in sqlArr)
                    {
                        if (sql.StartsWith("INSERT", StringComparison.CurrentCultureIgnoreCase))
                        {
                            count++;
                            try
                            {
                                var command = new SqlCommand() { CommandText = sql, Connection = conn };
                                sucCount += command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                errMsg.Append(ex.Message);
                            }
                        }
                    }
                    if (errMsg.Length > 0)
                    {
                        CommonUtils.Log(errMsg.ToString());
                    }
                }
                var msg = string.Format("执行完毕！导入数据{0}条，成功{1}条，失败{2}条。", count, sucCount, count - sucCount);
                if (errMsg.Length > 0)
                {
                    msg += "失败原因请查看详细日志！";
                }
                Init();
                MessageBox.Show(msg);
                if (OnGroupChanged != null)
                {
                    OnGroupChanged.Invoke();
                }
            }

        }


        private void Button_AppSetting_Click(object sender, RoutedEventArgs e)
        {
            this.Loading.ShowLoading("正在保存……");
            this.Dispatcher.RunAsync(() => {
                foreach (券商 o in this.dgGroup.ItemsSource)
                {
                    CommonUtils.SetConfig(o.名称, Cryptor.MD5Encrypt(o.ToJson()));
                }
            }, null, null, () => {
                this.Loading.HideLoading();
                MessageBox.Show("保存完毕！");
            });
            
        }

        private void Button_Download_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog() { Filter = "文本文件|*.txt" };
            if (dialog.ShowDialog() == true)
            {
                var text = Utils.ExcelUtils.RenderToString<Entity.券商>(dgGroup);
                File.WriteAllText(dialog.FileName, text);
                MessageBox.Show("导出完毕!");
            }
            
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            var o = (sender as Button).DataContext as 券商;
            using (var db = new DataComparisionDataset())
            {
                var item = db.券商ds.FirstOrDefault(_ => _.名称 == o.名称);
                if (item != null)
                {
                    db.券商ds.Remove(item);
                    db.SaveChanges();
                    Init();
                    MessageBox.Show("删除成功！");
                }
            }
        }
    }
}
