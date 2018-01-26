using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
/*
 *   version:1.0
 *   update date: 2017-10-19
 *   content: 开放资金与融资余额的查询结果，全部可见
 * 
 *   version: 1.1
 *   update date: 2017-10-25
 *   content: a. 修改融资余额的查询结果，修正在开放后仍然限制融资余额查询结果的问题逻辑。
 *            b. 添加公式计算逻辑，导入券单及股票数据，计算分数及市值。
 */

namespace TradeInterface
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        string VersionInfo = "1.1";
        string LastUpdateDate = "2017-10-25";

        int NotUseCount = 0;

        DispatcherTimer animationTimer = null;

        Dictionary<string, List<string>> dictDataColumn { get; set; }
        
        Dictionary<string, string> GroupDict { get; set; }

        DataTable QueryData { get; set; }

        public MainWindow()
        {
            try
            {
                
                CommonUtils.Log("1.Start Logon");
                var logon = new Logon();
                logon.ShowDialog();
                CommonUtils.Log("2.Logon Finished");
            }
            catch (Exception)
            {
                
                throw;
            }
            

            if (!Logon.IsLogon)
            {
                this.Close();
            } 
            //CommonUtils.UserName = "admin";
            CommonUtils.Log("3.Start Intialize!");
            InitializeComponent();
            this.Title = string.Format("交易数据查询 (版本号:{0} 发布日期:{1})", VersionInfo, LastUpdateDate);
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
        }

        #region Events
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
            if (dictDataColumn == null)
            {
                dictDataColumn = new Dictionary<string, List<string>>();
                //0,资金 1,股份 2, 当日委托 6,融资余额
                dictDataColumn.Add("0", new List<string>() { "资金余额", "可用资金" });//资金

                dictDataColumn.Add("1", new List<string>() { 
                        "证券代码", "证券名称", "证券数量", "当前持仓", "股份余额", "可卖数量", "可用余额", "可用股份", "参考持股", "买入冻结", "冻结数量", "卖出冻结", "参考市值价格", "参考市值", "库存数量" });//股份

                dictDataColumn.Add("2", new List<string>() { 
                        "时间", "委托时间", "股票代码", "证券代码", "股票名称", "证券名称", "买卖标志", "委托股数", "委托类别", "委托数量", "委托状态", "状态说明", "成交股数", "成交数量", "委托价格", "成交价格", "成交均价", "委托编号", "合同编号", "撤单数量" });//资金

                dictDataColumn.Add("6", new List<string>() { "可用保证金", "保证金可用余额", "信用资产信息", "信用资金额度", "资产负债综合信息", "数值", "保证金可用余额" });//融资余额
            }
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            TdxApi.CloseTdx();
        }

        private void p_Tick(object sender, EventArgs e)
        {
            this.NotUseCount++;
            if (NotUseCount > 120)
            {
                this.Close();
            }
        }
        #endregion

        private void Init()
        {
            try
            {
                CommonUtils.Log("TdxApi.OpenTdx 开始 ");
                TdxApi.OpenTdx();
                CommonUtils.Log("TdxApi.OpenTdx 结束 ");
            }
            catch (Exception ex)
            {
                CommonUtils.Log("TdxApi.OpenTdx 异常 ", ex);
            }

            if (GroupDict == null)
            {
                CommonUtils.Log("4.Init Group Info");
                var client = new ServiceReference.DataWebServiceSoapClient();
                var group = client.GetGroups(CommonUtils.UserName);

                GroupDict = group.FromJson<Dictionary<string,string>>();
                CommonUtils.Log("5.Group Count " + GroupDict.Keys.Count);
                if (GroupDict.Count > 0)
                {
                    cmbGroup.ItemsSource = GroupDict.Keys;
                }
            }

            animationTimer = new DispatcherTimer(DispatcherPriority.ContextIdle, Dispatcher);
            animationTimer.Interval = new TimeSpan(0, 0, 1);
            animationTimer.Tick += p_Tick;
            animationTimer.Start();
        }

        

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            this.NotUseCount = 0;
            int tradeDataType = int.Parse((cbTodayType.SelectedItem as ComboBoxItem).DataContext.ToString());
            bool isCheckPort = this.ckbIsCheckPort.IsChecked == true;
            var group = cmbGroup.SelectedItem as string;

            if (!string.IsNullOrEmpty(group) && GroupDict.ContainsKey(group))
            {
                this.loading.Visibility = Visibility.Visible;
                var action = new Action(() => 
                {
                    try
                    {
                        var o = Cryptor.MD5Decrypt(GroupDict[group]).FromJson<券商>();
                        if (o != null)
                        {
                            DateTime dt1 = DateTime.Now;
                            DataTable dt = DataAdapter.QueryTradeData(o, tradeDataType, isCheckPort);
                            var span = DateTime.Now - dt1;
                            var s = span.TotalSeconds;
                            
                            if (dt != null && dictDataColumn != null && dictDataColumn.ContainsKey(tradeDataType.ToString()))
                            {
                                CommonUtils.Log(string.Format("交易接口查询耗时记录, 交易类型 {0}, 耗时 {1}", tradeDataType, s));
                                var list = dictDataColumn[tradeDataType.ToString()];
                                //if ("FCAA14B3008F 005056C00001 005056C00008".IndexOf(CommonUtils.GetMac()) > -1 && CommonUtils.GetCpuID() == "BFEBFBFF000306C3")

                                if (tradeDataType != 0 && tradeDataType != 6)
                                {
                                    for (int i = dt.Columns.Count - 1; i > -1; i--)
                                    {

                                        if (!list.Contains(dt.Columns[i].ColumnName))
                                        {
                                            dt.Columns.RemoveAt(i);
                                        }
                                    }
                                }
                                
                                
                                if (tradeDataType == 2 && dt.Columns.Contains("买卖标志") && dt.Rows.Count > 0 && Regex.IsMatch(dt.Rows[0]["买卖标志"] + "", "^[01]+$"))
                                {
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        row["买卖标志"] = GetBuySaleInfo(row["买卖标志"] + "");
                                    }
                                }
                                if (tradeDataType == 1)
                                {
                                    AddPosition(isCheckPort, o, dt);
                                }
                            }
                            this.QueryData = dt;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Dispatcher.ShowMsg(ex.Message);
                    }

                });
                var completeAction = new Action(() => 
                {
                    if (QueryData == null)
                        dgMain.ItemsSource = null;
                    else
                    {
                        //dgMain.AutoGenerateColumns = false;
                        
                        dgMain.ItemsSource = QueryData.DefaultView;
                    }
                        
                    
                    this.loading.Visibility = Visibility.Collapsed;
                });
                
                this.Dispatcher.RunAsync(action, null, null, completeAction);

            }
        }

        private static void AddPosition(bool isCheckPort, 券商 o, DataTable dt)
        {
            //加一列“仓位” 仓位 =》 买入冻结-卖出冻结，或者 当前持仓-证券数量
            bool canAddColumn = false;
            string col1 = string.Empty;
            string col2 = string.Empty;
            if (dt.Columns.Contains("卖出冻结") && dt.Columns.Contains("买入冻结"))
            {
                canAddColumn = true;
                col1 = "买入冻结";
                col2 = "卖出冻结";
            }
            else if (dt.Columns.Contains("当前持仓") && dt.Columns.Contains("证券数量"))
            {
                canAddColumn = true;
                col1 = "当前持仓";
                col2 = "证券数量";
            }

            if (canAddColumn)
            {
                AddColumnCalculate(dt, col1, col2);
            }
            else if(o.名称 != "C03" && o.名称 != "C02")
            {
                AddPositionCountBySearch(isCheckPort, o, dt);
            }
        }

        private static void AddPositionCountBySearch(bool isCheckPort, 券商 o, DataTable dt)
        {
            if (dt.Rows.Count == 0)
                return;

            string codeColumnName0 = GetCodeColumnName(dt);
            if (codeColumnName0 != string.Empty)
            {
                //查询当日委托，统计各股票下单情况，进行合计，并将合计信息作为仓位列信息，加入到当前
                DataTable dt1 = DataAdapter.QueryTradeData(o, 2, isCheckPort);
                string codeColumnName1 = GetCodeColumnName(dt1);
                if (codeColumnName1 != string.Empty && dt1.Columns.Contains("买卖标志") && dt1.Columns.Contains("成交数量"))
                {
                    Dictionary<string, decimal> dict = CalculateTotalBuyCount(dt1, codeColumnName1);
                    dt.Columns.Add("仓位");
                    foreach (DataRow dr in dt.Rows)
                    {
                        var code = CommonUtils.GetCode(dr[codeColumnName0]);
                        if (dict.ContainsKey(code))
                        {
                            dr["仓位"] = dict[code];
                        }
                    }
                }
            }
        }

        private static string GetCodeColumnName(DataTable dt)
        {
            string codeColumnName = string.Empty;
            if (dt.Columns.Contains("股票代码"))
            {
                codeColumnName = "股票代码";
            }
            else if (dt.Columns.Contains("证券代码"))
            {
                codeColumnName = "证券代码";
            }
            return codeColumnName;
        }

        private static Dictionary<string, decimal> CalculateTotalBuyCount(DataTable dt1, string codeColumnName1)
        {
            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            foreach (DataRow dr in dt1.Rows)
            {
                var code = dr[codeColumnName1] + "";
                var count = CommonUtils.GetDecimal(dr["成交数量"]);
                if (CommonUtils.IsCode(code) && count > 0)
                {
                    bool isBuy = IsBuySymble(dr["买卖标志"]);
                    if (dict.ContainsKey(code))
                    {
                        if (isBuy)
                            dict[code] = dict[code] + count;
                        else
                            dict[code] = dict[code] - count;
                    }
                    else
                    {
                        if (isBuy)
                            dict.Add(code, count);
                        else
                            dict.Add(code, -count);
                    }
                }
            }
            return dict;
        }

        static bool IsBuySymble(string s)
        {
            if (s == "0" || Regex.IsMatch(s, "买"))
            {
                return true;
            }
            return false;
        }

        static bool IsBuySymble(object o)
        {
            string s = o + "";
            if (s == "0" || Regex.IsMatch(s, "买"))
            {
                return true;
            }
            return false;
        }

        private static void AddColumnCalculate(DataTable dt, string col1, string col2)
        {
            dt.Columns.Add("仓位");
            foreach (DataRow row in dt.Rows)
            {
                decimal d1, d2;
                if (decimal.TryParse(row[col1] + "", out d1) && decimal.TryParse(row[col2] + "", out d2))
                {
                    row["仓位"] = d1 - d2;
                }
            }
        }

        private string GetBuySaleInfo(string p)
        {
            switch (p)
            {
                case "0":
                    return "买";
                case "1":
                    return "卖";
                case "2":
                    return "融资买入";
                case "3":
                    return "融券卖出";
                case "4":
                    return "买券还券";
                case "5":
                    return "卖券还款";
                case "7":
                    return "担保品买入";
                case "8":
                    return "担保品卖出";
                default:
                    return p;
            }
        }

        private void ckbIsAllColumn_Click(object sender, RoutedEventArgs e)
        {
            this.NotUseCount = 0;
            if (ckbIsAllColumn != null && ckbIsAllColumn.IsChecked == true && QueryData != null)
            {
                dgMain.ItemsSource = QueryData.DefaultView;
            }
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            this.NotUseCount = 0;
        }


        private void cbTodayType_GotFocus(object sender, RoutedEventArgs e)
        {
            this.NotUseCount = 0;
        }

        private void cmbGroup_GotFocus(object sender, RoutedEventArgs e)
        {
            this.NotUseCount = 0;
        }

        private void btnRepay_Click(object sender, RoutedEventArgs e)
        {
            if (this.cmbGroup.SelectedItem == null)
            {
                MessageBox.Show("请选择还款账户");
                return;
            }
            else 
            {
                if (GroupServiceHelper.ExistsGroup(this.cmbGroup.SelectedItem as string))
                {
                    var win = new winRepay();
                    var group = cmbGroup.SelectedItem as string;
                    var o = Cryptor.MD5Decrypt(GroupDict[group]).FromJson<券商>();
                    win.Init(o);
                    win.ShowDialog();
                }
                else
                {
                    MessageBox.Show("缺少组合号接口IP.txt");
                }
            }
        }


    }
}
