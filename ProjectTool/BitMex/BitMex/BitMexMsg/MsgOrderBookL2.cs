
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMex.BitMexMsg
{
    public class MsgOrderBookL2 : BitMexMsgBase
    {
        public OrderBookL2Data[] data { get; set; }

        public class OrderBookL2Data
        {
            public string symbol { get; set; }

            public string id { get; set; }

            public string side { get; set; }

            public string size { get; set; }

            public string price { get; set; }
        }
    }
}
