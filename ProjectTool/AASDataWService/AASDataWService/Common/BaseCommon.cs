using AASDataWService.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace AASDataWService.Common
{
    public class BaseCommon
    {
        public const string CONFIGIP = "DataServerSubIp";
        public const string CONFIGMARKETSUBPORT = "MarketSubPort";
        public const string CONFIGTRANSSUBPORT = "TransSubPort";
        public const string CONFIGMARKETPUBPORT = "MarketPubPort";
        public const string CONFIGTRANSPUBPORT = "TransPubPort";
        public const string CONFIGICEPORT = "IcePort";
        public const string CONFIGWRITETXT = "WriteTxt";

        public static T DeserializeMessage<T>(ZMessage msg) where T : class
        {
            T t = null;
            ByteArrayPool _bytePool = new ByteArrayPool();
            byte[] tmp = _bytePool.Malloc((int)(msg[1].Length));
            msg[1].Read(tmp, 0, (int)(msg[1].Length));
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(tmp))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter fmt = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                t = fmt.Deserialize(stream) as T;
            }
            _bytePool.Free(tmp);

            msg.Dispose();
            return t;
        }
        public static void WriteMarketTxt(string code, List<MarketData> lstMarket)
        {
            if (lstMarket == null || lstMarket.Count == 0) { return; }
            string path = string.Format("{0}\\{1}\\{2}\\{3}.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.Date.ToString("yyyymmdd"), "market", code);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@path, true))
            {
                foreach (var market in lstMarket)
                {
                    string str = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20}", market.Code, market.ActionDay, market.Time, market.Status, market.PreClose, market.Open, market.High, market.Low, market.Match,
                        string.Join(",", market.AskPrice), string.Join(",", market.AskVol), string.Join(",", market.BidPrice), string.Join(",", market.BidVol), market.Volume, market.Turnover, market.HighLimited, market.LowLimited, market.TotalBidVol, market.TotalAskVol, market.NumTrades, DateTime.Now);

                    file.WriteLine(str);
                }
            }
        }

        public static void WriteTranTxt(string code, List<MarketTransaction> lstTran)
        {
            if (lstTran == null || lstTran.Count == 0) { return; }
            string path = string.Format("{0}\\{1}\\{2}\\{3}.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.Date.ToString("yyyymmdd"), "transaction", code);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@path, true))
            {
                foreach (var tran in lstTran)
                {
                    string str = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}", tran.Code, tran.ActionDay, tran.Index, tran.Price, tran.Volume, tran.Turnover, tran.Flag, tran.OrderKind, tran.FunctionCode, tran.AskOrder, tran.BidOrder, DateTime.Now);

                    file.WriteLine(str);
                }
            }
        }
    }
}
