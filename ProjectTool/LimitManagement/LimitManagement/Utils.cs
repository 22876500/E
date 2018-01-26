using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LimitManagement
{
    public static class Utils
    {
        public const string Version  = "1.0";

        public const string UpdateDate = "2017/10/21";

        public static string CurrentPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
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

        #region Json
        public static string ToJson(this object o)
        {
            try
            {
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception ex)
            {
                Log("Json序列化错误" + ex.Message);
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
                Log("Json序列化错误" + ex.Message);
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
                Log("Json序列化错误" + ex.Message);
                return new List<T>();
            }
        }
        #endregion

        #region Log
        static Logger log = new Logger();
        public static void Log(string msg)
        {
            log.LogInfo(msg);
        }

        public static void Log(string msg, Exception ex)
        {
            log.LogErr(msg, ex);
        }

        public static void LogFormat(string msg, params object[] param)
        {
            log.LogRunning(msg, param);
        }
        #endregion
    }
}
