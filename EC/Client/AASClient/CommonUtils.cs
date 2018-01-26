using DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AASClient
{
    public static class CommonUtils
    {

        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string ToJSON(this object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T FromJSON<T>(this string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> ListFromJSON<T>(this string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }

        public static bool IsCode(string code)
        {
            return Regex.IsMatch(code, @"^\d{6}$");
        }

        public static string GetCode(string code)
        {
            return Regex.Match(code, "[0-9]{6}").Value;
        }

        public static string[] GetCodes(string codes)
        {
            return Regex.Split(codes, "[^0-9]+");
        }
        

        public static string GetMacAddress()
        {
            try
            {
                string MAC = " ";
                ManagementClass ManagementClass1 = new ManagementClass("Win32_NetworkAdapterConfiguration");
                foreach (ManagementObject ManagementObject1 in ManagementClass1.GetInstances())
                {
                    if ((bool)ManagementObject1["IPEnabled"])
                    {
                        MAC = ManagementObject1["MacAddress"].ToString();
                    }
                }
                return MAC.ToString().Replace(":", "").ToUpper();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetCpuID()
        {
            try
            {
                //获取CPU序列号代码 
                string cpuInfo = "";//cpu序列号 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        public static List<string> GetIP()
        {
            var name = Dns.GetHostName();
            var addrs = Dns.GetHostAddresses(name);
            List<string> lstAddr = new List<string>();
            foreach (var element in addrs)
            {
                lstAddr.Add(element.ToString());
            }
            return lstAddr;
        }

        public static string CompareString(this Model.CompareType t)
        {
            switch (t)
            {
                case AASClient.Model.CompareType.More:
                    return ">";
                case AASClient.Model.CompareType.MoreOrEqual:
                    return ">=";
                case AASClient.Model.CompareType.Less:
                    return "<";
                case AASClient.Model.CompareType.LessOrEqual:
                    return "<=";
                default:
                    return null;
            }
        }

        public static string CalculateString(this Model.CalculateType c)
        {
            switch (c)
            {
                case AASClient.Model.CalculateType.Add:
                    return "+";
                case AASClient.Model.CalculateType.Sub:
                    return "-";
                case AASClient.Model.CalculateType.Mul:
                    return "*";
                case AASClient.Model.CalculateType.Div:
                    return "/";
                default:
                    return null;
            }
        }

        public static decimal Calculate(this Model.CalculateType c, decimal c1, decimal c2)
        {
            switch (c)
            {
                case AASClient.Model.CalculateType.Add:
                    return c1 + c2;
                case AASClient.Model.CalculateType.Sub:
                    return c1 - c2;
                case AASClient.Model.CalculateType.Mul:
                    return c1 * c2;
                case AASClient.Model.CalculateType.Div:
                    return c1 / c2;
                default:
                    //return decimal.Zero;
                    throw new Exception("未定义的计算方式");
            }
        }

        static readonly string Number_Zh_Cn = "一二三四五六七八九十";
        public static char Number_cn(int i)
        {
            if (i > -1 && i < 10)
            {
                return Number_Zh_Cn[i];
            }
            return ' ';
        }

        public static bool PingIPAddress(string ip)
        {
            bool online = false;
            int timeout = 120;
            Ping ping = new Ping();
            PingReply pingReply = ping.Send(ip, timeout);
            if (pingReply.Status == IPStatus.Success)
            {
                online = true;
            }
            return online;
        }

        public static bool TelnetPort(string ip, int port)
        {
            bool isOpen = false;

            try
            {
                IPAddress ip1 = IPAddress.Parse(ip);
                IPEndPoint ipend = new IPEndPoint(ip1, port);
                TimeOutSocket.Connect(ipend, 2000);
                if (TimeOutSocket.IsConnectionSuccessful)
                {
                    isOpen = true;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return isOpen;
        }
    }

    class TimeOutSocket
    {
        public static bool IsConnectionSuccessful = false;
        private static Exception socketexception;
        private static ManualResetEvent TimeoutObject = new ManualResetEvent(false);

        public static TcpClient Connect(IPEndPoint remoteEndPoint, int timeoutMSec)
        {
            TimeoutObject.Reset();
            socketexception = null;

            string serverip = Convert.ToString(remoteEndPoint.Address);
            int serverport = remoteEndPoint.Port;
            TcpClient tcpclient = new TcpClient();

            tcpclient.BeginConnect(serverip, serverport,
                new AsyncCallback(CallBackMethod), tcpclient);

            if (TimeoutObject.WaitOne(timeoutMSec, false))
            {
                if (IsConnectionSuccessful)
                {
                    return tcpclient;
                }
                else
                {
                    throw socketexception;
                }
            }
            else
            {
                tcpclient.Close();
                //throw new TimeoutException("TimeOut Exception");
                return null;
            }
        }
        private static void CallBackMethod(IAsyncResult asyncresult)
        {
            try
            {
                IsConnectionSuccessful = false;
                TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

                if (tcpclient.Client != null)
                {
                    tcpclient.EndConnect(asyncresult);
                    IsConnectionSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
        }
    }
}
