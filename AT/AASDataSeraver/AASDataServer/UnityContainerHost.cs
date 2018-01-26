using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AASDataServer.Service;
using AASDataServer.Server;
using AASDataServer.Manager;

namespace AASDataServer
{
    public class UnityContainerHost
    {
        private static readonly IUnityContainer _container;

        static UnityContainerHost()
        {
            _container = new UnityContainer();
            RegisterTypes(_container);
        }

        public static IUnityContainer Container
        {
            get { return _container; }
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            //注册窗口日志
            container.RegisterType<IWindowLogger, WindowLogger>(new ContainerControlledLifetimeManager());
            //注册数据源服务
            container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());
            //注册数据发布服务
            container.RegisterType<IPubService, PubService>(new ContainerControlledLifetimeManager());

            SettingManager sm = new SettingManager();
            sm.Load();
            container.RegisterInstance<IManager>("SettingManager", sm, new ContainerControlledLifetimeManager());
            StockCodeManager scm = new StockCodeManager();
            container.RegisterInstance<IManager>("StockCodeManager", scm, new ContainerControlledLifetimeManager());
        }
    }
}
