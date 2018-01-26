using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKCoinClient.OKCoinEntities
{
    public class OKCoinResponse
    {
        public string channel { get; set; }

        public string success { get; set; }

        public string errorcode { get; set; }

        public object data { get; set; }

        //public DateTime Time { get; set; }
    }

}
