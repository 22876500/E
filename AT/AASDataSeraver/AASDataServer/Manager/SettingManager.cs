using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Deployment.Application;
using AASDataServer.Helper;
using RamGecTools;
using AASDataServer.Model.Setting;
using Newtonsoft.Json;
using Microsoft.Practices.Unity;

namespace AASDataServer.Manager
{
    public class SettingManager : IManager
    {

        public const string SERVER_FILE = "server.dat";

        public const string DATASERVER_SECTION = "DataServer";
        public const string REALTIME_ADAPTER_SECTION = "RealTimeData";
        public const string HISTORY_ADAPTER_SECTION = "HistoryData";

        private static SettingManager _instance;

        private Settings4Net _settings;

        private DataServerSetting _serverSetting;
        private HHDataAdapterSetting _realtimeAdapter;
        private HHDataAdapterSetting _historyAdapter;

        public static SettingManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UnityContainerHost.Container.Resolve<IManager>("SettingManager") as SettingManager;
                }
                return _instance;
            }
        }


        public DataServerSetting DataServer
        {
            get
            {
                return _serverSetting;
            }
        }

        public HHDataAdapterSetting RealTimeAdapterSetting
        {
            get {
                return _realtimeAdapter;
            }
        }

        public HHDataAdapterSetting HistoryAdapterSetting
        {
            get {
                return _historyAdapter;
            }
        }


        public SettingManager()
        {
            _settings = new Settings4Net();
            _serverSetting = new DataServerSetting();

            _realtimeAdapter = new HHDataAdapterSetting();
            _realtimeAdapter.IsHistory = false;
            _historyAdapter = new HHDataAdapterSetting();
            _historyAdapter.IsHistory = true;

            string path = GetSettingPath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string GetSettingPath()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                return ApplicationDeployment.CurrentDeployment.DataDirectory;
            }
            else
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"\config\";
            }
        }

        public int Load()
        {
            try
            {
                JsonSerializerSettings js = new JsonSerializerSettings();
                js.TypeNameHandling = TypeNameHandling.Auto;

                string serverFile = GetSettingPath() + SERVER_FILE;
                if (File.Exists(serverFile) == true)
                {
                    Settings4Net appSetting = new Settings4Net();
                    appSetting.Open(serverFile);
                    if (appSetting.IsLoaded == true)
                    {
                        _serverSetting = JsonConvert.DeserializeObject<DataServerSetting>(appSetting.Settings[DATASERVER_SECTION].ToString(), js);
                        _realtimeAdapter = JsonConvert.DeserializeObject<HHDataAdapterSetting>(appSetting.Settings[REALTIME_ADAPTER_SECTION].ToString(), js);
                        _historyAdapter = JsonConvert.DeserializeObject<HHDataAdapterSetting>(appSetting.Settings[HISTORY_ADAPTER_SECTION].ToString(), js);
                    }

                    _serverSetting.SubPort = AppSettingsHelper.getInt("DataServerIcePort",
                        _serverSetting.SubPort);
                    _serverSetting.PubPort = AppSettingsHelper.getInt("DataServerPubPort",
                        _serverSetting.PubPort);
                }
                return 0;
            }
            catch (Exception ex)
            {
                App.Logger.Error("软件配置加载失败！", ex);
                return 1;
            }
        }

        public int Save()
        {
            try
            {
                JsonSerializerSettings js = new JsonSerializerSettings();
                js.TypeNameHandling = TypeNameHandling.Auto;

                string serverFile = GetSettingPath() + SERVER_FILE;
                Settings4Net appSetting = new Settings4Net();
                appSetting.Settings[DATASERVER_SECTION] = JsonConvert.SerializeObject(_serverSetting, js);
                appSetting.Settings[REALTIME_ADAPTER_SECTION] = JsonConvert.SerializeObject(_realtimeAdapter, js);
                appSetting.Settings[HISTORY_ADAPTER_SECTION] = JsonConvert.SerializeObject(_historyAdapter, js);
                appSetting.Save(serverFile);

                return 0;
            }
            catch (Exception ex)
            {
                App.Logger.Error("软件配置保存失败！", ex);
                return 1;
            }
        }
    }
}
