using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKCoinClient.OKCoinEntities
{
    /// <summary>
    /// OKCoin类型： btc, ltc, eth, etc, bch 
    /// </summary>
    public class OKCoinType
    {
        public const string Btc = "btc";

        public const string Ltc = "ltc";

        public const string Eth = "eth";

        public const string Etc = "etc";

        public const string Bch = "bch";

        public static string GetCoinType(string str)
        {
            if (str.Contains(Btc))
            {
                return Btc;
            }
            else if (str.Contains(Ltc))
            {
                return Ltc;
            }
            else if (str.Contains(Eth))
            {
                return Eth;
            }
            else if (str.Contains(Etc))
            {
                return Eth;
            }
            else if (str.Contains(Bch))
            {
                return Bch;
            }
            else
            {
                return null;
            }
        }
    }
}
