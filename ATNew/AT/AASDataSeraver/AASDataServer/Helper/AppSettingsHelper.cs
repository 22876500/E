using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.Helper
{
    public class AppSettingsHelper
    {
        public static bool getBoolean(string key, bool retval = false)
        {
            try
            {
                AppSettingsReader reader = new AppSettingsReader();
                return (bool)reader.GetValue(key, typeof(bool));
            }
            catch (Exception ex)
            {
                App.Logger.Error("获取配置参数" + key + "失败！", ex);
                return retval;
            }
        }

        public static short getShort(string key, short retval = 0)
        {
            try
            {
                AppSettingsReader reader = new AppSettingsReader();
                return (short)reader.GetValue(key, typeof(short));
            }
            catch (Exception ex)
            {
                App.Logger.Error("获取配置参数" + key + "失败！", ex);
                return retval;
            }
        }

        public static int getInt(string key, int retval = 0)
        {
            try
            {
                AppSettingsReader reader = new AppSettingsReader();
                return (int)reader.GetValue(key, typeof(int));
            }
            catch (Exception ex)
            {
                App.Logger.Error("获取配置参数" + key + "失败！", ex);
                return retval;
            }
        }

        public static string getString(string key, string retval = "")
        {
            try
            {
                AppSettingsReader reader = new AppSettingsReader();
                return (string)reader.GetValue(key, typeof(string));
            }
            catch (Exception ex)
            {
                App.Logger.Error("获取配置参数" + key + "失败！", ex);
                return retval;
            }
        }

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
                App.Logger.Error("配置{0}保存异常！" + name + "失败！", ex);
                return 0;
            }

        }
    }
}
