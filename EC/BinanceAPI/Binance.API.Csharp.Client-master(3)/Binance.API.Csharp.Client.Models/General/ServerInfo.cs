using Newtonsoft.Json;

namespace Binance.API.Csharp.Client.Models.General
{
    public class ServerInfo
    {
        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }
    }
}
