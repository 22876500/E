using Binance.API.Csharp.Client;
using Binance.API.Csharp.Client.Models.Enums;
using Binance.API.Csharp.Client.Models.General;
using Binance.API.Csharp.Client.Models.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Adapter
{
    public class BinanceAdapter : IAdapter
    {
        private static int stopMiniSeconds = 125;
        private static DateTime AutoBanTime = DateTime.MinValue;
        private static DateTime StartTime = new DateTime(1970, 1, 1, 8, 0, 0);
        private static ExchangeInfo exchangeInfo;
        private static object sync = new object();
        private static bool IsServerTimeChecked = false;

        private DateTime lastRequesTime;
        private object OrderQueueAddObj = new object();
        private BinanceClient binanceClient;
        private ConcurrentDictionary<string, Binance.API.Csharp.Client.Models.Market.Balance> dictBalance = new ConcurrentDictionary<string, Binance.API.Csharp.Client.Models.Market.Balance>(8, 256);
        private ConcurrentQueue<OrderOrTradeUpdatedMessage> queueOrder = new ConcurrentQueue<OrderOrTradeUpdatedMessage>();
        private ConcurrentQueue<OrderOrTradeUpdatedMessage> queueTrade = new ConcurrentQueue<OrderOrTradeUpdatedMessage>();
        private ConcurrentQueue<OrderTrader> queueOrderTraderRelation = new ConcurrentQueue<OrderTrader>();

        public bool IsInited { get; set; }
        public string AccountName { get; private set; }
        private string Account { get; set; }
        private DbDataSet.已发委托DataTable historyOrder = null;

        public void Init(string apiKey, string secretKey, string accountName, string account, DbDataSet.已发委托DataTable dt)
        {
            if (!IsInited)
            {
                lock (sync)
                {
                    if (!IsInited && DateTime.Now > AutoBanTime)
                    {
                        ApiClient apiClient = new ApiClient(apiKey, secretKey);
                        binanceClient = new BinanceClient(apiClient);
                        binanceClient.ListenUserDataEndpoint(AccountHandler, TradesHandler, OrdersHandler);
                        Thread.Sleep(stopMiniSeconds);

                        QueryServerTime();

                        var accInfo = binanceClient.GetAccountInfo().Result;
                        Thread.Sleep(stopMiniSeconds);
                        foreach (var item in accInfo.Balances)
                        {
                            dictBalance[item.Asset] = item;
                        }
                        lastRequesTime = DateTime.Now;

                        if (exchangeInfo == null)
                        {
                            exchangeInfo = binanceClient.GetExchangeInfo().Result;
                            Thread.Sleep(stopMiniSeconds);
                        }

                        AccountName = accountName;
                        Account = account;
                        InitHistoryOrder(dt);
                        historyOrder = dt;
                        IsInited = true;
                        
                    }

                }
            }
        }


        private static void LogException(Exception ex, string exceptionSource)
        {
            string s = (ex.InnerException ?? ex).Message;
            if (s.Contains("IP banned until "))
            {
                long tick = 0;
                if (long.TryParse(s.Substring(s.IndexOf("until ") + 6, s.IndexOf(".'}") - s.IndexOf("until ") - 6), out tick))
                {
                    AutoBanTime = StartTime.AddMilliseconds(tick);
                }
                Program.logger.LogInfo(s.Replace(tick.ToString(), AutoBanTime.ToString("yyyy-MM-dd HH:mm:ss")));
            }
            else
            {
                Program.logger.LogInfo(string.Format("{0}, source {1}", s,  exceptionSource));
            }
        }

        private void InitHistoryOrder(DbDataSet.已发委托DataTable dt)
        {
            if (dt != null && dt.Count > 0)
            {
                foreach (var item in dt)
                {
                    try
                    {
                        int orderId = int.Parse(item.委托编号);
                        queueOrderTraderRelation.Enqueue(new OrderTrader(item.交易员, orderId, item.委托编号.ToUpper()));
                        queueOrder.Enqueue(new OrderOrTradeUpdatedMessage()
                        {
                            Orderid = orderId,
                            LastFilledTradeQuantity = item.成交数量,
                            Price = item.委托价格,
                            OriginalQuantity = item.委托数量,
                            Symbol = item.证券代码,
                            Status = GetStatusByDescription(item.状态说明),
                            Side = item.买卖方向 == 0 ? "BUY" : "SELL",
                            EventTime = (long)(DateTime.Today.AddHours(9) - StartTime).TotalMilliseconds,
                        });
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfo("BinanceAdapter.InitHistoryOrder Exception: {0}", ex.Message);
                    }
                }
            }
        }

        public void SendOrder(string symbol, int bsFlag, decimal qty, decimal price, string trader, out string orderID, out string errInfo)
        {
            orderID = string.Empty;
            errInfo = string.Empty;
            try
            {
                if (exchangeInfo != null && !ExchangeCheck(symbol, qty, price, out errInfo))
                {
                    return;
                }
                var st = DateTime.Now;
                lock (sync)
                {
                    //Program.logger.LogInfoDetail("开始下单, coin {0}, qty {1} price {2}, bsflag {3} ", symbol, qty, price, bsFlag);
                    var order = binanceClient.PostNewOrder(symbol, qty, price, bsFlag == 0 ? OrderSide.BUY : OrderSide.SELL).Result;
                    //Program.logger.LogInfoDetail("下单结果, 委托编号 {0}, 详细信息{1}",order.OrderId, order.ToJson());
                    if (order != null && order.OrderId > 0)
                    {
                        if (queueOrder.FirstOrDefault(_ => _.Orderid == order.OrderId) == null)
                        {
                            queueOrderTraderRelation.Enqueue(new OrderTrader(trader, order.OrderId, order.Symbol));
                            queueOrder.Enqueue(new OrderOrTradeUpdatedMessage()
                            {
                                NewClientOrderId = order.ClientOrderId,
                                Orderid = order.OrderId,
                                Symbol = order.Symbol,
                                EventTime = order.TransactTime,
                                Price = price,
                                OriginalQuantity = qty,
                                Status = "New",
                                Side = bsFlag == 0 ? "BUY" : "SELL",
                            });
                        }
                        orderID = order.OrderId.ToString();
                    }
                    
                    Sleep(st);
                }
            }
            catch (Exception ex)
            {
                errInfo = string.Format("下单异常：{0}", (ex.InnerException ?? ex).Message);
                LogException(ex, "BinanceAdapter.SendOrder Exception");
            }
        }

        public void CancelOrder(string symbol, string OrderID, out string result, out string errInfo)
        {
            result = string.Empty;
            errInfo = string.Empty;
            try
            {
                int intID = int.Parse(OrderID);
                var orderInQueue = queueOrder.FirstOrDefault(_ => _.Orderid == intID);

                var st = DateTime.Now;
                lock (sync)
                {
                    var cancelResult = binanceClient.CancelOrder(symbol, long.Parse(OrderID)).Result;
                    if (cancelResult.OrderId > 0)
                    {
                        result = "撤单成功";
                    }
                    else
                    {
                        errInfo = "撤单失败";
                    }
                    Sleep(st);
                }
            }
            catch (Exception ex)
            {
                errInfo = string.Format("撤单异常:{0}", (ex.InnerException ?? ex).Message);
                LogException(ex, "BinanceAdapter.Cancel Order Exception");
            }
        }

        public bool ExchangeCheck(string symbol, decimal qty, decimal price, out string errInfo)
        {
            errInfo = string.Empty;
            if (exchangeInfo != null)
            {
                var filter = exchangeInfo.Symbols.FirstOrDefault(_ => _.Symbol == symbol);
                if (filter != null)
                {
                    var priceFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "PRICE_FILTER");
                    if (priceFilter != null && (decimal.Parse(priceFilter.MaxPrice) < price || decimal.Parse(priceFilter.MinPrice) > price))
                    {
                        errInfo = string.Format("{0}价格最大值 {1}, 最小值 {2},输入值 {3}不在允许范围内", symbol, priceFilter.MaxPrice, priceFilter.MinPrice, price);
                        return false;
                    }

                    var qtyFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "LOT_SIZE");
                    if (qtyFilter != null && (decimal.Parse(qtyFilter.MaxQty) < qty || decimal.Parse(qtyFilter.MinQty) > qty))
                    {
                        errInfo = string.Format("{0}数量最大值 {1}, 最小值 {2},输入值 {3}不在允许范围内", symbol, qtyFilter.MaxQty, qtyFilter.MinQty, qty);
                        return false;
                    }

                    var notionalFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "MIN_NOTIONAL");
                    if (notionalFilter != null && decimal.Parse(notionalFilter.MinNotional) > price * qty)
                    {
                        errInfo = string.Format("{0}金额最小值 {1} 小于 {2} = {3} * {4}", symbol, notionalFilter.MinNotional, price * qty, price, qty);
                        return false;
                    }
                }
            }
            else
            {
                lock (sync)
                {
                    exchangeInfo = binanceClient.GetExchangeInfo().Result;
                }
                
            }
            return true;
        }

        public List<AccounCoin> QueryAccountCoin(List<string> listCoin)
        {
            if (listCoin == null || listCoin.Count == 0)
            {
                return dictBalance.Values.Where(_ => _.Free > 0 || _.Locked > 0)
                                   .Select(_ => new AccounCoin() { Asset = _.Asset, Free = _.Free, Locked = _.Locked }).ToList();
            }
            else
            {
                return dictBalance.Values.Where(_ => listCoin.Contains(_.Asset))
                                   .Select(_ => new AccounCoin() { Asset = _.Asset, Free = _.Free, Locked = _.Locked }).ToList();
            }
        }

        #region Handlers
        private void AccountHandler(AccountUpdatedMessage messageData)
        {
            foreach (var item in messageData.Balances)
            {
                if (dictBalance.ContainsKey(item.Asset))
                {
                    dictBalance[item.Asset].Free = item.Free;
                    dictBalance[item.Asset].Locked = item.Locked;
                }
                else
                {
                    dictBalance[item.Asset] = new Binance.API.Csharp.Client.Models.Market.Balance() { Asset = item.Asset, Free = item.Free, Locked = item.Locked };
                }
                
            }
        }

        private void TradesHandler(OrderOrTradeUpdatedMessage messageData)
        {
            if (messageData == null)
            {
                return;
            }
            var existTrade = queueTrade.FirstOrDefault(_ => _.TradeId == messageData.TradeId);
            if (existTrade != null)
            {
                existTrade.Commission = messageData.Commission;
                existTrade.FilledTradesAccumulatedQuantity = messageData.LastFilledTradeQuantity;
                existTrade.LastFilledTradePrice = messageData.LastFilledTradePrice;
                existTrade.LastFilledTradeQuantity = messageData.LastFilledTradeQuantity;
                existTrade.EventTime = messageData.EventTime;
                existTrade.TradeTime = messageData.TradeTime;
                
            }
            else
            {
                var trader = GetOrderTrader(messageData.Orderid);
                if (!string.IsNullOrEmpty(trader))
                {
                    var order = queueOrder.FirstOrDefault(_ => _.Orderid == messageData.Orderid);
                    if (order != null)
                    {
                        messageData.Symbol = order.Symbol;
                        messageData.Side = order.Side;
                    }
                    queueTrade.Enqueue(messageData);
                    
                    
                }
            }
        }

        private void OrdersHandler(OrderOrTradeUpdatedMessage messageData)
        {
            if (messageData == null)
            {
                return;
            }
            lock (OrderQueueAddObj)
            {
                var existOrder = queueOrder.FirstOrDefault(_ => _.Orderid == messageData.Orderid);
                if (existOrder != null)
                {
                    existOrder.Commission = messageData.Commission;
                    existOrder.FilledTradesAccumulatedQuantity = messageData.LastFilledTradePrice;
                    existOrder.LastFilledTradeQuantity = messageData.LastFilledTradeQuantity;
                    existOrder.Status = messageData.Status;
                    existOrder.EventTime = messageData.EventTime;
                    existOrder.TradeTime = messageData.TradeTime;
                    existOrder.TradeId = messageData.TradeId;
                    var cacheItem = queueOrderTraderRelation.FirstOrDefault(_=> _.OrderID == messageData.Orderid);
                    var timeSpan = (long)(DateTime.Now - StartTime).TotalMilliseconds;
                    if (cacheItem != null && cacheItem.LastQueryTime < timeSpan )
                    {
                        cacheItem.LastQueryTime = timeSpan;
                    }
                }
                else
                {
                    var trader = GetOrderTrader(messageData.Orderid);
                    if (!string.IsNullOrEmpty(trader))
                    {
                        queueOrder.Enqueue(messageData);
                    }
                }
            }
            
        }
        #endregion

        #region Query Data
        public void QueryData(out JyDataSet.委托DataTable dtWt, out JyDataSet.成交DataTable dtCJ)
        {
            dtWt = new JyDataSet.委托DataTable();
            dtCJ = new JyDataSet.成交DataTable();

            foreach (var order in queueOrder)
            {
                var trader = GetOrderTrader(order.Orderid);
                if (!string.IsNullOrEmpty(trader))
                {
                    var row = dtWt.New委托Row();
                    row.买卖方向 = order.Side.Contains("BUY") ? 0 : 1;
                    row.交易员 = trader;
                    row.委托价格 = order.Price;
                    row.委托数量 = order.OriginalQuantity;
                    row.委托时间 = StartTime.AddMilliseconds(order.EventTime).ToString("yyyy-MM-dd HH:mm:ss");
                    row.委托编号 = order.Orderid + "";
                    row.市场代码 = 0;
                    row.撤单数量 = 0;
                    row.成交价格 = 0;
                    row.成交数量 = Math.Max(order.FilledTradesAccumulatedQuantity, order.LastFilledTradeQuantity);
                    row.状态说明 = GetStatusDescription(order.Status);
                    row.组合号 = AccountName;
                    row.证券代码 = order.Symbol;
                    row.证券名称 = order.Symbol;
                    InitOrderTradeInfo(order.Orderid, row);
                    if (order.Status == "CANCELED")
                    {
                        row.撤单数量 = row.委托数量 - row.成交数量;
                    }
                    dtWt.Add委托Row(row);
                }
            }

            foreach (var item in queueTrade)
            {
                var trader = GetOrderTrader(item.Orderid);
                if (!string.IsNullOrEmpty(trader))
                {
                    
                    var row = dtCJ.New成交Row();

                    row.买卖方向 = item.Side == "BUY" ? 0 : 1;
                    row.交易员 = trader;
                    row.委托编号 = item.Orderid.ToString();
                    row.市场代码 = 0;
                    row.成交价格 = item.LastFilledTradePrice;
                    row.成交数量 = item.LastFilledTradeQuantity;
                    row.成交时间 = StartTime.AddMilliseconds(item.TradeTime).ToString("yyyy-MM-dd hh:mm:ss");
                    row.成交编号 = item.TradeId.ToString();
                    row.成交金额 = item.LastFilledTradePrice * item.LastFilledTradeQuantity;
                    row.组合号 = this.AccountName;
                    row.证券名称 = item.Symbol;
                    row.证券代码 = item.Symbol;
                    dtCJ.Add成交Row(row);
                }
            }

            if (dtWt.FirstOrDefault(_=> _.委托数量 > (_.成交数量 + _.撤单数量)) != null && DateTime.Now > AutoBanTime)
            {
                List<int> lstOrderID = new List<int>();
                foreach (var item in dtWt.Where(_=> _.委托数量 > (_.成交数量 + _.撤单数量)))
                {
                    var cacheItem = queueOrderTraderRelation.FirstOrDefault(_=> _.OrderID.ToString() == item.委托编号);
                    if (cacheItem != null && (cacheItem.LastQueryTime == cacheItem.SendTime ||(DateTime.Now - StartTime).TotalMilliseconds - cacheItem.LastQueryTime >= 3000))
                    {
                        lstOrderID.Add(cacheItem.OrderID);
                        cacheItem.LastQueryTime = (long)(DateTime.Now - StartTime).TotalMilliseconds;
                    }
                }
                if (lstOrderID.Count > 0)
                {
                    var orders = queueOrder.Where(_ => !string.IsNullOrEmpty(_.Symbol) && lstOrderID.Contains(_.Orderid));
                    var orderIdMin = lstOrderID.Min();
                    var symbols = orders.Select(_ => _.Symbol).Distinct().ToList();
                    Program.logger.LogInfo("{0} query orders {1}, orderID {2}", this.AccountName, string.Join(",", symbols), string.Join(",", orders.Select(_=>_.Orderid)));
                    foreach (var symbol in symbols)
                    {
                        QueryOrders(symbol, orders);
                        Thread.Sleep(500);
                    }
                    Thread.Sleep(500);
                }
            }

            if (dtWt.FirstOrDefault(_ => _.成交数量 > 0 && (_.成交价格 == 0 || (_.委托数量 > _.成交数量 + _.撤单数量))) != null && DateTime.Now > AutoBanTime)
            {
                var lstNeedTradeID = dtWt.Where(_=> _.成交数量 > 0 && (_.成交价格 == 0 || (_.委托数量 > _.成交数量 + _.撤单数量))).Select(_ => _.委托编号).ToList();
                var symbols = queueOrder.Where(_=> lstNeedTradeID.Contains(_.Orderid + "")).Select(_=> _.Symbol).Distinct();
                Program.logger.LogInfo("{0} query trades {1}, orderID {2}", this.AccountName, string.Join(",", symbols), string.Join(",", lstNeedTradeID));
                //var needMax = lstNeedTradeID.Max(_ => int.Parse(_));
                //if (queueTrade.Count > 0 && queueTrade.Min(_=> _.Orderid) > needMax)
                //{
                //    var minTradeID = queueTrade.Min(_ => _.TradeId) -1;
                //    var minTradeTime = queueTrade.Min(_ => _.TradeTime)  - 1000;
                //    var lstNeedEqueu = historyOrder.Where(_ => lstNeedTradeID.Contains(_.委托编号)).ToList();
                //    lstNeedEqueu.ForEach(_ =>
                //        queueTrade.Enqueue(new OrderOrTradeUpdatedMessage() {
                //            FilledTradesAccumulatedQuantity = _.成交数量,
                //            LastFilledTradeQuantity = _.成交数量,
                //            LastFilledTradePrice = _.成交价格,
                //            Price = _.成交价格,
                //            Orderid =int.Parse(_.委托编号),
                //            Side = _.买卖方向 == 0 ? "BUY" : "SELL",
                //            Symbol = _.证券代码,
                //            TradeId = minTradeID--,
                //            TradeTime = minTradeTime,
                //        })
                //    );
                //}
                foreach (string symbol in symbols)
                {
                    QueryTraders(symbol);
                    Thread.Sleep(500);
                }
                Thread.Sleep(500);
            }

            if ((DateTime.Now - lastRequesTime).TotalMinutes >= 1 )
            {
                try
                {
                    binanceClient.TestConnectivity();
                    lastRequesTime = DateTime.Now;
                    //Program.logger.LogInfoDetail("{0} TestConnectivity OK, Time {1}.", this.AccountName, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss fff"));
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("{0} TestConnectivity Exception, StackTrace {1}.", this.AccountName, ex.ToString());
                    Thread.Sleep(30000);
                }
            }
        }

        private void QueryOrders(string symbol, IEnumerable<OrderOrTradeUpdatedMessage> ordersNeedQuery)
        {
            try
            {
                var idNeedQueryMin = ordersNeedQuery.Where(_ => _.Symbol == symbol).Min(_=> _.Orderid);
                //var idBefore = queueOrder.FirstOrDefault(_ => _.Orderid < idNeedQueryMin);
                //if (idBefore != null)
                //{
                //    idNeedQueryMin = queueOrder.Where(_ => _.Orderid < idNeedQueryMin).Max(_ => _.Orderid);
                //}

                IEnumerable <Binance.API.Csharp.Client.Models.Account.Order> orderList  = null;
                DateTime st = DateTime.Now;
                lock (sync)
                {
                    orderList = binanceClient.GetAllOrders(symbol, idNeedQueryMin).Result;
                    lastRequesTime = DateTime.Now;
                    Sleep(st);
                }
                foreach (var item in orderList)
                {
                    //委托与交易员映射关系存在。
                    if (queueOrderTraderRelation.FirstOrDefault(_ => _.OrderID == item.OrderId) != null)
                    {
                        var orderExist = queueOrder.FirstOrDefault(_ => _.Orderid == item.OrderId);
                        if (orderExist != null)
                        {
                            orderExist.FilledTradesAccumulatedQuantity = item.ExecutedQty;
                            orderExist.LastFilledTradeQuantity = item.ExecutedQty;
                            orderExist.EventTime = item.Time;
                            orderExist.Status = item.Status;
                        }
                        else
                        {
                            queueOrder.Enqueue(new OrderOrTradeUpdatedMessage()
                            {
                                LastFilledTradePrice = item.StopPrice,
                                LastFilledTradeQuantity = item.ExecutedQty,
                                FilledTradesAccumulatedQuantity = item.ExecutedQty,
                                OriginalQuantity = item.OrigQty,
                                EventTime = item.Time,
                                Price = item.Price,
                                Symbol = item.Symbol,
                                TradeTime = item.Time,
                                Type = item.Type,
                            });
                        }
                    }
                    else
                    {
                        //映射关系不存在，但在cache中存在价格，代码，数量相同，时间在一分钟内且无委托编号的项。则认为匹配。
                        if (CommonUtils.OrderCacheQueue.Count > 0)
                        {
                            DateTime tradeTime = GetDateTime(item.Time);
                            var orderCache = CommonUtils.OrderCacheQueue.FirstOrDefault(_ =>
                                (tradeTime - _.SendTime).TotalSeconds < 60 
                                && _.Zqdm == item.Symbol
                                && _.Price == item.Price
                                && _.Quantity == item.OrigQty
                                && string.IsNullOrEmpty(_.OrderID));
                            if (orderCache != null)
                            {
                                orderCache.OrderID = item.OrderId.ToString();
                                queueOrderTraderRelation.Enqueue(new OrderTrader(orderCache.Trader, item.OrderId, item.Symbol));
                                queueOrder.Enqueue(new OrderOrTradeUpdatedMessage()
                                {
                                    LastFilledTradePrice = item.StopPrice,
                                    LastFilledTradeQuantity = item.ExecutedQty,
                                    FilledTradesAccumulatedQuantity = item.ExecutedQty,
                                    OriginalQuantity = item.OrigQty,
                                    EventTime = item.Time,
                                    Price = item.Price,
                                    Symbol = item.Symbol,
                                    TradeTime = item.Time,
                                    Type = item.Type,
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Program.logger.LogInfoDetail("{0} Binance query orders Exception: {1}", this.AccountName, (ex.InnerException ?? ex).Message);
                LogException(ex, "BinanceAdapter.QueryOrders");
                Thread.Sleep(10000);
            }
        }

        private void QueryTraders(string symbol)
        {
            try
            {
                IEnumerable<Binance.API.Csharp.Client.Models.Account.Trade> symbolTrades = null;
                DateTime st = DateTime.Now;
                lock (sync)
                {
                    symbolTrades = binanceClient.GetTradeList(symbol).Result;
                    lastRequesTime = DateTime.Now;
                    Sleep(st);
                }
                foreach (var tradeItem in symbolTrades)
                {
                    var tradeCache = queueTrade.FirstOrDefault(_ => _.TradeId == tradeItem.Id);
                    if (tradeCache == null)
                    {
                        var traderInfo = GetOrderTrader(tradeItem.OrderId);
                        if (!string.IsNullOrEmpty(traderInfo))
                        {
                            var order = queueOrder.First(_ => _.Orderid == tradeItem.OrderId);
                            var tradeMsg = new OrderOrTradeUpdatedMessage()
                            {
                                Orderid = tradeItem.OrderId,
                                TradeId = tradeItem.Id,
                                Price = tradeItem.Price,
                                FilledTradesAccumulatedQuantity = tradeItem.Quantity,
                                LastFilledTradeQuantity = tradeItem.Quantity,
                                LastFilledTradePrice = tradeItem.Price,
                                Commission = tradeItem.Commission,
                                CommissionAsset = tradeItem.CommissionAsset,
                                TradeTime = tradeItem.Time,
                                Symbol = order.Symbol,
                                Side = order.Side,
                            };
                            
                            queueTrade.Enqueue(tradeMsg);
                        }
                    }
                    else
                    {
                        tradeCache.Price = tradeItem.Price;
                        tradeCache.FilledTradesAccumulatedQuantity = tradeItem.Quantity;
                        tradeCache.LastFilledTradeQuantity = tradeItem.Quantity;
                        tradeCache.LastFilledTradePrice = tradeItem.Price;
                        tradeCache.Commission = tradeItem.Commission;
                        tradeCache.CommissionAsset = tradeItem.CommissionAsset;
                        tradeCache.TradeTime = tradeItem.Time;
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex, "BinanceAdapter.QueryTraders Exception");
                Thread.Sleep(10000);
            }

        }

        private string GetOrderTrader(int orderID)
        {
            var traderInfo = queueOrderTraderRelation.FirstOrDefault(_ => _.OrderID == orderID);
            if (traderInfo == null)
            {
                return string.Empty;
            }
            else
            {
                return traderInfo.Trader;
            }
        }

        private void InitOrderTradeInfo(int orderID, JyDataSet.委托Row row)
        {
            if (row.成交数量 > 0)
            {
                decimal qty = 0;
                decimal amount = 0;
                var trades = queueTrade.Where(_ => _.Orderid == orderID).ToList();
                if (trades.Count() > 0)
                {
                    foreach (var item in trades)
                    {
                        qty += Math.Max(item.FilledTradesAccumulatedQuantity, item.LastFilledTradeQuantity);
                        amount += item.Price * Math.Max(item.FilledTradesAccumulatedQuantity, item.LastFilledTradeQuantity);
                    }
                    row.成交数量 = Math.Max(qty, row.成交数量);
                    row.成交价格 = qty == 0 ? 0 : Math.Round(amount / qty, 8);
                }
            }
        }

        private void QueryServerTime()
        {
            if (IsServerTimeChecked)
            {
                return;
            }
            try
            {

                var serverTime = GetDateTime(binanceClient.GetServerTime().Result.ServerTime);
                Thread.Sleep(300);
                if ((DateTime.Now - serverTime).TotalSeconds >= 0.5)
                {
                    Program.logger.LogInfo("Binance接口获取时间比本地时间慢超过0.5秒，建议调慢当前机器时间，或进行时间同步，否则主动查询接口可能异常!");
                }
                else
                {
                    Program.logger.LogInfo("Binance接口获取时间{1}，本地时间{2}，时间验证正常。", AccountName, serverTime.ToString("HH:mm:ss fff"), DateTime.Now.ToString("HH:mm:ss fff"));
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("{0} 时间验证异常：{1}", AccountName, (ex.InnerException ?? ex).Message);
            }
            IsServerTimeChecked = true;
        }

        DateTime lasQueryCoinTime = DateTime.MinValue;
        public decimal QueryAccountCoin(string coin)
        {
            var st = DateTime.Now;
            if ((DateTime.Now - lasQueryCoinTime).TotalMilliseconds >= 1000 && DateTime.Now > AutoBanTime)
            {
                lock (sync)
                {
                    var accInfo = binanceClient.GetAccountInfo().Result;
                    lastRequesTime = DateTime.Now;
                    foreach (var item in accInfo.Balances)
                    {
                        dictBalance[item.Asset] = item;
                    }
                    Sleep(st);
                }
            }

            return dictBalance.ContainsKey(coin) ? dictBalance[coin].Free : 0;
        }
        #endregion

        private static string GetStatusDescription(string orderStatus)
        {
            switch (orderStatus)
            {
                case "NEW":
                    return "已发";
                case "PARTIALLY_FILLED":
                    return "部成";
                case "FILLED":
                    return "已成";
                case "CANCELED":
                    return "已撤";
                case "PENDING_CANCEL":
                    return "等待撤单";
                case "REJECTED":
                    return "已拒绝";
                case "EXPIRED":
                    return "已过期";
                default:
                    return orderStatus;
            }
        }

        private static string GetStatusByDescription(string orderStatus)
        {
            switch (orderStatus)
            {
                case "已发":
                    return "NEW";
                case "部成":
                    return "PARTIALLY_FILLED";
                case "已成":
                    return "FILLED";
                case "已撤":
                    return "CANCELED";
                case "等待撤单":
                    return "PENDING_CANCEL";
                case "已拒绝":
                    return "REJECTED";
                case "已过期":
                    return "EXPIRED";
                case "委托成功":
                    return "NEW";
                default:
                    return orderStatus;
            }
        }

        private static bool IsFinalStatus(string orderStatus)
        {
            switch (orderStatus)
            {
                case "CANCELED":
                case "EXPIRED":
                case "FILLED":
                case "REJECTED":
                    return true;
                case "NEW":
                case "PARTIALLY_FILLED":
                case "PENDING_CANCEL":
                default:
                    return false;
            }
        }

        private static DateTime GetDateTime(long milliseconds)
        {
            return StartTime.AddMilliseconds(milliseconds);

        }

        private void Sleep(DateTime st)
        {
            var span = DateTime.Now - st;
            if (span.TotalMilliseconds < stopMiniSeconds)
            {
                Thread.Sleep(stopMiniSeconds - (int)span.TotalMilliseconds);
            }
        }

        class OrderTrader
        {
            public OrderTrader(string trader, int orderID, string symbol)
            {
                Trader = trader;
                OrderID = orderID;
                Symbol = symbol;
                SendTime = (long)(StartTime - DateTime.Now).TotalMilliseconds;
                LastQueryTime = SendTime;
            }

            public string Trader { get; set; }

            public int OrderID { get; set; }

            public long SendTime { get; set; }

            public string Symbol { get; set; }

            public string Side { get; set; }

            public long LastQueryTime { get; set; }

        }

    }
}
