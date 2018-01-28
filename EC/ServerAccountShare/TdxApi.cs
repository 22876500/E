using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static class TdxApi
    {
        ///基本版
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void OpenTdx();
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void CloseTdx();
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern int Logon(string IP, short Port, string Version, short YybID, string AccountNo, string TradeAccount, string JyPassword, string TxPassword, StringBuilder ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void Logoff(int ClientID);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void QueryData(int ClientID, int Category, StringBuilder Result, StringBuilder ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void SendOrder(int ClientID, int Category, int PriceType, string Gddm, string Zqdm, float Price, int Quantity, StringBuilder Result, StringBuilder ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void CancelOrder(int ClientID, string ExchangeID, string hth, StringBuilder Result, StringBuilder ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void GetQuote(int ClientID, string Zqdm, StringBuilder Result, StringBuilder ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void Repay(int ClientID, string Amount, StringBuilder Result, StringBuilder ErrInfo);




        ///普通批量版
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void QueryHistoryData(int ClientID, int Category, string StartDate, string EndDate, StringBuilder Result, StringBuilder ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void QueryDatas(int ClientID, int[] Category, int Count, IntPtr[] Result, IntPtr[] ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void SendOrders(int ClientID, int[] Category, int[] PriceType, string[] Gddm, string[] Zqdm, float[] Price, int[] Quantity, int Count, IntPtr[] Result, IntPtr[] ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void CancelOrders(int ClientID, string[] ExchangeID, string[] hth, int Count, IntPtr[] Result, IntPtr[] ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void GetQuotes(int ClientID, string[] Zqdm, int Count, IntPtr[] Result, IntPtr[] ErrInfo);







        ///高级批量版
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void QueryMultiAccountsDatas(int[] ClientID, int[] Category, int Count, IntPtr[] Result, IntPtr[] ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void SendMultiAccountsOrders(int[] ClientID, int[] Category, int[] PriceType, string[] Gddm, string[] Zqdm, float[] Price, int[] Quantity, int Count, IntPtr[] Result, IntPtr[] ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void CancelMultiAccountsOrders(int[] ClientID, string[] ExchangeID, string[] hth, int Count, IntPtr[] Result, IntPtr[] ErrInfo);
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void GetMultiAccountsQuotes(int[] ClientID, string[] Zqdm, int Count, IntPtr[] Result, IntPtr[] ErrInfo);


        #region 港股通
        /// <summary>
        /// 查询各种交易数据
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Category">0资金, 1股份， 2当日委托, 3当日成交, 4撤单, 5投票, 6公司行为,</param>
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void QueryHKData(int ClientID, int Category, StringBuilder Result, StringBuilder ErrInfo);

        /// <summary>
        /// 获取股票交易信息
        /// </summary>
        /// <param name="Category"> 0买   1卖</param>
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void GetHKStockTradeInfo(int ClientID, int Category, string Zqdm, StringBuilder Result, StringBuilder ErrInfo);

        /// <summary>
        /// 下委托交易证券
        /// </summary>
        /// <param name="Category">0买入 1卖出 </param>
        /// <param name="PriceType">2竞价限价盘 3增强限价盘</param>
        /// <param name="QuantityType">0整手  1碎股</param>
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void SendHKOrder(int ClientID, int Category, int PriceType, int QuantityType, string Gddm, string Zqdm, string Price, string Quantity, StringBuilder Result, StringBuilder ErrInfo);

        /// <summary>
        /// 撤委托
        /// </summary>
        /// <param name="hth">表示要撤的目标委托的编号</param>
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void CancelHKOrder(int ClientID, string hth, StringBuilder Result, StringBuilder ErrInfo);

        /// <summary>
        /// 查询历史数据
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="Category">0 历史委托, 1 历史成交, 2历史投票, 3历史公司行为, 4交割单</param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Result"></param>
        /// <param name="ErrInfo"></param>
        [DllImport("trade.dll", CharSet = CharSet.Ansi)]
        public static extern void QueryHKHistoryData(int ClientID, int Category, string StartDate, string EndDate, StringBuilder Result, StringBuilder ErrInfo);

        #endregion
    }
}
