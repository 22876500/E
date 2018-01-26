using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using DataComparision.Utils;
using System.Management;

namespace DataComparision
{
    public static class CommonUtils
    {
        public static string Version = "2017-07-28-0001";

        public static string UserName { get; set; }

        /// <summary>
        /// 可执行程序所在根目录
        /// </summary>
        public static string CurrentPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static SolidColorBrush normalBrush = new SolidColorBrush(Colors.White);
        public static SolidColorBrush totalBrush = new SolidColorBrush(Colors.LightSalmon);
        public static SolidColorBrush specalBrush = new SolidColorBrush(Colors.Orange);

        public static string DBConnection { get { return "Data Source=.\\SQLEXPRESS;Initial Catalog=DC_1;Integrated Security=True;"; } }

        internal static void ShowMsg(string msg)
        {
            System.Windows.MessageBox.Show(msg);
        }

        public static void ShowMsg(this Dispatcher dispatcher, string msg)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => 
            {
                ShowMsg(msg);
            }));
        }

        #region 读取CSV文本
        public static DataTable ReadCSV(string fileFullPath, int headerRowIndex = 0)
        {
            DataTable dt = new DataTable();
            var text = File.ReadAllLines(fileFullPath, Encoding.Default);
            var splitor = new[] { '\t', ',' };
            var headers = text[headerRowIndex].Split(splitor);
            foreach (var item in headers)
                dt.Columns.Add(item.TrimSpec());

            for (int i = headerRowIndex + 1; i < text.Length; i++)
            {
                if (text[i].Length > 0 && !string.IsNullOrWhiteSpace(text[i]))
                {
                    var cells = text[i].Split(splitor);
                    for (int m = 0; m < cells.Length; m++)
                        cells[m] = cells[m].TrimSpec();

                    for (int j = dt.Columns.Count; j < cells.Length; j++)
                        dt.Columns.Add();

                    dt.Rows.Add(cells);
                }
            }
            return dt;
        }

        static string TrimSpec(this string s)
        {
            return s.TrimStart(new[] { '\"', '=' }).TrimEnd('\"');
        }

        public static bool IsExcel(string fileFullPath)
        {
            bool ret = false;

            FileStream fs = File.OpenRead(fileFullPath);
            BinaryReader r = new BinaryReader(fs);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
                return false;
            }
            r.Close();
            fs.Close();


            String[] fileType = { "208207", "8075" };
            ret = fileType.Contains(fileclass);
            return ret;

            /*文件扩展名说明
             *4946/104116 txt
             *7173        gif 
             *255216      jpg
             *13780       png
             *6677        bmp
             *239187      txt,aspx,asp,sql
             *208207      xls.doc.ppt
             *6063        xml
             *6033        htm,html
             *4742        js
             *8075        xlsx,zip,pptx,mmap,zip
             *8297        rar   
             *01          accdb,mdb
             *7790        exe,dll           
             *5666        psd 
             *255254      rdp 
             *10056       bt种子 
             *64101       bat 
             *4059        sgf
             *239187      csv
             */
        }
        #endregion

        #region Log
        static Logger log = new Logger();

        public static void Log(string msg)
        {
            log.LogRunning(msg);
        }


        public static void Log(string msg, Exception ex)
        {
            log.LogErr(msg, ex);
        }

        public static void LogInfo(string msg)
        {
            log.LogInfo(msg);
        }
        
        public static void Log(string msg, params object[] param)
        {
            Log(string.Format(msg, param));
        }
        #endregion

        #region Safe Get Data
        internal static DateTime GetDate(string s)
        {
            DateTime dt;
            if (DateTime.TryParse(s, out dt))
            {
                return dt;
            }
            else if (!string.IsNullOrEmpty(s) && s.Length >= 8)
            {
                return new DateTime(int.Parse(s.Substring(0, 4)), int.Parse(s.Substring(4, 2)), int.Parse(s.Substring(6, 2)));
            }
            return dt;
        }

        internal static DateTime GetDate(object o)
        {
            if (o != null)
            {
                return GetDate(o.ToString());
            }
            return DateTime.MinValue;
        }

        internal static decimal GetDecimal(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                decimal d;
                if (decimal.TryParse(s, out d))
                    return d;
            }
            return 0;
        }

        internal static decimal GetDecimal(object o)
        {
            if (o != null)
            {
                return GetDecimal(o.ToString());
            }
            return 0;
        }

        internal static Int32 GetInt(object o)
        {
            if (o != null)
            {
                return GetInt(o.ToString());
            }
            return 0;
        }

        internal static int GetInt(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                int d;
                if (int.TryParse(s, out d))
                    return d;
            }
            return 0;
        }
        #endregion

        #region App Config
        static Configuration _config;
        static Configuration ConfigurationInstance
        {
            get
            {
                if (_config == null)
                {
                    _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                return _config;
            }
        }
        
        internal static string GetConfig(string name)
        {
            try
            {
                if (ConfigurationInstance.AppSettings.Settings.AllKeys.Contains(name))
                {
                    return ConfigurationInstance.AppSettings.Settings[name].Value;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log(string.Format("配置{0}获取出错！", name), ex);
                return null;
            }
        }

        internal static int SetConfig(string name, string value)
        {
            try
            {
                if (ConfigurationInstance.AppSettings.Settings.AllKeys.Contains(name))
                {
                    ConfigurationInstance.AppSettings.Settings[name].Value = value;
                }
                else
                {
                    ConfigurationInstance.AppSettings.Settings.Add(name, value);
                }
                ConfigurationInstance.Save();
                return 1;
            }
            catch (Exception ex)
            {
                Log(string.Format("配置{0}保存出错！", name), ex);
                return 0;
            }

        }

        internal static string GetGroupConfig(string group, ImportDataType type)
        {
            return GetConfig(group + Enum.GetName(typeof(ImportDataType) ,type));
        }
        #endregion

        #region Json
        public static string ToJson(this object o)
        {
            try
            {
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception ex)
            {
                Log("Json序列化错误", ex);
                ShowMsg("Json序列化错误，详情请查看日志！");
                return string.Empty;
            }
        }

        public static T FromJson<T>(this string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception ex)
            {
                Log("Json解析错误，解析对象：" + s, ex);
                ShowMsg("Json解析异常，详情请查看日志！");
                return default(T);
            }
        }

        public static List<T> ListFromJson<T>(this string s)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(s);
            }
            catch (Exception ex)
            {
                Log("Json解析错误，解析对象：" + s, ex);
                ShowMsg("Json解析异常，详情请查看日志！");
                return new List<T>();
            }
        } 
        #endregion

        private static Dictionary<string, DataComparision.Entity.券商> _dictGroup;
        public static Dictionary<string, DataComparision.Entity.券商> DictGroup
        {
            get {
                if (_dictGroup == null)
                {
                    try
                    {
                        var client = new ServiceReference.DataWebServiceSoapClient();
                        var group = client.GetGroups(CommonUtils.UserName);
                        var dict = group.FromJson<Dictionary<string, string>>();

                        var list = new Dictionary<string, Entity.券商>();
                        foreach (var item in dict)
                        {
                            list.Add(item.Key, Cryptor.MD5Decrypt(item.Value).FromJson<DataComparision.Entity.券商>());
                        }
                        _dictGroup = list;
                    }
                    catch (Exception ex)
                    {
                        Log("获取券商列表异常：", ex);
                    }
                    //if (_dictGroup == null)
                    //{
                    //    using (var db = new DataComparisionDataset())
                    //    {
                    //        var dict = new Dictionary<string, Entity.券商>();
                    //        var list =db.券商ds.Where(_ => _.启用).ToList();
                    //        foreach (var item in list)
                    //        {
                    //            dict.Add(item.名称, item);
                    //        }
                    //        _dictGroup = dict;
                    //    }
                    //}
                }
                return _dictGroup;
            }
        }

        public static bool IsIP(string s)
        {
            return Regex.IsMatch(s, "([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.]([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.]([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.](2([0-4][0-9]|5[0-5])|1[0-9][0-9]|[1-9][0-9]|[0-9])");
        }

        public static DataTable ChangeDataStringToTable(string Data)
        {
            if (Data == string.Empty)
            {
                return null;
            }


            DataTable DataTable1 = new DataTable("Result");

            string[] Lines = Data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] ColumnNames = Lines[0].Split(new char[] { '\t' });
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                if (DataTable1.Columns.IndexOf(ColumnNames[i]) == -1)
                {
                    DataTable1.Columns.Add(ColumnNames[i]);
                }
                else
                {
                    DataTable1.Columns.Add(ColumnNames[i] + "_" + i.ToString());
                }
            }


            for (int i = 1; i < Lines.Length; i++)
            {
                string[] Cells = Lines[i].Split(new char[] { '\t' });
                DataTable1.Rows.Add(Cells);
            }

            return DataTable1;
        }

        public static void RunAsync(this Dispatcher dispatcher, Action doWork, Action doUIWork = null, Action onComplete = null, Action onCompleteUIWork = null)
        {
            BackgroundWorker worker = new BackgroundWorker();

            // worker 要做的事情 使用了匿名的事件响应函数
            worker.DoWork += (o, ea) =>
            {
                if (doWork != null)
                {
                    doWork.Invoke();
                }
                if (dispatcher != null && doUIWork != null)
                {
                    dispatcher.Invoke(doUIWork);
                }
            };
            // worker 完成事件响应
            worker.RunWorkerCompleted += (o, ea) =>
            {
                if (onComplete != null)
                {
                    onComplete.Invoke();
                }

                if (dispatcher != null && onCompleteUIWork != null)
                {
                    dispatcher.Invoke(onCompleteUIWork);
                }
            };

            worker.RunWorkerAsync();
        }

        #region Mac
        static List<string> _lstMac;
        public static List<string> GetMacAddress()
        {
            if (_lstMac == null)
            {
                _lstMac = new List<string>();
                try
                {
                    ManagementClass ManagementClass1 = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    foreach (ManagementObject ManagementObject1 in ManagementClass1.GetInstances())
                    {
                        if ((bool)ManagementObject1["IPEnabled"])
                        {
                            var mac = ManagementObject1["MacAddress"].ToString();
                            _lstMac.Add(mac.ToString().Replace(":", "").ToUpper());
                        }
                    }
                }
                catch
                {

                }
            }
            return _lstMac;
        }

        public static List<string> PermitMacList = new List<string>() { "68F72855B1D0", "FCAA14B3008F" };
        #endregion

        // 2C337A092DFC
    }


}
