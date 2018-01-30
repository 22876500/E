using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASClient.MarketAdapter
{
    interface IAdapter
    {
        void Subscribe(string channel);

        void UnSubscribe(string channel);

        
    }

    public delegate void MarketCallback<T>(string symble, T t);
    public delegate void TransactionCallback<T>(string symble, T t);
}
