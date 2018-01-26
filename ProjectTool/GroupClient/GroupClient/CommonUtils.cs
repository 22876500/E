using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GroupClient
{
    public static class CommonUtils
    {
        private static object Sync = new object();

        public static string CurrentPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string DbConnectString
        {
            get
            {
                string conn = ConnName;
                if (string.IsNullOrEmpty(conn))
                {
                    conn = "localhost";
                }
                if (conn == "localhost")
                {
                    return ConfigurationManager.ConnectionStrings[conn].ConnectionString;
                }
                else
                {
                    return Cryptor.MD5Decrypt(ConfigurationManager.ConnectionStrings[conn].ConnectionString);
                }
            }
        }

        public static string ConnName { get; set; }

        #region Log
        static Logger log = new Logger();

        public static void Log(string msg)
        {
            log.LogRunning(msg);
        }

        public static void Log(string msg, params string[] args)
        {
            log.LogRunning(msg, args);
        }

        public static void Log(string msg, Exception ex)
        {
            log.LogErr(msg, ex);
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
                return new List<T>();
            }
        }
        #endregion

        #region App Config


        #region App Config
        static Configuration _config;
        internal static Configuration ConfigurationInstance
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

        internal static string GetConfig(string name, string defaultValue)
        {
            return GetConfig(name) ?? defaultValue;
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

     
        #endregion
        #endregion

        public static void RunAsync(this Dispatcher dispatcher, Action doWork, Action doUIWork = null, Action onComplete = null, Action onCompleteUIWork = null)
        {
            BackgroundWorker worker = new BackgroundWorker() { WorkerSupportsCancellation = true };

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

        public static bool IsIP(string s)
        {
            return Regex.IsMatch(s, "([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.]([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.]([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.](2([0-4][0-9]|5[0-5])|1[0-9][0-9]|[1-9][0-9]|[0-9])");
        }

        static string _pubIP;
        public static string GetPubIP()
        {
            if (string.IsNullOrEmpty(_pubIP))
            {
                _pubIP = CommonUtils.GetConfig("PubIP");
                if (string.IsNullOrEmpty(_pubIP))
                {
                    lock (Sync)
                    {
                        string tempip = "";
                        System.Net.WebRequest wr = System.Net.WebRequest.Create("http://www.ipip.net/");
                        Stream s = wr.GetResponse().GetResponseStream();
                        if (s != null)
                        {
                            StreamReader sr = new StreamReader(s, Encoding.UTF8);
                            string all = sr.ReadToEnd();
                            int start = all.IndexOf("您当前的IP：", StringComparison.Ordinal) + 7;
                            int end = all.IndexOf("<", start, StringComparison.Ordinal);
                            tempip = all.Substring(start, end - start);
                            sr.Close();
                            s.Close();
                        }
                        _pubIP = tempip;
                    }
                }
            }
            return _pubIP;
        }

        static string _mac;
        public static string GetMacAddress()
        {
            if (string.IsNullOrEmpty(_mac))
            {
                lock (Sync)
                {
                    if (string.IsNullOrEmpty(_mac))
                    {
                        try
                        {
                            var configMac = GetConfig("LocalMac");
                            if (string.IsNullOrEmpty(configMac))
                            {
                                ManagementClass ManagementClass1 = new ManagementClass("Win32_NetworkAdapterConfiguration");
                                foreach (ManagementObject ManagementObject1 in ManagementClass1.GetInstances())
                                {
                                    if ((bool)ManagementObject1["IPEnabled"])
                                    {
                                        configMac = ManagementObject1["MacAddress"].ToString();
                                        SetConfig("LocalMac", configMac);
                                        break;
                                    }
                                }
                            }
                            _mac = configMac.ToString().Replace(":", "-").ToUpper();
                        }
                        catch { _mac = string.Empty; }
                    }
                    
                }
            }
            return _mac;
        }

        static string _hdID;
        public static string GetHdID()
        {
            if (string.IsNullOrEmpty(_hdID))
            {
                lock (Sync)
                {
                    if (string.IsNullOrEmpty(_hdID))
                    {
                        _hdID = GetConfig("HDid");
                        if (string.IsNullOrEmpty(_hdID))
                        {
                            try
                            {
                                ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");
                                ManagementObjectCollection moc = cimobject.GetInstances();
                                foreach (ManagementObject mo in moc)
                                {
                                    _hdID = (string)mo.Properties["Model"].Value;
                                    SetConfig("HDid", _hdID);
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                Log("获取磁盘序列号异常,", ex);
                            }
                        }
                    }
                }
            }
            return _hdID;
        }

        static string _cpuID;
        public static string GetCpuID()
        {
            if (string.IsNullOrEmpty(_cpuID))
            {
                lock (Sync)
                {
                    if (string.IsNullOrEmpty(_cpuID))
                    {
                        _cpuID = CommonUtils.GetConfig("CpuID");
                        if (string.IsNullOrEmpty(_cpuID))
                        {
                            try
                            {
                                ManagementClass mc = new ManagementClass("Win32_Processor");
                                ManagementObjectCollection moc = mc.GetInstances();
                                foreach (ManagementObject mo in moc)
                                {
                                    _cpuID = mo.Properties["ProcessorId"].Value.ToString();
                                    SetConfig("CpuID", _cpuID);
                                    break;
                                }
                                moc = null;
                                mc = null;
                            }
                            catch (Exception ex)
                            {
                                Log("获取CpuID异常,", ex);
                            }
                        }
                    }
                }
            }
            return _cpuID;
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

        //static bool? _isFilteData = null;
        //public static bool IsFilterData
        //{
        //    get
        //    {
        //        if (_isFilteData == null)
        //        {
        //            _isFilteData = CommonUtils.GetConfig("FilteData") == "1";
        //        }
        //        return _isFilteData.Value;
        //    }
        //}

        public static bool IsTdxBuy(int category)
        {
            switch (category)
            {
                case 0://现金买入
                case 2://融资买入
                case 4://买券还款
                case 7://担保品买入
                    return true;
                case 1://现券卖出
                case 3://融券卖出
                case 5://卖券还款
                case 8://担保品卖出
                default:
                    return false;
            }
        }
    }
}
