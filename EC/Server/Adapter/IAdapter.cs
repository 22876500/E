
using System;
using System.Collections.Generic;

namespace Server.Adapter
{
    interface IAdapter
    {
        bool IsInited { get; set; }

        void Init(string apiKey, string secretKey, string accountName, string account, DbDataSet.已发委托DataTable dt);

        void SendOrder(string symbol, int BsFlag, decimal qty, decimal price, string trader, out string orderID, out string errInfo);

        void CancelOrder(string symbol, string OrderID, out string result, out string errInfo);

        void QueryData(out JyDataSet.委托DataTable dtWt, out JyDataSet.成交DataTable dtCJ);
        
        List<AccounCoin> QueryAccountCoin(List<string> listCoin);

        decimal QueryAccountCoin(string coin);
    }

    [Serializable]
    public class AccounCoin
    {
        public string Asset { get; set; }

        public decimal Free { get; set; }

        public decimal Locked { get; set; }
    }

}
