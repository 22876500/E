using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotaShareClient
{
    public class Quota
    {
        public string Trader { get; set; }
        public string Code { get; set; }
        public string Group { get; set; }
        public int Market { get; set; }
        public string CodeName { get; set; }
        public string PinYin { get; set; }
        public int BuyType { get; set; }
        public int SellType { get; set; }
        public decimal TradeQuota { get; set; }
        public decimal Rate { get; set; }
    }
}
