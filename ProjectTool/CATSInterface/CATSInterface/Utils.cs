using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CATSInterface
{
    public static class Utils
    {
        private static object Sync = new object();
        private static Logger _log;

        

        public static Logger logger {
            get
            {
                if (_log == null)
                {
                    lock (Sync)
                    {
                        if (_log == null)
                        {
                            _log = new Logger();
                            
                        }
                    }
                }
                return _log;
            }
        }

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
                Utils.logger.LogInfo(string.Format("配置{0}获取出错！", name) + ex.Message);
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
                string exInfo = ex.Message;
                Utils.logger.LogInfo(string.Format("配置{0}保存出错！错误信息：{1}", name, ex.Message));
                return 0;
            }

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
                Utils.logger.LogInfo("Json序列化错误" + ex.Message);
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
                Utils.logger.LogInfo("Json序列化错误" + ex.Message);
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
                Utils.logger.LogInfo("Json序列化错误" + ex.Message);
                return new List<T>();
            }
        }
        #endregion



        internal static string GetSymbol(string market, string stockID)
        {
            return string.Format("{0}.{1}", stockID, market == "0" ? "SZ" : "SH");
        }

        internal static string GetStockID(string symbol)
        {
            return symbol.Replace(".SZ", "").Replace(".SH", "").Trim(); 
        }

        internal static int GetMarket(string symbol)
        {
            if (symbol.EndsWith(".SZ"))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
       
    }
}
