using AASDataServer.Manager;
using AASDataServer.Model.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.ViewModel
{
    public class ServerSettingDataContext : AbstractDataContext
    {
        private DataServerSetting _serverSetting;
        private HHDataAdapterSetting _realtimeAdapterSetting;
        private HHDataAdapterSetting _historyAdapterSetting;

        public DataServerSetting ServerSetting
        {
            get {
                return _serverSetting;
            }
        }

        public HHDataAdapterSetting RealtimeAdapterSetting
        {
            get {
                return _realtimeAdapterSetting;
            }
        }

        public HHDataAdapterSetting HistoryAdapterSetting
        {
            get {
                return _historyAdapterSetting;
            }
        }

        public ServerSettingDataContext()
        {
            SettingManager.GetInstance.Load();
            _serverSetting = SettingManager.GetInstance.DataServer;
            _realtimeAdapterSetting = SettingManager.GetInstance.RealTimeAdapterSetting;
            _historyAdapterSetting = SettingManager.GetInstance.HistoryAdapterSetting;
        }

        public void Save()
        {
            SettingManager.GetInstance.Save();
        }

        public void Reload()
        {
            SettingManager.GetInstance.Load();
        }
    }
}
