using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;
using System.Collections.Concurrent;

namespace AASServer
{
    public class CATSAdapter
    {
        bool isServerConnected = false;
        public bool Connected { get { return isServerConnected; } }
        public string HeartBreakResult { get; set; }

        ConcurrentBag<List<CATSEntity.StandardOrderEntity>> bagOrdersChangeList = new ConcurrentBag<List<CATSEntity.StandardOrderEntity>>();
        public Action<List<CATSEntity.StandardOrderEntity>> OnOrdersChanged;

        object Sync = new object();
        string group;
        ZContext context;
        ZSocket reqSocket;
        Thread SubThread;
        Thread RefreshOrdersThread;


        static string configReqPort
        {
            get
            {
                var configPort = CommonUtils.GetConfig("CATS_REQ_PORT");
                if (!string.IsNullOrEmpty(configPort))
                {
                    return configPort;
                }
                return null;
            }
        }

        static string configPubPort
        {
            get
            {
                var configPort = CommonUtils.GetConfig("CATS_PUB_PORT");
                if (!string.IsNullOrEmpty(configPort))
                {
                    return configPort;
                }
                return null;
            }
        }

        public CATSAdapter(string groupName)
        {
            group = groupName;
            context = ZContext.Create();

            var ReqThread = new Thread(new ThreadStart(ReqMain)) { IsBackground = true };
            ReqThread.Start();

            SubThread = new Thread(new ThreadStart(SubMain)) { IsBackground = true };
            SubThread.Start();

            RefreshOrdersThread = new Thread(new ThreadStart(RefreshOrdersDataMain)) { IsBackground = true };
            RefreshOrdersThread.Start();
        }



        private void ReqMain()
        {
            while (true)
            {
                if (!isServerConnected)
                {
                    var config = CommonUtils.GetGroupConfig(group);
                    if (config != null)
                    {
                        if (reqSocket == null)
                        {
                            reqSocket = new ZSocket(context, ZSocketType.REQ);
                        }
                        reqSocket.Connect(string.Format("tcp://{0}:{1}", config.ServerIP, configReqPort));
                        try
                        {
                            if (Login())
                            {
                                InitHistoryOrder();
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.logger.LogInfoDetail("CATSAdapter.ReqMain Connect Exception, 组合号 {0}, IP {1}, Message {2}", group, config.ServerIP, ex.Message);
                            Thread.Sleep(1000);
                        }
                    }
                    else
                    {
                        Program.logger.LogInfoDetail("CATSAdapter.ReqMain config Not Found, 组合号：{0}", group);
                        Thread.Sleep(1500);
                    }
                }
                else
                {
                    //保持心跳，如果断开则将isServerConnected 置为false;
                    KeepAlive();
                    Thread.Sleep(500);
                }
            }
        }

        DateTime dtKeepAlivePoint = DateTime.Now;
        private void KeepAlive()
        {
            string sendMsgFormat = string.Format("10|{0}", CommonUtils.Mac);

            ZMessage msg = new ZMessage();
            msg.Add(new ZFrame(sendMsgFormat));
            lock (Sync)
            {
                try
                {
                    reqSocket.Send(msg);
                    var msgResult = reqSocket.ReceiveMessage();
                    var strResult = msgResult[0].ReadString();
                    if (strResult == "1")
                    {
                        HeartBreakResult = (DateTime.Now - dtKeepAlivePoint).TotalSeconds.ToString();
                        dtKeepAlivePoint = DateTime.Now;
                        return;
                    }
                    else
                    {
                        HeartBreakResult = strResult;
                        Program.logger.LogInfoDetail("心跳连接异常信息！返回结果{0}", strResult);
                    }
                }
                catch
                {
                    //this.isServerConnected = false;
                }
            }
            
        }

        public bool Login()
        {
            ZMessage connectMsg = new ZMessage();
            connectMsg.Add(new ZFrame("0|" + CommonUtils.Mac));
            lock (Sync)
            {
                try
                {
                    reqSocket.Send(connectMsg);
                    var msgResult = reqSocket.ReceiveMessage();
                    isServerConnected = true;
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("CATSAdapter.Login Exception, {0}", ex.Message);
                }
            }
            return true;
        }

        private void InitHistoryOrder()
        {
            ZMessage connectMsg = new ZMessage();
            connectMsg.Add(new ZFrame("4|" + CommonUtils.Mac));
            lock (Sync)
            {
                try
                {
                    reqSocket.Send(connectMsg);
                    var msgResult = reqSocket.ReceiveMessage();

                    var list = msgResult[0].ReadString().FromJson<List<CATSEntity.StandardOrderEntity>>();
                    if (list.Count > 0)
                    {
                        Program.logger.LogInfo("CATS历史委托总数:{0}", list.Count);
                        list = list.Where(_ => _.OrderTime >= DateTime.Today).ToList();
                    }
                    else
                    {
                        Program.logger.LogInfo("CATS接口历史委托为空");
                    }
                    bagOrdersChangeList.Add(list);
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("CATSAdapter.InitHistoryOrder Exception, {0}", ex.Message);
                }
            }
        }

        public void GetRecordedOrder(string account, List<string> lstOrder)
        {
            lock (Sync)
            {
                string sendMsgFormat = string.Format("5|{0}|{1}", account, lstOrder.ToJson());

                try
                {
                    ZMessage msg = new ZMessage();
                    msg.Add(new ZFrame(sendMsgFormat));
                    reqSocket.Send(msg);
                    var msgResult = reqSocket.ReceiveMessage();
                    var strResult = msgResult[0].ReadString();
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        var list = strResult.FromJson<List<CATSEntity.StandardOrderEntity>>();
                        if (list != null && list.Count > 0 && bagOrdersChangeList != null)
                        {
                            bagOrdersChangeList.Add(list);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("CATSAdapter.GetRecordedOrder Exception : {0}", ex.Message);
                }
            }
        }

        public void GetOrdersByClientID(List<string> lstOrderClientID)
        {
            lock (Sync)
            {
                string sendMsgFormat = string.Format("6|{0}|{1}", CommonUtils.Mac, lstOrderClientID.ToJson());

                try
                {
                    ZMessage msg = new ZMessage();
                    msg.Add(new ZFrame(sendMsgFormat));
                    reqSocket.Send(msg);
                    var msgResult = reqSocket.ReceiveMessage();
                    var strResult = msgResult[0].ReadString();
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        var list = strResult.FromJson<List<CATSEntity.StandardOrderEntity>>();
                        if (list != null)
                        {
                            if (list.Count > 0 && bagOrdersChangeList != null)
                            {
                                bagOrdersChangeList.Add(list);
                            }
                            else
                            {
                                Program.logger.LogInfoDetail("GetOrdersByClientID， param {0}, result {1}", lstOrderClientID.ToJson(), strResult);
                            }
                        }
                        else
                        {
                            Program.logger.LogInfoDetail("CATSAdapter.GetOrdersByClientID Special Result : {0}", strResult);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("CATSAdapter.GetRecordedOrder Exception : {0}", ex.Message);
                }
            }
        }

        DateTime _lastPubReceiveTime = DateTime.MinValue;
        private void SubMain()
        {
            using (var socket = new ZSocket(context, ZSocketType.SUB))
            {
                var config = CommonUtils.GetGroupConfig(group);
                socket.Connect(string.Format("tcp://{0}:{1}", config.ServerIP, configPubPort));

                socket.Subscribe(CommonUtils.Mac);
                socket.Subscribe("HEARTBEAT");
                while (true)
                {
                    var msgReceive = socket.ReceiveMessage();
                    if (msgReceive != null)
                    {
                        try
                        {
                            var ordersResult = msgReceive[1].ReadString();
                            if (ordersResult != "HEARTBEAT")
                            {
                                //如何处理消息信息，最好有个回调结果，把消息发回去。
                                List<CATSEntity.StandardOrderEntity> refreshItem = ordersResult.FromJson<List<CATSEntity.StandardOrderEntity>>();
                                if (refreshItem != null)
                                {
                                    bagOrdersChangeList.Add(refreshItem);
                                }
                            }
                            else
                            {
                                if (_lastPubReceiveTime == DateTime.MinValue)
                                {
                                    Program.logger.LogInfo("收到{0}CATS接口心跳信息!", this.group);
                                }
                                _lastPubReceiveTime = DateTime.Now;

                            }
                        }
                        catch (Exception ex)
                        {
                            Program.logger.LogInfoDetail("SubMain 解析异常:{0}", ex.Message);
                        }
                    }
                }
            }

        }

        private void RefreshOrdersDataMain()
        {
            while (true)
            {
                try
                {
                    List<CATSEntity.StandardOrderEntity> list = null;
                    if (bagOrdersChangeList.Count > 0 && bagOrdersChangeList.TryTake(out list))
                    {
                        if (OnOrdersChanged != null)
                        {
                            OnOrdersChanged.Invoke(list);
                        }
                    }
                    if (_lastPubReceiveTime > DateTime.MinValue && (DateTime.Now - _lastPubReceiveTime).TotalSeconds > 60)
                    {
                        Program.logger.LogInfo("60秒未更新pub线程，即将重启Sub线程，组合号 {0}", this.group);
                        SubThread.Abort();

                        SubThread = new Thread(new ThreadStart(SubMain)) { IsBackground = true };
                        SubThread.Start();
                        _lastPubReceiveTime = DateTime.MinValue;
                    }
                }
                catch (Exception) { }
            }
        }

        #region 下单/撤单
        public string SendOrder(string account, byte market, string stockID, decimal price, decimal qty, string tradeSide)
        {
            string strResult = string.Empty;
            string sendMsgFormat = string.Format("1|{0}|{1}|{2}|{3}|{4}|{5}|{6}", CommonUtils.Mac, account, market, stockID, price, qty, tradeSide);
            ZMessage msg = new ZMessage();
            msg.Add(new ZFrame(sendMsgFormat));
            lock (Sync)
            {
                try
                {
                    reqSocket.Send(msg);
                    var msgResult = reqSocket.ReceiveMessage();
                    strResult = msgResult[0].ReadString();
                    //Program.logger.LogInfo("CATS下单接口下单成功, 股票代码{0}, 返回消息{1}", stockID, strResult);
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("下单异常:{0}", ex.Message);
                    strResult = "下单失败," + ex.Message;
                }
            }
            return strResult;
        }

        public string CancelOrder(string account, string orderNo)
        {
            string strReuslt = null;

            ZMessage msg = new ZMessage();
            msg.Add(new ZFrame(string.Format("2|{0}|{1}|{2}", CommonUtils.Mac, account, orderNo)));
            lock (Sync)
            {
                try
                {
                    reqSocket.Send(msg);
                    var msgResult = reqSocket.ReceiveMessage();
                    strReuslt = msgResult[0].ReadString();
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("CATSAdapter.CancelOrder异常:{0}", ex.Message);
                    strReuslt = "下单失败，" + ex.Message;
                }
            }
            return strReuslt;
        }
        #endregion

    }
}
