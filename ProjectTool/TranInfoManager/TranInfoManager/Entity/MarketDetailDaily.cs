using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TranInfoManager.Entity
{
    public class MarketDetailDaily
    {
        /// <summary>
        /// BOT->buy , SLD -> sell (数据比较依据列)
        /// </summary>
        public string Action { get; set; }

        public string Customer { get; set; }

        [Key, Column(Order = 0)]
        public string TicketID { get; set; }
        
        public string ISIN { get; set; }
        
        /// <summary>
        /// 股票(数据比较依据列)
        /// </summary>
        public string Symbol2 { get; set; }

        public string Sym_Format { get { return Symbol2.GetFormatName(); } }

        /// <summary>
        /// Quantity (数据比较依据列)
        /// </summary>
        public int ExQuan { get; set; }

        /// <summary>
        /// Price (数据比较依据列)
        /// </summary>
        public decimal ExPrice { get; set; }

        //两位
        public decimal FirstMoney { get; set; }
        
        /// <summary>
        /// 未确定
        /// </summary>
        public string Exchange { get; set; }
        
        /// <summary>
        /// 4位
        /// </summary>
        public decimal TotalCommission_WithoutFeesAndCharges { get; set; }
        
        /// <summary>
        ///  if FirstMoney > 0 then  FirstMoney * 0.00218% 保留两位小数
        /// </summary>
        public decimal SecFee
        {
            get
            {
                if (FirstMoney > 0)
                {
                    //return Math.Ceiling(FirstMoney * (decimal)0.0218) / 1000;
                    return FirstMoney * (decimal)0.0000218;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 4位
        /// </summary>
        public decimal NetMoney { get; set; }

        /// <summary>
        /// 6位
        /// </summary>
        public decimal NasdTradingActivityFee { get; set; }

        /// <summary>
        /// 5位
        /// </summary>
        public decimal ExchangeFee { get; set; }
        
        /// <summary>
        /// 4位
        /// </summary>
        public decimal ECNRebate { get; set; }
        
        /// <summary>
        /// 14位小数
        /// </summary>
        public decimal NSCCPassThru { get; set; }
        
        /// <summary>
        /// 精度8位
        /// </summary>
        public decimal SIPC { get; set; }
        
        public decimal FTT { get; set; }

        [Key, Column(Order = 1)]
        public DateTime TradeDate { get; set; }
        
        public DateTime SettlementDate { get; set; }

    }
}
