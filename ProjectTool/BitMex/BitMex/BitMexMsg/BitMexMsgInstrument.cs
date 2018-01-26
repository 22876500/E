using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMex.BitMexMsg
{
    public class BitMexMsgInstrument : BitMexMsgBase
    {

        public InstrumentDataItem[] data { get; set; }

        public class InstrumentDataItem
        {
            public string symbol { get; set; }

            public string lastPrice { get; set; }

            public string lastTickDirection { get; set; }

            public string lastChangePcnt { get; set; }

            public string markPrice { get; set; }

            public string timestamp { get; set; }

            public string bidPrice { get; set; }

            public string askPrice { get; set; }

        }
    }
}
