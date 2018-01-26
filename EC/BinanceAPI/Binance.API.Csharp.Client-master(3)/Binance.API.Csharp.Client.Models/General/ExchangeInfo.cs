using Newtonsoft.Json;
using System.Collections.Generic;
namespace Binance.API.Csharp.Client.Models.General
{
    public class ExchangeInfo
    {
        [JsonProperty("timezone")]
        public string TimeZone { get; set; }
        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }
        [JsonProperty("rateLimits")]
        public IEnumerable<RateLimit> RateLimits { get; set; }
        [JsonProperty("symbols")]
        public IEnumerable<Symbols> Symbols { get; set; }
    }
    public class RateLimit
    {
        [JsonProperty("rateLimitType")]
        public string RateLimitType { get; set; }
        [JsonProperty("interval")]
        public string Interval { get; set; }
        [JsonProperty("limit")]
        public decimal Limit { get; set; }
    }
    public class Symbols
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("baseAsset")]
        public string BaseAsset { get; set; }
        [JsonProperty("baseAssetPrecision")]
        public decimal BaseAssetPrecision { get; set; }
        [JsonProperty("quoteAsset")]
        public string QuoteAsset { get; set; }
        [JsonProperty("quotePrecision")]
        public decimal quotePrecision { get; set; }
        [JsonProperty("orderTypes")]
        public IEnumerable<string> orderTypes { get; set; }
        [JsonProperty("icebergAllowed")]
        public bool icebergAllowed { get; set; }
        [JsonProperty("filters")]
        public IEnumerable<Filter> Filters { get; set; }
    }
    public class Filter
    {
        [JsonProperty("filterType")]
        public string FilterType { get; set; }
        [JsonProperty("minPrice")]
        public string MinPrice { get; set; }
        [JsonProperty("maxPrice")]
        public string MaxPrice { get; set; }
        [JsonProperty("tickSize")]
        public decimal TickSize { get; set; }
        [JsonProperty("minQty")]
        public string MinQty { get; set; }
        [JsonProperty("maxQty")]
        public string MaxQty { get; set; }
        [JsonProperty("stepSize")]
        public string StepSize { get; set; }
        [JsonProperty("minNotional")]
        public string MinNotional { get; set; }
    }
}
