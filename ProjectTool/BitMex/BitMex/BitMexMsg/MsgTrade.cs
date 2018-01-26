using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMex.BitMexMsg
{
    public class MsgTrade : BitMexMsg.BitMexMsgBase
    {
        public MsgTradeData[] data { get; set; }

        public class MsgTradeData
        {
            public string timestamp { get; set; }
            public string symbol { get; set; }
            public string side { get; set; }
            public string size { get; set; }
            public string price { get; set; }
            public string tickDirection { get; set; }
            public string trdMatchID { get; set; }
            public string grossValue { get; set; }
            public string homeNotional { get; set; }
            public string foreignNotional { get; set; }
        }
    }
}
