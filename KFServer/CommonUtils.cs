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

        public static byte GetCodeMarket(this string code)
        {
            //byte1 == 0 ? "深圳" : "上海";
            //00、200、300
            if (Regex.IsMatch(code, "^(00|03|1|200|3)"))
                return 0;
            else
                return 1;
        }

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


        public DateTime SendTime { get; set; }

        public string OrderID { get; set; }

        public string Sender { get; set; }

        public string ClientGUID { get; set; }

        /// <summary>
        /// 是否风控下单
        /// </summary>
        public bool IsRiskControl { get { return Sender == Trader; } }

        /// <summary>
        /// 是否已收到券商返回结果
        /// </summary>
        public bool IsReturnedResult { get; set; }


        public string IsTimeOutError { get; set; }
    }

}
