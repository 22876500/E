using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using OKCoinClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace OKCoinClient.View
{
    /// <summary>
    /// TradeIndexView.xaml 的交互逻辑
    /// </summary>
    public partial class TradeIndexView : UserControl
    {
        private ObservableDataSource<KeyValuePair<DateTime, double>> dataSourceThisWeek = new ObservableDataSource<KeyValuePair<DateTime, double>>();
        private ObservableDataSource<KeyValuePair<DateTime, double>> dataSourceNextWeek = new ObservableDataSource<KeyValuePair<DateTime, double>>();
        private ObservableDataSource<KeyValuePair<DateTime, double>> dataSourceQuarter = new ObservableDataSource<KeyValuePair<DateTime, double>>();
        private ObservableDataSource<KeyValuePair<DateTime, double>> dataSourceIndex = new ObservableDataSource<KeyValuePair<DateTime, double>>();
        private DispatcherTimer timer = new DispatcherTimer();
        TradeIndexModel dataModel;

        public TradeIndexView()
        {
            InitializeComponent();
        }

        private void TradeIndexView_Loaded(object sender, RoutedEventArgs e)
        {
            dataModel = new TradeIndexModel();
            this.DataContext = dataModel;
            
            //this.chartThisWeek.AddLineGraph(this.dataSourceThisWeek, 2, "本周升贴水");
            this.dataSourceThisWeek.SetXMapping(_ => Axis_X_ThisWeek1.ConvertToDouble(_.Key));
            this.dataSourceThisWeek.SetYMapping(_ => _.Value);

            //this.chartNextWeek.AddLineGraph(this.dataSourceNextWeek, 2, "次周升贴水");
            this.dataSourceNextWeek.SetXMapping(_ => Axis_X_ThisWeek1.ConvertToDouble(_.Key));
            this.dataSourceNextWeek.SetYMapping(_ => _.Value);

            //this.chartQuarter.AddLineGraph(this.dataSourceQuarter, 2, "季度升贴水");
            this.dataSourceQuarter.SetXMapping(_ => Axis_X_ThisWeek1.ConvertToDouble(_.Key));
            this.dataSourceQuarter.SetYMapping(_ => _.Value);

            this.chartThisWeek1.AddLineGraph(this.dataSourceThisWeek, Colors.Red, 2, "本周升贴水");
            //Axis_X_ThisWeek1.Visibility = System.Windows.Visibility.Collapsed;

            this.chartNextWeek1.AddLineGraph(this.dataSourceNextWeek, Colors.Blue, 2, "次周升贴水");
            //Axis_X_NextWeek1.Visibility = System.Windows.Visibility.Collapsed;

            this.chartQuarter1.AddLineGraph(this.dataSourceQuarter, Colors.Purple, 2, "季度升贴水");
            //Axis_X_Quarter1.Visibility = System.Windows.Visibility.Collapsed;

            this.chartIndex.AddLineGraph(this.dataSourceIndex, Colors.Green, 2, "指数");
            this.dataSourceIndex.SetXMapping(_ => Axis_X_ThisWeek1.ConvertToDouble(_.Key));
            this.dataSourceIndex.SetYMapping(_ => _.Value);

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(AnimatedLine);
            timer.IsEnabled = true;

            chartThisWeek1.Viewport.FitToView();
            chartNextWeek1.Viewport.FitToView();
            chartQuarter1.Viewport.FitToView();
            chartIndex.Viewport.FitToView();
        }

        private void AnimatedLine(object sender, EventArgs e)
        {
            var model = dataModel;
            DateTime dt = DateTime.Now;
            if (!string.IsNullOrEmpty(model.ThisWeekSubResult))
            {
                double y = Math.Round(double.Parse(model.ThisWeekSubResult.Split(' ')[0]));
                dataSourceThisWeek.AppendAsync(this.Dispatcher, new KeyValuePair<DateTime, double>(dt, y));
            }

            if (!string.IsNullOrEmpty(model.NextWeekSubResult))
            {
                double y = Math.Round(double.Parse(model.NextWeekSubResult.Split(' ')[0]));
                dataSourceNextWeek.AppendAsync(this.Dispatcher, new KeyValuePair<DateTime, double>(dt, y));
            }

            if (!string.IsNullOrEmpty(model.QuarterSubResult))
            {
                double y = Math.Round(double.Parse(model.QuarterSubResult.Split(' ')[0]));
                dataSourceQuarter.AppendAsync(this.Dispatcher, new KeyValuePair<DateTime, double>(dt, y));
            }

            if (model.LatestIndex != null)
            {
                double y = Math.Round(double.Parse(model.LatestIndex.futureIndex));
                dataSourceIndex.AppendAsync(this.Dispatcher, new KeyValuePair<DateTime, double>(dt, y));
            }
        }

    }
}
