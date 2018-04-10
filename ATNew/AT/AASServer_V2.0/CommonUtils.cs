using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AASServer
{
    public static class CommonUtils
    {

        public static bool UseOpenTdx = true;

        public static readonly List<string> HKLoginGroup = new List<string>() { "260500043078" };

        static ConcurrentQueue<OrderCacheEntity> _orderCacheQueue;

        /// <summary>
        /// 已下委托缓存
        /// </summary>
        public static ConcurrentQueue<OrderCacheEntity> OrderCacheQueue
        {
            get
            {
                if (_orderCacheQueue == null)
                {
                    _orderCacheQueue = new ConcurrentQueue<OrderCacheEntity>();
                }
                return _orderCacheQueue;
            }
        }

        public static List<OrderCacheEntity> Orders = new List<OrderCacheEntity>();

        #region Group Client
        static Dictionary<string, GroupConfig> _dictGroupIP;
        static Dictionary<string, GroupConfig> DictGroupIP
        {
            get
            {
                if (_dictGroupIP == null)
                {
                    lock (Sync)
                    {
                        System.Net.ServicePointManager.DefaultConnectionLimit = 512;
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
                                        var config = new GroupConfig() { ServerIP = groupInfo[1].Trim() };

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
                    
                }
                return _dictGroupIP;
            }
        }

        public static void ResetGroupConfig()
        {
            _dictGroupIP = null;
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

        static Dictionary<string, ServiceReference.GroupServiceClient> ClientInstanceDict = new Dictionary<string, ServiceReference.GroupServiceClient>();
        
        public static ServiceReference.GroupServiceClient GetGroupClient(string groupName)
        {
            if (DictGroupIP.ContainsKey(groupName))
            {
                var ipConfig = DictGroupIP[groupName];
                if (IsIP(ipConfig.ServerIP))
                {
                    EndpointIdentity ei = null;
                    //if (ipConfig.ServerIP.StartsWith("192."))
                    //{
                    //    ei = EndpointIdentity.CreateDnsIdentity("localhost");
                    //}
                    //else
                    //{
                    //    ei = EndpointIdentity.CreateDnsIdentity(ipConfig.ServerIP);
                    //}
                    ei = EndpointIdentity.CreateDnsIdentity(ipConfig.ServerIP);
                    var endpointAddress = new EndpointAddress(new Uri(ipConfig.EndPoint), ei);
                    var c = new ServiceReference.GroupServiceClient(ipConfig.Binding, endpointAddress);
                    return c;
                }
            }
            return null;
        } 
        #endregion

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
                Program.logger.LogInfo(string.Format("配置{0}获取出错！", name) + ex.Message);
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
                //Program.logger.LogInfo("配置{0}保存出错！错误信息：{1}", name, ex.Message);
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
                Program.logger.LogInfo("Json序列化错误" + ex.Message);
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
                Program.logger.LogInfo("Json序列化错误" + ex.Message);
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
                Program.logger.LogInfo("Json序列化错误" + ex.Message);
                return new List<T>();
            }
        }
        #endregion

        public static byte GetCodeMarket(this string code)
        {
            //byte1 == 0 ? "深圳" : "上海";
            //00、200、300
            if (Regex.IsMatch(code, "^(00|03|1|200|3)"))
                return 0;
            else
                return 1;
        }

        internal static string GetAyersReference()
        {
            return System.Guid.NewGuid().ToString();
        }

        public static System.Data.DataTable GetDataTable(string header, string[] content)
        {
            System.Data.DataTable DataTable1 = new System.Data.DataTable();
            try
            {
                if (header == null)
                    return null;

                string[] ColumnNames = header.Split(new char[] { '\t' });
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    if (DataTable1.Columns.IndexOf(ColumnNames[i]) == -1)
                    {
                        DataTable1.Columns.Add(ColumnNames[i]);
                    }
                    else
                    {
                        DataTable1.Columns.Add(ColumnNames[i] + "_" + i.ToString());
                    }
                }
                if (content != null && content.Length > 0)
                {
                    for (int i = 0; i < content.Length; i++)
                    {
                        string[] Cells = content[i].Split(new char[] { '\t' });
                        DataTable1.Rows.Add(Cells);
                    }
                }
                
            }
            catch (Exception ex) {
                Program.logger.LogInfoDetail("CommonUtils.GetDataTable Exception: Param:{0},{1}, Error:{2}", header, content == null ? "" : content.ToJson(), ex.Message);
            }
            
            return DataTable1;
        }
        public static string GetImsDateTimeString(string s)
        {
            if (s.Length == 14)
            {
                return string.Format("{0}-{1}-{2} {3}:{4}:{5}", s.Substring(0, 4), s.Substring(4, 2), s.Substring(6, 2), s.Substring(8, 2), s.Substring(10, 2), s.Substring(12, 2), s.Substring(14, 0));
            }
            return s;
        }
        public static DataTable ConvertListToDataTable(string[] lstResult)
        {
            DataTable dt = new DataTable("Result");
            try
            {
                if (lstResult.Length < 2) return null;
                string[] ColumnNames = lstResult[0].Split(new char[] { '\t' });
                for (int i = 0; i < ColumnNames.Length; i++)
                {
                    if (dt.Columns.IndexOf(ColumnNames[i]) == -1)
                    {
                        dt.Columns.Add(ColumnNames[i]);
                    }
                    else
                    {
                        dt.Columns.Add(ColumnNames[i] + "_" + i.ToString());
                    }
                }

                for (int i = 1; i < lstResult.Length; i++)
                {
                    dt.Rows.Add(lstResult[i].Split(new char[] { '\t' }));
                }

            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("CommonUtils.ConvertListToDataTable Exception: Param:{0},Error:{1}", lstResult.ToJson(), ex.Message);
            }

            return dt;
        }
        #region Mac
        public static string Mac
        {
            get
            {
                if (string.IsNullOrEmpty(_mac))
                {
                    _mac = GetMacAddress();
                }
                return _mac;
            }
        }
        static string _mac;
        private static string GetMacAddress()
        {
            var configMac = Program.appConfig.GetValue("LocalMac", "");
            try
            {
                string strMac = " ";
                ManagementClass ManagementClass1 = new ManagementClass("Win32_NetworkAdapterConfiguration");
                foreach (ManagementObject ManagementObject1 in ManagementClass1.GetInstances())
                {
                    if ((bool)ManagementObject1["IPEnabled"])
                    {
                        strMac = ManagementObject1["MacAddress"].ToString();
                        if (configMac == strMac)
                        {
                            break;
                        }
                    }
                }
                if (configMac != strMac)
                {
                    Program.appConfig.SetValue("LocalMac", strMac);
                }
                return strMac.ToString().Replace(":", "").ToUpper();
            }
            catch
            {
                return string.Empty;
            }
        } 
        #endregion

        static string _limitServiceGUID;
        public static string LimitServiceID
        {
            get
            {
                if (_limitServiceGUID == null)
                {
                    lock (Sync)
                    {
                        if (_limitServiceGUID == null)
                        {
                            _limitServiceGUID = System.Guid.NewGuid().ToString();
                        }
                    }
                }
                return _limitServiceGUID;
            }
        }

        private static object Sync = new object();

        public static bool IsTdxBuy(int category)
        {
            switch (category)
            {
                case 0://现金买入
                case 2://融资买入
                case 4://买券还款
                case 7://担保品买入
                    return true;
                case 1://现券卖出
                case 3://融券卖出
                case 5://卖券还款
                case 8://担保品卖出
                default:
                    return false;
            }
        }
    }



    public class OrderCacheEntity
    {
        /// <summary>
        /// 买卖方向
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 证券代码
        /// </summary>
        public string Zqdm { get; set; }

        public string ZqName { get; set; }

        /// <summary>
        /// 委托价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 委托数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// 交易员名称
        /// </summary>
        public string Trader { get; set; }

        /// <summary>
        /// 组合号
        /// </summary>
        public string GroupName { get; set; }

        public byte Market { get; set; }

        /// <summary>
        /// 下单前查询结果
        /// </summary>
        public string ResultBefore { get; set; }

        public DateTime SendTime { get; set; }

        public string OrderID { get; set; }

        public string OrderIDMatched { get; set; }

        public string Sender { get; set; }

        public string ClientGUID { get; set; }

        /// <summary>
        /// 是否风控下单
        /// </summary>
        public bool IsRiskControl { get { return Sender == Trader; } }

        /// <summary>
        /// 如为Ayers接口委托，则记录MsgNum
        /// </summary>
        public string MsgNum { get; set; }

        /// <summary>
        /// 是否已收到券商返回结果
        /// </summary>
        public bool IsReturnedResult { get; set; }

        //public string Status { get; set; }

        public string IsTimeOutError { get; set; }
    }

    public class GroupConfig
    {
        public enum BindingType { NetTcpBindg = 0, WSHttpBinding = 1, BasicHttpBinding = 2 }

        public string GroupName { get; set; }

        public string ServerIP { get; set; }

        public string EndPoint { 
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
            get {
                if(_binding == null)
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



    public class AyersCancelRecord
    {
        public string OrderID { get; set; }

        public string Sender { get; set; }

        public string MsgNum { get; set; }
    }
}
