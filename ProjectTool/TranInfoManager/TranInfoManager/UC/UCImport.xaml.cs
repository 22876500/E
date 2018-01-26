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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TranInfoManager.Entity;
using TranInfoManager.Utils;
using System.Reflection;
using System.Data.SqlClient;

namespace TranInfoManager.UC
{
    /// <summary>
    /// UCImport.xaml 的交互逻辑
    /// </summary>
    public partial class UCImport : UserControl
    {

        static readonly string[] MarketDetailCol = new[] { "Action", "Customer", "TicketID", "ISIN", "Symbol2", "ExQuan", "ExPrice", "FirstMoney", "Exchange", "TotalCommission_WithoutFeesAndCharges", "SecFee", "NetMoney", "NasdTradingActivityFee", "ExchangeFee", "ECNRebate", "NSCCPassThru", "SIPC", "FTT", "TradeDate", "SettlementDate" };
        static readonly string[] TraderDetailCol = new[] { "Time", "Sym", "Side", "Qty", "Exe_Price", "Acct", "Date", "Lvs_Qty", "Dest", "Price", "Trader", "Sol", "Seq", "Cl_Ord_ID", "Exch_Cl_Ord_ID", "Exch_Ord_ID", "Exch_Ord_ID_2", "Exch_Exec_ID", "Clear", "ECN_Fee", "Tif", "AON", "DNR", "DNI", "Acct_Type", "Specialist", "Source", "Poss_Dupe", "Updated", "Order_Date", "Order_Time", "Ord_Rec_ID", "Rec_ID", "Attrib", "Cvr_Qty", "Comm", "Creator", "Batch_ID", "Strategy", "Alloc_ID", "Acronym", "Expiration", "Strike", "Call_Put", "Underlying", "Open_Close", "Cover_Un", "Currency", "Sett_Cur", "FutSettDate", "SettCurrAmt", "Basket_Name", "Basket_ID", "Last_Mkt", "Inst" };
        static readonly string[] Trader_Col_Prim = new[] { "Time", "Sym", "Side", "Qty", "Exe Price", "Acct", "Date", "Lvs Qty", "Dest", "Price", "Trader", "Sol", "Seq #", "Cl Ord ID", "Exch Cl Ord ID", "Exch Ord ID", "Exch Ord ID 2", "Exch Exec ID", "Clear", "ECN Fee", "Tif", "AON", "DNR", "DNI", "Acct Type", "Specialist", "Source", "Poss Dupe", "Updated", "Order Date", "Order Time", "Ord Rec ID", "Rec ID", "Attrib", "Cvr Qty", "Comm", "Creator", "Batch ID", "Strategy", "Alloc ID", "Acronym", "Expiration", "Strike", "Call/Put", "Underlying", "Open/Close", "Cover/Un", "Currency", "Sett Cur", "FutSettDate", "SettCurrAmt", "Basket Name", "Basket ID", "Last Mkt", "Inst" };

        private static object sync = new object();
        private bool IsUpdating { get; set; }

        public DataTable SelectedTable { get; set; }

        public List<DataTable> ListMatchedData { get; set; }
        public List<DataTable> ListNotMatchedData { get; set; }


        public UCImport()
        {
            InitializeComponent();
            ListMatchedData = new List<DataTable>();
            ListNotMatchedData = new List<DataTable>();
        }

        private void Button_Import_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = true, Multiselect = true, Filter = "All Excel|*.xls;*.xlsx|Excel 97~2003|*.xls|Excel 2007/2010|*.xlsx" };
            if (dialog.ShowDialog() == true)
            {
                var names = dialog.FileNames;
                ImportFiles(names);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void txtHeaderIndex_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void listBoxHasConfigItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tableName = e.AddedItems[0] as string;
                if (!string.IsNullOrEmpty(tableName))
                {
                    var dt = ListMatchedData.First(_ => _.TableName.EndsWith(tableName));
                    if (dt != null)
                    {
                        SetPageData(dt);
                    }
                }
            }
        }

        private void listBoxNoConfigItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tableName = e.AddedItems[0] as string;
                if (!string.IsNullOrEmpty(tableName))
                {
                    var dt = ListNotMatchedData.FirstOrDefault(_ => _.TableName.EndsWith(tableName));
                    if (dt != null)
                    {
                        SelectedTable = dt;
                        SetPageData(dt);
                    }
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetPageData(SelectedTable);
        }

        private void txtHeaderIndex_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #region Import To DataTable

        private void ImportFiles(string[] names)
        {
            this.loading.ShowLoading("载入中…");
            var doWork = new Action(() =>
            {
                for (int i = 0; i < names.Length; i++)
                {
                    string loadingInfo = string.Format("共载入{0}个文件，,正在加载第{1}个。\n文件名：\"{2}\"", names.Length, i + 1, names[i].GetFileName());
                    this.Dispatcher.RunUI(() => { this.loading.ShowLoading(loadingInfo); });
                    string errMsg;
                    var dt = ReadToDataTable(names[i], out errMsg);
                    if (string.IsNullOrEmpty(errMsg))
                    {
                        dt.TableName = names[i];
                        var date = dt.TableName.GetDate();

                        bool isMatch = true;
                        foreach (var item in MarketDetailCol)
                            isMatch = isMatch && dt.Columns.Contains(item);

                        SetTableToList(dt, names[i]);
                    }
                    else
                    {
                        this.Dispatcher.ShowMsg(errMsg);
                    }
                }
            });

            var onCompleteUI = new Action(() =>
            {
                this.listBoxHasConfigItem.ItemsSource = this.ListMatchedData.Select(_ => _.TableName.GetFileName());
                this.listBoxNoConfigItem.ItemsSource = this.ListNotMatchedData.Select(_ => _.TableName.GetFileName());
                if (listBoxHasConfigItem.Items.Count > 0)
                {
                    listBoxHasConfigItem.SelectedIndex = 0;
                }
                else if (listBoxNoConfigItem.Items.Count > 0)
                {
                    listBoxNoConfigItem.SelectedIndex = 0;
                }
                this.loading.HideLoading();
            });
            Dispatcher.RunAsync(doWork, null, null, onCompleteUI);

        }

        private void SetTableToList(DataTable dt, string fileName)
        {
            bool isMatchMarket = true;
            foreach (var item in MarketDetailCol)
                isMatchMarket = isMatchMarket && dt.Columns.Contains(item);

            bool isMatchTrader = true;
            foreach (var item in Trader_Col_Prim)
                isMatchTrader = isMatchTrader && dt.Columns.Contains(item);

            if (isMatchMarket || isMatchTrader)
            {
                var type = isMatchMarket ? ImportType.Market : ImportType.Trader;
                var arr = isMatchMarket ? MarketDetailCol : TraderDetailCol;
                ListMatchedData.Add(dt);
            }
            else
            {
                ListNotMatchedData.Add(dt);
            }
        }

        private DataTable ReadToDataTable(string FileName, out string errMsg)
        {
            DataTable dt = null;
            errMsg = null;
            try
            {
                //Action<int,int> ac = new Action<int,int>((i, j) => { Dispatcher.RunUI(() => { this.loading.ShowLoading(string.Format("正在加载第{0}行…共{1}行", i, j)); }); });
                bool readAsTxt = !CommonUtils.IsExcel(FileName);
                dt = readAsTxt ? CommonUtils.ReadDataTable(FileName, 0) : ExcelUtils.ReadExcelFileDataTable(FileName, 0, 0);

                if (!dt.Columns.Contains(EmptyColumn))
                    dt.Columns.Add(EmptyColumn);
            }
            catch (NPOI.POIFS.FileSystem.NotOLE2FileException)
            {
                errMsg = "文件格式不正确，请尝试用Excel另存为后打开";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return dt;
        }


        #region Render Page by Selected DataTable
        private void SetPageData(DataTable dt)
        {
            dgImport.ItemsSource = dt.DefaultView;

            bool isMatchMarket = true;
            foreach (var item in MarketDetailCol)
                isMatchMarket = isMatchMarket && dt.Columns.Contains(item);

            bool isMatchTrader = true;
            foreach (var item in TraderDetailCol)
                isMatchTrader = isMatchTrader && dt.Columns.Contains(item);

            //comboBoxType.SelectedIndex = isMatchMarket ? 0 : 1;

            var properties = (isMatchMarket || comboBoxType.SelectedIndex == 0) ? MarketDetailCol : TraderDetailCol;

            for (int i = this.gridColumnConfig.RowDefinitions.Count; i < properties.Length; i++)
                gridColumnConfig.RowDefinitions.Add(new RowDefinition());

            CreateControls(properties, dt);
        }

        private void CreateControls(string[] properties, DataTable dt)
        {
            gridColumnConfig.Children.Clear();
            var arr = new string[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                arr[i] = dt.Columns[i].ColumnName;

            for (int i = 0; i < properties.Length; i++)
            {
                var lbl = new Label() { Content = properties[i] };
                var cmb = new ComboBox() { ItemsSource = arr, Name = "combobox_" + properties[i] };

                if (arr.Contains(properties[i]))
                    cmb.SelectedValue = properties[i];
                else
                {
                    foreach (var item in arr)
                    {
                        if (item.Replace("#", "").Replace('/', '_').Replace(' ', '_').Trim().TrimEnd('_') == properties[i])
                        {
                            cmb.SelectedValue = item;
                            break;
                        }
                    }
                }

                Grid.SetRow(lbl, i);
                Grid.SetRow(cmb, i);
                Grid.SetColumn(lbl, 0);
                Grid.SetColumn(cmb, 1);
                gridColumnConfig.Children.Add(lbl);
                gridColumnConfig.Children.Add(cmb);
            }
        }

        #endregion


        #endregion

        #region Save To Database
        private void Save()
        {
            this.loading.ShowLoading("保存中……");
            lock (sync)
            {
                if (!IsUpdating)
                {
                    IsUpdating = true;

                    var delieryConfig = GetPageConfig(ImportType.Market);
                    var softConfig = GetPageConfig(ImportType.Trader);


                    if (delieryConfig.Count == 0 && softConfig.Count == 0)
                    {
                        MessageBox.Show("无法保存！");
                        this.loading.HideLoading();
                        return;
                    }
                    if (SelectedTable != null)
                    {
                        SaveOneTable(SelectedTable, delieryConfig, softConfig);
                    }
                    else if (ListMatchedData.Count > 0)
                    {
                        var saving = new Action(() => { SaveMatchedTables(); });

                        var completeUIAction = new Action(() =>
                        {
                            ListMatchedData.Clear();
                            listBoxHasConfigItem.ItemsSource = null;
                            dgImport.ItemsSource = null;
                            gridColumnConfig.Children.Clear();

                            listBoxNoConfigItem.ItemsSource = ListNotMatchedData.Select(_ => _.TableName.GetFileName());
                            this.loading.HideLoading();
                            //if (OnDBChangeComplete != null)
                            //    OnDBChangeComplete.Invoke();
                        });

                        CommonUtils.RunAsync(this.Dispatcher, saving, null, null, completeUIAction);
                    }
                    IsUpdating = false;
                }
            }
        }

        private void SaveOneTable(DataTable dt, Dictionary<string, string> deliConfig, Dictionary<string, string> softConfig)
        {
            string msg;
            bool isSuccess = true;
            bool isDeli = comboBoxType.SelectedIndex == 0;

            var doWork = new Action(() =>
            {
                if (isDeli)
                    isSuccess = SaveAsMarket(dt, deliConfig, true, out msg);
                else
                    isSuccess = SaveAsTrader(dt, softConfig, true, out msg);

                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    if (isSuccess || !string.IsNullOrEmpty(msg))
                    {
                        Dispatcher.ShowMsg(isSuccess ? "保存完毕!" : msg);
                    }
                    if (ListNotMatchedData.Contains(dt))
                    {
                        ListNotMatchedData.Remove(dt);

                        listBoxNoConfigItem.ItemsSource = ListNotMatchedData.Select(_ => _.TableName.GetFileName());
                    }
                    SelectedTable = null;
                    dgImport.ItemsSource = null;
                    gridColumnConfig.Children.Clear();
                    this.loading.HideLoading();
                    
                }));
            });

            this.Dispatcher.RunAsync(doWork, null, null, null);
        }

        private void SaveMatchedTables()
        {
            StringBuilder sb = new StringBuilder(64);
            try
            {
                int successCount = 0;
                foreach (DataTable dt in ListMatchedData)
                {
                    var config = GetDataTableConfig(dt);
                    bool isSuccess = false;
                    string msg;
                    ImportType type = config.Count == MarketDetailCol.Length ? ImportType.Market : ImportType.Trader;
                    if (config.Count == 0)
                    {
                        msg = "未能从文件名中识别出导入数据类型，将跳过此文件：" + dt.TableName;
                    }
                    else if (config.Count == MarketDetailCol.Length)
                        isSuccess = SaveAsMarket(dt, config, false, out msg);
                    else
                        isSuccess = SaveAsTrader(dt, config, false, out msg);


                    if (isSuccess)
                        successCount++;
                    else
                    {
                        if (!string.IsNullOrEmpty(msg)) sb.AppendLine(msg);
                        ListNotMatchedData.Add(dt);
                    }
                }
                if (sb.Length > 0)
                {
                    CommonUtils.Log("保存记录：" + sb.ToString());
                }

                var message = string.Format("批量保存{0}个文件，成功{1}个，失败{2}个。", ListMatchedData.Count, successCount, ListMatchedData.Count - successCount);
                if (successCount < ListMatchedData.Count)
                    message += "失败文件将转入单独保存列表，详细信息请查看日志中的保存记录。";

                this.Dispatcher.ShowMsg(message);
            }
            catch (Exception ex)
            {
                CommonUtils.Log("存入数据库时出现异常！", ex);
                this.Dispatcher.ShowMsg("保存失败,详情请检查日志！");
            }
        }

        private Dictionary<string, string> GetDataTableConfig(DataTable dt)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var item in MarketDetailCol)
            {
                if (dt.Columns.Contains(item))
                {
                    dict.Add(item, item);
                }
                else
                {
                    dict.Clear();
                    break;
                }
            }
            if (dict.Count == 0)
            {
                for (int i = 0; i < TraderDetailCol.Length; i++)
                {
                    if (dt.Columns.Contains(Trader_Col_Prim[i]))
                    {
                        dict.Add(TraderDetailCol[i], Trader_Col_Prim[i]);
                    }
                    else
                    {
                        dict.Clear();
                        break;
                    }
                }
            }

            return dict;
        }

        private bool SaveAsMarket(DataTable dt, Dictionary<string, string> pageConfig, bool isSaveFromPage, out string msg)
        {
            msg = string.Empty;
            try
            {
                DataTable table = null;
                var fileName = dt.TableName.GetFileName();
                var date = fileName.GetDate();
                if (date == DateTime.MinValue)
                {
                    msg = string.Format("文件名中必须包含交易日期，文件{0}未符合条件。", fileName);
                    return false;
                }

                var list = GetList<MarketDetailDaily>(date, dt, pageConfig);
                if (list.Count == 0)
                {
                    msg = "符合条件的数据条数为0，交割单默认保存条件为发生金额大于0，将跳过此文件：" + fileName;
                    return false;
                }
                table = GetTable(list);

                DataHandler.DeleteMarketDataInfo(date);
                bool saveResult = WriteToDB(table, "MarketDetailDailies");
                if (!saveResult)
                    msg = string.Format("文件{0}对应信息保存入数据库失败！", dt.TableName);
                return saveResult;
            }
            catch (Exception ex)
            {
                msg = string.Format("导入失败, 关联文件名{0}！\r\n异常信息：{1}", dt.TableName, ex.StackTrace);
                return false;
            }
        }

        private bool SaveAsTrader(DataTable dt,  Dictionary<string, string> pageConfig, bool isSaveFromPage, out string msg)
        {
            msg = string.Empty;
            try
            {
                var fileName = dt.TableName.GetFileName();
                var date = fileName.GetDate();
                var group = fileName;
                if (date == DateTime.MinValue)
                {
                    msg = string.Format("交易详情文件名中必须包含交易日期，文件{0}未符合条件。", fileName);
                    return false;
                }

                var lstTrade = GetList<TraderDailyDetail>(date, dt, pageConfig);
                if (lstTrade.Count == 0)
                {
                    msg = "符合条件的数据条数为0，将跳过此文件：" + fileName;
                    return false;
                }

                DataHandler.DeleteTraderData(date);
                var table = GetTable(lstTrade);

                bool saveResult = WriteToDB(table, "TraderDailyDetails");
                if (!saveResult)
                    msg = string.Format("文件{0}对应信息保存入数据库失败！", dt.TableName);
                return saveResult;
            }
            catch (Exception ex)
            {
                CommonUtils.Log(string.Format("交易详情导入失败, 关联文件名{0}！", dt.TableName), ex);
                msg = "保存交易员交易详情失败,详情请查看日志!";
                return false;
            }
        }

        private List<T> GetList<T>(DateTime date, DataTable dt, Dictionary<string, string> map) where T : new()
        {
            List<T> list = new List<T>();
            var property = typeof(T).GetProperties();

            foreach (DataRow row in dt.Rows)
            {
                if (IsEmptyRow(row))
                    break;

                var entity = new T();

                foreach (var item in property)
                {
                    if (item.CanWrite)
                    {
                        string value = null;
                        try
                        {
                            if (map.ContainsKey(item.Name))
                            {
                                value = row[map[item.Name]] + "";
                                if (item.PropertyType == typeof(DateTime))
                                {
                                    item.SetValue(entity, DateTime.Parse(value));
                                }
                                else if (item.PropertyType == typeof(decimal))
                                {
                                    item.SetValue(entity, string.IsNullOrEmpty(value) ? 0 : value.GetDec());
                                }
                                else if (item.PropertyType == typeof(int))
                                {
                                    item.SetValue(entity, string.IsNullOrEmpty(value) ? 0 : int.Parse(value, System.Globalization.NumberStyles.Any));
                                }
                                else
                                {
                                    if (item.Name.Contains("Time") && value.StartsWith("1899/12/31 "))
                                    {
                                        item.SetValue(entity, Convert.ChangeType(value.Substring(value.IndexOf(' ') + 1), item.PropertyType), null);
                                    }
                                    else
                                    {
                                        item.SetValue(entity, Convert.ChangeType(value, item.PropertyType), null);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var msg = string.Format("导入失败, 关联文件名{0}！\r\n属性：{1}，值：{2}\r\n异常信息：{3}", dt.TableName, item.Name, value, ex.Message);
                            CommonUtils.Log(msg);
                            throw;
                        }
                    }
                }
                list.Add(entity);
            }

            return list;
        }

        private static bool WriteToDB(DataTable dt, string tableName)
        {
            if (dt == null || dt.Rows.Count == 0)
                return false;

            bool IsSavedToDB = true;
            string connectionString = CommonUtils.DbConnectString;

            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connectionString))
            {
                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.BatchSize = dt.Rows.Count;
                foreach (DataColumn item in dt.Columns)
                    sqlBulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);

                try
                {
                    sqlBulkCopy.WriteToServer(dt);
                    IsSavedToDB = true;
                }
                catch (Exception ex)
                {
                    IsSavedToDB = false;
                    CommonUtils.Log("SqlBulkCopy数据导入出错", ex);
                }
                sqlBulkCopy.Close();
            }

            return IsSavedToDB;
        }

        private DataTable GetTable<T>(List<T> list) where T : new()
        {
            DataTable dt = new DataTable();
            var properties = typeof(T).GetProperties();
            foreach (var item in properties)
                if (item.CanWrite) dt.Columns.Add(item.Name, item.PropertyType);
            foreach (var item in list)
            {
                var row = dt.NewRow();
                foreach (var p in properties)
                {
                    if (p.CanWrite)
                    {
                        row[p.Name] = p.GetValue(item);
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static bool IsEmptyRow(DataRow row)
        {
            bool isEmptyRow = true;
            foreach (var item in row.ItemArray)
            {
                if (!string.IsNullOrEmpty(item + "") && item != DBNull.Value)
                {
                    return false;
                }
            }
            return isEmptyRow;
        }

        #region Get Config From Page
        private Dictionary<string, string> GetPageConfig(ImportType type)
        {
            var dict = new Dictionary<string, string>();
            var arr = type == ImportType.Market ? MarketDetailCol : TraderDetailCol;

            foreach (var propertyName in arr)
            {
                foreach (UIElement item in gridColumnConfig.Children)
                {
                    var cbBox = item as ComboBox;
                    if (cbBox != null && cbBox.Name == ("combobox_" + propertyName) && !string.IsNullOrEmpty(cbBox.Text))
                    {
                        dict.Add(propertyName, cbBox.Text);
                        break;
                    }
                }
            }
            return dict;
        }
        #endregion

        #endregion

        public string EmptyColumn { get { return "EmptyColumn"; } }
    }
}
