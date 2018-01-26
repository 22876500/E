using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    [Serializable]
    public class StandardOrderEntity
    {
        public string ClientID { get; set; }

        //public int ClientIDNum
        //{
        //    get
        //    {
        //        int i = -1;
        //        int.TryParse(ClientID, out i);
        //        return i;
        //    }
        //}

        public string OrderNo { get; set; }

        public string OrderStatus { get; set; }

        public string Group { get; set; }

        public string StockCode { get; set; }

        public string StockName { get; set; }

        public int TradeSide { get; set; }

        public int OrderQty { get; set; }

        public decimal OrderPrice { get; set; }

        public int FilledQty { get; set; }


        public decimal FilledPrice { get; set; }

        public int CancelQty { get; set; }

        public int Market { get; set; }

        public DateTime OrderTime { get; set; }

        public string ErrMsg { get; set; }

        public string Account { get; set; }

    }
}
