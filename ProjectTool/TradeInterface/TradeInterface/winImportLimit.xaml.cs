using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace TradeInterface
{
    /// <summary>
    /// winImportLimit.xaml 的交互逻辑
    /// </summary>
    public partial class winImportLimit : Window
    {
        List<string> stockInfoKeywords = new List<string>() { "股票代码", "股票简称", "现价", "区间换手率", "区间振幅", "成交量", };
        List<string> limitInfoKeywords = new List<string>() { "证券代码", "当前持仓", "组合号", "交易员" };

        DataTable dtImport;

        public winImportLimit()
        {
            InitializeComponent();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = true, Multiselect = false, };
            dialog.Filter = "All Files|*.xls;*.xlsx;*.csv;*.txt|Excel 97~2003|*.xls|Excel 2007/2010|*.xlsx|csv File|*.csv|Text File|*.txt";
            if (dialog.ShowDialog() == true)
            {
                this.loading.ShowLoading();
                string fileName = dialog.FileName;
                Encoding encode = CommonUtils.GetEncoding((cmbEncode.SelectedItem as ComboBoxItem).Content.ToString());
                Task task = new Task(() => { RunImport(fileName, encode); });
                task.Start();
            }
        }

        private void RunImport(string path, Encoding encode)
        {
            try
            {
                var dt = CommonUtils.IsExcel(path) ? ExcelUtils.ReadExcel(path) : ExcelUtils.ReadCSV(path, encode);
                dtImport = dt;
                if (dt.Rows.Count > 5000)
                {
                    Dispatcher.Invoke(() => { MessageBox.Show("行数超出5000，请检查导入数据"); });
                    return;
                }

                dtImport.TableName = path.Substring(path.LastIndexOf('\\') + 1);

                bool isStockInfo = (dt.Columns[0].ColumnName == "股票代码" && dt.Columns[1].ColumnName == "股票简称");

                RepairStockID(dt, isStockInfo);
                RepairColumns(dt, isStockInfo);

                this.Dispatcher.Invoke(() =>
                {
                    cmbDataType.SelectedIndex = isStockInfo ? 1 : 0;
                    this.dgImportData.ItemsSource = dtImport.DefaultView;
                    this.loading.HideLoading();
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => { 
                    MessageBox.Show("导入异常," + ex.Message);
                    this.loading.HideLoading();
                });
            }
            
        }

        private void RepairColumns(DataTable dt, bool isStockInfo)
        {
            //1.替换所有列名包含特殊字符导致无法显示数据的问题
            foreach (DataColumn column in dtImport.Columns)
            {
                //if (item.ColumnName.IndexOf(')') > 0)
                //{
                //    item.ColumnName = item.ColumnName.Substring(0, item.ColumnName.IndexOf(')') + 1);
                //}
                if (Regex.IsMatch(column.ColumnName, "[^\u4e00-\u9fa5()]+"))
                {
                    column.ColumnName = Regex.Replace(column.ColumnName, "[^\u4e00-\u9fa5()%]+", string.Empty);
                }
            }

            //2.如是股票数据，去掉非必要列，防止数据导入错误。
            if (isStockInfo)
            {
                for (int i = dt.Columns.Count - 1; i > -1; i--)
                {
                    if (!stockInfoKeywords.Exists(_ => dt.Columns[i].ColumnName.Contains(_)))
                    {
                        dt.Columns.RemoveAt(i);
                    }
                }
            }
            //3.如不是股票数据，则检查是否需要去除非必要列
            else
            {
                if (dt.Columns.Count >= 3 && dt.Columns.Count <= 4)
                {
                    for (int i = dt.Columns.Count - 1; i > -1; i--)
                    {
                        if (!limitInfoKeywords.Exists(_ => dt.Columns[i].ColumnName.Contains(_)))
                        {
                            dt.Columns.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private static void RepairStockID(DataTable dt, bool isStockInfo)
        {
            foreach (DataRow item in dt.Rows)
            {
                if (isStockInfo)
                {
                    item[0] = CommonUtils.AutoAddZero((item[0] + "").Replace(".SH", "").Replace(".SZ", ""));
                }
                else if (dt.Columns.Count == 2)
                {
                    item[0] = CommonUtils.AutoAddZero(item[0] + "");
                }
                else if (dt.Columns.Contains("证券代码"))
                {
                    item["证券代码"] = CommonUtils.AutoAddZero(item["证券代码"] + "");
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDataType.SelectedIndex < -1)
            {
                MessageBox.Show("请选择要导入的数据类型！");
                return;
            }
            if (dtImport == null)
            {
                MessageBox.Show("请导入数据表！");
                return;
            }
            if (cmbDataType.SelectedIndex == 0)
            {
                ImportLimits();
            }
            else
            {
                ImportStockInfo();
            }
        }

        private void ImportStockInfo()
        {
            this.loading.ShowLoading();
            Task t = new Task(() =>
            {
                try
                {
                    List<StockItemEntity> lstStock = new List<StockItemEntity>();
                    foreach (DataRow item in dtImport.Rows)
                    {
                        var stock = new StockItemEntity();
                        stock.StockID = item[0] + "";
                        stock.StockName = item[1] + "";
                        stock.PriceNow = item[2] + "";
                        stock.TurnoverRate = item[3] + "";
                        stock.FluctuationRate = item[4] + "";
                        stock.FillQty = item[5] + "";
                        lstStock.Add(stock);
                    }
                    MarketValueCaculateAdapter.StockEntityList = lstStock;
                    MarketValueCaculateAdapter.CalculateTraderStockList();
                    Dispatcher.Invoke(() => { this.Close(); });
                }
                catch (Exception ex)
                {
                    CommonUtils.Log("保存市场股票统计数据异常!", ex);
                    Dispatcher.Invoke(() => 
                    {
                        MessageBox.Show("保存市场股票统计数据异常,详细信息请查看日志!"); 
                    });
                }
                
            });
            t.Start();
        }

        private void ImportLimits()
        {
            var task = new Task(() => {
                try
                {
                    List<LimitItemEntity> lstLimit = new List<LimitItemEntity>();
                    foreach (DataRow item in dtImport.Rows)
                    {
                        var limitItem = new LimitItemEntity();
                        if (dtImport.Columns.Count == 2)
                        {
                            limitItem.StockID = item[0] + "";
                            limitItem.TotalQty = item[1] + "";
                        }
                        else if (dtImport.Columns.Count >= 3 && dtImport.Columns.Count <= 4)
                        {
                            limitItem.StockID = item["证券代码"] + "";
                            limitItem.TotalQty = item["当前持仓"] + "";
                            if (dtImport.Columns.Contains("组合号"))
                            {
                                limitItem.Group = item["组合号"] + "";
                            }
                            if (dtImport.Columns.Contains("交易员"))
                            {
                                limitItem.TraderAccount = item["交易员"] + "";
                            }
                        }
                        else if (dtImport.Columns.Count >= 10)
                        {
                            limitItem.TraderAccount = item[0] + "";
                            limitItem.StockID = item[1] + "";
                            limitItem.TotalQty = item[8] + "";
                            limitItem.Group = item[2] + "";
                        }
                        lstLimit.Add(limitItem);
                    }
                    MarketValueCaculateAdapter.LimitEntityList = lstLimit;
                    MarketValueCaculateAdapter.CalculateTraderStockList();
                    Dispatcher.Invoke(() => { this.Close(); });
                }
                catch (Exception ex)
                {
                    string errInfo = string.Format("保存券单数据异常, {0}!",ex.Message);
                    Dispatcher.Invoke(() =>{
                        this.loading.HideLoading();
                        MessageBox.Show(errInfo);
                    });
                }
            });
            task.Start();
        }


    }
}
