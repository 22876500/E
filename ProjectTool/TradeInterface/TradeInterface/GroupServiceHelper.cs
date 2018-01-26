using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TradeInterface
{
    public class GroupServiceHelper
    {
        #region Group Client
        static Dictionary<string, GroupConfig> _dictGroupIP;
        static Dictionary<string, GroupConfig> DictGroupIP
        {
            get
            {
                if (_dictGroupIP == null)
                {
                    _dictGroupIP = new Dictionary<string, GroupConfig>();
                    string groupIPFile = "组合号接口IP.txt";
                    if (File.Exists(groupIPFile))
                    {
                        var arr = File.ReadAllLines(groupIPFile);
                        foreach (var item in arr)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                var groupInfo = item.Split(new char[] { '|', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                if (groupInfo.Length >= 2)
                                {
                                    var groupName = groupInfo[0].Trim();
                                    var ip = groupInfo[1].Trim();
                                    var config = new GroupConfig() { ServerIP = groupInfo[1].Trim(), GroupName = groupName };

                                    if (groupInfo.Length == 3)
                                    {
                                        var bindTypeInfo = groupInfo[2].ToLower();
                                        if (bindTypeInfo.Contains("tcp"))
                                        {
                                            config.ConnectType = GroupConfig.BindingType.NetTcpBindg;
                                        }
                                        else if (bindTypeInfo.Contains("basic"))
                                        {
                                            config.ConnectType = GroupConfig.BindingType.BasicHttpBinding;
                                        }
                                        else
                                        {
                                            config.ConnectType = GroupConfig.BindingType.WSHttpBinding;
                                        }
                                    }
                                    else
                                    {
                                        config.ConnectType = GroupConfig.BindingType.WSHttpBinding;
                                    }
                                    if (!_dictGroupIP.ContainsKey(groupName))
                                    {
                                        _dictGroupIP.Add(groupName, config);
                                    }
                                }

                            }
                        }
                    }
                }
                return _dictGroupIP;
            }
        }

        public static GroupConfig GetGroupConfig(string group)
        {
            if (DictGroupIP.ContainsKey(group))
            {
                return DictGroupIP[group];
            }
            else
            {
                return null;
            }
        }

        public static bool ExistsGroup(string GroupName)
        {
            return DictGroupIP.ContainsKey(GroupName);
        }

        public static bool IsIP(string s)
        {
            return Regex.IsMatch(s, "([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.]([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.]([0-9]|[1-9][0-9]|1[0-9][0-9]|2([0-4][0-9]|5[0-5]))[.](2([0-4][0-9]|5[0-5])|1[0-9][0-9]|[1-9][0-9]|[0-9])");
        }

        static Dictionary<string, GroupServiceReference.GroupServiceClient> ClientInstanceDict = new Dictionary<string, GroupServiceReference.GroupServiceClient>();

        public static GroupServiceReference.GroupServiceClient GetGroupClient(string groupName)
        {
            if (DictGroupIP.ContainsKey(groupName))
            {
                var ipConfig = DictGroupIP[groupName];
                if (IsIP(ipConfig.ServerIP))
                {
                    EndpointIdentity ei = null;
                    ei = EndpointIdentity.CreateDnsIdentity(ipConfig.ServerIP);
                    var endpointAddress = new EndpointAddress(new Uri(ipConfig.EndPoint), ei);
                    var c = new GroupServiceReference.GroupServiceClient(ipConfig.Binding, endpointAddress);
                    return c;
                }
            }
            return null;
        }
        #endregion
    }

    public class GroupConfig
    {
        public enum BindingType { NetTcpBindg = 0, WSHttpBinding = 1, BasicHttpBinding = 2 }

        public string GroupName { get; set; }

        public string ServerIP { get; set; }

        public string EndPoint
        {
            get
            {
                if (!string.IsNullOrEmpty(ServerIP))
                {
                    if (ConnectType == BindingType.NetTcpBindg)
                    {
                        return string.Format("net.tcp://{0}/", ServerIP);
                    }
                    else
                    {
                        return string.Format("http://{0}/", ServerIP);
                    }
                }
                return null;
            }
        }

        public BindingType ConnectType { get; set; }

        private System.ServiceModel.Channels.Binding _binding;
        public System.ServiceModel.Channels.Binding Binding
        {
            get
            {
                if (_binding == null)
                {
                    if (ConnectType == GroupConfig.BindingType.NetTcpBindg)
                    {
                        var tcpBinding = new NetTcpBinding(SecurityMode.Message);
                        tcpBinding.MaxReceivedMessageSize = 2147483647;
                        tcpBinding.ReceiveTimeout = new TimeSpan(0, 0, 10);
                        _binding = tcpBinding;
                    }
                    else if (ConnectType == BindingType.WSHttpBinding)
                    {
                        var httpBinding = new WSHttpBinding(SecurityMode.None);
                        httpBinding.MaxReceivedMessageSize = 2147483647;
                        httpBinding.ReceiveTimeout = new TimeSpan(0, 0, 10);
                        httpBinding.UseDefaultWebProxy = false;
                        _binding = httpBinding;
                    }
                    else
                    {
                        var basicHttpBinding = new BasicHttpBinding();
                        basicHttpBinding.MaxReceivedMessageSize = 2147483647;
                        basicHttpBinding.ReceiveTimeout = new TimeSpan(0, 0, 10);
                        basicHttpBinding.UseDefaultWebProxy = false;
                        _binding = basicHttpBinding;
                    }
                }
                return _binding;
            }
        }
    }
}
