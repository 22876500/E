using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AASDataServer.Model;
using DataModel;
using AASDataServer.DataAdapter;
using DataServerIce;

namespace AASDataServer.Service
{
    public interface IDataService
    {
        Dictionary<string, SubCode> Codes
        {
            get;
        }

        Dictionary<string, UserClient> Clients
        {
            get;
        }

        IDataAdapter DataSource
        { get; set; }

        int SubscribeCodes(string username, string[] codelist);

        int UnsubscribeCodes(string username, string[] codelist);

        void FlushCodes(string username, string[] codelist);

        int GetStockCodes(out List<StockCode> codes);

        string GetStockCodesInfo();

        string GetVipCodes();

        void ClearAll();
    }
}
