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
using System.Windows.Threading;
using TranInfoManager.Entity;

namespace TranInfoManager.UC
{
    /// <summary>
    /// UCComparer.xaml 的交互逻辑
    /// </summary>
    public partial class UCComparer : UserControl
    {
        public UCComparer()
        {
            InitializeComponent();
            this.Loaded += UCComparer_Loaded;
            
        }

        void UCComparer_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        public void Init()
        {
            try
            {
                using (var db = new ManageDataset())
                {
                    var t = db.TraderDailyDS.Select(_=>_.Date).Distinct().ToList();
                    var m = db.MarketDailyDS.Select(_=>_.TradeDate).Distinct().ToList();
                    t.AddRange(m);
                    t = t.Distinct().ToList();
                    this.cmbDate.ItemsSource = t.Select(_=>_.ToShortDateString());
                }
            }
            catch (Exception)
            {
                //CommonUtils.ShowMsg("比较器初始化错误：" + ex.Message);
            }
        }

        public void RefreshPageData(DateTime dt)
        {
            try
            {
                using (var db = new ManageDataset())
                {
                    var t = db.TraderDailyDS.Where(_ => _.Date == dt).ToList();
                    var m = db.MarketDailyDS.Where(_ => _.TradeDate == dt.Date).ToList();

                    var listNotMatchedMarket = new List<MarketDetailDaily>();
                    var result = EntityCompareHelper.GetCompareData(m, t, listNotMatchedMarket);

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                        this.loading.HideLoading();
                        this.dgMarketMatched.ItemsSource = result.Keys;
                        this.dgTradeMatched.ItemsSource = result.Values;
                        this.dgNotMatchedMarket.ItemsSource = listNotMatchedMarket;

                        this.dgMarket.ItemsSource = m;
                        this.dgTrader.ItemsSource = t;
                    }));
                }
            }
            catch (Exception ex)
            {
                CommonUtils.ShowMsg(ex.Message);
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
            if (dpDate.SelectedDate.HasValue)
            {
                var dt = dpDate.SelectedDate.Value;
                this.loading.ShowLoading("查询及比较中，可能比较耗时！");
                Dispatcher.RunAsync(() => { RefreshPageData(dt); });
            }
            else
            {
                CommonUtils.ShowMsg("请选择日期！");
            }
        }

        private void DataGridSoftware_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
