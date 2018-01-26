using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace TradeInterface
{
    static class CommonUtils
    {

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

        public static string UserName { get; set; }

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

        public static bool IsCode(string s)
        {
            return Regex.IsMatch(s, "[0-9]{6}");
        }

        public static bool IsCode(object o)
        {
            return Regex.IsMatch(o + "", "[0-9]{6}");
        }

        public static string GetCode(string s)
        {
            return Regex.Match(s, "[0-9]{6}").Value;
        }

        public static string GetCode(object o)
        {
            return Regex.Match(o + "", "[0-9]{6}").Value;
        }

        public static decimal GetDecimal(string s)
        {
            decimal d = 0;
            decimal.TryParse(s, out d);
            return d;
        }

        public static decimal GetDecimal(object o)
        {
            decimal d = 0;
            decimal.TryParse(o + "", out d);
            return d;
        }

        #region Config
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
            catch (Exception)
            {
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

        internal static Dictionary<string, string> GetConfigs()
        {
            var dict = new Dictionary<string, string>();
            foreach (string item in ConfigurationInstance.AppSettings.Settings.AllKeys)
            {
                if (Regex.IsMatch(item, "^[A-Z][0-9]{2}$"))
                {
                    dict.Add(item, ConfigurationInstance.AppSettings.Settings[item].Value);
                }
            }
            return dict;
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
        #endregion

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
        }

        public static string AutoAddZero(string stockID)
        {
            if (stockID.Length < 6)
            {
                StringBuilder preStr = new StringBuilder();
                for (int i = stockID.Length; i < 6; i++)
                {
                    preStr.Append('0');
                }
                preStr.Append(stockID);
                return preStr.ToString();
            }
            return stockID;
        }

        public static Encoding GetEncoding(string encodeName)
        {
            switch (encodeName)
            {
                case "ASCII":
                    return Encoding.ASCII;
                case "UTF8":
                    return Encoding.UTF8;
                case "Unicode":
                    return Encoding.Unicode;
                case "Default":
                default:
                    return Encoding.Default;
            }
        }

        //public static bool ExistsMac(string mac)
        //{
        //    try
        //    {
        //        string MAC = " ";
        //        ManagementClass ManagementClass1 = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //        foreach (ManagementObject ManagementObject1 in ManagementClass1.GetInstances())
        //        {
        //            if ((bool)ManagementObject1["IPEnabled"])
        //            {
        //                MAC = ManagementObject1["MacAddress"].ToString();
        //                if (MAC.ToString().Replace(":", "").ToUpper() == mac)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //    return false;
        //}

        //public static string GetMac()
        //{
        //    string MAC = " ";
        //    ManagementClass ManagementClass1 = new ManagementClass("Win32_NetworkAdapterConfiguration");
        //    foreach (ManagementObject ManagementObject1 in ManagementClass1.GetInstances())
        //    {
        //        if ((bool)ManagementObject1["IPEnabled"])
        //        {
        //            MAC = ManagementObject1["MacAddress"].ToString();
        //            return MAC.ToString().Replace(":", "").ToUpper();
        //        }
        //    }
        //    return null;
        //}

        //public static string GetCpuID()
        //{
        //    try
        //    {
        //        //获取CPU序列号代码 
        //        string cpuInfo = "";//cpu序列号 
        //        ManagementClass mc = new ManagementClass("Win32_Processor");
        //        ManagementObjectCollection moc = mc.GetInstances();
        //        foreach (ManagementObject mo in moc)
        //        {
        //            cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
        //        }
        //        moc = null;
        //        mc = null;
        //        return cpuInfo;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}


    }
}
