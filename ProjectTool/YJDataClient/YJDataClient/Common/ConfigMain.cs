using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YjDataClient.Common
{
    public class ConfigMain
    {
        public string strFileName;
        public string configName;
        public string configValue;

        public string ReadConfig(string configKey)
        {

            configValue = "";
            configValue = ConfigurationManager.AppSettings["" + configKey + ""];
            return configValue;
        }
        public void SetConfigName(string strConfigName)
        {
            configName = strConfigName;
            //获得配置文件的全路径
            GetFullPath();
        }
        public void GetFullPath()
        {
            //获得配置文件的全路径
            strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + configName;
        }
        /// <summary>
        /// 修改AppSettings中配置
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">相应值</param>
        public static bool SetConfigValue(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] != null)
                    config.AppSettings.Settings[key].Value = value;
                else
                    config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取AppSettings中某一节点值
        /// </summary>
        /// <param name="key"></param>
        public static string GetConfigValue(string key)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[key] != null)
                return config.AppSettings.Settings[key].Value;
            else

                return string.Empty;
        }

        public static void removeItem(string keyName)
        {
            //删除配置文件键为keyName的项  
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.AppSettings.Settings[keyName] != null)
                config.AppSettings.Settings.Remove(keyName);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        } 
    }
}
