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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace TranInfoManager
{
    public static class CommonUtils
    {
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
                    return Utils.Cryptor.MD5Decrypt(ConfigurationManager.ConnectionStrings[conn].ConnectionString);
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

        #region Read CSV
        public static DataTable ReadDataTable(string fileFullPath, int headerRowIndex = 0, Action<int,int> onRowRead = null)
        {
            DataTable dt = new DataTable();
            var text = File.ReadAllLines(fileFullPath, Encoding.Default);
            var splitor = new[] { '\t', ',' };
            var headers = text[headerRowIndex].Split(splitor);
            foreach (var item in headers)
                dt.Columns.Add(item);

            for (int i = headerRowIndex + 1; i < text.Length; i++)
            {
                if (onRowRead != null)
                {
                    onRowRead.Invoke(i, text.Length);
                }
                if (text[i].Length > 0 && !string.IsNullOrWhiteSpace(text[i]))
                {
                    var cells = text[i].Split(splitor);

                    for (int j = dt.Columns.Count; j < cells.Length; j++)
                        dt.Columns.Add();

                    dt.Rows.Add(cells);
                }
            }
            return dt;
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
        #endregion

        #region String Helper
        public static string GetFileName(this string fullPathName)
        {
            return fullPathName.Substring(fullPathName.LastIndexOf('\\') + 1);
        }

        static Regex regDate = new Regex("[0-9-\\/]{4,}");
        static Regex regDateChs = new Regex("[0-9]{1,2}[\u4e00-\u9fa5]?[-/][0-9]{1,2}[\u4e00-\u9fa5]?[-/][0-9]{4}");
        static Regex regTime = new Regex("[0-9]{1,2}:[0-9]{2}:[0-9]{2}");
        public static DateTime GetDate(this string fileName)
        {
            var str = fileName.Contains('\\') ? fileName.GetFileName() : fileName;

            DateTime dt = DateTime.MinValue;
            var matche = regDate.Match(str);
            if (matche.Success && !DateTime.TryParse(matche.Value, out dt))
            {
                if (matche.Value.Length == 8)
                {
                    dt = new DateTime(int.Parse(matche.Value.Substring(0, 4)), int.Parse(matche.Value.Substring(4, 2)), int.Parse(matche.Value.Substring(6, 2)));
                }
                else if (matche.Value.Length == 4)
                {
                    dt = new DateTime(DateTime.Today.Year, int.Parse(matche.Value.Substring(0, 2)), int.Parse(matche.Value.Substring(2, 2)));
                }
            }

            return dt;
        }

        public static bool IsDate(this string str)
        {
            return regDateChs.IsMatch(str);
        }

        public static bool IsTime(this string str)
        {
            return regTime.IsMatch(str);
        }
        
        public static decimal GetDec(this string str)
        {
            decimal d = 0;
            if (decimal.TryParse(str, System.Globalization.NumberStyles.Any, null, out d))
                return d;
            else if (Regex.IsMatch(str, "[0-9]*[.]?[0-9]*"))
                return decimal.Parse(Regex.Match(str, "[0-9]*[.]?[0-9]*").Value, System.Globalization.NumberStyles.Any);
            else
                return 0;
        }
        #endregion

        #region Window Message Show
        internal static void ShowMsg(string errMsg)
        {
            MessageBox.Show(errMsg);
        }

        public static void ShowMsg(this Dispatcher dispatcher, string errMsg)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                MessageBox.Show(errMsg);
            }));
        }
        #endregion

        public static void RunAsync(this Dispatcher dispatcher, Action doWork, Action doUIWork = null, Action onComplete = null, Action onCompleteUIWork = null)
        {
            BackgroundWorker worker = new BackgroundWorker();

            // worker 要做的事情 使用了匿名的事件响应函数
            worker.DoWork += (o, ea) =>
            {
                try
                {
                    if (doWork != null)
                    {
                        doWork.Invoke();
                    }
                    if (dispatcher != null && doUIWork != null)
                    {
                        dispatcher.Invoke(doUIWork);
                    }
                }
                catch (Exception ex)
                {
                    CommonUtils.Log("异步执行异常：" + dispatcher.ToString(), ex);
                }
               
            };
            // worker 完成事件响应
            worker.RunWorkerCompleted += (o, ea) =>
            {
                try
                {
                    if (onComplete != null)
                    {
                        onComplete.Invoke();
                    }

                    if (dispatcher != null && onCompleteUIWork != null)
                    {
                        //dispatcher.BeginInvoke(DispatcherPriority.Normal, onCompleteUIWork);
                        dispatcher.Invoke(onCompleteUIWork);
                    }
                }
                catch (Exception ex)
                {
                    CommonUtils.Log("异步执行异常：" + dispatcher.ToString(), ex);
                }
            };

            worker.RunWorkerAsync();
        }

        public static void RunUI(this Dispatcher dispatcher, Action doUI)
        {
            dispatcher.BeginInvoke(DispatcherPriority.Normal, doUI);
        }

    }
}
