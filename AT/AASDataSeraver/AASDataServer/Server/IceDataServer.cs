using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ice;
using Microsoft.Practices.Unity;
using AASDataServer.Helper;
using AASDataServer.Service;
using AASDataServer.Server.IceService;
using AASDataServer.Manager;

namespace AASDataServer.Server
{
    public class IceDataServer : Application
    {
        private const string Title = "数据订阅服务";
        private Communicator _ic;
        private bool _isRunning;

        public bool IsRunning
        {
            get {
                return _isRunning;
            }
            set {
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
            IWindowLogger logger = UnityContainerHost.Container.Resolve<IWindowLogger>();
            try
            {
                string endpoint = string.Format("default:tcp -p {0}", SettingManager.GetInstance.DataServer.SubPort);
                _ic = communicator();
                ObjectAdapter adapter = _ic.createObjectAdapterWithEndpoints("AASDataServer", endpoint);

                IceDataService dataService = new IceDataService();
                adapter.add(dataService, _ic.stringToIdentity("AASDataServer/DataServant"));
                adapter.activate();
                _isRunning = true;
                logger.Warn(Title, "已启动！");
                _ic.waitForShutdown();
                if (interrupted())
                {
                    logger.Error(Title, "已中断！");
                    return 1;
                }
                logger.Warn(Title, "已停止！");
                _isRunning = false;
                return 0;
            }
            catch (System.Exception ex)
            {
                _isRunning = false;
                logger.Error(Title, "运行出错！", ex);
                return 1;
            }
        }
    }
}
