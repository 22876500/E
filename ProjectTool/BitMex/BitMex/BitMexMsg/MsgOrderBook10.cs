using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMex.BitMexMsg
{
    class MsgOrderBook10 : BitMexMsgBase
    {
        public OrderBookL2Data[] data { get; set; }
        public class OrderBookL2Data
        {
            public string symbol { get; set; }

            /// <summary>
            /// 价格低，买盘
            /// </summary>
            public double[][] bids { get; set; }

            /// <summary>
            /// 价格高，卖盘
            /// </summary>
            public double[][] asks { get; set; }

            public string timestamp { get; set; }
        }
    }
}
