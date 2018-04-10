using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using AASDataServer.Model.Setting;

namespace AASDataServer.DataAdapter
{
    public interface IDataAdapter
    {
        
        bool IsAllCodes
        {
            get;
        }

        bool IsRegistVip { get; }

        bool IsVIPRegisted { get; set; }

        bool IsRunning
        { 
            get; 
        }

        long RecvDataCount
        { 
            get; 
        }

        long CacheCount
        {
            get;
        }

        List<string> Codes
        { 
            get; 
        }

        HHDataAdapterSetting Setting
        {
            get;
            set;
        }

        event Action<string> NewSysEvent;

        event Action<MarketData[]> NewMarketData;

        event Action<MarketTransaction[]> NewMarketTransction;

        event Action<MarketOrder[]> NewMarketOrder;

        event Action<MarketOrderQueue[]> NewMarketOrderQueue;

        int RegisterCodes(List<string> codes);

        int DeRegisterCodes(List<string> codes);
        
        void DeRegisterAll();

        void ClearCache();

        int GetCodeTable(out List<MarketCode> codes);

        int Start();

        int Stop();

    }
}
