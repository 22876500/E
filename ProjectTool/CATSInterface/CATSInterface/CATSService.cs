using Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace CATSInterface
{
    public class CATSService
    {
        public bool IsStarted = false;

        ZContext context;
        Thread RepThread = null;
        Thread PubThread = null;
        List<string> MacList = null;
        ConcurrentDictionary<string, string> ClientIDMacDict = null;

        private static CATSService _instance;
        public static CATSService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new CATSService();
                        }
                    }
                }
                return _instance;
            }
        }

        private static object Sync = new object();
        private CATSService()
        {
            context = ZContext.Create();
        }

        public void StartService()
        {
            if (!IsStarted)
            {
                lock (Sync)
                {
                    if (!IsStarted)
                    {
                        MacList = new List<string>();
                        ClientIDMacDict = new ConcurrentDictionary<string, string>();
                        RepThread = new Thread(new ThreadStart(RepMain)) { IsBackground = true };
                        RepThread.Start();

                        PubThread = new Thread(new ThreadStart(PubMain)) { IsBackground = true };
                        PubThread.Start();
                        IsStarted = true;
                    }
                }
            }
        }

        public void StopService()
        {
            if (IsStarted)
            {
                lock (Sync)
                {
                    if (IsStarted)
                    {
                        try
                        {
                            RepThread.Abort();
                            PubThread.Abort();
                        }
                        catch (Exception) { }
                    }
                }
            }
        }

        private void PubMain()
        {
            string PubPort = Utils.GetConfig("PUB_PORT");
            using (var socket = new ZSocket(context, ZSocketType.PUB))
            {
                try
                {
                    socket.Bind(string.Format("tcp://*:{0}", PubPort));
                    Utils.logger.LogInfo("ZeroMQ Pub接口正常开启，端口：{0}。", PubPort);
                }
                catch (Exception ex)
                {
                    Utils.logger.LogInfo("PubMain Binding Exception:\r\n  bind info: {0}\r\n  Exception Message: {1}\r\n  StackTrace:{2}", PubPort, ex.Message, ex.StackTrace);
                    return;
                }
                Dictionary<string, DateTime> dictAllRefreshTime = new Dictionary<string, DateTime>();
                Dictionary<string, List<StandardOrderEntity>> dictMacOrderAll = new Dictionary<string, List<StandardOrderEntity>>();

                while (true)
                {
                    StringBuilder sb = new StringBuilder(64);
                    
                    try
                    {
                        PubHeartBeat(socket);

                        Dictionary<string, List<StandardOrderEntity>> dict = new Dictionary<string, List<StandardOrderEntity>>();
                        foreach (var item in CATSAdapter.Instance.OrderChange)
                        {
                            if (!ClientIDMacDict.ContainsKey(item.Key))
                            {
                                continue;
                            }

                            var mac = ClientIDMacDict[item.Key];
                            var orderItem = CATSAdapter.Instance.StandardOrderDict[item.Key];

                            if (item.Value)
                            {
                                if (dict.ContainsKey(mac) && dict[mac] != null)
                                    dict[mac].Add(orderItem);
                                else
                                    dict.Add(mac, new List<StandardOrderEntity>() { orderItem });

                                CATSAdapter.Instance.OrderChange[item.Key] = false;
                            }
                            if (!dictMacOrderAll.ContainsKey(mac))
                            {
                                dictMacOrderAll.Add(mac, new List<StandardOrderEntity>() { orderItem });
                            }
                            else if (!dictMacOrderAll[mac].Contains(orderItem))
                            {
                                dictMacOrderAll[mac].Add(orderItem);
                            }
                        }
                        
                        foreach (var item in dict)
                        {
                            var message = new ZMessage();
                            message.Add(new ZFrame(item.Key + ".raw"));
                            message.Add(new ZFrame(item.Value.ToJson()));
                            socket.Send(message);
                        }

                        foreach (var item in dictMacOrderAll)
                        {
                            //循环所有mac，如超过设置时间，则发送所有订单信息。
                            if (!dictAllRefreshTime.ContainsKey(item.Key) || (DateTime.Now - dictAllRefreshTime[item.Key]).Seconds >= 60)
                            {
                                var message = new ZMessage();
                                message.Add(new ZFrame(item.Key + ".raw"));
                                message.Add(new ZFrame(item.Value.ToJson()));

                                if (!dictAllRefreshTime.ContainsKey(item.Key))
                                    dictAllRefreshTime.Add(item.Key, DateTime.Now);
                                else
                                    dictAllRefreshTime[item.Key] = DateTime.Now;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Utils.logger.LogInfo("pub线程异常： Message:{1}", ex.Message);
                    }
                    Thread.Sleep(100);
                }
            }
        }

        DateTime lastAliveTime = DateTime.MinValue;
        private void PubHeartBeat(ZSocket socket)
        {
            if ((DateTime.Now - lastAliveTime).TotalSeconds >= 5)
            {
                try
                {
                    ZMessage msg = new ZMessage();
                    msg.Add(new ZFrame("HEARTBEAT.raw"));
                    msg.Add(new ZFrame("HEARTBEAT"));
                    socket.Send(msg);
                    lastAliveTime = DateTime.Now;
                }
                catch (Exception ex)
                {
                    Utils.logger.LogInfo("PubHeartBeat Exception: {0}", ex.Message);
                }
            }
           
        }

        private void RepMain()
        {
            string RepPort = Utils.GetConfig("REP_PORT");
            using (ZSocket socket = new ZSocket(context, ZSocketType.REP))
            {
                try
                {
                    socket.Bind(string.Format("tcp://*:{0}", RepPort));
                    Utils.logger.LogInfo("ZeroMQ Replay接口正常开启，端口：{0}。", RepPort);
                }
                catch (Exception)
                {
                    Utils.logger.LogInfo("ZeroMQ Replay接口正常开启，端口：{0}。", RepPort);
                    return;
                }
                while (true)
                {
                    var msg = socket.ReceiveMessage();
                    ZMessage zMsgReply = new ZMessage();
                    string message = null;

                    try
                    {
                        message = msg[0].ReadString();
                        var result = SwitchMessage(message);
                        zMsgReply.Add(new ZFrame(result));
                    }
                    catch (Exception ex)
                    {
                        string ExceptionInfo = string.Format("RepMain Exception, Message:{0}", ex.Message);
                        Utils.logger.LogInfo(ExceptionInfo);
                        zMsgReply.Add(new ZFrame(ExceptionInfo));
                    }
                    socket.Send(zMsgReply);
                    zMsgReply.Dispose();
                }
            }
        }

        #region Reply Message
        private string SwitchMessage(string message)
        {
            var infos = message.Split('|');
            string type = infos[0];
            switch (type)
            {
                case "0":
                    return Connect(infos);
                case "1":
                    return SendOrder(infos);
                case "2":
                    return CancelOrder(infos);
                case "3"://查询特定订单状态
                    return QueryOrder(infos);
                case "4"://查询所有订单信息
                    return QueryAllOrder(infos);
                case "5":
                    return RefreshOrderRelation(infos);
                case "6":
                    return QueryOrderByClientID(infos);
                case "10":
                    return KeepAlive(infos);
                default:

                    return "";
            }
        }


        

        private string Connect(string[] infos)
        {
            if (Regex.IsMatch(infos[1], "([0-9a-fA-F]{2})(([0-9a-fA-F]{2}){5})$"))
            {
                if (MacList.Contains(infos[1]))
                {
                    return "1|已存在mac";
                }
                else
                {
                    MacList.Add(infos[1]);
                    return "1";
                }
            }
            else
            {
                Utils.logger.LogInfo("异常Mac记录{0}");
                if (MacList.Contains(infos[1]))
                {
                    return "1|已存在mac";
                }
                else
                {
                    MacList.Add(infos[1]);
                    return "0";
                }
            }
        }

        private string SendOrder(string[] infos)
        {
            string returnValue;
            if (MacList.Contains(infos[1]))
            {
                if (!IsSendOrderTimeFit())
                {
                    returnValue = string.Format("下单时限为9:00-15:00, 当前时间{0}超出下单时限", DateTime.Now);
                }
                else
                {
                    
                    string clientID;
                    if (ConfigData.CHECK_OPPO)
                    {
                        string errInfo;
                        bool existOppositeOrder = CATSAdapter.Instance.CheckOppositeOrder(infos[2], infos[4], infos[5], infos[7], out errInfo);
                        if (existOppositeOrder)
                        {
                            return errInfo;
                        }
                    }
                    Utils.logger.LogInfo("开始下单操作：");
                    var msg = CATSAdapter.Instance.AddOrder(infos[2], infos[3], infos[4], infos[5], infos[6], infos[7], out clientID);
                    returnValue = msg;
                    if (clientID == msg)
                    {
                        ClientIDMacDict[clientID] = infos[1];
                    }
                    if (CATSAdapter.Instance.StandardOrderDict.ContainsKey(clientID))
                    {
                        returnValue = CATSAdapter.Instance.StandardOrderDict[clientID].OrderNo;
                    }
                    Utils.logger.LogInfo("下单结果:{0}, ClientID: {1}", msg, clientID);
                }
                
            }
            else
            {
                returnValue = "未记录MAC不能下单撤单操作！";
            }
            return returnValue;
        }

        private string CancelOrder(string[] infos)
        {
            //撤单
            if (MacList.Contains(infos[1]))
            {
                var result = CATSAdapter.Instance.CancelOrder(infos[2], infos[3]);
                Utils.logger.LogInfo("Cancel Order {0}, return value {1}", infos[3], result);
                return result.ToString();
            }
            else
            {
                return "未记录MAC不能进行下单撤单操作！";
            }
        }

        private string QueryOrder(string[] infos)
        {
            if (MacList.Contains(infos[1]))
            {
                return CATSAdapter.Instance.QueryOrderItem(infos[2]);
            }
            else
            {
                return "未记录MAC，请登录后操作！";
            }
        }

        private string QueryAllOrder(string[] infos)
        {
            if (MacList.Contains(infos[1]))
            {
                var clientIDList = ClientIDMacDict.Where(_ => _.Value == infos[1]).Select(_ => _.Key).ToList();
                return CATSAdapter.Instance.QueryOrdersByClientID(clientIDList);
            }
            else
            {
                return "未记录MAC，请登录后操作！";
            }
        }

        private string RefreshOrderRelation(string[] infos)
        {
            var lstOrderNo = infos[2].FromJson<List<string>>();
            if (lstOrderNo != null && lstOrderNo.Count > 0)
            {
                var Orders = CATSAdapter.Instance.QueryOrdersByOrderNo(lstOrderNo);
                foreach (var orderItem in Orders)
                {
                    if (!ClientIDMacDict.ContainsKey(orderItem.ClientID))
                    {
                        ClientIDMacDict[orderItem.ClientID] = infos[1];
                    }
                    else if (ClientIDMacDict[orderItem.ClientID] != infos[1])
                    {
                        Utils.logger.LogInfo("异常状态，记录的下单mac为{0}, 请求此订单信息的mac为{1},订单编号{2},ClientID{3}", ClientIDMacDict[orderItem.ClientID], infos[1], orderItem.OrderNo, orderItem.ClientID);
                        ClientIDMacDict[orderItem.ClientID] = infos[1];
                    }

                }
                return Orders.ToJson();
            }
            else
            {
                return string.Empty;
            }
        }

        private string QueryOrderByClientID(string[] infos)
        {
            var lstClientID = infos[2].FromJson<List<string>>();
            if (lstClientID != null && lstClientID.Count >= 0)
            {
                return CATSAdapter.Instance.QueryOrdersByClientID(lstClientID);
            }
            else
            {
                return string.Empty;
            }
        }

        private string KeepAlive(string[] infos)
        {
            if (MacList.Contains(infos[1]))
            {
                return "1";
            }
            else
            {
                MacList.Add(infos[1]);
                return "0";
            }
        }
        #endregion

        const int StartSendOrderHour = 9;
        const int EndSendOrderHour = 15;
        public static bool IsSendOrderTimeFit()
        {
            if (DateTime.Now.Hour >= StartSendOrderHour && DateTime.Now.Hour <= EndSendOrderHour)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
