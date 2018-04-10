using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASServer.AyersEntity
{
    public class AyersFeeConfig
    {
        /// <summary>
        /// 1、佣金 交易金额的0.25% 最低50-150  准确的根据券商来收
        /// </summary>
        public decimal Commission {get;set;}

        public decimal CommissionMin { get; set; }
        
        /// <summary>
        /// 2、印花税 交易金额的0.1% 
        /// </summary>
        public decimal StampTax {get;set;}

        /// <summary>
        /// 3、交易征费 交易金额的0.0027%
        /// </summary>
        public decimal TransactionLevy  {get;set;}

        /// <summary>
        /// 4、交易费 交易金额的0.005%
        /// </summary>
        public decimal  TradingFee {get;set;}
        
        /// <summary>
        ///  5、过户费 部分券商收取
        /// </summary>
        public decimal TransferFee {get;set;}

    }
}
