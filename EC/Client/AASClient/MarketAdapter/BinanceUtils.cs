using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASClient.MarketAdapter
{
    public static class BinanceUtils
    {
        static List<string> TradeUnit = new List<string>() { "BNB", "BTC", "ETH", "USDT" };

        public static KeyValuePair<string, string> GetBinanceCoinInfo(string stockID)
        {
            var unit = TradeUnit.First(_ => stockID.EndsWith(_, StringComparison.CurrentCultureIgnoreCase));
            return new KeyValuePair<string, string>(stockID.Substring(0, stockID.Length - unit.Length).ToUpper(), unit.ToUpper());
        }

        public static int GetDigit(decimal d)
        {
            return d > decimal.Truncate(d) ? d.ToString().Split('.')[1].Length : 0;
        }

        public static string GetShortStr(decimal d)
        {
            return d > decimal.Truncate(d) ? d.ToString().TrimEnd('0') : decimal.Truncate(d).ToString();
        }
    }
}
