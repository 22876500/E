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
        public static void PartialDepthHandler(PartialDepthMessage messageData)
        {
            var depthData = messageData;
            Console.WriteLine(depthData.LastUpdateId.ToString());
            Console.WriteLine(depthData.Stream);
            foreach (Binance.API.Csharp.Client.Models.Market.OrderBookOffer t in depthData.Asks)
            {
                Console.Write(t.Price.ToString(), t.Quantity.ToString(), " ");
            }
            Console.WriteLine();
        }

        private static void AggregateTradesHandler(AggregateTradeMessage messageData)
        {
            var aggregateTrades = messageData;
            Console.WriteLine(messageData.Price.ToString());
        }
        private static void AccountHandler(AccountUpdatedMessage messageData)
        {
            var accountData = messageData;
        }

        private static void TradesHandler(OrderOrTradeUpdatedMessage messageData)
        {
            var tradesData = messageData;
        }
        private static void OrdersHandler(OrderOrTradeUpdatedMessage messageData)
        {
            var ordersData = messageData;
            Console.WriteLine(ordersData.TradeId.ToString());
            Console.WriteLine(ordersData.Orderid.ToString());
            Console.WriteLine(ordersData.Symbol.ToString());
            Console.WriteLine(ordersData.Side.ToString());
            Console.WriteLine(ordersData.EventTime.ToString());
            Console.WriteLine(ordersData.Type.ToString());
            Console.WriteLine(ordersData.Status.ToString());
        }
        static void Main(string[] args)
        {
            //ApiClient apiClient = new ApiClient("", "");
            ApiClient apiClient = new ApiClient("6xsi2bJRs2walo2wdGACHA77SiXcYOhsC7uv2Y6vJOf6XyrIWtNqT5fho7HvfOAD", "VgSHN3IB8Ss8BIfB9CfKm96uvV6oDTUWdoEIT5hgqXZixw3MNj2MZXipv8R6QIZO");//实盘
            //"@37m9DafnGbTZXEeSkwEw4MI5WrJ3H9lLNW42zdS8bHVeFFfeNA0gauGn7baukCRp", "@2x10SdNLY1K9oLvZHwPCWNeZEhRnlAuM9d281At8pCofHkGMfPQPh4qe3NhwR7jh");
            BinanceClient binanceClient = new BinanceClient(apiClient, false);
            
            var test = binanceClient.TestConnectivity().Result;
            Console.WriteLine(test);
            //var serverTime = binanceClient.GetServerTime().Result;
            //Console.WriteLine(serverTime.ServerTime);
            //var listenKey = binanceClient.StartUserStream().Result.ListenKey;
            //var ping = binanceClient.KeepAliveUserStream(listenKey).Result;
            //binanceClient.ListenUserDataEndpoint(AccountHandler, TradesHandler, OrdersHandler);

            //var accountInfo = binanceClient.GetAccountInfo().Result;
            //var testOrder = binanceClient.PostNewOrder("trxbtc", 200m, 0.00001m, OrderSide.BUY).Result;

            //Thread.Sleep(5000);
            var allOrders = binanceClient.GetAllOrders("trxbtc").Result;
            foreach (Binance.API.Csharp.Client.Models.Account.Order t in allOrders)
            {
                Console.WriteLine(t.ClientOrderId.ToString());
                Console.WriteLine(t.OrderId.ToString());
                Console.WriteLine(t.Price.ToString());
                Console.WriteLine(t.Side.ToString());
                Console.WriteLine(t.Time.ToString());
                Console.WriteLine(t.ExecutedQty.ToString());
                Console.WriteLine(t.Status.ToString());
                Console.WriteLine();
            }
            //try
            //{
            //    var canceledOrder = binanceClient.CancelOrder("trxbtc", 13693965).Result;
            //}
            //catch (Exception e)
            //{
            //    Console.Write(e.Message.ToString());
            //}

            //var testOrder = binanceClient.PostNewOrderTest("trxbtc", 1m, 0.1m, OrderSide.BUY).Result;
            var tradeList = binanceClient.GetTradeList("ethbtc").Result;
            foreach (Binance.API.Csharp.Client.Models.Account.Trade t in tradeList)
            {
                Console.WriteLine(t.Id.ToString());
                Console.WriteLine(t.Price.ToString());
                Console.WriteLine(t.Quantity.ToString());
                Console.WriteLine(t.IsBuyer.ToString());
                Console.WriteLine(t.IsMaker.ToString());
                Console.WriteLine(t.Time.ToString());
                Console.WriteLine(t.IsBestMatch.ToString());
                Console.WriteLine(t.Commission.ToString());
                Console.WriteLine(t.CommissionAsset.ToString());
            }

            //Thread.Sleep(500000);
            Console.ReadLine();

        }

        private static void GetMarketAndTransactionData()
        {
            //ApiClient apiClient = new ApiClient("", "");
            //BinanceClient binanceClient = new BinanceClient(apiClient, false);
            //var test_c = new BinanceTest();
            //var orderBook = binanceClient.GetOrderBook("ethbtc").Result;
            //binanceClient.ListenTradeEndpoint("ethbtc", AggregateTradesHandler);
            //Thread.Sleep(50000);

            //binanceClient.ListenDepthEndpoint("ethbtc", Program.DepthHandler);
            //binanceClient.ListenPartialDepthEndpoint("ethbtc", PartialDepthHandler);
            //Thread.Sleep(50000);
        }
    }
}
