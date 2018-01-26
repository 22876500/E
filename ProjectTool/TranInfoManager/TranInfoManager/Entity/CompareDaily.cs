using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranInfoManager.Entity
{
    public  class CompareDaily
    {
        public CompareDaily()
        { 
        
        }

        public CompareDaily(MarketDetailDaily m, string trader)
        {
            this.TRADER = trader;
            this.Comm = m.TotalCommission_WithoutFeesAndCharges;
            this.DATE = m.TradeDate.Date;
            this.ECN = m.ECNRebate;
            this.Gross = m.FirstMoney;
            this.Shares = m.ExQuan;
            this.Symbol2 = m.Symbol2;
            this.Other = m.SecFee + m.NasdTradingActivityFee + m.ExchangeFee + m.NSCCPassThru + m.SIPC + m.FTT;
        }


        [Key, Column(Order=0)]
        public DateTime DATE { get; set; }

        public string DateFormat { get { return DATE.ToString("yyyy/MM/dd"); } }

        [Key, Column(Order = 1)]
        public string TRADER { get; set; }

        [Key, Column(Order = 2)]
        public string Symbol2 { get; set; }

        public int Shares { get; set; }

        public decimal Gross { get; set; }

        public decimal Comm { get; set; }

        public decimal ECN { get; set; }

        public decimal Other { get; set; }

        public decimal Net { get { return Gross - Other - Comm - ECN; } }

        public void Add(MarketDetailDaily m)
        {
            this.Comm += m.TotalCommission_WithoutFeesAndCharges;
            this.ECN += m.ECNRebate;
            this.Gross += m.FirstMoney;
            this.Shares += m.ExQuan;
            this.Other += m.SecFee + m.NasdTradingActivityFee + m.ExchangeFee + m.NSCCPassThru + m.SIPC + m.FTT;
        }

        public int Seq { get; set; }

        public void FormatData()
        {
            this.Comm = Math.Round(this.Comm, 2);
            this.ECN = Math.Round(this.ECN, 2);
            this.Gross = Math.Round(this.Gross, 2);
            this.Other = Math.Round(this.Other, 2);
        }
    }

    public class CompareTrader
    {
        public CompareTrader()
        {
        
        }

        public CompareTrader(DateTime dtStart, DateTime dtEnd, CompareDaily o)
        {
            this.StartDate = dtStart;
            this.EndDate = dtEnd;
            this.Trader = o.TRADER;

            this.Symbol2 = "小计";
            this.Shares = o.Shares;
            this.Gross = o.Gross;
            this.Comm = o.Comm;
            this.ECN = o.ECN;
            this.Other = o.Other;
        }

        public void Add(CompareDaily o)
        {
            this.Shares += o.Shares;
            this.Gross += o.Gross;
            this.Comm += o.Comm;
            this.ECN += o.ECN;
            this.Other += o.Other;
        }

        public void Add(CompareTrader o)
        {
            this.Shares += o.Shares;
            this.Gross += o.Gross;
            this.Comm += o.Comm;
            this.ECN += o.ECN;
            this.Other += o.Other;
        }

        public DateTime StartDate { get; set; }

        public string StartDateFormat { get { return StartDate.ToString("yyyy/MM/dd"); } }

        public DateTime EndDate { get; set; }

        public string EndDateFormat { get { return EndDate.ToString("yyyy/MM/dd"); } }

        public string Trader { get; set; }

        public string Symbol2 { get; set; }

        public int Shares { get; set; }

        public decimal Gross { get; set; }

        public decimal Comm { get; set; }

        public decimal ECN { get; set; }

        public decimal Other { get; set; }

        public decimal Net { get { return Gross - Other - Comm - ECN; } }

        public void FormatData()
        {
            this.Gross = Math.Round(this.Gross, 2);
            this.Comm = Math.Round(this.Comm, 2);
            this.ECN = Math.Round(this.ECN, 2);
            this.Other = Math.Round(this.Other, 2);
        }
    }
}
