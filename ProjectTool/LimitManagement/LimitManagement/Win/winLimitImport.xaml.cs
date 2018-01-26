using LimitManagement.AASServiceReference;
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

namespace LimitManagement.Win
{
    /// <summary>
    /// winLimitImport.xaml 的交互逻辑
    /// </summary>
    public partial class winLimitImport : Window
    {
        DataTable dtImport;

        public winLimitImport()
        {
            InitializeComponent();
            this.Loaded += winLimitImport_Loaded;
        }

        public winLimitImport(bool isDeleteWindow)
        {
            InitializeComponent();
            
            if (isDeleteWindow)
            {
                this.btnSave.Visibility = System.Windows.Visibility.Collapsed;
                this.btnDelete.Visibility = System.Windows.Visibility.Visible;
            }
            this.Loaded += winLimitImport_Loaded;
        }

        #region Events
        void winLimitImport_Loaded(object sender, RoutedEventArgs e)
        {
            cmbServer.ItemsSource = ServiceConnectHelper.Instance.ServerInfoList.Where(_ => _.IsConnected).ToList();
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { CheckFileExists = true, Multiselect = false, };
            dialog.Filter = "All Files|*.xls;*.xlsx;*.csv;*.txt|Excel 97~2003|*.xls|Excel 2007/2010|*.xlsx|csv File|*.csv|Text File|*.txt";
            if (dialog.ShowDialog() == true)
            {
                this.loading.ShowLoading();
                string fileName = dialog.FileName;
                Encoding encode = Utils.GetEncoding((cmbEncode.SelectedItem as ComboBoxItem).Content.ToString());
                Task task = new Task(() => { RunImport(fileName, encode); });
                task.Start();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveImportTable();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPageData())
            {
                if (MessageBox.Show("当前导入数据{0}条，确认全部删除？", "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    loading.ShowLoading("正在导入数据，请耐心等待……");
                    var colNum = dtImport.Columns.Count;
                    var server = cmbServer.SelectedItem as Entities.ServerInfo;

                    var task = new Task(() =>
                    {
                        string msg;
                        if (colNum <= 10)
                        {
                            var table = ChangeTableToLimitDt(dtImport, out msg);
                            if (table != null) msg = server.DeleteLimits(table);
                        }
                        else
                        {
                            var limitTables = ChangeToDict(dtImport, out msg);
                            if (limitTables != null)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (var item in limitTables)
                                {
                                    string result = item.Key.DeleteLimits(item.Value);
                                    sb.AppendLine(result);
                                }
                                msg = sb.ToString();
                            }
                        }

                        Dispatcher.Invoke(() =>
                        {
                            this.loading.HideLoading();
                            MessageBox.Show(msg);
                            this.Close();
                        });
                    });
                    task.Start();
                }
            }
        }
        #endregion

        #region Import Data
        private void RunImport(string path, Encoding encode)
        {
            var dt = ExcelUtils.ReadDataFile(path, encode);
            dtImport = dt;
            foreach (DataRow item in dt.Rows)
            {
                if (item.ItemArray.Length > 2 && Regex.IsMatch(item[1].ToString(), "^[0-9]{1,5}$"))
                {
                    item[1] = Utils.AutoAddZero(item[1].ToString());
                }
            }
            dtImport.TableName = path.Substring(path.LastIndexOf('\\') + 1);
            this.Dispatcher.Invoke(() =>
            {
                this.dgImportData.ItemsSource = dtImport.DefaultView;
                txtMsg.AppendText(string.Format("已读取文件{0}，数据共{1}行.{2}", path, dt.Rows.Count, Environment.NewLine));
                this.loading.HideLoading();
            });
        }
        #endregion

        #region Save To Server
        private void SaveImportTable()
        {
            if (CheckPageData())
            {
                if (dtImport.Columns.Count <= 10)
                {
                    ImportOneTable();
                }
                else
                {
                    ImportMutiTable();
                }
            }
        }

        #region Check Data
        private bool CheckPageData()
        {
            if (dtImport == null)
            {
                MessageBox.Show("请导入数据！");
                return false;
            }

            if (cmbServer.SelectedIndex < 0 && dtImport.Columns.Count <= 10)
            {
                var serverIPMatch = Regex.Match(dtImport.TableName, "[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}");
                if (!serverIPMatch.Success)
                {
                    MessageBox.Show("请选择要导入的服务器，或在文件名中写入对应服务器ip, 服务器列表参见Server列表");
                    return false;
                }
            }

            //其次校验数据。
            return MatchLimitData(dtImport);
        }

        private bool MatchLimitData(DataTable dtImport)
        {
            bool LimitMatchSuccess = true;
            StringBuilder sb = new StringBuilder();
            if (dtImport != null)
            {
                int i = 0;
                Dictionary<string, List<string>> dictTraderCode = new Dictionary<string, List<string>>();
                foreach (DataRow item in dtImport.Rows)
                {
                    i++;
                    var stockCode = item[1].ToString();
                    if (!Regex.IsMatch(stockCode, "^[0-9]{6}$"))
                    {
                        sb.AppendFormat("第{0}行，第2列，证券代码验证失败，数据{1}不符合规范，证券代码应为6位数字\r\n", i, stockCode);
                        LimitMatchSuccess = false;
                    }

                    var groupInfo = item[2].ToString();
                    if (!Regex.IsMatch(groupInfo, "^[A-Z][0-9]{2}$"))
                    {
                        sb.AppendFormat("第{0}行，第3列，组合号验证失败，数据{1}不符合规范，组合号应为字母加两位数字\r\n", i, groupInfo);
                        LimitMatchSuccess = false;
                    }

                    var market = item[3].ToString();
                    if (!Regex.IsMatch(market, "^(0|1)$"))
                    {
                        sb.AppendFormat("第{0}行，第4列，市场验证失败，数据{1}不符合规范，市场应为0或1，0表示深市，1表示沪市\r\n", i, market);
                        LimitMatchSuccess = false;
                    }

                    var trader = item[0].ToString();
                    if (dictTraderCode.ContainsKey(trader) && dictTraderCode[trader].Contains(stockCode))
                    {
                        sb.AppendFormat("第{0}行，数据唯一性验证失败，已存在交易员{1} 股票代码{2}对应额度数据\r\n", i, trader, stockCode);
                        LimitMatchSuccess = false;
                    }
                    else
                    {
                        if (dictTraderCode.ContainsKey(trader))
                        {
                            dictTraderCode[trader].Add(stockCode);
                        }
                        else
                        {
                            dictTraderCode.Add(trader, new List<string>() { stockCode });
                        }
                    }
                }
            }
            if (sb.Length > 0)
            {
                sb.Append("\r\n验证失败，请根据提示信息修改正确后重新导入!");
                MessageBox.Show(sb.ToString());
            }
            return LimitMatchSuccess;
        }
        #endregion

        private void ImportOneTable()
        {
            Entities.ServerInfo server = cmbServer.SelectedItem as Entities.ServerInfo;
            if (server != null)
            {
                loading.ShowLoading("正在导入数据，请耐心等待……");
                var task = new Task(() =>
                {
                    string msg;
                    var table = ChangeTableToLimitDt(dtImport, out msg);
                    if (table != null)
                    {
                        msg = server.AddLimit(table);
                    }
                    Dispatcher.Invoke(() => {
                        loading.HideLoading();
                        MessageBox.Show(msg); 
                        this.Close();
                    });
                });
                task.Start();
            }
            else
            {
                MessageBox.Show("请选择要导入的服务器！");
            }
        }

        private void ImportMutiTable()
        {
            loading.ShowLoading();
            var task = new Task(() =>
            {
                string errMsg;
                var limitTables = ChangeToDict(dtImport, out errMsg);
                if (limitTables != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in limitTables)
                    {
                        sb.Append("服务器").Append(item.Key.Remark).Append('(').Append(item.Key.Ip).Append(')');
                        if (item.Key.IsConnected)
                        {
                            string result = item.Key.AddLimit(item.Value);
                            sb.AppendFormat("server {0} 导入额度结果：{1}", item.Key.Remark, result).Append(Environment.NewLine);
                        }
                        else
                        {
                            sb.Append("未连接，无法导入！").Append(Environment.NewLine);
                        }
                    }
                    errMsg = sb.ToString();
                }
                Dispatcher.Invoke(() => {
                    loading.HideLoading();
                    MessageBox.Show(errMsg);
                    this.Close();
                });
            });
            task.Start();
        }
        #endregion

        #region Transfer Data
        private DbDataSet.额度分配DataTable ChangeTableToLimitDt(DataTable dtImport, out string erMsg)
        {
            erMsg = string.Empty;
            DbDataSet.额度分配DataTable dtLimit = new DbDataSet.额度分配DataTable();
            try
            {
                foreach (DataRow row in dtImport.Rows)
                {
                    买模式 买模式1 = (买模式)Enum.Parse(typeof(买模式), row[6].ToString(), false);
                    卖模式 卖模式1 = (卖模式)Enum.Parse(typeof(卖模式), row[7].ToString(), false);
                    dtLimit.Add额度分配Row(row[0].ToString(), row[1].ToString(), row[2].ToString(), byte.Parse(row[3].ToString()), row[4].ToString(), row[5].ToString(), (int)买模式1, (int)卖模式1, decimal.Parse(row[8].ToString()), decimal.Parse(row[9].ToString()));
                }
                return dtLimit;
            }
            catch (Exception ex)
            {
                erMsg = ex.Message;
            }
            return null;
        }

        private Dictionary<Entities.ServerInfo, DbDataSet.额度分配DataTable> ChangeToDict(DataTable dtImport, out string errMsg)
        {
            errMsg = string.Empty;
            var dictServerDT = new Dictionary<Entities.ServerInfo, DbDataSet.额度分配DataTable>();
            StringBuilder sb = new StringBuilder();
            try
            {
                List<int> lstErrorRow = new List<int>();
                int i = 0;
                foreach (DataRow row in dtImport.Rows)
                {
                    i++;
                    买模式 买模式1 = (买模式)Enum.Parse(typeof(买模式), row[6].ToString(), false);
                    卖模式 卖模式1 = (卖模式)Enum.Parse(typeof(卖模式), row[7].ToString(), false);

                    var serverInfo = row[10].ToString();
                    var server = ServiceConnectHelper.Instance.ServerInfoList.FirstOrDefault(_ => _.Remark == serverInfo || _.Ip == serverInfo);
                    if (server != null)
                    {
                        if (dictServerDT.ContainsKey(server))
                        {
                            dictServerDT[server].Add额度分配Row(row[0].ToString(), row[1].ToString(), row[2].ToString(), byte.Parse(row[3].ToString()), row[4].ToString(), row[5].ToString(), (int)买模式1, (int)卖模式1, decimal.Parse(row[8].ToString()), decimal.Parse(row[9].ToString()));
                        }
                        else
                        {
                            var dt = new DbDataSet.额度分配DataTable();
                            dt.Add额度分配Row(row[0].ToString(), row[1].ToString(), row[2].ToString(), byte.Parse(row[3].ToString()), row[4].ToString(), row[5].ToString(), (int)买模式1, (int)卖模式1, decimal.Parse(row[8].ToString()), decimal.Parse(row[9].ToString()));
                            dictServerDT.Add(server, dt);
                        }
                    }
                    else
                    {
                        lstErrorRow.Add(i);
                    }
                }
                if (lstErrorRow.Count > 0)
                {
                    sb.Append("第").Append(string.Join(",", lstErrorRow)).Append("行未找到匹配的服务器信息，无法导入！");
                    errMsg = sb.ToString();
                }
                else
                {
                    return dictServerDT;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return null;
        } 
        #endregion

    }
}
