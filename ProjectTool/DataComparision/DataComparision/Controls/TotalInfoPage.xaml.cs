using DataComparision.Entity;
using DataComparision.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// TotalMain.xaml 的交互逻辑
    /// </summary>
    public partial class TotalInfoPage : UserControl
    {
        public TotalInfoPage()
        {
            InitializeComponent();

            this.Loaded += TotalMain_Loaded;
        }

        #region Events
        void TotalMain_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshPage();
        }

        private void dgCal_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var entity = e.Row.DataContext as Entity.合计表;
            if (entity == null)
                return;
            
            //if (entity.剔除备注 != null && entity.剔除备注.Length > 5000)
            if(entity.账户== "C02" || entity.账户 == "C03")
            {
                entity.剔除备注 = string.Empty;
            }

            if (entity.账户 == "合计")
                e.Row.Background = CommonUtils.totalBrush;
            else
                e.Row.Background = CommonUtils.normalBrush;
        }

        private void dgMerge_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var entity = e.Row.DataContext as MergeTotal;
            if (entity == null)
                return;

            if (entity.账户 == "合计")
                e.Row.Background = CommonUtils.totalBrush;
            else
                e.Row.Background = CommonUtils.normalBrush;
        }

        private void dgCal_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                CopydgDataGrid<合计表>(dgCal);
            }
        }

        private void dgMerge_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                CopydgDataGrid<MergeTotal>(dgMerge);
            }
        }

        private void ckbMergeGroup_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage();
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage();
        }

        private void Button_Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (!dpDateStart.SelectedDate.HasValue)
                return;
            if (!dpDateEnd.SelectedDate.HasValue)
                return;

            var dtStart = this.dpDateStart.SelectedDate.Value;
            var dtEnd = this.dpDateEnd.SelectedDate.Value;

            CalculateAsync(dtStart, dtEnd, this.cmbGroup.SelectedIndex < 1 ? "" : cmbGroup.Text);
        }
        #endregion

        private void CalculateAsync(DateTime dtStart, DateTime dtEnd, string groupName = null)
        {
            ControlUtils.ShowLoading(this.loading);
            Task t = new Task(() =>
            {
                try
                {
                    using (var db = new DataComparisionDataset())
                    {
                        var lstTotalExists = db.合计表ds.Where(_ => _.日期 >= dtStart && _.日期 <= dtEnd);
                        var lstDeli = db.交割单ds.Where(_ => _.交割日期 >= dtStart && _.交割日期 <= dtEnd).OrderBy(_=> _.组合号).ThenBy(_ => _.SortSequence).ToList();
                        var lstSoft = db.软件委托ds.Where(_ => _.成交日期 >= dtStart && _.成交日期 <= dtEnd).OrderBy(_ => _.组合号).ThenBy(_=> _.SortSequence).ToList();

                        if (!string.IsNullOrEmpty(groupName))
                        {
                            lstTotalExists = lstTotalExists.Where(_ => _.账户 == groupName);
                            lstDeli = lstDeli.Where(_ => _.组合号 == groupName).ToList();
                            lstSoft = lstSoft.Where(_ => _.组合号 == groupName).ToList();
                        }

                        var list = Entity.EntityCompareUtil.CompareData(lstDeli, lstSoft);

                        db.合计表ds.RemoveRange(lstTotalExists);
                        db.合计表ds.AddRange(list);
                        db.SaveChanges();

                        Dispatcher.HideLoading(this.loading);
                        Dispatcher.RunAsync(null, () => { RefreshPage(); });
                    }
                }
                catch (Exception ex)
                {
                    CommonUtils.Log(DateTime.Now.ToShortTimeString() + " 计算合并信息时出错 ", ex);
                    Dispatcher.ShowMsg("运行异常，计算合并信息时出错，详情请查看日志！");
                    Dispatcher.HideLoading(this.loading);
                }
            });

            t.Start();

            //var thread = new Thread(new ThreadStart(() =>
            //{
                
            //}));
            //thread.Start();
        }

        void RefreshPage()
        {
            List<合计表> lstTotal = null;

            DateTime? dtStart = dpDateStart.SelectedDate;
            DateTime? dtEnd = dpDateEnd.SelectedDate;
            var actionDo = new Action(() => { lstTotal = GetTotalData(dtStart, dtEnd); });
            var actionDoUI = new Action(() => 
            {
                dpDateStart.SelectedDate = dtStart;
                dpDateEnd.SelectedDate = dtEnd;
            });

            var actionComplete = new Action(() => { InitPageByList(lstTotal); });

            CommonUtils.RunAsync(this.Dispatcher, actionDo, actionDoUI, null, actionComplete);
           
        }

        private void InitPageByList(List<合计表> lstTotal)
        {
            if (lstTotal == null)
            {
                return;
            }
            DateTime dt = DateTime.Now;
            var groups = lstTotal.Select(_ => _.账户).ToList();
            if (cmbGroup.Items.Count != groups.Count + 1)
            {
                groups.Insert(0, "全部");
                cmbGroup.ItemsSource = groups;
            }

            if (cmbGroup.SelectedIndex < 0)
                cmbGroup.SelectedIndex = 0;
            else if (cmbGroup.SelectedIndex > 0)
                lstTotal = lstTotal.Where(_ => _.账户 == cmbGroup.Text).ToList();
            DateTime dt1 = DateTime.Now;
            var cost0 = (dt1 - dt).TotalSeconds;
            if (ckbMergeGroup.IsChecked == true)
            {
                var lstMerged = Merge(lstTotal);

                dgMerge.ItemsSource = lstMerged;
                dgCal.Visibility = Visibility.Collapsed;
                dgMerge.Visibility = Visibility.Visible;
            }
            else
            {
                if (lstTotal.Count > 0)
                {
                    var totalItem = new 合计表() { 成交金额 = lstTotal.Sum(_ => _.成交金额), 发生金额 = lstTotal.Sum(_ => _.发生金额), 账户 = "合计", 日期 = lstTotal.Max(_ => _.日期) };
                    lstTotal.Add(totalItem);
                }
                dgCal.ItemsSource = lstTotal;
                dgCal.Visibility = Visibility.Visible;
                dgMerge.Visibility = Visibility.Collapsed;
            }
        }

        private List<合计表> GetTotalData(DateTime? dtStart, DateTime? dtEnd)
        {
            DateTime dt = DateTime.Now;
            List<合计表> lstTotal = null;
            try
            {
                
                using (var dbDataset = new DataComparisionDataset())
                {
                    if (!dtStart.HasValue && !dtEnd.HasValue)
                    {
                        var dateNewest = dbDataset.交割单ds.Max(_ => _.交割日期);

                        dtStart = dateNewest;
                        dtEnd = dateNewest;
                    }

                    lstTotal = dbDataset.合计表ds.Where(_ => _.日期 >= dtStart && _.日期 <= dtEnd).ToList();
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("合计界面刷新失败：" + ex.Message);
            }

            //var cost = (DateTime.Now - dt).TotalSeconds;
            //CommonUtils.Log("合计查询总耗时" + cost);
            return lstTotal;
        }

        private List<MergeTotal> Merge(List<合计表> lstTotal)
        {
            var dict = new Dictionary<string, MergeTotal>();
            foreach (var item in lstTotal)
            {
                if (dict.ContainsKey(item.账户))
                {
                    dict[item.账户].成交金额 += item.成交金额;
                    dict[item.账户].发生金额 += item.发生金额;
                }
                else
                {
                    dict.Add(item.账户, new MergeTotal()
                    {
                        账户 = item.账户,
                        成交金额 = item.成交金额,
                        发生金额 = item.发生金额,
                        开始日期 = this.dpDateStart.SelectedDate.Value.ToString("yyyy/MM/dd"),
                        结束日期 = this.dpDateEnd.SelectedDate.Value.ToString("yyyy/MM/dd"),
                    });
                }
            }

            var lstMerged = dict.Values.ToList();
            if (lstMerged.Count > 0)
            {
                var totalInfo = new MergeTotal()
                {
                    开始日期 = this.dpDateStart.SelectedDate.Value.ToString("yyyy/MM/dd"),
                    结束日期 = this.dpDateEnd.SelectedDate.Value.ToString("yyyy/MM/dd"),
                    账户 = "合计",
                    成交金额 = lstMerged.Sum(_ => _.成交金额),
                    发生金额 = lstMerged.Sum(_ => _.发生金额),
                };
                lstMerged.Add(totalInfo);
            }
            return lstMerged;
        }

        private void CopydgDataGrid<T>(DataGrid dg)
        {
            var str = ExcelUtils.RenderToString<T>(dg);
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Clipboard.SetDataObject(str);
                    break;
                }
                catch { }
                Thread.Sleep(10);
            }
        }


        public class MergeTotal
        {
            public string 开始日期 { get; set; }

            public string 结束日期 { get; set; }

            public string 账户 { get; set; }

            public decimal 成交金额 { get; set; }

            public decimal 发生金额 { get; set; }

            public string 剔除备注 { get; set; }
        }

        private void dgCalOutput_Click(object sender, RoutedEventArgs e)
        {
            //SaveFileDialog s = new SaveFileDialog() { Filter = "Excel 2003|*.xls " };
            //if (s.ShowDialog() == true && !string.IsNullOrEmpty(s.FileName))
            //{
            //    this.loading.ShowLoading();
            //    this.Dispatcher.InvokeAsync(() =>
            //    {
            //        var str = ExcelUtils.RenderToString<合计表>(dgCal);
            //        File.WriteAllText(s.FileName, str, Encoding.UTF8);
            //        this.loading.HideLoading();
            //    });
                
            //}
            ExcelUtils.ExportToExcel<合计表>(dgCal);
        }

    }
}
