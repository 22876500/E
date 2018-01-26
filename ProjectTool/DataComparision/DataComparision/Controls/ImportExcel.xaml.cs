using Microsoft.Win32;
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
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
using DataComparision.Utils;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;
using DataComparision.Entity;
using System.Configuration;
using DataComparision.DataAdapter;

namespace DataComparision.Controls
{
    /// <summary>
    /// wpImportDelivery.xaml 的交互逻辑
    /// </summary>
    public partial class ImportExcel : UserControl
    {
        public Action OnDBChangeComplete;
        public Action<List<string>, DateTime> OnImportComplete;

        #region Members
        static object sync = new object();

        bool IsUpdating = false;

        /// <summary>
        /// 委托属性名
        /// </summary>
        readonly string[] softPropertyNames = new[] { "交易员", "组合号", "证券代码", "证券名称", "买卖标志", "委托价格", "委托数量", "成交价格", "成交数量", "撤单数量", "状态说明", "委托时间", "委托编号" };

        readonly string[] softPropertyNamesExtend = new[] { "交易员", "组合号", "证券代码", "证券名称", "买卖方向", "委托价格", "委托数量", "成交价格", "成交数量", "撤单数量", "状态说明", "委托时间", "委托编号" };

        /// <summary>
        /// 交割单属性名
        /// </summary>
        readonly string[] deliPropertyNames = new[] { "组合号", "交割日期", "证券代码", "证券名称", "买卖标志", "成交数量", "成交价格", "成交金额", "成交编号", "发生金额", "手续费", "印花税", "过户费", "其他费", "备注", };

        const string EmptyColumn = "空白列";

        #endregion

        #region Properties
        DataTable _selectedTable = null;
        DataTable SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                if (_selectedTable != null)
                {
                    this.btnSave.IsEnabled = true;
                }
                else if (MatchConfigData.Count == 0)
                {
                    this.btnSave.IsEnabled = false;
                }
            }
        }

        List<DataTable> MatchConfigData { get; set; }

        List<DataTable> NotMatchConfigData { get; set; }

        /// <summary>
        /// 交割单属性对应的列名别名
        /// </summary>
        Dictionary<string, string[]> PropertyMap { get; set; }
        #endregion

        #region Init
        public ImportExcel()
        {
            InitializeComponent();
            Init();
            this.dgImportData.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.DataGrid_LoadingRow);


        }

        private void Init()
        {
            try
            {
                var formatInfo = CommonUtils.GetConfig("别名配置");
                if (string.IsNullOrEmpty(formatInfo))
                {
                    PropertyMap = new Dictionary<string, string[]>();
                    PropertyMap.Add("交割日期", new[] { "成交日期", "发生日期" });
                    PropertyMap.Add("买卖标志", new[] { "操作", "业务类型", "业务名称", "摘要" });
                    PropertyMap.Add("成交价格", new[] { "成交均价", });
                    PropertyMap.Add("成交编号", new[] { "合同编号", "协议编号", "委托编号" });
                    PropertyMap.Add("发生金额", new[] { "变动金额", "清算金额" });
                    PropertyMap.Add("手续费", new[] { "佣金", });
                    CommonUtils.SetConfig("别名配置", PropertyMap.ToJson());
                }
                else
                {
                    PropertyMap = formatInfo.FromJson<Dictionary<string, string[]>>();
                }

                MatchConfigData = new List<DataTable>();
                NotMatchConfigData = new List<DataTable>();
            }
            catch { }
        }
        #endregion

        #region Events
        private void Button_Import_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = true, Multiselect = true, Filter = "All Excel|*.xls;*.xlsx|Excel 97~2003|*.xls|Excel 2007/2010|*.xlsx" };
            if (dialog.ShowDialog() == true)
            {
                Import(dialog.FileNames);
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (IsUpdating)
                return;

            string errMsg;
            if (IsValid(out errMsg))
            {
                Save();
            }
            else
            {
                CommonUtils.ShowMsg(errMsg);
            }
        }


        private void cboDataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gdDelivery == null || gdSoftSet == null)
            {
                return;
            }
            if (cboDataType.SelectedIndex == 0)
            {
                gdDelivery.Visibility = System.Windows.Visibility.Visible;
                gdSoftSet.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                gdSoftSet.Visibility = System.Windows.Visibility.Visible;
                gdDelivery.Visibility = System.Windows.Visibility.Collapsed;

            }
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void listBoxMatched_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tableName = e.AddedItems[0] as string;
                if (!string.IsNullOrEmpty(tableName))
                {
                    var dt = MatchConfigData.First(_ => _.TableName.EndsWith(tableName));
                    InitPageByFileName(dt.TableName);
                    SetPageData(dt);
                }
            }

        }

        private void listBoxNotMatched_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var tableName = e.AddedItems[0] as string;
                if (!string.IsNullOrEmpty(tableName))
                {
                    var dt = NotMatchConfigData.First(_ => _.TableName.EndsWith(tableName));
                    InitPageByFileName(dt.TableName);
                    SetPageData(dt);
                    SelectedTable = dt;
                }
            }

        }

        #region Header Index Change Events
        private void txtHeaderIndex_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox != null)
            {
                textbox.Text = Regex.Replace(textbox.Text, "[^0-9]", "");
            }
        }

        private void txtHeaderIndex_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.D0:
                case Key.D1:
                case Key.D2:
                case Key.D3:
                case Key.D4:
                case Key.D5:
                case Key.D6:
                case Key.D7:
                case Key.D8:
                case Key.D9:
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    e.Handled = false;
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }
        #endregion
        #endregion

        #region Import To DataTable
        private void Import(string[] names)
        {
            lock (sync)
            {
                if (!IsUpdating)
                {
                    IsUpdating = true;
                    ControlUtils.ShowLoading(this.loading);
                    Clear();

                    if (names.Length == 1)
                    {
                        ImportFile(names[0]);
                    }
                    else if (names.Length > 1)
                    {
                        ImportFiles(names);
                    }

                    ControlUtils.HideLoading(this.loading);
                    IsUpdating = false;
                }
            }
        }

        private void ImportFile(string fileName)
        {
            InitPageByFileName(fileName);

            string errMsg;
            DataTable dt;
            ReadToDataTable(fileName,out dt, out errMsg);

            SelectedTable = dt;
            if (string.IsNullOrEmpty(errMsg))
            {
                SelectedTable.TableName = fileName;
                SetPageData(SelectedTable);
            }
            else
            {
                CommonUtils.ShowMsg(errMsg);
            }

        }

        private void ImportFiles(string[] names)
        {
            var action = new Action(() => 
            {
                StringBuilder sb = new StringBuilder(32);
                int failCount = 0;
                for (int i = 0; i < names.Length; i++)
                {
                    try
                    {
                        string errMsg;
                        DataTable dt = null;
                        if (ReadToDataTable(names[i],out dt, out errMsg))
                        {
                            var fileName = names[i].GetFileName();
                            if (fileName.GetImportType() == ImportDataType.交割单 && string.IsNullOrEmpty(fileName.GetGroupName()))
                            {
                                NotMatchConfigData.Add(dt);
                            }
                            else
                            {
                                var date = fileName.GetDate();
                                if (date == DateTime.MinValue)
                                {
                                    NotMatchConfigData.Add(dt);
                                }
                                else
                                {
                                    var type = names[i].GetImportType();
                                    SetTableToList(dt, names[i].GetFileName().GetGroupName() + Enum.GetName(type.GetType(), type), type);
                                }
                            }
                        }
                        else
                        {
                            sb.AppendLine(errMsg);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine(string.Format("读取文件{0}异常，错误信息：{1}", names[i], ex.Message));
                        failCount++;
                    }
                }
                CommonUtils.Log(sb.ToString());
                Dispatcher.ShowMsg(string.Format("读取文件共{0}个，失败{1}个，成功{2}个，错误信息请查看日志文件。", names.Length, failCount, names.Length - failCount, sb.ToString()));
            });

            var completeUIAction = new Action(() => 
            {
                this.listBoxMatched.ItemsSource = this.MatchConfigData.Select(_ => _.TableName.GetFileName());
                this.listBoxNotMatched.ItemsSource = this.NotMatchConfigData.Select(_ => _.TableName.GetFileName());

                btnSave.IsEnabled = listBoxMatched.Items.Count > 0;
                tabFiles.SelectedIndex = listBoxMatched.Items.Count == 0 ? 1 : 0;
                this.loading.HideLoading();
            });


            Dispatcher.RunAsync(action, null, null, completeUIAction);
            
        }

        private void SetTableToList(DataTable dt, string configName, ImportDataType dataType)
        {
            var arr = (dataType == ImportDataType.交割单) ? deliPropertyNames : softPropertyNames;
            var config = CommonUtils.GetConfig(configName);
            if (string.IsNullOrEmpty(config))
            {
                NotMatchConfigData.Add(dt);
            }
            else
            {
                bool isFitConfig = true;

                var dict = config.FromJson<Dictionary<string, string>>();
                foreach (var item in arr)
                    isFitConfig = isFitConfig && (!dict.ContainsKey(item) || dt.Columns.Contains(dict[item]));

                if (isFitConfig)
                    MatchConfigData.Add(dt);
                else
                    NotMatchConfigData.Add(dt);
            }
        }

        #region Read File To DataTable
        private bool ReadToDataTable(string FileName,out DataTable dt, out string errMsg)
        {
            dt = null;
            errMsg = null;
            try
            {
                dt = CommonUtils.IsExcel(FileName) ?  ExcelUtils.ReadExcel(FileName) : CommonUtils.ReadCSV(FileName);
                FixTableData(dt);
                dt.TableName = FileName;
                return true;
            }
            catch (NPOI.POIFS.FileSystem.NotOLE2FileException)
            {
                errMsg = "文件格式不正确，请尝试用Excel另存为后打开";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return false;
        }

        private void FixTableData(DataTable ExcelData)
        {
            if (ExcelData.Columns.Contains("摘要") && !ExcelData.Columns.Contains("证券代码") && !ExcelData.Columns.Contains("买卖标志"))
            {
                ExcelData.Columns.Add("证券代码");
                ExcelData.Columns.Add("买卖标志");
                foreach (DataRow item in ExcelData.Rows)
                {
                    var detail = item["摘要"];
                    if (detail != null)
                    {
                        var info = detail.ToString();
                        item["证券代码"] = Regex.Match(info, "[0-9]{6}(?<=\\d)").Value;
                        item["买卖标志"] = info;
                    }
                }
            }

            if (ExcelData.Columns.Contains(EmptyColumn))
            {
                ExcelData.Columns.Remove(EmptyColumn);
            }
            ExcelData.Columns.Add(EmptyColumn);
        }
        #endregion

        #region Render Page by Selected DataTable
        private void SetPageData(DataTable dt)
        {
            dgImportData.ItemsSource = dt.DefaultView;
            spSave.Visibility = Visibility.Visible;


            Dictionary<string, int> dictColIndex = new Dictionary<string, int>();
            //识别类型后默认选择保存类型，并将信息显示在lblMsg中
            bool isSoftData = true;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (isSoftData && i < softPropertyNames.Length)
                {
                    isSoftData = isSoftData && (dt.Columns[i].ColumnName == softPropertyNames[i] || dt.Columns[i].ColumnName == softPropertyNamesExtend[i]);
                    dictColIndex.Add(dt.Columns[i].ColumnName, i);
                }
                else
                {
                    dictColIndex.Add(dt.Columns[i].ColumnName, i);
                }
                //if (dt.Columns[i].ColumnName == softPropertyNames[i])
                //{
                //    dictColIndex.Add(dt.Columns[i].ColumnName, i);
                //}
                //else if (dt.Columns[i].ColumnName == softPropertyNamesExtend[i])
                //{
                //    dictColIndex.Add(softPropertyNamesExtend[i], i);
                //}

                
            }

            UIElementCollection uiElemColl = null;
            if (isSoftData)
            {
                cboDataType.SelectedIndex = 1;//识别为软件委托
                uiElemColl = gdSoftSet.Children;
            }
            else
            {
                uiElemColl = gdDelivery.Children;
                cboDataType.SelectedIndex = 0;//识别为交割单
            }

            SetComboxSource(dictColIndex, gdSoftSet.Children);
            SetComboxSource(dictColIndex, gdDelivery.Children);
        }

        private void SetComboxSource(Dictionary<string, int> dictColIndex, UIElementCollection uiElemColl)
        {
            StringBuilder sb = new StringBuilder(64);
            foreach (var item in uiElemColl)
            {
                if (item is ComboBox)
                {
                    var cb = item as ComboBox;
                    if (SetItemSelectIndex(dictColIndex, cb))
                    {
                        sb.Append(Regex.Replace(cb.Name, "[a-zA-Z0-9]", "") + ",");
                        cb.SelectedIndex = cb.Items.Count - 1;
                    }
                }
            }

            if (sb.Length > 0)
            {
                sb.AppendLine(" 以上属性未找到对应列，默认选择为空白列。");
                txtNotice.Text = sb.ToString();
                txtNotice.Visibility = Visibility.Visible;
            }
        }

        private bool SetItemSelectIndex(Dictionary<string, int> dictColIndex, ComboBox cb)
        {
            
            cb.ItemsSource = dictColIndex.Keys.ToList();

            var propertyName = Regex.Replace(cb.Name, "[a-zA-Z0-9]", "");
            if (propertyName.Length > 0)
            {
                if (dictColIndex.ContainsKey(propertyName))
                {
                    cb.SelectedIndex = dictColIndex[propertyName];
                }
                else if (propertyName == "买卖标志" && dictColIndex.ContainsKey("买卖方向"))
                {
                    cb.SelectedIndex = dictColIndex["买卖方向"];
                }
            }
            

            if (cb.SelectedIndex == -1)
            {
                foreach (var columnName in PropertyMap)
                {
                    if (cb.Name.EndsWith(columnName.Key))
                    {
                        foreach (var otherName in columnName.Value)
                        {
                            if (dictColIndex.ContainsKey(otherName))
                            {
                                cb.SelectedIndex = dictColIndex[otherName];
                            }
                        }
                    }
                }
            }
            return cb.SelectedIndex == -1;
        }
        #endregion
        #endregion

        #region Save To Database
        private void Save()
        {
            ControlUtils.ShowLoading(this.loading, "保存中……");
            bool isAutoSearch = cbNeedAutoSearchDel.IsChecked == true;
            lock (sync)
            {
                if (!IsUpdating)
                {
                    IsUpdating = true;
                    
                    var delieryConfig = GetPageConfig(ImportDataType.交割单);
                    var softConfig = GetPageConfig(ImportDataType.软件委托);

                    if (SelectedTable != null)
                    {
                        SaveOneTable(SelectedTable, delieryConfig, softConfig);
                    }
                    else if (MatchConfigData.Count > 0)
                    {
                        List<string> groupNames = new List<string>();
                        foreach (string item in listBoxMatched.Items)
                        {
                            var groupName = item.GetFileName().GetGroupName();
                            if (item.GetImportType() == ImportDataType.软件委托 && !string.IsNullOrEmpty(groupName) && !groupNames.Contains(groupName))
                                groupNames.Add(groupName);
                        }

                        listBoxMatched.ItemsSource = null;
                        listBoxNotMatched.ItemsSource = NotMatchConfigData.Select(_ => _.TableName.GetFileName());
                        MatchConfigData.Clear();

                        var saving = new Action(() => { SaveMatchedTables(delieryConfig, softConfig); });

                        var completeUIAction = new Action(() => {
                            ControlUtils.HideLoading(this.loading);
                            if (OnDBChangeComplete != null)
                                OnDBChangeComplete.Invoke();

                            if (cbNeedAutoSearchDel.IsChecked == true)
                            {
                                AutoSaveDelivery(groupNames, this.dpDate.SelectedDate.Value);
                            }
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
            bool isDeli = cboDataType.SelectedIndex == 0;

            var doWork = new Action(() => 
            {
                using (DataComparisionDataset db = new DataComparisionDataset())
                {
                    if (isDeli)
                    {
                        isSuccess = SaveAsDeliveryData(dt, db, deliConfig, true, out msg);
                        
                    }
                    else
                        isSuccess = SaveAsSoftwareData(dt, db, softConfig, true, out msg);
                }
                
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    MessageBox.Show(isSuccess ? "保存完毕!" : msg);
                    if (isSuccess)
                    {
                        if (!isDeli)
                        {
                            if (OnImportComplete != null)
                            {
                                //批量的软件委托，或者单独一个。
                                var listGroupName = new List<string>();
                                foreach (DataRow row in dt.Rows)
                                {
                                    var groupName = row["组合号"].ToString();
                                    if (!string.IsNullOrEmpty(groupName) && !listGroupName.Contains(groupName))
                                    {
                                        listGroupName.Add(groupName);
                                    }
                                }
                                OnImportComplete.Invoke(listGroupName, this.dpDate.SelectedDate.Value);
                            }

                            List<string> groupNames = new List<string>();
                            foreach (DataRow row in dt.Rows)
                            {
                                if (!groupNames.Contains(row["组合号"] + ""))
                                {
                                    groupNames.Add(row["组合号"] + "");
                                }
                            }
                            if (cbNeedAutoSearchDel.IsChecked == true)
                                AutoSaveDelivery(groupNames, this.dpDate.SelectedDate.Value);
                        }
                    }
                    if (NotMatchConfigData.Contains(dt))
                    {
                        NotMatchConfigData.Remove(dt);
                        listBoxNotMatched.ItemsSource = NotMatchConfigData.Select(_ => _.TableName.GetFileName());
                    }

                    SelectedTable = null;
                    ControlUtils.HideLoading(this.loading);
                    dgImportData.ItemsSource = null;

                    if (OnDBChangeComplete != null)
                        OnDBChangeComplete.Invoke();

                    
                }));
            });

            this.Dispatcher.RunAsync(doWork,  null, null, null);
        }

        private void SaveMatchedTables(Dictionary<string,string> deliPageConfig, Dictionary<string, string> softPageConfig)
        {
            StringBuilder sb = new StringBuilder(64);
            try
            {
                int successCount = 0;
                using (var db = new DataComparisionDataset())
                {
                    //foreach (DataTable dt in MatchConfigData)
                    //{
                    //    bool isSuccess = false;
                    //    string msg;
                    //    var saveAsDeli = dt.TableName.GetImportType();
                    //    if (saveAsDeli == ImportDataType.软件委托)
                    //        isSuccess = SaveAsSoftwareData(dt, db, softPageConfig, false, out msg);
                    //    else if (saveAsDeli == ImportDataType.交割单)
                    //        isSuccess = SaveAsDeliveryData(dt, db, deliPageConfig, false, out msg);
                    //    else
                    //        msg = "未能从文件名中识别出导入数据类型，将跳过此文件：" + dt.TableName;

                    //    if (isSuccess)
                    //        successCount++;
                    //    else 
                    //    {
                    //        if (!string.IsNullOrEmpty(msg)) sb.AppendLine(msg);
                    //        NotMatchConfigData.Add(dt);
                    //    }
                    //}
                    for (int i = 0; i < MatchConfigData.Count; i++)
                    {
                        DataTable dt = MatchConfigData[i];
                        this.Dispatcher.RunAsync(() => { this.loading.ShowLoading(string.Format("正在存储第{0}个文件，总计{1}个文件\r\n当前文件名：{2}", i, MatchConfigData.Count, dt.TableName.GetFileName())); });
                        
                        bool isSuccess = false;
                        string msg;
                        var saveAsDeli = dt.TableName.GetImportType();
                        if (saveAsDeli == ImportDataType.软件委托)
                            isSuccess = SaveAsSoftwareData(dt, db, softPageConfig, false, out msg);
                        else if (saveAsDeli == ImportDataType.交割单)
                            isSuccess = SaveAsDeliveryData(dt, db, deliPageConfig, false, out msg);
                        else
                            msg = "未能从文件名中识别出导入数据类型，将跳过此文件：" + dt.TableName;

                        if (isSuccess)
                            successCount++;
                        else
                        {
                            if (!string.IsNullOrEmpty(msg)) sb.AppendLine(msg);
                            NotMatchConfigData.Add(dt);
                        }
                    }
                    CommonUtils.Log("保存记录：" + sb.ToString());
                }

                var message = string.Format("批量保存{0}个文件，成功{1}个，失败{2}个。", MatchConfigData.Count, successCount, MatchConfigData.Count - successCount);
                if (successCount < MatchConfigData.Count)
                    message += "失败文件将转入单独保存列表，详细信息请查看日志中的保存记录。";

                this.Dispatcher.ShowMsg(message);
            }
            catch (Exception ex)
            {
                CommonUtils.Log("存入数据库时出现异常！", ex);
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { CommonUtils.ShowMsg("保存失败,详情请检查日志！"); }));
            }
        }

        #region Save Delivery
        private bool SaveAsDeliveryData(DataTable dt, DataComparisionDataset db, Dictionary<string, string> pageConfig, bool isSaveFromPage, out string msg)
        {
            msg = string.Empty;
            try
            {
                var fileName = dt.TableName.GetFileName();
                var group = fileName.GetGroupName();
                var date = fileName.GetDate();
                if (date == DateTime.MinValue || string.IsNullOrEmpty(group))
                {
                    msg = string.Format("交割单文件名中必须同时包含组合号及交易日期，文件{0}未符合条件。", fileName);
                    return false;
                }

                var dict = isSaveFromPage ? pageConfig : CommonUtils.GetConfig(group + "交割单").FromJson<Dictionary<string, string>>();
                var list = GetDeliveryList(dt, group, dict);
                if (list.Count == 0)
                {
                    msg = "符合条件的数据条数为0，交割单默认保存条件为发生金额大于0，将跳过此文件：" + fileName;
                    return false;
                }

                var groupList = list.Select(_ => _.组合号).ToList();
                var formatData = db.交割单ds.Where(_ => _.交割日期 == date && groupList.Contains(_.组合号));
                if (formatData.Count() > 0)
                {
                    if (isSaveFromPage && MessageBox.Show("已存在对应交割单数据，是否删除旧数据并保存？", "交割单保存", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return false;
                    db.交割单ds.RemoveRange(formatData);
                }

                db.交割单ds.AddRange(list);
                db.SaveChanges();
                if (isSaveFromPage) CommonUtils.SetConfig(group + "交割单", dict.ToJson());
            }
            catch (Exception ex)
            {
                CommonUtils.Log(string.Format("交割单导入失败, 关联文件名{0}！", dt.TableName), ex);
                msg = "保存交割单失败,详情请查看日志!";
                return false;
            }
            return true;
        }

        private List<交割单> GetDeliveryList(DataTable dt, string group, Dictionary<string, string> map)
        {
            List<交割单> list = new List<交割单>();
            foreach (DataRow row in dt.Rows)
            {
                var entity = new Entity.交割单()
                {
                    OrderID = Guid.NewGuid().ToString(),
                    组合号 = group,
                    成交编号 = row[map["成交编号"]].ToString(),
                    成交价格 = CommonUtils.GetDecimal(row[map["成交价格"]]),
                    成交金额 = CommonUtils.GetDecimal(row[map["成交金额"]]),
                    成交数量 = CommonUtils.GetDecimal(row[map["成交数量"]]),
                    交割日期 = CommonUtils.GetDate(row[map["交割日期"]]),

                    买卖标志 = row[map["买卖标志"]].ToString(),
                    证券代码 = row[map["证券代码"]].ToString().FixStockCode(),
                    证券名称 = row[map["证券名称"]].ToString(),
                    发生金额 = CommonUtils.GetDecimal(row[map["发生金额"]]),
                    手续费 = CommonUtils.GetDecimal(row[map["手续费"]]),
                    印花税 = CommonUtils.GetDecimal(row[map["印花税"]]),
                    过户费 = CommonUtils.GetDecimal(row[map["过户费"]]),
                    其他费 = CommonUtils.GetDecimal(row[map["其他费"]]),
                    备注 = row[map["备注"]].ToString(),
                    SortSequence = list.Count,
                };

                if (entity.发生金额 == 0)
                    continue;

                if (entity.成交数量 > 0 && entity.买卖标志.Contains("卖"))
                    entity.成交数量 = 0 - entity.成交数量;

                list.Add(entity);
            }
            return list;
        }

        #endregion

        #region Save Soft
        private bool SaveAsSoftwareData(DataTable dt, DataComparisionDataset db, Dictionary<string, string> pageConfig, bool isSaveFromPage, out string msg)
        {
            msg = string.Empty;
            try
            {
                var fileName = dt.TableName.GetFileName();
                var date = fileName.GetDate();
                var group = fileName.GetGroupName();
                if (date == DateTime.MinValue)
                {
                    msg = string.Format("软件委托文件名中必须包含交易日期，文件{0}未符合条件。", fileName);
                    return false;
                }
                
                var dict = (isSaveFromPage || string.IsNullOrEmpty(group) ) ? pageConfig : CommonUtils.GetConfig(group + "软件委托").FromJson<Dictionary<string, string>>();
                var lstSoft = GetSoftList(date, dt, dict);
                if (lstSoft.Count == 0)
                {
                    msg = "符合条件的数据条数为0，将跳过此文件：" + fileName;
                    return false;
                }

                var groupList = lstSoft.Select(_ => _.组合号).ToList();
                var formatData = db.软件委托ds.Where(_ => _.成交日期 == date && groupList.Contains(_.组合号));
                if (formatData.Count() > 0)
                {
                    if (isSaveFromPage && MessageBox.Show("已存在对应软件委托数据，是否删除并保存？", "Save", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return false;
                    db.软件委托ds.RemoveRange(formatData);
                }

                db.软件委托ds.AddRange(lstSoft);
                db.SaveChanges();
                if (isSaveFromPage) CommonUtils.SetConfig(group + "软件委托", dict.ToJson());
            }
            catch (Exception ex)
            {
                CommonUtils.Log(string.Format("软件委托导入失败, 关联文件名{0}！", dt.TableName), ex);
                msg = "保存软件委托失败,详情请查看日志!";
                return false;
            }
            return true;
        }

        private List<软件委托> GetSoftList(DateTime date, DataTable dt, Dictionary<string, string> map)
        {
            List<软件委托> lstSoft = new List<软件委托>();
            foreach (DataRow row in dt.Rows)
            {
                var code = row[map["组合号"]].ToString().FixStockCode();
                if (string.IsNullOrWhiteSpace(code))
                {
                    continue;
                }
                string strCancelQty = row[map["撤单数量"]].ToString();
                var entity = new Entity.软件委托()
                {
                    交易员 = row[map["交易员"]].ToString(),
                    组合号 = row[map["组合号"]].ToString(),
                    证券代码 = row[map["证券代码"]].ToString().FixStockCode(),
                    证券名称 = row[map["证券名称"]].ToString(),
                    买卖标志 = row[map["买卖标志"]].ToString(),
                  
                    状态说明 = row[map["状态说明"]].ToString(),
                    委托时间 = row[map["委托时间"]].ToString(),
                    委托编号 = row[map["委托编号"]].ToString(),
                    券商名称 = row[map["组合号"]].ToString(),
                    成交日期 = date,
                    SortSequence = lstSoft.Count,
                };
                decimal num;
                if (Decimal.TryParse(row[map["委托价格"]] + "", out num))
                {
                    entity.委托价格 = num;
                }
                else
                {
                    throw new Exception(string.Format("异常委托数据，委托价格必须是数字，交易员{0},组合号{1},证券代码{2}, 委托编号{3}", entity.交易员, entity.组合号, entity.证券代码, entity.委托编号));
                }
                if (Decimal.TryParse(row[map["委托数量"]].ToString(), out num))
                {
                    entity.委托数量 = num;
                }
                else
                {
                    throw new Exception(string.Format("异常委托数据，委托数量必须是数字，交易员{0},组合号{1},证券代码{2}, 委托编号{3}", entity.交易员, entity.组合号, entity.证券代码, entity.委托编号));
                }
                if (Decimal.TryParse(row[map["成交价格"]].ToString(), out num))
                {
                    entity.成交价格 = num;
                }
                else
                {
                    throw new Exception(string.Format("异常委托数据，成交价格必须是数字，交易员{0},组合号{1},证券代码{2}, 委托编号{3}", entity.交易员, entity.组合号, entity.证券代码, entity.委托编号));
                }
                if (Decimal.TryParse(row[map["成交数量"]].ToString(), out num))
                {
                    entity.成交数量 = num;
                }
                else
                {
                    throw new Exception(string.Format("异常委托数据，成交数量必须是数字，交易员{0},组合号{1},证券代码{2}, 委托编号{3}", entity.交易员, entity.组合号, entity.证券代码, entity.委托编号));
                }
                if (Decimal.TryParse(row[map["撤单数量"]].ToString(), out num))
                {
                    entity.撤单数量 = Decimal.Parse(row[map["撤单数量"]].ToString());
                }
                else
                {
                    throw new Exception(string.Format("异常委托数据，撤单数量必须是数字，交易员{0},组合号{1},证券代码{2}, 委托编号{3}", entity.交易员, entity.组合号, entity.证券代码, entity.委托编号));
                }

                lstSoft.Add(entity);
            }

            return lstSoft;
        }
        #endregion

        #region Get Config From Page
        private Dictionary<string, string> GetConfigDefault(DataTable dt, ImportDataType importDataType, string configName, bool isSave = true)
        {
            var dict = new Dictionary<string, string>();

            var arr = importDataType == ImportDataType.交割单 ? deliPropertyNames : softPropertyNames;
            foreach (var propertyName in arr)
            {
                var mappedColumn = GetColumnByProperty(propertyName, dt);
                dict.Add(propertyName, mappedColumn);
            }

            if (!string.IsNullOrEmpty(configName) && isSave)
            {
                CommonUtils.SetConfig(configName, dict.ToJson());
            }
            return dict;
        }

        private Dictionary<string, string> GetPageConfig(ImportDataType type)
        {
            var dict = new Dictionary<string, string>();
            var arr = type == ImportDataType.交割单 ? deliPropertyNames : softPropertyNames;
            foreach (var propertyName in arr)
            {
                var cbBox = this.FindName(GetNameByType(type, propertyName)) as ComboBox;
                if (cbBox != null && cbBox.SelectedIndex > -1)
                {
                    dict.Add(propertyName, cbBox.Text);
                }
                //else if (type == ImportDataType.交割单 &&  propertyName == "组合号" && !string.IsNullOrEmpty(txtDelivery组合号.Text))
                //{
                //    dict.Add(propertyName, txtDelivery组合号.Text);
                //}
            }

            return dict;
        }

        private string GetColumnByProperty(string property, DataTable dt)
        {
            if (dt.Columns.Contains(property))
            {
                return property;
            }
            else if (PropertyMap.ContainsKey(property))
            {
                foreach (var item in PropertyMap[property])
                {
                    if (item == property)
                        return item;
                }
            }
            return EmptyColumn;
        }

        private string GetNameByType(ImportDataType type, string property)
        {
            string header = string.Empty;
            if (type == ImportDataType.交割单)
            {
                header = "cbDelivery";
            }
            else if (type == ImportDataType.软件委托)
            {
                header = "cbSoft";
            }
            return header + property;
        }
        #endregion

        private bool IsValid(out string errMsg)
        {
            errMsg = string.Empty;
            if (SelectedTable != null && cboDataType.SelectedIndex < 0)
                errMsg = "请选择要保存的目标类型";
            else if (SelectedTable == null && MatchConfigData.Count == 0)
                errMsg = "请选择要保存的文件！";
            else
            {
                if (dpDate.SelectedDate.HasValue && dpDate.SelectedDate.Value.Date == DateTime.Today)
                {
                    var result = MessageBox.Show("确认导入日期为今天吗？", "导入", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.Cancel)
                        return false;
                }

                //单表导入交割单时检测以下项。
                if (cboDataType.SelectedIndex == 0 && SelectedTable != null)
                {
                    errMsg = CheckDelivery(errMsg);
                }
            }

            return string.IsNullOrEmpty(errMsg);
        }

        private string CheckDelivery(string errMsg)
        {
            if (string.IsNullOrEmpty(txtDelivery组合号.Text))
            {
                errMsg = "请输入组合号！";
            }
            if (cbDelivery交割日期.SelectedIndex == cbDelivery交割日期.Items.Count - 1)
            {
                errMsg = "交割日期不能为空白列，请选择交割日期对应的数据列！";
            }
            if (cbDelivery证券代码.SelectedIndex == cbDelivery证券代码.Items.Count - 1)
            {
                errMsg = "证券代码不能为空白列，请选择证券代码对应的数据列！";
            }
            if (cbDelivery成交数量.SelectedIndex == cbDelivery成交数量.Items.Count - 1)
            {
                errMsg = "成交数量不能为空白列，请选择成交数量对应的数据列！";
            }
            if (cbDelivery成交价格.SelectedIndex == cbDelivery成交价格.Items.Count - 1)
            {
                errMsg = "成交价格不能为空白列，请选择成交价格对应的数据列！";
            }
            return errMsg;
        }
        #endregion

        #region Auto Search Delivery
        private void AutoSaveDelivery(List<string> groupNames, DateTime date)
        {
            if (groupNames.Count < 1)
                return;

            int totalCount = 0;
            int successCount = 0;
            List<string> successGroupName = new List<string>();
            List<string> failGroupName = new List<string>();
            List<Entity.券商> groupList = CommonUtils.DictGroup.Values.ToList();

            var doAction = new Action(() => {

                totalCount = groupNames.Count;
                var groupExists = groupList.Where(_ => groupNames.Contains(_.名称));

                foreach (var groupItem in groupExists)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => { this.loading.ShowLoading(string.Format("开始查询{0}对应的交割单。\r\n查询组合号总计{1}个，正在查询第{2}个", groupItem.名称, groupList.Count, successCount + failGroupName.Count + 1)); }));
                    try
                    {
                        if (SaveGroupDelivery(groupItem, date))
                        {
                            successCount++;
                            successGroupName.Add(groupItem.名称);
                            CommonUtils.Log("{0}自动查询交割单完毕!", groupItem.名称);
                        }
                        else
                        {
                            failGroupName.Add(groupItem.名称);
                        }
                    }
                    catch (Exception ex)
                    {
                        CommonUtils.Log("{0}自动查询交割单异常! 异常信息:{1}", groupItem.名称, ex.Message);
                    }
                    Thread.Sleep(100);
                }
                var notAddGroup = string.Join(",", groupNames.Except(successGroupName).Except(failGroupName));
                var info = string.Format("自动查询交割单，总交割单列表：{0}，成功列表{1}，失败列表：{2},未添加帐号列表：{3}", 
                    string.Join(",", groupNames), string.Join(",", successGroupName), string.Join("," , failGroupName), notAddGroup);

                CommonUtils.Log(info);
                CommonUtils.ShowMsg(string.Format("自动查询{0}个交割单，成功{1}个，失败{2}个，未添加券商有：{3}，详细信息可检查日志。", totalCount, successCount, failGroupName.Count, notAddGroup));
                
            });
            Dispatcher.RunAsync(doAction, null, null, () => { this.loading.HideLoading(); });
        }

        private static bool SaveGroupDelivery(Entity.券商 groupItem, DateTime date)
        {
            var saveResult = false;
            var data = DataHelper.QueryHisData(date, date, groupItem);
            if (data != null)
            {
                if (data.Rows.Count > 0)
                {
                    DataHelper.StandardDeliveryDataTable(data, date);
                    using (var db = new DataComparisionDataset())
                    {
                        var oldData = db.交割单ds.Where(_ => _.交割日期 == date && _.组合号 == groupItem.名称);
                        if (oldData.Count() > 0)
                        {
                            db.交割单ds.RemoveRange(oldData);
                            db.SaveChanges();
                        }
                    }
                    saveResult = DataHelper.WriteToDB(data, "交割单");
                }
                else
                {
                    saveResult = true;
                }
            }
            return saveResult;
        } 
        #endregion

        private void Clear()
        {
            SelectedTable = null;
            this.MatchConfigData.Clear();
            this.NotMatchConfigData.Clear();
            this.dpDate.SelectedDate = DateTime.Today;
            this.txtNotice.Text = string.Empty;
            this.dgImportData.ItemsSource = null;
            this.listBoxMatched.ItemsSource = null;
            this.listBoxNotMatched.ItemsSource = null;
        }

        private void InitPageByFileName(string fileFullPath)
        {
            try
            {
                var fileName = fileFullPath.GetFileName();
                var date = fileName.GetDate();
                if (!string.IsNullOrEmpty(fileName) && date > DateTime.MinValue)
                {
                    dpDate.SelectedDate = fileName.GetDate();
                    txtDelivery组合号.Text = fileName.GetGroupName();
                }
                else
                {
                    CommonUtils.Log("初始化界面日期及组合号时出错,文件名{0}, 截取文件名结果{1},截取日期结果{2}", fileFullPath, fileName, date.ToString());
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("导入文件后，初始化界面日期及组合号时出错,文件名：" + fileFullPath, ex);
            }
        }

    }


}
