using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using AASDataServer.DataAdapter;

namespace AASDataServer.Service
{
    public interface IPubService
    {
        bool IsRunning
        { get; }

        long PubCount
        { get; }

        IDataAdapter DataSource
        { get; set; }

        int Start();

        int Stop();

        void PubMarketData(MarketData[] datas);

        void PubMarketOrder(MarketOrder[] orders);

        void PubMarketTransction(MarketTransaction[] transactions);
    }
}
