using Binance.API.Csharp.Client;
using Binance.API.Csharp.Client.Models.General;
using Binance.API.Csharp.Client.Models.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AASClient.MarketAdapter
{
    public class BinanceAdapter: IAdapter
    {
        private static object sync = new object();

        public MarketCallback<PartialDepthMessage> MCallBack;
        public TransactionCallback<AggregateTradeMessage> TCallBack;
        public ExchangeInfo exchangeInfo;
        #region Properties
        private static BinanceAdapter _instance;
        public static BinanceAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new BinanceAdapter();
                        }
                    }
                }
                return _instance;
            }
        }
        
        private BinanceClient Client { get; set; }
        private List<string> Symbles { get; set; }
        private ConcurrentQueue<PartialDepthMessage> DepthQueue { get; set; }
        private ConcurrentQueue<AggregateTradeMessage> TradeQueue { get; set; } 
        private ConcurrentDictionary<string, SymbolRound> RoundDict { get; set; }
        public ConcurrentDictionary<string, decimal> PriceDict { get; set; }
        #endregion

        private BinanceAdapter()
        {
            Symbles = new List<string>();
            DepthQueue = new ConcurrentQueue<PartialDepthMessage>();
            TradeQueue = new ConcurrentQueue<AggregateTradeMessage>();
            RoundDict = new ConcurrentDictionary<string, SymbolRound>();
            PriceDict = new ConcurrentDictionary<string, decimal>();
            Task.Run(()=> {
                ApiClient apiClient = new ApiClient("", "");
                Client = new BinanceClient(apiClient, false);
                Client.GetExchangeInfo().ContinueWith(_ => exchangeInfo = _.Result);

                Thread DepthDataThread = new Thread(new ThreadStart(DepthMain)) { IsBackground = true };
                DepthDataThread.Start();

                Thread TradeDataThread = new Thread(new ThreadStart(TradeMain)) { IsBackground = true };
                TradeDataThread.Start();

                Thread heartbeatThread = new Thread(new ThreadStart(HeartBeatMain)) { IsBackground = true };
                heartbeatThread.Start();

                Subscribe("ethbtc");
            });
        }

        private void HeartBeatMain()
        {
            while (true)
            {
                try
                {
                    Client.TestConnectivity();
                    Thread.Sleep(1200);
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo((ex.InnerException ?? ex).Message);
                    ReConnect();
                }
            }
        }

        private void ReConnect()
        {
            lock (sync)
            {
                try
                {
                    ApiClient apiClient = new ApiClient("", "");
                    Client = new BinanceClient(apiClient, false);
                    foreach (var item in Symbles)
                    {
                        Client.ListenTradeEndpoint(item, AggregateTradesHandler);
                        Client.ListenPartialDepthEndpoint(item, PartialDepthHandler);
                    }
                }
                catch  { }
            }
        }

        private void DepthMain()
        {
            PartialDepthMessage msg;
            while (true)
            {
                try
                {
                    if (DepthQueue.Count > 0 && DepthQueue.TryDequeue(out msg) && msg.Stream != null)
                    {
                        var stockID = msg.Stream.Substring(0, msg.Stream.IndexOf('@')).ToUpper();
                        if (MCallBack != null)
                        {
                            PriceDict[stockID] = msg.Asks.First().Price;
                            MCallBack.Invoke(stockID, msg);
                        }
                        Thread.Sleep(1);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch { }  
            }
        }

        private void TradeMain()
        {
            while (true)
            {
                if (TradeQueue.Count > 0 && TradeQueue.TryDequeue(out AggregateTradeMessage msg))
                {
                    if (TCallBack != null && msg.Symbol != null)
                    {
                        TCallBack.Invoke(msg.Symbol, msg);
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void PartialDepthHandler(PartialDepthMessage message)
        {
            try
            {
                DepthQueue.Enqueue(message);
            }
            catch (Exception ex)
            {
                Program.logger.LogRunning("PartialDepthHandler Exception: {0}", ex.Message);
            }
            
        }

        private void AggregateTradesHandler(AggregateTradeMessage message)
        {
            TradeQueue.Enqueue(message);
        }
        
        /// <summary>
        /// 行情订阅
        /// </summary>
        /// <param name="symble"></param>
        public void Subscribe(string symble)
        {
            lock (sync)
            {
                symble = symble.ToLower();
                if (!Symbles.Contains(symble))
                {
                    Symbles.Add(symble);
                    Client.ListenTradeEndpoint(symble, AggregateTradesHandler);
                    Client.ListenPartialDepthEndpoint(symble, PartialDepthHandler);

                }
            }
            
        }

        public void UnSubscribe(string symble)
        {
            lock (sync)
            {
                if (Symbles.Contains(symble))
                {
                    Symbles.Remove(symble);
                    //取消订阅考虑使用单独线程进行验证，确认不需要对应信息时再取消。
                    //目前无对应接口，先不处理。
                }
            }
        }

        public void GetRoundNum(string symbol, out int priceRoundDigit, out int qtyRoundDigit, out int notionalDigit)
        {
            priceRoundDigit = 8;
            qtyRoundDigit = 4;
            notionalDigit = 8;
            var symbolUp = symbol.ToUpper();
            if (RoundDict.ContainsKey(symbolUp))
            {
                notionalDigit = RoundDict[symbolUp].NotionRound;
                priceRoundDigit = RoundDict[symbolUp].PriceRound;
                qtyRoundDigit = RoundDict[symbolUp].QtyRound;
            }
            else if (exchangeInfo != null)
            {
                var filter = exchangeInfo.Symbols.FirstOrDefault(_ => _.Symbol.Equals(symbolUp));
                if (filter != null)
                {
                    try
                    {
                        var priceFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "PRICE_FILTER");
                        priceRoundDigit = BinanceUtils.GetDigit(decimal.Parse(priceFilter.MinPrice.TrimEnd('0')));

                        var qtyFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "LOT_SIZE");
                        qtyRoundDigit = BinanceUtils.GetDigit(decimal.Parse(qtyFilter.MinQty.TrimEnd('0')));

                        var notionalFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "MIN_NOTIONAL");
                        notionalDigit = BinanceUtils.GetDigit(decimal.Parse(notionalFilter.MinNotional.TrimEnd('0')));

                        RoundDict[symbolUp] = new SymbolRound() { NotionRound = notionalDigit, PriceRound = priceRoundDigit, QtyRound = qtyRoundDigit };
                    }
                    catch { }
                }
            }
        }

        public decimal GetMinQty(string symbol)
        {
            var symbolUp = symbol.ToUpper();
            if (exchangeInfo != null)
            {
                var filter = exchangeInfo.Symbols.FirstOrDefault(_ => _.Symbol.Equals(symbolUp));
                if (filter != null)
                {
                    try
                    {
                        var qtyFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "LOT_SIZE");
                        return decimal.Parse(qtyFilter.MinQty.TrimEnd('0'));
                    }
                    catch { }
                }
            }
            return 0;
        }

        public void GetFilter(string symbol,out decimal priceMin, out decimal qtyMin, out decimal notionMin)
        {
            priceMin = 0;
            qtyMin = 0;
            notionMin = 0;
            var symbolUp = symbol.ToUpper();
            if (exchangeInfo != null)
            {
                var filter = exchangeInfo.Symbols.FirstOrDefault(_ => _.Symbol.Equals(symbolUp));
                if (filter != null)
                {
                    try
                    {
                        var priceFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "PRICE_FILTER");
                        priceMin = decimal.Parse(priceFilter.MinPrice.TrimEnd('0'));

                        var qtyFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "LOT_SIZE");
                        qtyMin = decimal.Parse(qtyFilter.MinQty.TrimEnd('0'));

                        var notionalFilter = filter.Filters.FirstOrDefault(_ => _.FilterType == "MIN_NOTIONAL");
                        notionMin = decimal.Parse(notionalFilter.MinNotional.TrimEnd('0'));
                    }
                    catch { }
                }
            }
        }



        struct SymbolRound
        {
            public int NotionRound;
            public int PriceRound;
            public int QtyRound;
        }
    }

}
