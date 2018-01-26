using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMex.Config
{
    public static class BitMexChannel
    {
        //"announcement",// 网站通知
        //"chat",        // 聊天室
        //"connected",   // 在线用户/机器人的统计信息
        //"instrument",  // 产品更新，包括交易量以及报价
        //"insurance",   // 每日保险基金的更新
        //"liquidation", // 强平委托
        //"orderBookL2", // 完整的 level 2 委托列表
        //"orderBook10", // 完整的 10 层深度委托列表
        //"publicNotifications", // 通知和告示（用于短暂显示）
        //"quote",       // 报价
        //"quoteBin1m",  // 每分钟报价数据
        //"settlement",  // 结算信息
        //"trade",       // 实时交易
        //"tradeBin1m",  // 每分钟交易数据
        
        /// <summary>
        /// 网站通知
        /// </summary>
        public const string Announcement = "announcement";

        /// <summary>
        /// 聊天室
        /// </summary>
        public const string Chat = "chat";

        /// <summary>
        /// 在线用户/机器人的统计信息
        /// </summary>
        public const string Connected = "connected";

        /// <summary>
        /// 产品更新，包括交易量以及报价
        /// </summary>
        public const string Instrument = "instrument";

        /// <summary>
        /// 每日保险基金的更新
        /// </summary>
        public const string Insurance = "insurance";

        /// <summary>
        /// 强平委托
        /// </summary>
        public const string Liquidation = "liquidation";

        /// <summary>
        /// 完整的 level 2 委托列表
        /// </summary>
        public const string OrderBookL2 = "orderBookL2";

        /// <summary>
        /// 完整的 10 层深度委托列表
        /// </summary>
        public const string OrderBook10 = "orderBook10";

        /// <summary>
        /// 通知和告示（用于短暂显示）
        /// </summary>
        public const string PublicNotifications = "publicNotifications";
        
        /// <summary>
        /// 报价
        /// </summary>
        public const string Quote = "quote";

        /// <summary>
        /// 每分钟报价数据
        /// </summary>
        public const string QuoteBin1m = "quoteBin1m";

        /// <summary>
        /// 结算信息
        /// </summary>
        public const string Settlement = "settlement";

        /// <summary>
        /// 实时交易
        /// </summary>
        public const string Trade = "trade";

        /// <summary>
        /// 每分钟交易数据
        /// </summary>
        public const string TradeBin1m = "tradeBin1m";
    }

    
}
