using DataComparision.DataAdapter;
using DataComparision.Entity;
using DataComparision.Utils;
using DataComparision.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DataComparision.Controls
{
    /// <summary>
    /// ComparePage.xaml 的交互逻辑
    /// </summary>
    public partial class ComparePage : UserControl
    {
        

        public ComparePage()
        {
            InitializeComponent();
            this.Loaded += ComparePage_Loaded;
        }

        void ComparePage_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshCombInfo();
        }

        public void RefreshWindow(bool isShowMsg = true)
        {
            var groupID = this.txt组合号.Text.Trim();
            var date = this.dpDate.SelectedDate;

            if (ValidSearchInfo(groupID, date, isShowMsg))
            {
                Utils.ControlUtils.ShowLoading(this.Loading);

                Dispatcher.RunAsync(() => { LoadData(groupID, date.Value); });
            }
        }

        public void RefreshCombInfo()
        {
            if (cmbGroup.Items.Count == 0 || cmbDate.Items.Count == 0)
            {
                try
                {
                    using (var db = new DataComparisionDataset())
                    {
                        var dtList =db.软件委托ds.Select(_ => _.成交日期).Distinct().ToList();
                        var gList = db.软件委托ds.Select(_ => _.组合号).Distinct().ToList();

                        var date1 = dtList.OrderByDescending(_ => _).Select(_ => _.ToString("yyyy/MM/dd")).Take(10).ToList();
                        var group1 = gList.OrderBy(_ => _).ToList();

                        //var lst1 = db.交割单ds.ToList();
                        //var lst2 = db.软件委托ds.ToList();

                        //var group1 = lst1.Select(_ => _.组合号).Union(lst2.Select(_ => _.组合号)).Distinct().OrderBy(_ => _).ToList();
                        //var date1 = lst1.Select(_ => _.交割日期.ToString("yyyy/MM/dd")).Union(lst2.Select(_ => _.成交日期.ToString("yyyy/MM/dd"))).Distinct().OrderByDescending(_ => _).Take(10).ToList();
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            cmbGroup.ItemsSource = group1;
                            cmbDate.ItemsSource = date1;

                            ControlUtils.HideLoading(this.Loading);
                        }));
                    }
                }
                catch { }
            }
           
        }

        private void LoadData(string groupID, DateTime date)
        {
            List<Entity.交割单> lstDelivery = null;
            List<Entity.软件委托> lstSoft = null;

            var delMatched = new List<交割单>();
            var delNotMatched = new List<交割单>();
            var softMatched = new List<软件委托>();
            var softNotMatched = new List<软件委托>();
            try
            {
                using (var db = new DataComparisionDataset())
                {
                    lstDelivery = db.交割单ds.Where(_ => _.组合号 == groupID && _.交割日期 == date).OrderBy(_ => _.SortSequence).ToList();
                    lstSoft = db.软件委托ds.Where(_ => _.组合号 == groupID && _.成交日期 == date).OrderBy(_ => _.SortSequence).ToList();
                    if (lstDelivery.Count == 0 && lstSoft.Count != 0)
                    {
                        var groupItem = CommonUtils.DictGroup[groupID];
                        if (groupItem != null)
                        {
                            var dt = DataHelper.QueryHisData(date, date, groupItem);
                            if (dt != null)
                            {
                                DataHelper.StandardDeliveryDataTable(dt, date);
                                var isSaveSuccess = DataHelper.WriteToDB(dt, "交割单");
                                if (isSaveSuccess)
                                {
                                    lstDelivery = db.交割单ds.Where(_ => _.组合号 == groupID && _.交割日期 == date).OrderBy(_ => _.SortSequence).ToList();
                                }
                                
                            }
                        }
                        else
                        {
                            this.Dispatcher.ShowMsg("该组合号对应券商信息未添加！");
                        }
                    }

                    if (lstDelivery.Count > 0)
                    {
                        EntityCompareUtil.CompareData(lstDelivery, lstSoft, delMatched, softMatched, delNotMatched, softNotMatched);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Dispatcher.ShowMsg(ex.Message);
            }
           

            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                dgMatchedDelivery.ItemsSource = delMatched;
                dgNotMatchedDelivery.ItemsSource = delNotMatched;

                dgMatchedSoft.ItemsSource = softMatched;
                dgSoftwareLost.ItemsSource = softNotMatched;

                dgDelivery.ItemsSource = lstDelivery;
                dgSoftware.ItemsSource = lstSoft;
                ControlUtils.HideLoading(this.Loading);
            }));
        }

        private static bool ValidSearchInfo(string groupID, DateTime? date, bool isShowMsg = true)
        {
            bool IsValid = true;
            string errMsg = string.Empty;
            if (!date.HasValue)
            {
                errMsg = "请选择日期！";
                IsValid = false;
            }
            else if (string.IsNullOrEmpty(groupID))
            {
                errMsg = "组合号不能为空，请选择组合号！";
                IsValid = false;
            }
            if (!string.IsNullOrEmpty(errMsg) && isShowMsg)
            {
                CommonUtils.ShowMsg(errMsg);
            }
            return IsValid;
        }

        #region Events
        private void cmbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGroup.SelectedIndex > -1)
            {
                txt组合号.Text = cmbGroup.SelectedItem.ToString();
            }

        }

        private void cmbDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDate.SelectedIndex > -1)
            {
                var date = DateTime.Parse(cmbDate.SelectedItem.ToString());
                if (dpDate.SelectedDate != date)
                {
                    dpDate.SelectedDate = date;
                }
            }
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            RefreshWindow();
        }

        private void Button_Delivery_Delete_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as 交割单;

            if (item != null)
            {
                var result = MessageBox.Show("确认删除此交割单吗?", "交割单删除", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    using (var db = new DataComparisionDataset())
                    {
                        var lst = db.交割单ds.ToList();
                        var editItem = lst.FirstOrDefault(_ => EntityCompareUtil.IsSame(item, _));
                        if (editItem != null)
                        {
                            db.交割单ds.Remove(editItem);
                            db.SaveChanges();
                        }
                    }
                    RefreshWindow(false);
                }

            }
        }

        private void Button_Delivery_Edit_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as 交割单;
            if (item != null)
            {
                var win = new View.DeliveryEdit(false, item);
                win.OnEditComplete += new Action(() => { this.RefreshWindow(false); });
                win.Show();
            }
        }

        private void Button_Delivery_CopyAdd_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as 交割单;
            if (item != null)
            {
                var win = new View.DeliveryEdit(true, item);
                win.OnEditComplete += new Action(() => { this.RefreshWindow(false); });
                win.ShowDialog();
            }
        }

        private void Button_Soft_Delete_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as 软件委托;

            if (item != null)
            {
                var result = MessageBox.Show("确认删除此委托吗?", "委托删除", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    using (var db = new DataComparisionDataset())
                    {
                        var lst = db.软件委托ds.ToList();
                        var deleteItem = lst.FirstOrDefault(_ => EntityCompareUtil.IsSame(item, _));
                        if (deleteItem != null)
                        {
                            db.软件委托ds.Remove(deleteItem);
                            db.SaveChanges();
                        }
                    }
                    RefreshWindow(false);
                }
            }
        }

        private void Button_Soft_Edit_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as 软件委托;

            if (item != null)
            {
                var win = new View.SoftwareEdit(false, item);
                win.OnEditComplete += new Action(() => { this.RefreshWindow(false); });
                win.ShowDialog();
            }
        }

        private void Button_Soft_CopyAdd_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as 软件委托;

            if (item != null)
            {
                var win = new View.SoftwareEdit(true, item);
                win.OnEditComplete += new Action(() => { this.RefreshWindow(false); });
                win.ShowDialog();
            }
        }

        private void Button_DeliveryNotMatched_SetSpecal_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as 交割单;

            if (item != null)
            {
                var result = MessageBox.Show("确认强制保留此交割单吗?", "交割单编辑", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    using (var db = new DataComparisionDataset())
                    {
                        var lst = db.交割单ds.ToList();
                        var editItem = lst.FirstOrDefault(_ => EntityCompareUtil.IsSame(item, _));
                        if (editItem != null)
                        {
                            editItem.备注 = "强制保留";
                            db.SaveChanges();
                        }
                    }
                    RefreshWindow(false);
                }
            }
        }

        

        private void dgMatchedDelivery_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var entity = e.Row.DataContext as 交割单;
            if (entity == null)
                return;

            if (entity.备注 == "合计")
                e.Row.Background = CommonUtils.totalBrush;
            else
                e.Row.Background = CommonUtils.normalBrush;
        }

        

        
        #endregion

        

       
    }
}
