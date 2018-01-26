using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKCoinClient.Model
{
    class TradeModel
    {
        //[交易序号, 价格, 成交量(张), 时间, ，]
        //[string, string, string, string, string, string]
        public string TradeID { get;set; }

        public string Price { get; set; }

        /// <summary>
        /// 成交量(张)
        /// </summary>
        public string TradeQty { get; set; }

        /// <summary>
        /// 时间 （格式：15:25:03）
        /// </summary>
        public string Time { get; set; }

        public DateTime TradeTime
        {
            get
            {
                DateTime dt;
                if (DateTime.TryParse(Time, out dt))
                {
                    return dt;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// 买卖类型
        /// </summary>
        public string BSFlag { get; set; }
    }
}
