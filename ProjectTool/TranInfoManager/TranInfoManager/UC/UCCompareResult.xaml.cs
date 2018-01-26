using System;
using System.Collections.Generic;
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
using System.Windows.Threading;
using TranInfoManager.Entity;

namespace TranInfoManager.UC
{
    /// <summary>
    /// UCCompareResult.xaml 的交互逻辑
    /// </summary>
    public partial class UCCompareResult : UserControl
    {
        SolidColorBrush normalBrush = new SolidColorBrush(Colors.White);
        SolidColorBrush totalBrush = new SolidColorBrush(Colors.LightSalmon);
        SolidColorBrush specalBrush = new SolidColorBrush(Colors.Orange);

        DateTime StartDate { get { return dpDateStart.SelectedDate.Value.Date; } }

        DateTime EndDate { get { return dpDateEnd.SelectedDate.Value.Date.AddDays(1).AddMilliseconds(-1); } }

        public List<CompareDaily> CompareDailyList { get; set; }

        public List<CompareTrader> CompareTraderList { get; set; }

        public UCCompareResult()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            try
            {
                using (var db = new ManageDataset())
                {
                    var t = db.TraderDailyDS.Select(_ => _.Date).Distinct().ToList();
                    var m = db.MarketDailyDS.Select(_ => _.TradeDate).Distinct().ToList();
                    
                    var exceptDate = t.Except(m).ToList();
                    foreach (var item in exceptDate)
                    {
                        t.Remove(item);
                    }
                    
                    this.cmbDate.ItemsSource = t.OrderByDescending(_=> _ ).Take(10).Select(_ => _.ToString("yyyy/MM/dd"));
                }
            }
            catch (Exception)
            {
                //CommonUtils.ShowMsg("比较器初始化错误：" + ex.Message);
            }
        }

        #region Events
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (Valid())
            {
                try
                {
                    this.loading.ShowLoading();
                    SetPageData(StartDate, EndDate);

                }
                catch (Exception ex)
                {
                    CommonUtils.ShowMsg("查询出错！" + ex.Message);
                }
            }
        }

        private void btnCaculate_Click(object sender, RoutedEventArgs e)
        {
            if (Valid())
            {
                try
                {
                    DateTime startDate = StartDate;
                    DateTime endDate = EndDate;
                    this.loading.ShowLoading("统计中，请耐心等待！");
                    Dispatcher.RunAsync(() => { RefreshCompareData(startDate, endDate); }, null, null,() => { SetPageData(startDate, endDate); });
                }
                catch (Exception ex)
                {
                    CommonUtils.ShowMsg("查询出错！" + ex.Message);
                }
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            FiltePageData();
        }

        private void cbOnlySum_Click(object sender, RoutedEventArgs e)
        {
            FiltePageData();
        }

        private void dgDaily_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;

            var entity = e.Row.DataContext as Entity.CompareDaily;
            if (entity == null)
                return;

            if (entity.Symbol2 == "合计" || entity.Symbol2 == "小计")
                e.Row.Background = totalBrush;
            else
                e.Row.Background = normalBrush;
        }

        private void dgTraderDetail_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var entity = e.Row.DataContext as Entity.CompareTrader;
            if (entity == null)
                return;

            if (entity.Symbol2 == "合计")
                e.Row.Background = totalBrush;
            else
                e.Row.Background = normalBrush;
        }

        private void cbTrader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTrader.SelectedIndex != -1)
            {
                cbTrader.SelectedIndex = -1;
            }
        }
        #endregion

        private bool Valid()
        {
            if (!dpDateStart.SelectedDate.HasValue)
            {
                CommonUtils.ShowMsg("请选择开始日期！");
                dpDateStart.Focus();
                return false;
            }
            if (!dpDateEnd.SelectedDate.HasValue)
            {
                dpDateEnd.Focus();
                CommonUtils.ShowMsg("请选择截止日期！");
                return false;
            }
            return true;
        }

        private void RefreshCompareData(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ManageDataset())
                {
                    //var edt = endDate.Date;
                    var listCompareDailyOld = db.CompareDailyDS.Where(_ => _.DATE >= startDate && _.DATE <= endDate).ToList();
                    //var listCompareTraderOld = db.CompareTraderDS.Where(_ => _.StartDate == startDate && _.EndDate == edt).ToList();

                    var lstM = db.MarketDailyDS.Where(_ => _.TradeDate >= startDate && _.TradeDate <= endDate).ToList();
                    var lstT = db.TraderDailyDS.Where(_ => _.Date >= startDate && _.Date <= endDate).ToList();

                    var listCompareDaily = Entity.EntityCompareHelper.GetCompareDaily(lstM, lstT);
                    db.CompareDailyDS.RemoveRange(listCompareDailyOld);
                    db.CompareDailyDS.AddRange(listCompareDaily);

                    //var listCompareTrader = EntityCompareHelper.GetCompareTrader(startDate, edt, listCompareDaily);
                    //db.CompareTraderDS.RemoveRange(listCompareTraderOld);
                    //db.CompareTraderDS.AddRange(listCompareTrader);

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log(ex.Message);
            }
        }

        private void SetPageData(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<CompareDaily> lstCompareDaily = null;
                List<CompareTrader> lstCompareTrader = null;

                using (var db = new ManageDataset())
                {
                    lstCompareDaily = db.CompareDailyDS.Where(_ => _.DATE >= startDate && _.DATE <= endDate).ToList();

                    lstCompareDaily = lstCompareDaily.OrderBy(_ => _.DATE).ThenBy(_ => _.TRADER).ThenBy(_ => _.Symbol2).ThenBy(_ => _.Seq).ToList();
                    lstCompareTrader = EntityCompareHelper.GetCompareTrader(startDate, endDate.Date, lstCompareDaily);

                    CompareDailyList = lstCompareDaily;
                    CompareTraderList = lstCompareTrader;
                }

                cbTrader.Items.Clear();
                List<string> lstTrader = lstCompareTrader.Select(_ => _.Trader).Distinct().ToList();
                lstTrader.ForEach(_ => this.cbTrader.Items.Add(new CheckBox() { Content = _, IsChecked = true }));

                lstCompareDaily.ForEach(_ => { _.FormatData(); });
                lstCompareTrader.ForEach(_=>_.FormatData());

                this.dgDailyDetail.ItemsSource = lstCompareDaily;
                this.dgTraderDetail.ItemsSource = lstCompareTrader;
                this.bdFilter.Visibility = Visibility.Visible;
                this.loading.HideLoading();
            }
            catch (Exception ex)
            {
                this.loading.HideLoading();
                CommonUtils.Log("在显示界面时出错了!", ex);
                CommonUtils.ShowMsg("出错了，请查看日志");
            }
        }

        private void FiltePageData()
        {
            DateTime startDate = dpDateStart.SelectedDate.Value.Date;
            DateTime endDate = dpDateEnd.SelectedDate.Value.Date.AddDays(1).AddMilliseconds(-1);
            bool isSumOnly = cbOnlySum.IsChecked == true;

            List<string> lstTrader = new List<string>();
            foreach (CheckBox item in cbTrader.Items)
            {
                if (item.IsChecked == true)
                    lstTrader.Add(item.Content.ToString());
            }

            this.loading.ShowLoading();

            List<CompareDaily> lstC = null;
            List<CompareTrader> lstT = null;
            Dispatcher.RunAsync(() =>
            {
                if (isSumOnly)
                {
                    lstC = CompareDailyList.Where(_ => _.Symbol2 == "小计" || _.Symbol2 == "合计").ToList();
                    lstT = CompareTraderList.Where(_ => _.Symbol2 == "合计").ToList();
                }
                else
                {
                    lstC = this.CompareDailyList.Where(_ => lstTrader.Contains(_.TRADER)).ToList();
                    lstT = this.CompareTraderList.Where(_ => lstTrader.Contains(_.Trader)).ToList();
                }
            },
            null,
            null,
            () =>
            {
                this.dgDailyDetail.ItemsSource = lstC;
                this.dgTraderDetail.ItemsSource = lstT;
                this.loading.HideLoading();
            });
        }

        private void cmbDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDate.SelectedIndex > -1)
            {
                var date = DateTime.Parse(cmbDate.SelectedItem.ToString());
                dpDateStart.SelectedDate = date;
                dpDateEnd.SelectedDate = date;
            }
        }


    }
}
