using LimitManagement.AASServiceReference;
using LimitManagement.Entities;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace LimitManagement.Ctrl
{
    /// <summary>
    /// StockLimitTotal.xaml 的交互逻辑
    /// </summary>
    public partial class ctrlStockLimitTotal : UserControl
    {
        Dictionary<ServerInfo, DbDataSet.额度分配DataTable> dictLimits;

        public ctrlStockLimitTotal()
        {
            InitializeComponent();
            Loaded += ctrlStockLimitTotal_Loaded;
        }

        void ctrlStockLimitTotal_Loaded(object sender, RoutedEventArgs e)
        {
            if (cmbServerFilter.Items.Count == 0)
            {
                //List<string> lstIP = new List<string>() ;
                cmbServerFilter.DisplayMemberPath = "Remark";

                foreach (var item in ServiceConnectHelper.Instance.ServerInfoList)
                {
                    if (!cmbServerFilter.Items.Contains(item))
                    {
                        cmbServerFilter.Items.Add(item);
                    }
                }
            }
        }

        private void Init(ServerInfo server = null)
        {
            InitLimitsData(server);

            var list = GetTotalBindingData();
            if (list == null)
                return;

            var groups = list.Select(_ => _.组合号).Distinct().OrderBy(_ => _).ToList();

            this.Dispatcher.Invoke(() =>
            {
                cmbGroupFilter.Items.Clear();
                cmbGroupFilter.Items.Add("全部");
                groups.ForEach(_ => cmbGroupFilter.Items.Add(_));

                this.dgStocksMain.ItemsSource = list;
                this.loading.HideLoading();
            });
        }

        private void InitFiltedData()
        {
            var list = GetTotalBindingData();
            if (list == null)
            {
                return;
            }
            if (txtGroup.Text.Length > 0)
            {
                list = list.Where(_ => _.组合号.Contains(txtGroup.Text.Trim())).ToList();
            }
            if (cmbServerFilter.SelectedIndex> -1)
            {
                list = list.Where(_ => _.Ip == (cmbServerFilter.SelectedItem as ServerInfo).Ip).ToList();
            }
            if (txtStock.Text.Length > 0 && Regex.IsMatch(txtStock.Text, "^[0-9]{5,6}$"))
            {
                list = list.Where(_ => _.证券代码.Contains(txtStock.Text)).ToList();
            }
            if (txtAccount.Text.Length >= 3)
            {
                list = list.Where(_ => _.交易员.Contains(txtAccount.Text)).ToList();
            }
            this.dgStocksMain.ItemsSource = list;
        }

        /// <summary>
        /// 获取所有额度分配数据
        /// </summary>
        /// <returns></returns>
        private List<ServerLimitRow> GetTotalBindingData()
        {
            if (this.dictLimits == null)
            {
                return null;
            }

            List<ServerLimitRow> lstLimists = new List<ServerLimitRow>();
           
            foreach (var item in dictLimits)
            {
                if (item.Value != null)
                {
                    foreach (DbDataSet.额度分配Row rowItem in item.Value.Rows)
                    {
                        lstLimists.Add(new ServerLimitRow(rowItem, item.Key));
                    }
                }
            }
            return lstLimists;
        }

        /// <summary>
        /// 初始化额度分配数据，如选择服务器则只初始化对应服务器数据
        /// </summary>
        /// <param name="server"></param>
        private void InitLimitsData(ServerInfo server = null)
        {
            dictLimits = new Dictionary<ServerInfo, DbDataSet.额度分配DataTable>();
            string errMsg = null;
            foreach (var item in ServiceConnectHelper.Instance.ServerInfoList)
            {
                if (server == null || item == server)
                {
                    try
                    {
                        var ipName = item.Ip + "_" + item.Remark;
                        var dt = item.QueryLimit(out errMsg);
                        if (dictLimits.ContainsKey(item))
                        {
                            dictLimits[item] = dt;
                        }
                        else
                        {
                            dictLimits.Add(item, dt);
                        }
                    }
                    catch { }
                }
            }
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            var serverInfo = (sender as Button).DataContext as Entities.ServerLimitRow;
            if (serverInfo != null)
            {
                //转至 编辑总股数及自动分配额度界面。
                var win = new Win.winLimitEdit();
                win.Init(serverInfo);
                win.ShowDialog();
            }
            else
            {
                MessageBox.Show("server 信息为  null !");
            }
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确认删除吗？", "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                var serverInfo = (sender as Button).DataContext as Entities.ServerLimitRow;
                if (serverInfo != null)
                {
                    ////转至 编辑总股数及自动分配额度界面。
                    //var win = new Win.winLimitEdit();
                    //win.Init(serverInfo);
                    //win.ShowDialog();
                    string errMsg;
                    if (serverInfo.DeleteLimit(out errMsg))
                    {
                        MessageBox.Show("删除成功！");
                        DeleteLimitFromCacheDict(serverInfo);
                        InitFiltedData();
                    }
                    else
                    {
                        MessageBox.Show("删除失败！" + errMsg);
                    }
                }
                else
                {
                    MessageBox.Show("server 信息为  null !");
                }
            }
        }

        private void DeleteLimitFromCacheDict(ServerLimitRow serverInfo)
        {
            foreach (var item in dictLimits)
            {
                if (item.Key.Ip == serverInfo.Ip)
                {
                    var row = item.Value.FirstOrDefault(_ => _.交易员 == serverInfo.交易员 && _.证券代码 == serverInfo.证券代码 && _.组合号 == serverInfo.组合号);
                    item.Value.Remove额度分配Row(row);
                    break;
                }
            }
        }

        private void cmbServerFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitFiltedData();
        }

        private void cmbGroupFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGroupFilter.SelectedIndex > 0)
            {
                txtGroup.Text = cmbGroupFilter.SelectedItem.ToString();
            }
            else
            {
                txtGroup.Text = string.Empty;
            }
            InitFiltedData();
        }

        private void txtGroup_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtGroup.Text.Length == 3 || txtGroup.Text.Length == 0)
            {
                InitFiltedData();
                txtGroup.Background  = new SolidColorBrush(Colors.White);
            }
            else 
            {
                txtGroup.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void txtStock_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtStock.Text.Length == 0 || Regex.IsMatch(txtStock.Text, "^[0-9]{5,6}$"))
            {
                InitFiltedData();
                txtStock.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                txtStock.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            this.loading.ShowLoading();
            var task = new Task(() =>
            {
                Init();
                return;
            });
            task.Start();
        }



        private void Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            InitFiltedData();
        }

        private void txtAccount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtAccount.Text.Length == 0 || txtAccount.Text.Length >= 3)
            {
                InitFiltedData();
            }
        }

        private void CheckBox_All_Click(object sender, RoutedEventArgs e)
        {
            var ckBox = sender as CheckBox;
            if (ckBox != null)
            {
                foreach (var item in dgStocksMain.ItemsSource)
                {
                    var limitItem = item as Entities.ServerLimitRow;
                    if (limitItem != null)
                    {
                        limitItem.IsSelected = ckBox.IsChecked == true;
                    }
                }
            }
        }

        private void Button_DeleteSeleteced_Click(object sender, RoutedEventArgs e)
        {
            List<ServerLimitRow> lstLimitList = new List<ServerLimitRow>();
            foreach (var item in dgStocksMain.ItemsSource)
            {
                var limitItem = item as Entities.ServerLimitRow;
                if (limitItem != null && limitItem.IsSelected)
                {
                    lstLimitList.Add(limitItem);
                }
            }
            if (lstLimitList.Count <= 0)
            {
                MessageBox.Show("请选中删除项");
                return;
            }
            var remindMsg = string.Format("选中项{0}条，确认删除吗?", lstLimitList.Count);
            if (MessageBox.Show(remindMsg, "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                DeleteLimits(lstLimitList);
            }

        }

        private void DeleteLimits(List<ServerLimitRow> lstLimitList)
        {
            loading.ShowLoading("开始逐项删除.");
            var task = new Task(() => {
                int rowNum = 0;
                int successCount = 0;
                
                foreach (var item in lstLimitList)
                {
                    rowNum++;
                    string errMsg;
                    try
                    {
                        Dispatcher.Invoke(() => { loading.ShowLoading(string.Format("总计{0}项，正在删除第{1}项", lstLimitList.Count, rowNum)); });
                        if (item.DeleteLimit(out errMsg))
                        {
                            successCount++;
                            this.DeleteLimitFromCacheDict(item);
                        }
                        else 
                        {
                            Utils.LogFormat("删除失败，交易员{0},证券代码{1},组合号{2}", item.交易员, item.证券代码, item.组合号);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.LogFormat("删除异常，Server {0}，IP {1}，交易员{2}, 证券代码{3}, 组合号{4}, 异常信息{5}", item.Remark, item.Ip, item.交易员, item.证券代码, item.组合号, ex.Message);
                    }
                }
                Dispatcher.Invoke(() => 
                {
                    this.InitFiltedData();
                    loading.HideLoading();
                    MessageBox.Show(string.Format("删除操作完成，总计{0}项，删除成功{1}项。", lstLimitList.Count, successCount ));
                });
            });
            task.Start();
        }

     
    }
}
