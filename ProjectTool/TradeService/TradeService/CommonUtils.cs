using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace TradeService
{
    public static class CommonUtils
    {

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
     

        internal static string GetConfig(string name)
        {
            try
            {
                if (WebConfigurationManager.AppSettings.AllKeys.Contains(name))
                {
                    return WebConfigurationManager.AppSettings[name];
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
                //if (WebConfigurationManager.AppSettings.AllKeys.Contains(name))
                //{
                //    WebConfigurationManager.AppSettings[name] = value;
                //}
                //else
                //{
                //    WebConfigurationManager.AppSettings.Add(name, value);
                //}
                Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                if (config.AppSettings.Settings.AllKeys.Contains(name))
                {
                    config.AppSettings.Settings[name].Value = value;
                }
                else
                {
                    config.AppSettings.Settings.Add(name, value);
                }
                
                config.Save();
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

        static Regex NotDigitalChar = new Regex("[^0-9a-zA-Z]");
        public static string GetFormatName(this string a)
        {
            return NotDigitalChar.Replace(a, "");
        }
        #endregion

        /// <summary>
        /// 取得网站根目录的物理路径
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath;
        }
    }
}