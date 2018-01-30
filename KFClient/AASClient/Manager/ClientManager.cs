using System.Collections.Generic;
using AASTrader.Client;
//using AASTrader.Client.DataServer;
//using AASTrader.Trade;
//using Microsoft.Practices.Unity;

namespace AASClient.Manager
{
    public class ClientManager : IManager
    {
        private static object sync = new object();
        private static ClientManager _instance;

        public static ClientManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new ClientManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private DataServerClient _dataClient;
        //private TradeServerClient _tradeClient;
        //private ClientServerClient _clientClient;


        public DataServerClient DataClient
        {
            get { return _dataClient; }
        }

        //public TradeServerClient TradeClient
        //{
        //    get { return _tradeClient; }
        //}

        //public ClientServerClient ClientClient
        //{
        //    get { return _clientClient; }
        //}

        public bool IsDataConnected
        {
            get { return _dataClient.IsConnected; }
        }

        //public bool IsTradeConnected
        //{
        //    get { return _tradeClient.IsConnected; }
        //}

        //public bool IsServiceConnected
        //{
        //    get { return _clientClient.IsConnected; }
        //}

        public ClientManager()
        {
            _dataClient = new DataServerClient();
            //_clientClient = new ClientServerClient();
            //_tradeClient = new TradeServerClient();
        }

        public int ConnectToDataServer()
        {
            //连接数据服务器
            int retval = _dataClient.Connect();
            return retval;
        }

        //public int ConnectToTradeServer()
        //{
        //    return _tradeClient.Connect();
        //}

        //public int ConnectToServiceServer()
        //{
        //    return _clientClient.Connect();
        //}

        //public bool InitTradeServer()
        //{
        //    if (_tradeClient.IsConnected)
        //    {
        //        TradeManager.GetInstance.InitLoad(_tradeClient);
        //        return true;
        //    }

        //    return false;
        //}
    }
}