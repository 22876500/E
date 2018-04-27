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

namespace DataComparision.Controls
{
    /// <summary>
    /// OrderCancelRate.xaml 的交互逻辑
    /// </summary>
    public partial class OrderCancelRate : UserControl
    {
        public OrderCancelRate()
        {
            InitializeComponent();
            this.Loaded += OrderCancelRate_Loaded;
        }

        void OrderCancelRate_Loaded(object sender, RoutedEventArgs e)
        {
            this.dpDateStart.SelectedDate = DateTime.Today.AddDays(-1);
            this.dpDateEnd.SelectedDate = DateTime.Today.AddDays(-1);
            try
            {
                using (var db = new DataComparisionDataset())
                {
                    var dates = db.软件委托ds.Select(_=> _.成交日期).Max();
                    this.dpDateStart.SelectedDate = dates;
                    this.dpDateEnd.SelectedDate = dates;
                }
            }
            catch (Exception)
            {
            }

            SelectCancelRateInfo();
            
        }

        private void SelectCancelRateInfo()
        {
            var dtStart = dpDateStart.SelectedDate.Value;
            var dtEnd = dpDateEnd.SelectedDate.Value;
            
            try
            {
                var list = new List<TraderOrderCancelRate>();
                using (var db = new DataComparisionDataset())
                {
                    var orders = db.软件委托ds.Where(_ => _.成交日期 >= dtStart && _.成交日期 <= dtEnd);
                    foreach (var item in orders)
                    {
                        if (list.Exists(_ => _.Trader == item.交易员))
                        {
                            var rateItem = list.First(_ => _.Trader == item.交易员);

                            rateItem.OrderTotalCount++;
                            if (item.撤单数量 > 0)
                            {
                                rateItem.OrderCancelCount++;
                            }
                        }
                        else
                        {
                            var rateItem = new TraderOrderCancelRate()
                            {
                                StartDate = dtStart,
                                EndDate = dtEnd,
                                OrderCancelCount = item.撤单数量 > 0 ? 1 : 0,
                                OrderTotalCount = 1,
                                Trader = item.交易员
                            };
                            list.Add(rateItem);
                        }
                    }
                }

                this.dgCancelRate.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("SelectCancelRateInfo Exception:" + ex.Message);
            }
        }

        class TraderOrderCancelRate
        {
            public string Trader { get; set; }

            public decimal OrderTotalCount { get; set; }

            public decimal OrderCancelCount { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public string CancelRate 
            {
                get 
                {
                    if (OrderTotalCount <= 0)
                    {
                        return "0";
                    }
                    else
                    {
                        return Math.Round((OrderCancelCount / OrderTotalCount) * 100, 2) + "%";
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SelectCancelRateInfo();
        }
    }
}
