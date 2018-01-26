using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKCoinClient.Model
{
    class TickerModel
    {
        /// <summary>
        /// 最高买入限制价格
        /// </summary>
        public string limitHigh { get; set; }

        /// <summary>
        /// 最低卖出限制价格
        /// </summary>
        public string limitLow { get; set; }

        /// <summary>
        /// 24小时成交量
        /// </summary>
        public double vol { get; set; }

        /// <summary>
        /// 买一价格
        /// </summary>
        public double buy { get; set; }


        /// <summary>
        /// 卖一价格
        /// </summary>
        public double sell { get; set; }

        /// <summary>
        /// 合约价值
        /// </summary>
        public double unitAmount { get; set; }

        /// <summary>
        /// 当前持仓量
        /// </summary>
        public double hold_amount { get; set; }

        /// <summary>
        /// 合约ID
        /// </summary>
        public long contractId { get; set; }

        /// <summary>
        /// 24小时最高价格
        /// </summary>
        public double high { get; set; }

        /// <summary>
        /// 24小时最低价格
        /// </summary>
        public double low { get; set; }
    }
}
