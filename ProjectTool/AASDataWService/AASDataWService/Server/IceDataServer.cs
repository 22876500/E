using AASDataWService.Common;
using AASDataWService.Logger;
using AASDataWService.Server.IceService;
using Ice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASDataWService.Server
{
    public class IceDataServer:Application
    {
        private const string Title = "数据订阅服务";
        private Communicator _ic;
        private bool _isRunning;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
            }
        }

        public Communicator Ic
        {
            get { return _ic; }
        }

        public override int run(string[] args)
        {
            shutdownOnInterrupt();
            try
            {
                string endpoint = string.Format("default:tcp -p {0}", ConfigMain.GetConfigValue(BaseCommon.CONFIGICEPORT));
                _ic = communicator();
                ObjectAdapter adapter = _ic.createObjectAdapterWithEndpoints("AASDataServer", endpoint);

                IceDataService dataService = new IceDataService();
                adapter.add(dataService, _ic.stringToIdentity("AASDataServer/DataServant"));
                adapter.activate();
                _isRunning = true;
                LogHelper.Instance.Info(Title + "已启动，端口：" + endpoint);
                _ic.waitForShutdown();
                if (interrupted())
                {
                    LogHelper.Instance.Info(Title + "已中断");
                    return 1;
                }
                LogHelper.Instance.Info(Title + "已停止");
                _isRunning = false;
                return 0;
            }
            catch (System.Exception ex)
            {
                _isRunning = false;
                LogHelper.Instance.Info(Title + "运行出错" + ex);
                return 1;
            }
        }
    }
}
