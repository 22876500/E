using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AASServer
{
    public class TdxHqApi
    {
        #region Members
        const int AliveMinites = 3;

        static object sync = new object();

        static TdxHqApi _instance;

        bool isStopping = false;
        bool IsMarketStoped = false;
        bool IsTransaStoped = false;

        private List<string> ServerUsed = new List<string>();

        private ConcurrentDictionary<string, string> DictTran = new ConcurrentDictionary<string, string>();

        private Dictionary<string, DateTime> DictAliveTime = new Dictionary<string, DateTime>();

        private ConcurrentDictionary<string, HKMarketData> DictMarketEntity = new ConcurrentDictionary<string, HKMarketData>();

        private ConcurrentQueue<string> QueueMarket = new ConcurrentQueue<string>();

        private List<string> Codes;

        Thread QueryMarketThread;
        Thread QueryTranThread;
        Thread CheckCodesThread;
        Thread TransformThread;
        #endregion

        #region Properties
        public static TdxHqApi Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new TdxHqApi();
                        }
                    }
                }
                return _instance;
            }
        }

        public static bool IsOpened { get; private set; }

        public int MarketConnectionID { get; private set; }

        public int TransaConnectionID { get; private set; }

        #endregion

        private TdxHqApi()
        {
            MarketConnectionID = int.MinValue;
            TransaConnectionID = int.MinValue;
            Codes = new List<string>();
            Codes.Add("00001");
            Codes.Add("00002");
            Codes.Add("00003");
        }

        public bool Start()
        {
            if (Program.appConfig.GetValue("UseTdxHKData", "0") == "1")
            {
                if (!IsOpened)
                {
                    StringBuilder errorInfo = new StringBuilder(1024 * 16);
                    try
                    {
                        IsOpened = OpenTdx(errorInfo);
                        if (IsOpened)
                        {
                            Program.logger.LogInfoDetail("TdxHqApi:OpenTdx成功");
                        }
                        else
                        {
                            Program.logger.LogInfo("TdxHKData: Tdx Open 失败! ErrorInfo:{0}", errorInfo.ToString()); 
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfo("TdxHKData: Tdx Open 失败! Message:{0}, StackTrace:{1}", ex.Message, ex.StackTrace);
                    }
                }
            }

            if (IsOpened)
            {
                QueryMarketThread = new Thread(new ThreadStart(QueryMarketMain)) { IsBackground = true };
                QueryMarketThread.Start();

                QueryTranThread = new Thread(new ThreadStart(QueryTranMain)) { IsBackground = true };
                QueryTranThread.Start();

                CheckCodesThread = new Thread(new ThreadStart(CheckAliveMain)) { IsBackground = true };
                CheckCodesThread.Start();

                TransformThread = new Thread(new ThreadStart(TransformMain)) { IsBackground = true };
                TransformThread.Start();
            }

            return IsOpened;
        }

     

        public void Stop()
        {
            try
            {
                if (!IsOpened)
                {
                    return;
                }
                isStopping = true;

                if (CheckCodesThread != null)
                    CheckCodesThread.Abort();

                while (!IsMarketStoped || !IsTransaStoped)
                {
                    Thread.Sleep(100);
                }
                CloseTdx();
            }
            catch (Exception) { }
        }

        #region Connect
        private int ConnectHqServer(out string serverIP)
        {
            StringBuilder Result = new StringBuilder(2048);
            StringBuilder ErrInfo = new StringBuilder(1024);
            string[] servers = null;
            serverIP = string.Empty;
            try
            {
                if (GetHqServer(out servers))
                {
                    foreach (var item in servers)
                    {
                        if (ServerUsed.Contains(item)) continue;

                        var ipInfo = item.Split(':');
                        var ip = ipInfo[1];
                        var port = ipInfo[2];
                        int connectionID = Connect(ip, int.Parse(port), Result, ErrInfo);
                        if (ErrInfo.ToString() == string.Empty)
                        {
                            ServerUsed.Add(item);
                            return connectionID;
                        }
                    }
                    Program.logger.LogInfoDetail("行情服务器连接失败，请检查Tdx.exe是否启动！");
                    Thread.Sleep(120000);
                }
            }
            catch (Exception) { }
            return int.MinValue;
        }

        private bool GetHqServer(out string[] servers)
        {
            string FileName = "港股行情服务器.txt";
            if (File.Exists(FileName))
            {
                servers = File.ReadAllLines(FileName, Encoding.Default);
                return true;
            }
            else
            {
                servers = new string[] { };
                Program.logger.LogInfo("未找到行情服务器文件：港股行情服务器.txt");
                return false;
            }
        } 
        #endregion

        #region Pub Method
        public void KeepAlive(string[] codes)
        {
            try
            {
                lock (sync)
                {
                    foreach (var item in codes)
                    {
                        if (!Codes.Contains(item))
                            Codes.Add(item);

                        if (DictAliveTime.ContainsKey(item))
                            DictAliveTime[item] = DateTime.Now;
                        else
                            DictAliveTime.Add(item, DateTime.Now);
                    }
                }

            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("TdxHqApi KeepAlive Error: {0}", ex.Message);
            }
        }

        public List<HKMarketData> GetMarketDatas(string[] codes)
        {
            //Subscribe(codes);
            List<HKMarketData> lstData = new List<HKMarketData>();
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var item in codes)
            {
                //if (DictMarket.ContainsKey(item))
                //{
                //    dict.Add(item, DictMarket[item]);
                //}
                if (DictMarketEntity.ContainsKey(item))
                {
                    lstData.Add(DictMarketEntity[item]);
                }
            }
            return lstData;
        }

        public Dictionary<string, string> GetTranDatas(string[] codes)
        {
            Subscribe(codes);

            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var item in codes)
            {
                if (DictTran.ContainsKey(item))
                {
                    dict.Add(item, DictTran[item]);
                }
            }
            return dict;
        } 
        #endregion

        #region Check
        private void CheckAliveMain()
        {
            while (true)
            {
                string code = string.Empty;
                for (int i = Codes.Count - 1; i > -1; i--)
                {
                    try
                    {
                        code = Codes[i];
                        if (DictAliveTime.ContainsKey(code))
                        {
                            if (code == "00001")
                            {
                                continue;
                            }
                            if ((DictAliveTime[code] - DateTime.Now).TotalSeconds > 60000 * AliveMinites)
                            {
                                Codes.Remove(code);
                            }
                        }
                    }
                    catch (Exception ex) {
                        Program.logger.LogInfoDetail("港股行情活跃订阅股票检测异常, 当前Code{0}, 当前订阅列表：{1} Excepion：{2}", code, Codes.ToJson(), ex.Message);
                        Thread.Sleep(60000);
                    }
                }
                Thread.Sleep(1000);
            }
        } 
        #endregion

        #region Query Data
        public void Subscribe(string[] codes)
        {
            foreach (var item in codes)
            {
                try
                {
                    if (!Codes.Contains(item))
                        Codes.Add(item);

                    if (!DictAliveTime.ContainsKey(item))
                        DictAliveTime.Add(item, DateTime.Now);
                    else
                        DictAliveTime[item] = DateTime.Now;
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail(ex.Message);
                }
            }
        }

        private void QueryMarketMain()
        {
            StringBuilder result = new StringBuilder(1024 * 1024);
            StringBuilder error = new StringBuilder(2048);
            string serverIP = string.Empty;
            int connectID = int.MinValue;
            int errorCount = 0;

            while (true)
            {
                if (connectID <= -1)
                {
                    connectID = ConnectHqServer(out serverIP);
                    if (connectID > int.MinValue) Program.logger.LogInfoDetail("港股买卖盘线程连接行情服务器成功！");
                }
                else
                {
                    string[] codeSearch = Codes.ToArray();
                    foreach (var item in codeSearch)
                    {
                        if (GetMarketData(connectID, item, result, error))
                        {
                            QueueMarket.Enqueue(result.ToString());
                            //DictMarket[item] = result.ToString();
                        }
                        else if (error.Length > 0)
                        {
                            errorCount++;
                            if (error.ToString().Contains("重新连接服务器"))
                            {
                                Program.logger.LogInfoDetail("港股买卖盘查询错误, 将重新连接服务器，错误信息:" + error.ToString());
                                ResetConnection(ref serverIP, ref connectID, ref errorCount);
                                break;
                            }
                            else if (errorCount < 2)
                            {
                                errorCount++;
                                Program.logger.LogInfoDetail("港股买卖盘查询错误, 错误信息:" + error.ToString());
                            }
                            else
                            {
                                ResetConnection(ref serverIP, ref connectID, ref errorCount);
                                break;
                            }
                        }
                    }
                }
                if (isStopping) break;

                Thread.Sleep(100);
            }
            if (connectID > int.MinValue)
                TdxHKHq_Multi_Disconnect(connectID);
            IsMarketStoped = true;
        }

        private void QueryTranMain()
        {
            StringBuilder result = new StringBuilder(1024 * 1024);
            StringBuilder error = new StringBuilder(1024);
            
            string serverIP = string.Empty;
            int connectID = int.MinValue;
            int errorCount = 0;

            while (true)
            {
                if (connectID == int.MinValue)
                {
                    connectID = ConnectHqServer(out serverIP);
                    if (connectID > int.MinValue)
                        Program.logger.LogInfoDetail("港股逐笔线程连接行情服务器成功！");
                }
                else
                {
                    string[] codeSearch = Codes.ToArray();
                    foreach (var item in codeSearch)
                    {
                        if (GetTransaction(connectID, item, result, error))
                        {
                            errorCount = 0;
                            DictTran[item] = result.ToString();
                        }
                        else if (error.Length > 0)
                        {
                            if (error.ToString().Contains("重新连接服务器"))
                            {
                                Program.logger.LogInfoDetail("港股逐笔成交查询错误, 将重新连接服务器，错误信息:" + error.ToString());
                                ResetConnection(ref serverIP, ref connectID, ref errorCount);
                                break;
                            }
                            else if (errorCount < 3)
                            {
                                errorCount++;
                                Program.logger.LogInfoDetail("港股逐笔成交查询错误：" + error.ToString());
                            }
                            else
                            {
                                ResetConnection(ref serverIP, ref connectID, ref errorCount);
                                break;
                            }
                        }
                    }
                }
                if (isStopping) break;
            }

            if (connectID > int.MinValue) DisConnect(connectID);
            IsTransaStoped = true;
        }

        string[] N = { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
        private void TransformMain()
        {
            string item;
            while (true)
            {
                if (QueueMarket.Count > 0 && QueueMarket.TryDequeue(out item))
                {
                    try
                    {
                        var dt = Tool.ChangeDataStringToTable(item);
                        var DataRow1 = dt.Rows[0];
                        decimal ZS = decimal.Parse(DataRow1["昨收"] as string);
                        decimal XJ = decimal.Parse(DataRow1["现价"] as string);
                        decimal High = decimal.Parse(DataRow1["最高"] as string);
                        decimal Low = decimal.Parse(DataRow1["最低"] as string);
                        string code = DataRow1["代码"] as string;
                        HKMarketData md = null;
                        if (DictMarketEntity.ContainsKey(code))
                        {
                            md = DictMarketEntity[code];
                        }
                        else
                        {
                            md = new HKMarketData() { Code = code, Date = DataRow1["日期"] as string };
                            md.QtyPermit = decimal.Parse(DataRow1["每手"] as string);
                            md.PriceB = new decimal[10];
                            md.PriceS = new decimal[10];
                            md.QtyB = new decimal[10];
                            md.QtyS = new decimal[10];
                            DictMarketEntity[code] = md;
                        }
                        md.Highest = High;
                        md.Lowest = Low;
                        md.PricePRE = ZS;
                        md.Price = XJ;

                        for (int i = 0; i < 10; i++)
                        {
                            md.PriceB[i] = decimal.Parse(DataRow1["买" + this.N[i] + "价"] as string);
                            md.PriceS[i] = decimal.Parse(DataRow1["卖" + this.N[i] + "价"] as string);
                            md.QtyB[i] = decimal.Parse(DataRow1["买" + this.N[i] + "量"] as string);
                            md.QtyS[i] = decimal.Parse(DataRow1["卖" + this.N[i] + "量"] as string);
                        }
                    }
                    catch (Exception) { }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void ResetConnection(ref string serverIP, ref int connectID, ref int errorCount)
        {
            ServerUsed.Remove(serverIP);
            serverIP = string.Empty;
            connectID = int.MinValue;
            errorCount = 0;
        }

        private int Connect(string IP, int Port, StringBuilder Result, StringBuilder ErrInfo)
        {
            try
            {
                int id = TdxHKHq_Multi_Connect("127.0.0.1", IP, Port, Result, ErrInfo);
                if (ErrInfo.ToString() == string.Empty)
                {
                    return id;
                }
            }
            catch (Exception ex)
            {
                ErrInfo.Append(ex.Message);
            }
            return -1;
        }

        private void DisConnect(int connectionID)
        {
            try
            {
                TdxHKHq_Multi_Disconnect(connectionID);
            }
            catch (Exception) { }
        }

        private bool GetMarketData(int connectionID, string stockCode, StringBuilder Result, StringBuilder ErrInfo)
        {
            try
            {
                return TdxHKHq_Multi_GetHKInstrumentQuote10(connectionID, 31, stockCode, Result, ErrInfo);
            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("获取港股十档行情异常:{0}", ex.Message);
                return false;
            }
        }

        short count = 50;
        private bool GetTransaction(int connectionID, string stockCode, StringBuilder Result, StringBuilder ErrInfo)
        {
            try
            {
                return TdxHKHq_Multi_GetHKTransactionData(connectionID, 31, stockCode, 0, ref count, Result, ErrInfo);
            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("获取指定港股的分时成交数据异常:{0}", ex.Message);
                return false;
            }
        } 
        #endregion

        #region Interface Function
        //连接券商行情服务器
        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I4)]
        static extern int TdxHKHq_Multi_Connect(string TdxLicServerIP, string IP, int Port, StringBuilder Result, StringBuilder ErrInfo);


        //断开服务器
        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        static extern void TdxHKHq_Multi_Disconnect(int ConnectionID);


        //获取所有市场代码
        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool TdxHKHq_Multi_GetMarkets(int ConnectionID, StringBuilder Result, StringBuilder ErrInfo);

        //获取所有品种数目
        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool TdxHKHq_Multi_GetInstrumentCount(int ConnectionID, ref int Result, StringBuilder ErrInfo);

        //获取所有品种代码
        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool TdxHKHq_Multi_GetInstrumentInfo(int ConnectionID, short Start, short Count, StringBuilder Result, StringBuilder ErrInfo);


        //获取指定港股的十档报价
        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool TdxHKHq_Multi_GetHKInstrumentQuote10(int ConnectionID, byte Market, string Zqdm, StringBuilder Result, StringBuilder ErrInfo);

        //获取指定港股的分时成交数据
        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool TdxHKHq_Multi_GetHKTransactionData(int ConnectionID, byte Market, string Zqdm, int Start, ref short Count, StringBuilder Result, StringBuilder ErrInfo);

        //获取指定品种的分时成交数据
        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool TdxHKHq_Multi_GetTransactionData(int ConnectionID, byte Market, string Zqdm, int Start, ref short Count, StringBuilder Result, StringBuilder ErrInfo);
        


        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.I1)]
        static extern bool OpenTdx(StringBuilder ErrInfo);


        [DllImport("TdxHqApi.dll", CharSet = CharSet.Ansi)]
        static extern void CloseTdx();

        
        #endregion
    }

    public class HKMarketData
    {
        public string Code { get; set; }

        public decimal[] PriceB { get; set; }

        public decimal[] PriceS { get; set; }

        public decimal[] QtyB { get; set; }

        public decimal[] QtyS { get; set; }

        /// <summary>
        /// 现价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 昨收
        /// </summary>
        public decimal PricePRE { get; set; }

        public decimal Highest { get; set; }

        public decimal Lowest { get; set; }

        public decimal QtyPermit { get; set; }

        public string Date { get; set; }

    }

    public class HKTrasaction
    { 
        
    }
}
