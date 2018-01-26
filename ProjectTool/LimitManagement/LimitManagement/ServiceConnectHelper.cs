using LimitManagement.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace LimitManagement
{
    public class ServiceConnectHelper
    {
        public const string ConnectionKey = "x7V-5!9dC08w";
        static object Sync = new object();

        #region Server Info
        List<Entities.ServerInfo> _lstServerInfo = new List<Entities.ServerInfo>();
        public List<Entities.ServerInfo> ServerInfoList
        {
            get
            {
                if (_lstServerInfo.Count == 0)
                {
                    lock (Sync)
                    {
                        if (_lstServerInfo.Count == 0)
                        {
                            InitServerInfo();
                        }
                    }
                }
                return _lstServerInfo;
            }
        }

        private void InitServerInfo()
        {
            if (File.Exists(@"config\serverConfig.con"))
            {
                var lines = File.ReadAllLines(@"config\serverConfig.con", Encoding.UTF8);
                foreach (var lineItem in lines)
                {
                    var matchItem = Regex.Match(lineItem, "([^,]+),([0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3})([:][0-9]+){0,1}");
                    if (matchItem.Success)
                    {
                        var port = string.IsNullOrEmpty(matchItem.Groups[3].Value) ? "58000" : matchItem.Groups[3].Value.TrimStart(':');
                        var serverInfo = new Entities.ServerInfo();
                        serverInfo.Remark = matchItem.Groups[1].Value;
                        serverInfo.Ip = matchItem.Groups[2].Value;
                        serverInfo.Port = int.Parse(port);
                        _lstServerInfo.Add(serverInfo);
                    }
                }
            }
            else
            {
                if (!Directory.Exists(Utils.CurrentPath + "config"))
                    Directory.CreateDirectory(Utils.CurrentPath + "config");
                File.CreateText(Utils.CurrentPath + @"config\serverConfig.con");
            }
        }
        #endregion


        private static ServiceConnectHelper _instance;
        public static ServiceConnectHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        _instance = new ServiceConnectHelper();
                    }
                }
                return _instance;
            }
        }

        private ServiceConnectHelper()
        { }

        public void Start()
        {
            foreach (var item in ServerInfoList)
            {
                item.Start();
            }
            
        }

        public void Stop()
        {
            foreach (var item in ServerInfoList)
            {
                try
                {
                    item.Stop();
                }
                catch (Exception ex)
                {
                    Utils.Log("ServiceConnectHelper.Stop Exception: server " + item.Ip, ex);
                }
            }
        }


    }
}
