using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKCoinClient.Model
{
    public class IndexModel
    {
        public string usdCnyRate { get; set; }

        public string futureIndex { get; set; }

        public string timestamp { get; set; }

        public DateTime IndexTime {
            get
            {
                if (!string.IsNullOrEmpty(timestamp))
                {
                    return Utils.SpanToDateTime(timestamp);
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
    }
}
