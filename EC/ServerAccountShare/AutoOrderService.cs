using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace Server
{
    public class AutoOrderService
    {
        #region Members
        Thread _replyThread;
        Thread _pubThread;

        public ConcurrentDictionary<string, DateTime> DictUserLogin = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// 委托变化
        /// </summary>
        public ConcurrentDictionary<string, JyDataSet.委托DataTable> DictConsignmentChange = new ConcurrentDictionary<string, JyDataSet.委托DataTable>();

        /// <summary>
        /// 订单
        /// </summary>
        public ConcurrentDictionary<string, DbDataSet.订单DataTable> DictOrderChange = new ConcurrentDictionary<string, DbDataSet.订单DataTable>();

        public ConcurrentDictionary<string, bool> DictUserConsignmentChange = new ConcurrentDictionary<string, bool>();
        public ConcurrentDictionary<string, bool> DictUserOrderChange = new ConcurrentDictionary<string, bool>();
        
        ConcurrentBag<KeyValuePair<string, string>> BagNewOrderTraderStock = new ConcurrentBag<KeyValuePair<string, string>>(); //key trader, value stockID
        ConcurrentBag<KeyValuePair<string, string>> BagCancelGroupOrder = new ConcurrentBag<KeyValuePair<string, string>>();//key - group, value-orderID
        Dictionary<string, DateTime> DictLastPubInfo = new Dictionary<string, DateTime>();

        ZContext context = null;

        int PubPort = -1;
        int RepPort = -1;

        bool IsOpenZmqInterface
        {
            get {
                var needOpen = Program.appConfig.GetValue("UseZmqInterface", "0");
                return needOpen == "1";
            }
        }
        #endregion

        #region Instance
        private static object Sync = new object();
        private static AutoOrderService _instance;
        private AutoOrderService()
        {
            #region 配置信息初始化
            var port = Program.appConfig.GetValue("ZeroMQPubPort", "");
            if (!string.IsNullOrEmpty(port) && Regex.IsMatch(port, "^[0-9]+$"))
            {
                this.PubPort = int.Parse(port);
            }
            var repPort = Program.appConfig.GetValue("ZeroMQRepPort", "");
            if (!string.IsNullOrEmpty(repPort) && Regex.IsMatch(repPort, "^[0-9]+$"))
            {
                this.RepPort = int.Parse(repPort);
            }
            #endregion

        }

        public static AutoOrderService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        try
                        {
                            if (_instance == null)
                                _instance = new AutoOrderService();
                        }
                        catch (Exception ex)
                        {
                            Program.logger.LogInfoDetail("AutoOrderService Init Failed:{0}", ex.Message);
                            return null;
                        }
                        
                    }
                    return _instance;

                }
                return _instance;
            }
        }
        #endregion

        public void Start()
        {
            if (IsOpenZmqInterface)
            {
                this.context = ZContext.Create();
                ReplyThreadStart();
                PubThreadStart();
            }
        }

        #region Reply
        private void ReplyThreadStart()
        {
            _replyThread = new Thread(new ThreadStart(ReplyMain)) { IsBackground = true };
            _replyThread.Start();
        }

        private void ReplyMain()
        {
            using (ZSocket socket = new ZSocket(context, ZSocketType.REP))
            {
                try
                {
                    socket.Bind(string.Format("tcp://*:{0}", RepPort));
                    Program.logger.LogInfoDetail("ZeroMQ Replay接口正常开启，端口：{0}。", RepPort);
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("REP Binding Exception:\r\n  bind info: {0}\r\n  Exception Message: {1}\r\n  StackTrace:{2}", RepPort, ex.Message, ex.StackTrace);
                    return;
                }
                
                while (true)
                {
                    var msg = socket.ReceiveMessage();
                    ZMessage zMsgReply = new ZMessage();
                    string message = null;
                    try
                    {
                        if (msg != null)
                        {
                            message = msg[0].ReadString();
                            if (message.IndexOf('|') < 0)
                            {
                                zMsgReply.Add(new ZFrame("Invalid Message:" + message + " " + DateTime.Now.ToString()));
                            }
                            else
                            {
                                string[] infos = message.Split('|');
                                if (infos[0] == "0")
                                    Login(zMsgReply, infos[1], infos[2]);
                                else if (DictUserLogin.ContainsKey(infos[1]))
                                    SwitchMessage(zMsgReply, message, infos);
                                else
                                    zMsgReply.Add(new ZFrame("Invalid: Please login your account"));
                            }
                            msg.Dispose();
                        }
                        else
                        {
                            zMsgReply.Add(new ZFrame("无效：消息为空!"));
                        }
                    }
                    catch (Exception ex)
                    {
                        zMsgReply.Add(new ZFrame(string.Format("4|自动下单功能异常, Message:{0}， Exception {1}", message, ex.Message)));
                        //Program.logger.LogInfoDetail("自动下单功能异常：\r\n  ZMessage:{0}\r\n  Message:{1}\r\n  StackTrace:{2}", msg[0].ReadString(), ex.Message, ex.StackTrace);
                    }

                    socket.Send(zMsgReply);
                    zMsgReply.Dispose();
                }
            }
        }

        private void SwitchMessage(ZMessage zMsgReply, string message, string[] info)
        {
            string traderName = info[1];
            ShareLimitGroupItem shareLimitGroup = ShareLimitAdapter.Instance.GetLimitGroup(traderName);
            switch (info[0])
            {
                case "1": // 策略端发送指令格式：        1|交易员id|股票编码|开/平|开仓价格|股数  .     开/平：OPEN\CLOSE;  code:股票编码
                    SendOrderMessageInit(zMsgReply, info, traderName, shareLimitGroup);
                    break;
                case "2":
                    CancelOrderMessageInit(zMsgReply, info, traderName);
                    break;
                case "3": // 查询 额度分配，订单，委托 3|交易员id|订单json|委托json
                    QueryOrderMessageInit(zMsgReply, traderName);
                    break;
                case "5":
                    LimitQueryMessageInit(zMsgReply, traderName, shareLimitGroup);
                    break;
                default:
                    zMsgReply.Add(new ZFrame("Invalid Message, Not Implement branch, branch Value:" + info[0] + "\r\n  Total Message:" + message));
                    break;
            }
        }

        #region Message Init

        private static void LimitQueryMessageInit(ZMessage zMsgReply, string traderName, ShareLimitGroupItem shareLimitGroup)
        {
            var dict = new Dictionary<string, decimal>();
            if (shareLimitGroup == null)
            {
                Program.logger.LogInfoDetail("{0}额度共享组为空,将返回额度分配表对应配置。", traderName);
                Program.db.额度分配.Where(_ => _.交易员 == traderName).ToList().ForEach(_ => dict.Add(_.证券代码, _.交易额度));
                zMsgReply.Add(new ZFrame(string.Format("5|{0}|{1}", traderName, dict.ToJson())));
            }
            else
            {
                foreach (var item in shareLimitGroup.GroupStockList)
                {
                    var limit = decimal.Parse(item.LimitCount);
                    decimal buyCount, saleCount;
                    List<string> lstOrderID = new List<string>();

                    var dictStockBuySaleList = shareLimitGroup.GetShareGroupBuySaleList(item.StockID);
                    shareLimitGroup.GetShareGroupHasBuyCount(item.StockID, out buyCount, out saleCount);

                    decimal needPlaceBuyCount = 0;
                    decimal needPlaceSaleCount = 0;
                    foreach (var TraderBuySaleItem in dictStockBuySaleList)
                    {
                        if (TraderBuySaleItem.Key != traderName)
                        {
                            if (TraderBuySaleItem.Value.SoldCount > TraderBuySaleItem.Value.BuyCount)
                            {
                                needPlaceBuyCount += TraderBuySaleItem.Value.SoldCount - TraderBuySaleItem.Value.BuyCount;
                            }
                            else
                            {
                                needPlaceSaleCount += TraderBuySaleItem.Value.BuyCount - TraderBuySaleItem.Value.SoldCount;
                            }
                        }
                    }
                    if (dict.ContainsKey(item.StockID))
                    {
                        Program.logger.LogInfoDetail("已存在股票{0}的配置项，股票代码重复", item.StockID);
                    }
                    else
                    {
                        dict.Add(item.StockID, Math.Min(limit - buyCount - needPlaceBuyCount, limit - saleCount - needPlaceSaleCount));
                    }
                }
            }
            

            zMsgReply.Add(new ZFrame(string.Format("5|{0}|{1}", traderName, dict.ToJson())));
        }

        private static void QueryOrderMessageInit(ZMessage zMsgReply, string traderName)
        {
            Program.logger.LogInfoDetail("自动下单接口 3, 订单、委托查询：收到消息时间{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
            string strOrderChange = string.Empty;
            var rowsOrder = Program.db.订单.Where(_ => _.交易员 == traderName && _.开仓时间.Date == DateTime.Today).ToList();
            if (rowsOrder.Count > 0)
            {
                var lstOrder = new List<Server.订单>();
                rowsOrder.ForEach(_ => { lstOrder.Add(GetOrderItem(_)); });
                strOrderChange = lstOrder.ToJson();
            }

            string strWt = string.Empty;
            if (Program.交易员委托DataTable.ContainsKey(traderName) && Program.交易员委托DataTable[traderName] != null)
            {
                strWt = Program.交易员委托DataTable[traderName].ToJson();
            }
            var strReplyMsg = string.Format("3|{0}|{1}|{2}", traderName, strOrderChange, strWt);
            zMsgReply.Add(new ZFrame(strReplyMsg));
            Program.logger.LogInfoDetail("自动下单接口 3, 订单、委托查询：返回消息时间{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
        }

        private void CancelOrderMessageInit(ZMessage zMsgReply, string[] infos, string traderName)
        {
            Program.logger.LogInfoDetail("自动下单接口2 取消订单：收到消息时间{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
            var strCancel = CancelOrder(traderName, infos[2], infos[3]);
            zMsgReply.Add(new ZFrame(strCancel));
            Program.logger.LogInfoDetail("自动下单接口2 取消订单：返回结果消息时间{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
        }

        private void SendOrderMessageInit(ZMessage zMsgReply, string[] infos, string traderName, ShareLimitGroupItem shareLimitGroup)
        {
            //Program.logger.LogInfoDetail("自动下单接口1 订单发送：收到消息时间{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
            string buySaleFlag = infos[3].ToUpper() == "OPEN" ? "0" : "1";
            string strSend = string.Empty;

            if (shareLimitGroup == null)
            {
                strSend = string.Format("1|{0}|0|7|对应共享额度组不存在!", traderName);
            }
            else
            {
                strSend = SendSharedLimitOrder(traderName, infos[2], buySaleFlag, infos[5], infos[4], shareLimitGroup);
            }
            zMsgReply.Add(new ZFrame(strSend));
            //Program.logger.LogInfoDetail("自动下单接口1 订单发送：返回下单结果时间{0}", DateTime.Now.ToString("HH:mm:ss:fff"));
        } 
        #endregion

        private static 订单 GetOrderItem(DbDataSet.订单Row item)
        {
            return new 订单()
            {
                组合号 = item.组合号,
                证券名称 = item.证券名称,
                证券代码 = item.证券代码,
                已平数量 = item.已平数量,
                已平金额 = item.已平金额,
                已开数量 = item.已开数量,
                已开金额 = item.已开金额,
                市场代码 = item.市场代码,
                平仓时间 = item.平仓时间,
                平仓类别 = item.平仓类别,
                平仓价位 = item.平仓价位,
                开仓时间 = item.开仓时间,
                开仓类别 = item.开仓类别,
                开仓价位 = item.开仓价位,
                交易员 = item.交易员,
                当前价位 = item.当前价位,
                浮动盈亏 = item.浮动盈亏,
            };
        }

        private void Login(ZMessage zMsgReply,  string traderName, string psw)
        {
            if (DictUserLogin.ContainsKey(traderName))
            {
                if (Program.db.平台用户.ExistsUser(traderName, psw))
                {
                    DictUserLogin[traderName] = DateTime.Now;
                    FectchAllTable(traderName);
                    zMsgReply.Add(new ZFrame(string.Format("0|{0}|{1}|{2}|登录更新成功", traderName, 1, string.Empty)));
                }
                else
                {
                    zMsgReply.Add(new ZFrame(string.Format("0|{0}|{1}|{2}|登录更新失败，用户名或密码错误", traderName, 0, string.Empty)));
                }
            }
            else if (Program.db.平台用户.ExistsUser(traderName, psw))
            {
                if (!DictUserLogin.TryAdd(traderName, DateTime.Now))
                {
                    DictUserLogin[traderName] = DateTime.Now;
                }
                FectchAllTable(traderName);
                zMsgReply.Add(new ZFrame(string.Format("0|{0}|{1}|{2}|登录成功", traderName, 1, string.Empty)));
            }
            else
            {
                zMsgReply.Add(new ZFrame(string.Format("0|{0}|{1}|{2}|登录失败，用户名或密码错误", traderName, 0, string.Empty)));
            }
        }
        #endregion

        #region Pub
        private void PubThreadStart()
        {
            _pubThread = new Thread(new ThreadStart(PubMain)) { IsBackground = true };
            _pubThread.Start();
        }

        private void PubMain()
        {
            using (var socket = new ZSocket(context, ZSocketType.PUB))
            {
                try
                {
                    socket.Bind(string.Format("tcp://*:{0}", PubPort));
                    Program.logger.LogInfoDetail("ZeroMQ Pub接口正常开启，端口：{0}。", PubPort);
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("REP Binding Exception:\r\n  bind info: {0}\r\n  Exception Message: {1}\r\n  StackTrace:{2}", PubPort, ex.Message, ex.StackTrace);
                    return;
                }

                List<KeyValuePair<string, string>> lstCancelOrder = new List<KeyValuePair<string,string>>();
                 var dtEnd = Program.appConfig.GetValue("结束查询时间", "15:30");
                while (true)
                {
                    if (DateTime.Now >= DateTime.Parse(dtEnd))
                    {
                        Thread.Sleep(60000);
                        continue;
                    }

                    try
                    {
                        foreach (var item in DictUserLogin)
                        {
                            if (Program.IsServiceUpdating) break;
                            PubTraderInfo(socket, item.Key);
                        }

                        CheckLimitSending(socket, lstCancelOrder);
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfoDetail("自动下单功能pub线程异常： \r\n  Message:{0}\r\n  StackTrace:{1}", ex.Message, ex.StackTrace);
                    }
                    Thread.Sleep(20);
                }
            }
        }

        private void CheckLimitSending(ZSocket socket, List<KeyValuePair<string, string>> lstCancelOrder)
        {
            KeyValuePair<string, string> pair;
            while (BagNewOrderTraderStock.TryTake(out pair))
            {
                SendLimitMessage(socket, pair.Key, pair.Value);
            }


            while (BagCancelGroupOrder.TryTake(out pair))
            {
                lstCancelOrder.Add(pair);
            }
            if (lstCancelOrder.Count > 0)
            {
                for (int i = lstCancelOrder.Count - 1; i > -1; i--)
                {
                    //var row = Program.db.已发委托.FirstOrDefault(_ =>
                    //    _.日期 == DateTime.Today &&
                    //    _.组合号 == lstCancelOrder[i].Key &&
                    //    _.委托编号 == lstCancelOrder[i].Value);
                    var row = Program.db.已发委托.Get已发委托(DateTime.Today, lstCancelOrder[i].Key, lstCancelOrder[i].Value);
                    if (row != null && row.撤单数量 > 0)
                    {
                        lstCancelOrder.RemoveAt(i);
                        SendLimitMessage(socket, row.交易员, row.证券代码);
                    }
                }
            }
        }

        private static void SendLimitMessage(ZSocket socket, string trader, string stock)
        {
            var message = new ZMessage();
            message.Add(new ZFrame("limit.raw"));

            StringBuilder sb = new StringBuilder(128);
            sb.Append("6|").Append(stock).Append("|");
            var group = ShareLimitAdapter.Instance.GetLimitGroup(trader);

            decimal buyCount, saleCount;
            group.GetShareGroupHasBuyCount(stock, out buyCount, out saleCount);
            var stockLImitItem = group.GroupStockList.FirstOrDefault(_ => _.StockID == stock);
            sb.Append(decimal.Parse(stockLImitItem.LimitCount) - Math.Max(buyCount, saleCount));
            message.Add(new ZFrame(sb.ToString()));
            socket.Send(message);
            
        }

        private void PubTraderInfo(ZSocket socket, string trader)
        {
            bool HasPubData = false;
            StringBuilder s = null;

            if (!DictLastPubInfo.ContainsKey(trader))
                DictLastPubInfo.Add(trader, DateTime.MinValue);

            #region Order Info Check
            if (DictUserOrderChange.ContainsKey(trader) && DictUserOrderChange[trader])
            {
                DictUserOrderChange[trader] = false;
                s = new StringBuilder(256);
                s.AppendFormat("4|{0}|", trader);

                HasPubData = AddOrderInfo(HasPubData, s, trader);
            } 
            #endregion

            #region 委托 Info Check
            if (HasPubData)
            {
                s.Append(Program.交易员委托DataTable[trader].ToJson());
            }
            else if (DictUserConsignmentChange.ContainsKey(trader) && DictUserConsignmentChange[trader])
            {
                DictUserConsignmentChange[trader] = false;
                HasPubData = true;

                if (s == null)
                {
                    s = new StringBuilder(256);
                    s.AppendFormat("4|{0}|", trader);
                    AddOrderInfo(HasPubData, s, trader);
                }

                if (Program.交易员委托DataTable.ContainsKey(trader) && Program.交易员委托DataTable[trader] != null)
                {
                    s.Append(Program.交易员委托DataTable[trader].ToJson());
                }
                else
                {
                    s.Append("异常,交易员委托DataTable无数据！");
                }

            } 
            #endregion

            if (HasPubData)
            {
                DictLastPubInfo[trader] = DateTime.Now;
                var message = new ZMessage();
                message.Add(new ZFrame(trader + ".raw"));
                message.Add(new ZFrame(s.ToString()));
                socket.Send(message);
                Program.logger.LogInfoDetail("自动下单功能: pub 查询到新数据，主动发送委托及订单，时间 {0}, 交易员：{1}", DateTime.Now.ToString("HH:mm:ss:fff"), trader);
            }
            else if ((DateTime.Now - DictLastPubInfo[trader]).Seconds >= 10)
            {
                DictLastPubInfo[trader] = DateTime.Now;
                var message = new ZMessage();
                message.Add(new ZFrame(trader + ".raw"));
                message.Add(new ZFrame("HEARTBEAT"));
                socket.Send(message);
                //Program.logger.LogInfoDetail("自动下单功能: pub 5分钟定时发送");
            }
        }

        private static bool AddOrderInfo(bool HasPubData, StringBuilder s, string trader)
        {
            var rowsOrder = Program.db.订单.Where(_ => _.交易员 == trader && _.开仓时间.Date == DateTime.Today).ToList();
            if (rowsOrder.Count > 0)
            {
                HasPubData = true;
                var lstOrder = new List<Server.订单>();
                rowsOrder.ForEach(_ => { lstOrder.Add(GetOrderItem(_)); });
                s.Append(lstOrder.ToJson());
            }
            s.Append('|');
            return HasPubData;
        }
        #endregion

        #region Order Func
        public void FectchAllTable(string UserName)
        {
            Program.成交表Changed[UserName] = true;
            Program.委托表Changed[UserName] = true;
            Program.平台用户表Changed[UserName] = true;
            Program.额度分配表Changed[UserName] = true;
            Program.订单表Changed[UserName] = true;
            Program.已平仓订单表Changed[UserName] = true;
        }

        public string SendOrder(string UserName, string 证券代码, string tradeDirection, string quatity, string price)
        {
            // 交易端返回消息格式：         1|交易员id|委托编号|错误码id|错误信息.     委托编号:非0表示成功。0表示失败； 错误码id|错误信息：对应的错误信息
            // 策略端发送指令格式：         1|交易员id|股票编码|开/平|开仓价格|股数  . 开/平：OPEN\CLOSE;
            // 交易端返回消息格式：         1|交易员id|委托编号|错误码id|错误信息.     委托编号:非0表示成功。0表示失败； 错误码id|错误信息：对应的错误信息
            StringBuilder sb = new StringBuilder(128);
            try
            {
                sb.Append("1|").Append(UserName).Append('|');

                int 买卖方向 = int.Parse(tradeDirection);
                decimal 委托数量 = decimal.Parse(quatity);
                decimal 委托价格 = decimal.Parse(price);

                DbDataSet.平台用户Row AASUser1 = Program.db.平台用户.Get平台用户(UserName);
                DbDataSet.额度分配Row TradeLimit1 = Program.db.额度分配.Get额度分配(UserName, 证券代码);
                var shareLimitGroup = ShareLimitAdapter.Instance.GetLimitGroup(UserName);

                if (TradeLimit1 == null)
                {
                    sb.Append("0|0|无此证券交易额度");
                    return sb.ToString();
                }

                decimal commissionCharge = TradeLimit1.手续费率;
                string zqName = TradeLimit1.证券名称;

                decimal 已用仓位 = Program.db.已发委托.Get已用仓位(UserName);
                decimal 当日委托交易费用 = Program.db.已发委托.Get交易费用(UserName, commissionCharge);

                List<string> lstSendedID = new List<string>();
                decimal 已买股数 = 0;
                decimal 已卖股数 = 0;
                Program.db.已发委托.Get已买卖股票(UserName, 证券代码, lstSendedID, out 已买股数, out 已卖股数);

                decimal 开仓数量 = Tool.Get开仓数量From已买卖数量(买卖方向, 委托数量, 已买股数, 已卖股数);

                #region 仓位，亏损，买数量，卖数量，限制判定
                if (开仓数量 > 0)
                {
                    //仓位限制
                    decimal 欲下仓位 = 委托价格 * 开仓数量;
                    if (已用仓位 + 欲下仓位 > AASUser1.仓位限制)
                    {
                        sb.AppendFormat("0|1|仓位超限, 已用仓位{0:f2},欲下仓位{1:f2}, 超过设定值{2:f2}", 已用仓位, 欲下仓位, AASUser1.仓位限制);
                        return sb.ToString();
                    }

                    //亏损限制  
                    decimal 当日亏损 = Program.db.已平仓订单.Get当日已平仓亏损(UserName) + 当日委托交易费用;
                    if (当日亏损 >= AASUser1.亏损限制)
                    {
                        sb.AppendFormat("0|2|用户亏损{0:f2}超过设定值{1:f2}", 当日亏损, AASUser1.亏损限制);
                        return sb.ToString();
                    }
                }

                //将缓存数量加上。
                if (CommonUtils.OrderCacheQueue.Count > 0)
                {
                    var cache = CommonUtils.OrderCacheQueue.Where(_ =>
                        _.Trader == UserName
                        && _.Zqdm == 证券代码
                        && !lstSendedID.Contains(_.OrderID)
                        && (DateTime.Now - _.SendTime).TotalSeconds < 10).ToList();
                    if (cache.Count > 0)
                    {
                        var buy = cache.Where(_ => _.Category % 2 == 0).Sum(_ => _.Quantity);
                        var sale = cache.Where(_ => _.Category % 2 == 1).Sum(_ => _.Quantity);
                        已买股数 += buy;
                        已卖股数 += sale;
                    }
                }

                if (买卖方向 == 0)
                {
                    if (已买股数 + 委托数量 > TradeLimit1.交易额度)
                    {
                        sb.AppendFormat("0|3|买数量超限, 已买数量{0:f0}, 欲买数量{1:f0}, 超过设定值{2:f0}", 已买股数, 委托数量, TradeLimit1.交易额度);
                        return sb.ToString();
                    }
                }
                else
                {
                    if (已卖股数 + 委托数量 > TradeLimit1.交易额度)
                    {
                        sb.AppendFormat("0|4|卖数量超限, 已卖数量{0:f0}, 欲卖数量{1:f0}, 超过设定值{2:f0}", 已卖股数, 委托数量, TradeLimit1.交易额度);
                        return sb.ToString();
                    }
                }
                #endregion

                string 委托编号;
                string ErrInfo;

                var orderCacheObj = new OrderCacheEntity()
                {
                    Category = 买卖方向,
                    Zqdm = 证券代码,
                    ZqName = zqName,
                    Price = 委托价格,
                    Quantity = 委托数量,
                    Trader = UserName,
                    Sender = UserName,
                    SendTime = DateTime.Now,
                };
                CommonUtils.OrderCacheQueue.Enqueue(orderCacheObj);
                bool hasOrderNo;
                if (Program.db.券商帐户.Exists(TradeLimit1.组合号))
                {
                    Program.db.券商帐户.SendOrder(TradeLimit1.组合号, TradeLimit1.Get券商帐户买卖类别(买卖方向), TradeLimit1.市场, 证券代码, 委托价格, 委托数量, orderCacheObj, out 委托编号, out ErrInfo, out hasOrderNo);
                }
                else
                {
                    sb.Append("0|5|帐户不存在");
                    return sb.ToString();
                }

                if (ErrInfo == string.Empty)
                {
                    if (hasOrderNo)
                    {
                        orderCacheObj.OrderID = 委托编号;

                        Program.AddConsignmentCache(UserName, 证券代码, 买卖方向, 委托数量, 委托价格, 委托编号, TradeLimit1.组合号, TradeLimit1.证券名称, TradeLimit1.市场);
                        Program.db.已发委托.Add(DateTime.Today, TradeLimit1.组合号, 委托编号, UserName, "程序自动委托下单成功", TradeLimit1.市场, 证券代码, TradeLimit1.证券名称, 买卖方向, 0m, 0m, (decimal)委托价格, (decimal)委托数量, 0m);
                        string Msg = UserName + " 程序自动下单成功";
                        Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), UserName, TradeLimit1.组合号, 证券代码, TradeLimit1.证券名称, 委托编号, 买卖方向, 委托数量, 委托价格, Msg);
                        sb.AppendFormat("{0}||{1}", TradeLimit1.组合号 + '_' + 委托编号, Msg);
                    }
                    sb.AppendFormat("{0}|6|已报，等待接口处理");
                }
                else
                {
                    sb.AppendFormat("0|6|{0}", ErrInfo);
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("服务器下单异常:{0} {1}", ex.Message, ex.StackTrace);
                sb.AppendFormat("0|7|程序自动下单接口异常\r\n  交易员账号:{0}\r\n  证券代码{1}\r\n  ExceptionMessage:{2}", UserName, 证券代码, ex.Message);
            }
            return sb.ToString();
        }

        public string SendSharedLimitOrder(string UserName, string 证券代码, string tradeDirection, string quatity, string price, ShareLimitGroupItem shareLimitGroup)
        {
            StringBuilder sb = new StringBuilder(128);
            lock (Sync)
            {
                try
                {
                    sb.Append("1|").Append(UserName).Append('|');

                    int 买卖方向 = int.Parse(tradeDirection);
                    decimal 委托数量 = decimal.Parse(quatity);
                    decimal 委托价格 = decimal.Parse(price);
                    var market = 证券代码.GetCodeMarket();
                    var limitItem = shareLimitGroup.GroupStockList.FirstOrDefault(_ => _.StockID == 证券代码);
                    if (limitItem == null)
                    {
                        sb.AppendFormat("0|1|该股票没有额度|0");
                        return sb.ToString();
                    }
                    decimal commissionCharge = decimal.Parse(limitItem.Commission);
                    string zqName = limitItem.StockName;

                    List<string> lstSendedOrderID = new List<string>();
                    decimal 已买股数, 已卖股数, traderBuy, traderSale;
                    shareLimitGroup.GetShareGroupHasBuyCount(证券代码, UserName, lstSendedOrderID, out 已买股数, out 已卖股数, out traderBuy, out traderSale);
                    decimal 开仓数量 = Tool.Get开仓数量From已买卖数量(买卖方向, 委托数量, 已买股数, 已卖股数);

                    #region 仓位，亏损，买数量，卖数量，限制判定
                    #region 仓位亏损限制
                    if (开仓数量 > 0)
                    {
                        DbDataSet.平台用户Row AASUser1 = Program.db.平台用户.Get平台用户(UserName);
                        //仓位限制
                        decimal 欲下仓位 = 委托价格 * 开仓数量;
                        decimal 已用仓位 = Program.db.已发委托.Get已用仓位(UserName);
                        if (已用仓位 + 欲下仓位 > AASUser1.仓位限制)
                        {
                            sb.AppendFormat("0|1|仓位超限, 已用仓位{0:f2},欲下仓位{1:f2}, 超过设定值{2:f2}|0", 已用仓位, 欲下仓位, AASUser1.仓位限制);
                            return sb.ToString();
                        }

                        //亏损限制  
                        decimal 当日委托交易费用 = Program.db.已发委托.Get交易费用(UserName, commissionCharge);
                        decimal 当日亏损 = Program.db.已平仓订单.Get当日已平仓亏损(UserName) + 当日委托交易费用;
                        if (当日亏损 >= AASUser1.亏损限制)
                        {
                            sb.AppendFormat("0|2|用户亏损{0:f2}超过设定值{1:f2}|0", 当日亏损, AASUser1.亏损限制);
                            return sb.ToString();
                        }
                    }
                    #endregion

                    #region 买卖数量限制
                    // 
                    //将缓存数量加上。
                    if (CommonUtils.OrderCacheQueue.Count > 0)
                    {
                        var cache = CommonUtils.OrderCacheQueue.Where(_ =>
                            _.Trader == UserName
                            && _.Zqdm == 证券代码
                            && !lstSendedOrderID.Contains(_.OrderID)
                            && (DateTime.Now - _.SendTime).TotalSeconds < 10).ToList();
                        if (cache.Count > 0)
                        {
                            var buy = cache.Where(_ => _.Category % 2 == 0).Sum(_ => _.Quantity);
                            var sale = cache.Where(_ => _.Category % 2 == 1).Sum(_ => _.Quantity);
                            已买股数 += buy;
                            已卖股数 += sale;
                        }
                    }

                    var dict = shareLimitGroup.GetShareGroupBuySaleList(证券代码);
                    decimal needPlaceBuyCount = 0;
                    if (买卖方向 == 0)
                    {//如已反方向下了大于等于委托数的单子，则可以跳过判断
                        if (traderSale - traderBuy < 委托数量)
                        {
                            foreach (var item in dict)
                            {
                                if (item.Key != UserName && item.Value.SoldCount > item.Value.BuyCount)
                                {
                                    needPlaceBuyCount += item.Value.SoldCount - item.Value.BuyCount;
                                }
                            }
                            if (needPlaceBuyCount + 已买股数 + 委托数量 > decimal.Parse(limitItem.LimitCount))
                            {
                                var wt = Program.帐户委托DataTable[limitItem.GroupAccount].Where(_ => _.证券代码 == 证券代码);
                                sb.AppendFormat("0|3|额度不足,总额度{0},已买股数{1},需保留仓位{2}|{3}",
                                    limitItem.LimitCount, 已买股数, needPlaceBuyCount, decimal.Parse(limitItem.LimitCount) - needPlaceBuyCount - 已买股数);
                                Program.logger.LogInfoDetail("买数量超限！交易员买卖此股票数量:{0}，总额度{1},已买股数{2},需保留仓位{3},该交易员买数{4} 卖数{5}",
                                    dict.ToJson(), limitItem.LimitCount, 已买股数, needPlaceBuyCount, traderBuy, traderSale);
                                return sb.ToString();
                            }
                        }
                    }
                    else
                    {
                        if (traderBuy - traderSale < 委托数量)
                        {
                            foreach (var item in dict)
                            {
                                if (item.Key != UserName && item.Value.BuyCount > item.Value.SoldCount)
                                    needPlaceBuyCount += item.Value.BuyCount - item.Value.SoldCount;
                            }
                            if (needPlaceBuyCount + 已卖股数 + 委托数量 > decimal.Parse(limitItem.LimitCount))
                            {
                                var wt = Program.帐户委托DataTable[limitItem.GroupAccount].Where(_ => _.证券代码 == 证券代码);
                                sb.AppendFormat("0|4|额度不足,总额度{0},已卖股数{1},需保留仓位{2}|{3}",
                                    limitItem.LimitCount, 已卖股数, needPlaceBuyCount, decimal.Parse(limitItem.LimitCount) - needPlaceBuyCount - 已卖股数);
                                Program.logger.LogInfoDetail("卖数量超限！交易员买卖此股票数量:{0}，总额度{1},已卖股数{2},需保留仓位{3},该股票相关委托{4}", dict.ToJson(),
                                    limitItem.LimitCount, 已卖股数, needPlaceBuyCount, wt.ToJson());
                                return sb.ToString();
                            }
                        }
                    }

                    #endregion
                    #endregion

                    string 委托编号;
                    string ErrInfo;

                    var orderCacheObj = new OrderCacheEntity()
                    {
                        Category = 买卖方向,
                        Zqdm = 证券代码,
                        ZqName = zqName,
                        Price = 委托价格,
                        Quantity = 委托数量,
                        Trader = UserName,
                        Sender = UserName,
                        SendTime = DateTime.Now,
                    };
                    CommonUtils.OrderCacheQueue.Enqueue(orderCacheObj);
                    bool hasOrderNo;

                    if (Program.db.券商帐户.Exists(limitItem.GroupAccount))
                    {
                        var bsFlag = limitItem.GetTradeType(买卖方向);
                        Program.logger.LogInfo("额度共享策略：组合号{0}, 买卖方向原始值{1},买卖方向最终值{2}, 股票代码{3}", limitItem.GroupAccount, 买卖方向, bsFlag, 证券代码);
                        Program.db.券商帐户.SendOrder(limitItem.GroupAccount, bsFlag, market, 证券代码, 委托价格, 委托数量, orderCacheObj, out 委托编号, out ErrInfo, out hasOrderNo);
                    }
                    else
                    {
                        sb.Append("0|5|帐户不存在|0");
                        return sb.ToString();
                    }

                    if (ErrInfo == string.Empty)
                    {
                        if (hasOrderNo)
                        {
                            orderCacheObj.OrderID = 委托编号;
                            Program.AddConsignmentCache(UserName, 证券代码, 买卖方向, 委托数量, 委托价格, 委托编号, limitItem.GroupAccount, limitItem.StockName, market);
                            Program.db.已发委托.Add(DateTime.Today, limitItem.GroupAccount, 委托编号, UserName, "程序自动委托下单成功", market, 证券代码, limitItem.StockName, 买卖方向, 0m, 0m, (decimal)委托价格, (decimal)委托数量, 0m);
                            string Msg = UserName + " 程序自动下单成功";
                            Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), UserName, limitItem.GroupAccount, 证券代码, limitItem.StockName, 委托编号, 买卖方向, 委托数量, 委托价格, Msg);
                            sb.AppendFormat("{0}||{1}", limitItem.GroupAccount + '_' + 委托编号, Msg);
                            BagNewOrderTraderStock.Add(new KeyValuePair<string, string>(UserName, 证券代码));
                        }
                        else
                        {
                            sb.AppendFormat("0|6|已报，等待接口处理");
                        }
                    }
                    else
                    {
                        sb.AppendFormat("0|6|{0}|0", ErrInfo);
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("服务器下单异常:{0} {1}", ex.Message, ex.StackTrace);
                    sb.AppendFormat("0|7|程序自动下单接口异常\r\n  交易员账号:{0}\r\n  证券代码{1}\r\n  ExceptionMessage:{2}|0", UserName, 证券代码, ex.Message);
                }
            }
            
            return sb.ToString();
        }

        public string CancelOrder(string UserName, string 证券代码, string 委托编号)
        {
            // 策略端发送指令格式：2|交易员id|股票编码|委托编号
            // 交易端返回消息格式：2|交易员id|指令结果状态|错误码id|错误信息.   指令结果状态:1表示成功。0表示失败； 错误码id|错误信息：对应的错误信息
            StringBuilder sb = new StringBuilder(128);
            try
            {
                var order = Program.db.已发委托.Get已发委托(DateTime.Today, "", 委托编号);
                sb.Append("2|").Append(UserName).Append('|');
                var shareLimitGroup = ShareLimitAdapter.Instance.GetLimitGroup(UserName);
                if (shareLimitGroup == null)
                {
                    //如果没有共享组，就该走普通撤单流程。
                }
                var stockLimit = shareLimitGroup.GroupStockList.FirstOrDefault(_ => _.StockID == 证券代码);
                

                var con = Program.db.已发委托.FirstOrDefault(_ => _.交易员 == UserName && _.日期 == DateTime.Today && _.证券代码 == 证券代码 && _.委托编号 == 委托编号);
                if (con == null)
                {
                    sb.Append("0|3|账户委托不存在");
                }

                string 组合号 = stockLimit.GroupAccount;
                byte 市场代码 = CommonUtils.GetCodeMarket(证券代码);
                string 证券名称 = stockLimit.StockName;
                int 买卖方向 = con.买卖方向;
                decimal 委托数量 = con.委托数量;
                decimal 委托价格 = con.委托价格;

                string Result;
                string ErrInfo;
                if (Program.db.券商帐户.Exists(组合号))
                {
                    Program.db.券商帐户.CancelOrder(证券代码, 组合号, 市场代码, 委托编号, out Result, out ErrInfo);
                }
                else
                {
                    sb.Append("0|0|帐户不存在");
                    return sb.ToString();
                }

                if (ErrInfo == string.Empty)
                {
                    Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), UserName, 组合号, 证券代码, 证券名称, 委托编号, 买卖方向, 委托数量, 委托价格, "自动撤单成功");
                    sb.Append("1||撤单完成");
                    var wtGroup = Program.帐户委托DataTable[组合号].FirstOrDefault(_ => _.委托编号 == 委托编号 && _.交易员 == UserName);
                    var wtUser = Program.交易员委托DataTable[UserName].FirstOrDefault(_ => _.委托编号 == 委托编号 && _.组合号 == 组合号);
                    BagCancelGroupOrder.Add(new KeyValuePair<string, string>(组合号, 委托编号));
                }
                else
                {
                    sb.Append("0|1|").Append(ErrInfo);
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("服务器撤单异常:{0} {1}", ex.Message, ex.StackTrace);
                sb.Append("0|4|").Append(ex.Message);
            }
            return sb.ToString();
        }
        #endregion
    }
}
