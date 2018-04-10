using AASClient.AASServiceReference;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AASClient
{
    public class L2HkApi
    {
        #region Static Members
        static object sync = new object(); 
        #endregion

        #region Members
        private List<string> Codes;
        private ConcurrentDictionary<string, HKMarketData> DictMarket;
        private ConcurrentDictionary<string, DataTable> DictTran;
        private ConcurrentDictionary<string, int> DictQtyLimit;

        private Thread KeepAliveThread;
        private Thread TranDataThread;
        private Thread MarketDataThread;
        #endregion

        #region Instance
        private L2HkApi()
        {
            if (CommonUtils.EnableTdxHKApi)
            {
                Codes = new List<string>();
                DictMarket = new ConcurrentDictionary<string, HKMarketData>();
                DictTran = new ConcurrentDictionary<string, DataTable>();
                DictQtyLimit = new ConcurrentDictionary<string, int>();

                KeepAliveThread = new Thread(new ThreadStart(KeepAlive)) { IsBackground = true };
                KeepAliveThread.Start();

                MarketDataThread = new Thread(new ThreadStart(MarketDataMain)) { IsBackground = true };
                MarketDataThread.Start();

                TranDataThread = new Thread(new ThreadStart(TranDataMain)) { IsBackground = true };
                TranDataThread.Start();
            }
            
        }

        private static L2HkApi _instance;
        public static L2HkApi Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new L2HkApi();
                        }
                    }
                }
                return _instance;
            }
        } 
        #endregion

        public void Submit(string [] codes)
        {
            if (!CommonUtils.EnableTdxHKApi)
                return;

            lock (Codes)
            {
                if (Codes.Count == codes.Length)
                {
                    if (Codes.Except(codes).Count() > 0)
                    {
                        Codes.Clear();
                        Codes.AddRange(codes);
                    }
                }
                else
                {
                    Codes.Clear();
                    Codes.AddRange(codes);
                }
            }
        }

        public HKMarketData GetMarket(string code)
        {
            if (CommonUtils.EnableTdxHKApi && DictMarket.ContainsKey(code))
            {
                return DictMarket[code];
            }
            return null;
        }

        public DataTable GetTran(string code)
        {
            if (CommonUtils.EnableTdxHKApi && DictTran.ContainsKey(code))
            {
                return DictTran[code];
            }
            return null;
        }

        public bool GetQty(string code, out int qty)
        {
            if (DictQtyLimit == null)
            {
                qty = -1;
                return false;
            }
            if ( DictQtyLimit.ContainsKey(code))
            {
                qty = DictQtyLimit[code];
                return true;
            }
            else if (DictMarket.ContainsKey(code))
            {
                qty = (int)DictMarket[code].QtyPermit;
                DictQtyLimit[code] = qty;
                return true;
            }
            else 
            {
                
                qty = -1;
                return false;
            }
        }

        #region Interface
        private void KeepAlive()
        {
            while (true)
            {
                if (Codes.Count > 0)
                {
                    try
                    {
                        var arrCodes = Codes.ToArray();
                        Program.AASServiceClient.KeepAlive(arrCodes);
                    }
                    catch (Exception) { }
                }
                Thread.Sleep(5000);
            }
        }

        private void MarketDataMain()
        {
            while (true)
            {
                if (Codes.Count > 0)
                {
                    try
                    {
                        var MdDatas = Program.AASServiceClient.GetHkMarket(Codes.ToArray());
                        if (MdDatas != null && MdDatas.Length > 0)
                        {
                            foreach (var item in MdDatas)
                            {
                                DictMarket[item.Code] = item;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                        Program.logger.LogRunning("HK 买卖盘获取线程异常：{0}", ex.Message);
                    }

                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void TranDataMain()
        {
            while (true)
            {
                if (Codes.Count > 0)
                {
                    try
                    {
                        var tranData = Program.AASServiceClient.GetHkTran(Codes.ToArray());
                        if (!string.IsNullOrEmpty(tranData))
                        {
                            var dict = tranData.FromJSON<Dictionary<string, string>>();
                            foreach (var item in dict)
                            {
                                var data = Tool.ChangeDataStringToTable(item.Value);
                                DictTran[item.Key] = data;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogRunning("HK 逐笔获取线程异常：{0}", ex.Message);
                    }
                    Thread.Sleep(100);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        } 
        #endregion
    }
}
