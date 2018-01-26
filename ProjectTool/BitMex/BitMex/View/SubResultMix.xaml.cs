using Microsoft.Research.DynamicDataDisplay.DataSources;
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
using Microsoft.Research.DynamicDataDisplay;
using BitMex.ViewModel;

namespace BitMex.View
{
    /// <summary>
    /// SubResultMix.xaml 的交互逻辑
    /// </summary>
    public partial class SubResultMix : UserControl
    {
        private ObservableDataSource<KeyValuePair<DateTime, double>> dataSourceSubMix = new ObservableDataSource<KeyValuePair<DateTime, double>>();
        private DispatcherTimer timer = new DispatcherTimer();
        SubResultMixViewModel viewModel;

        public SubResultMix()
        {
            InitializeComponent();
            this.Loaded += SubResultMix_Loaded;
        }

        void SubResultMix_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = new SubResultMixViewModel();
            this.DataContext = viewModel;

            this.dataSourceSubMix.SetXMapping(_ => Axis_X_SubMix.ConvertToDouble(_.Key));
            this.dataSourceSubMix.SetYMapping(_ => _.Value);
            this.chartSubMix.AddLineGraph(this.dataSourceSubMix, Colors.Red, 2, "季度升贴水");

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(AnimatedLine);
            timer.IsEnabled = true;

            chartSubMix.Viewport.FitToView();
        }

        private void AnimatedLine(object sender, EventArgs e)
        {
            //定时更新数据
            if (viewModel.PriceTradeQuarter != 0 && viewModel.PriceBitMexTrade != 0)
            {
                double subResult = viewModel.PriceTradeQuarter - viewModel.PriceBitMexTrade;
                dataSourceSubMix.AppendAsync(this.Dispatcher, new KeyValuePair<DateTime, double>(DateTime.Now, subResult));
                App.Log.LogInfo("季度升贴水：" + subResult);
            }
            
        }
    }
}
