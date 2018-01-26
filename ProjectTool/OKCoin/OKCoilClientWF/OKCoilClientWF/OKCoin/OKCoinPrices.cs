using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKCoilClientWF.OKCoin
{
    public class OKCoinPrices
    {
        public double ThisWeekBuy { get; set; }

        public double ThisWeekSale { get; set; }

        public double NextWeekBuy { get; set; }

        public double NextWeekSale { get; set; }

        public double QuarterBuy { get; set; }

        public double QuaterSale { get; set; }

        public double PriceDiffOpen { get { return Math.Round(QuarterBuy - ThisWeekSale); } }

        public double PriceDiffClose { get { return Math.Round(QuaterSale - ThisWeekBuy); } }

        public double NextWeekPriceOpen { get { return Math.Round(QuarterBuy - NextWeekSale); } }

        public double NextWeekPriceClose { get { return Math.Round(QuaterSale - NextWeekBuy); } }
    }
}
