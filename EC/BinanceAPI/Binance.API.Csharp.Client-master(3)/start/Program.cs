using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.API.Csharp.Client.Test;
using Binance.API.Csharp.Client;
using Binance.API.Csharp.Client.Models.Enums;
using Binance.API.Csharp.Client.Models.WebSocket;
using Binance.API.Csharp.Client.Models.Market;
using System.Threading;

namespace start
{
    class Program
    {
        private static Logger logger = new Logger();

        public static void PartialDepthHandler(PartialDepthMessage messageData)
        {
            //var depthData = messageData;
            //Console.WriteLine(depthData.LastUpdateId.ToString());
            //Console.WriteLine(depthData.Stream);
            
        }

        private static void AggregateTradesHandler(AggregateTradeMessage messageData)
        {
            var aggregateTrades = messageData;
            //Console.WriteLine(messageData.Price.ToString());
            //logger.LogInfo("Symbol {0}, Qty {1}, Price {2}, BuyerIsMaker {3}", messageData.Symbol, messageData.Quantity, messageData.Price, messageData.BuyerIsMaker);
        }
        private static void AccountHandler(AccountUpdatedMessage messageData)
        {
            var accountData = messageData;
            logger.LogInfo(Newtonsoft.Json.JsonConvert.SerializeObject(messageData));
        }

        private static void TradesHandler(OrderOrTradeUpdatedMessage messageData)
        {
            var tradesData = messageData;
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(messageData));
        }
        private static void OrdersHandler(OrderOrTradeUpdatedMessage messageData)
        {
            var ordersData = messageData;
            //Console.WriteLine(ordersData.TradeId.ToString());
            //Console.WriteLine(ordersData.Orderid.ToString());
            //Console.WriteLine(ordersData.Symbol.ToString());
            //Console.WriteLine(ordersData.Side.ToString());
            //Console.WriteLine(ordersData.EventTime.ToString());
            //Console.WriteLine(ordersData.Type.ToString());
            //Console.WriteLine(ordersData.Status.ToString());
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(messageData));
        }
        static void Main(string[] args)
        {
            //logger.Init();
            try
            {
                ApiClient apiClient = new ApiClient("", "");
                //TQ2CjjiiMOGcaVflu9vq0wWIBIPiZzVVRyV6BSIlfyhzSOpxTpNLyYlGefa5dZQ8,pGTCCylx4Yb2WFTPyNueFlxkgZCrqQm0ueS605TrEsLbHh3ygve0xka5HBHJCOHD
                //ApiClient apiClient = new ApiClient("TQ2CjjiiMOGcaVflu9vq0wWIBIPiZzVVRyV6BSIlfyhzSOpxTpNLyYlGefa5dZQ8", "pGTCCylx4Yb2WFTPyNueFlxkgZCrqQm0ueS605TrEsLbHh3ygve0xka5HBHJCOHD");
                //ApiClient apiClient = new ApiClient("6xsi2bJRs2walo2wdGACHA77SiXcYOhsC7uv2Y6vJOf6XyrIWtNqT5fho7HvfOAD", "VgSHN3IB8Ss8BIfB9CfKm96uvV6oDTUWdoEIT5hgqXZixw3MNj2MZXipv8R6QIZO");
                Console.WriteLine("Init client finish");
                //"@37m9DafnGbTZXEeSkwEw4MI5WrJ3H9lLNW42zdS8bHVeFFfeNA0gauGn7baukCRp", "@2x10SdNLY1K9oLvZHwPCWNeZEhRnlAuM9d281At8pCofHkGMfPQPh4qe3NhwR7jh");
                BinanceClient binanceClient = new BinanceClient(apiClient, false);

                var serverTime = binanceClient.GetServerTime().Result;
                Console.WriteLine("Server Time {0}, Client Time {1}", GetTime(serverTime.ServerTime), DateTime.Now.ToString("HH:mm:ss fff"));
                SetSystemDateTime.SetLocalTimeByStr(DateTime.Now.ToString());
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff"));

                var testOrder = binanceClient.PostNewOrder("trxbtc", 2000m, 0.000001m, OrderSide.BUY).Result;
                //var accountInfo = binanceClient.GetAccountInfo().Result;
                //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(accountInfo));
                //var allOrders = binanceClient.GetAllOrders("trxbtc", 21293338).Result;

                //var order = allOrders.First(_ => _.OrderId == 21293338);
                //Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(order));
                //Console.WriteLine(" orderID:{0}, time {1}", order.OrderId, GetTIme(order.Time));
                //var trades = binanceClient.GetTradeList("trxbtc").Result;
                //var list = trades.ToList();
                //decimal qty = 0;
                //decimal cash = 0;
                //foreach (var item in list)
                //{
                //    qty += item.Quantity;
                //    cash += item.Quantity * item.Price;
                //    Console.WriteLine(" id:{0}, time:{1}, qty:{2}", item.Id, GetTIme(item.Time), item.Quantity);
                //}
                //Console.WriteLine("qty:{0}, cash {1}, price {2}", qty, cash, qty > 0 ? cash / qty:0);
            }
            catch (Exception ex)
            {
                
                CheckAutoBanInfo(ex);
            }

            //var test_c = new BinanceTest();
            //var orderBook = binanceClient.GetOrderBook("xlmbtc").Result;
            //binanceClient.ListenTradeEndpoint("xlmbtc", AggregateTradesHandler);
            //Thread.Sleep(50000);

            //binanceClient.ListenDepthEndpoint("ethbtc", Program.DepthHandler);
            //binanceClient.ListenPartialDepthEndpoint("ethbtc", PartialDepthHandler);



            //var serverTime = binanceClient.GetServerTime().Result;
            //Console.WriteLine(serverTime.ServerTime);
            //var listenKey = binanceClient.StartUserStream().Result.ListenKey;
            //var ping = binanceClient.KeepAliveUserStream(listenKey).Result;

            //binanceClient.ListenUserDataEndpoint(AccountHandler, TradesHandler, OrdersHandler);
            //var exchangeinfo = binanceClient.GetExchangeInfo().Result;
            //Console.WriteLine(exchangeinfo == null ? "null" : Newtonsoft.Json.JsonConvert.SerializeObject(exchangeinfo));

            //var accountInfo = binanceClient.GetAccountInfo().Result;
            //var info = (Newtonsoft.Json.JsonConvert.SerializeObject(accountInfo));
            //logger.LogWarn(info);
            //var testOrder = binanceClient.PostNewOrder("trxbtc", 2000m, 0.000001m, OrderSide.BUY).Result;
            //Thread.Sleep(5000);

            //var allOrders = binanceClient.GetAllOrders("xlmbtc").Result;

            //foreach (Binance.API.Csharp.Client.Models.Account.Order t in allOrders)
            //{
            //    if (t.OrderId == 6287916 || t.OrderId == 6287965 || t.OrderId == 6288095)
            //    {
            //        Console.WriteLine("ExecutedQty {0}， OrderId {1}, Qty {2}, Price {3}, Side {4}, Status {5}", t.ExecutedQty, t.OrderId, t.OrigQty, t.Price, t.Side, t.Status);
            //    }
            //}

            //try
            //{
            //    var canceledOrder = binanceClient.CancelOrder("trxbtc", 13693965).Result;
            //}
            //catch (Exception e)
            //{
            //    Console.Write(e.Message.ToString());
            //}

            //var testOrder = binanceClient.PostNewOrder("trxbtc", 1m, 0.1m, OrderSide.BUY).Result;
            //var tradeList = binanceClient.GetTradeList("ethbtc").Result;
            //foreach (Binance.API.Csharp.Client.Models.Account.Trade t in tradeList)
            //{
            //    Console.WriteLine(t.Id.ToString());
            //    Console.WriteLine(t.Price.ToString());
            //    Console.WriteLine(t.Quantity.ToString());
            //    Console.WriteLine(t.IsBuyer.ToString());
            //    Console.WriteLine(t.IsMaker.ToString());
            //    Console.WriteLine(t.Time.ToString());
            //    Console.WriteLine(t.IsBestMatch.ToString());
            //    Console.WriteLine(t.Commission.ToString());
            //    Console.WriteLine(t.CommissionAsset.ToString());                
            //}
            Console.ReadLine();

        }
        static DateTime AutoBanTime = DateTime.MinValue;
        private static void CheckAutoBanInfo(Exception ex)
        {
            string s = (ex.InnerException ?? ex).Message;
            if (s.Contains("IP banned until "))
            {
                CeckTimePoint(s);
            }
            else
            {
                Console.WriteLine((ex.InnerException ?? ex).Message);
            }
        }

        private static void CeckTimePoint(string s)
        {
            if (s == null)
            {
                return;
            }

            long tick = 0;
            if (long.TryParse(s.Substring(s.IndexOf("until ") + 6, s.IndexOf(".'}") - s.IndexOf("until ") - 6), out tick))
            {
                AutoBanTime = (new DateTime(1970, 1, 1, 8, 0, 0)).AddMilliseconds(tick);
                Console.WriteLine(s.Replace(tick.ToString(), AutoBanTime.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }

        private static string GetTime(long s)
        {
            return  (new DateTime(1970, 1, 1, 8, 0, 0)).AddMilliseconds(s).ToString("HH:mm:ss fff");
        }
    }
}
