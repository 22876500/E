using Newtonsoft.Json;

namespace Binance.API.Csharp.Client.Models.WebSocket
{
    public class AggregateTradeMessage
    {        
        public string EventType { get; set; }        
        public long EventTime { get; set; }        
        public string Symbol { get; set; }        
        public int AggregatedTradeId { get; set; }        
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }        
        public int FirstBreakdownTradeId { get; set; }        
        public int LastBreakdownTradeId { get; set; }        
        public long TradeTime { get; set; }       
        public bool BuyerIsMaker { get; set; }
    }
}
