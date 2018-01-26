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

namespace TradeInterface.Ctrl
{
    /// <summary>
    /// FormulaCal.xaml 的交互逻辑
    /// </summary>
    public partial class FormulaCal : UserControl
    {
        public FormulaCal()
        {
            InitializeComponent();
            this.Loaded += FormulaCal_Loaded;
        }

        private void BindSource()
        {
            
            this.dgLimit.ItemsSource = MarketValueCaculateAdapter.LimitEntityList;
            this.dgStock.ItemsSource = MarketValueCaculateAdapter.StockEntityList;
            this.dgCal.ItemsSource = MarketValueCaculateAdapter.CalculateList;
        }

        void FormulaCal_Loaded(object sender, RoutedEventArgs e)
        {
            txtDiv.Text = MarketValueCaculateAdapter.ParamDivide.ToString();
            txtMul.Text = MarketValueCaculateAdapter.ParamMultiply.ToString();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            winImportLimit win = new winImportLimit();
            win.ShowDialog();
            BindSource();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (MarketValueCaculateAdapter.CalculateList != null && MarketValueCaculateAdapter.CalculateList.Count > 0)
            {
                if (Regex.IsMatch(txtDiv.Text, "^[0-9]+([.][0-9]+){0,1}") && Regex.IsMatch(txtMul.Text, "^[0-9]+([.][0-9]+){0,1}"))
                {
                    MarketValueCaculateAdapter.ParamDivide = decimal.Parse(txtDiv.Text);
                    MarketValueCaculateAdapter.ParamMultiply = decimal.Parse(txtMul.Text);
                    MarketValueCaculateAdapter.CalculateTraderStockList();
                    dgCal.ItemsSource = null;
                    BindSource();
                }
                else
                {
                    MessageBox.Show("检测到参数不匹配！");
                }
            }
            else
            {
                MessageBox.Show("请先导入数据！");
            }
        }
    }
}
