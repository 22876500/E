using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMex.Config
{
    public static class OKCoinConfig
    {
        #region 数据源地址
        /// <summary>
        /// 数据主站国际站
        /// </summary>
        public const string urlOKCoinInternational = "wss://real.okcoin.com:10440/websocket/okcoinapi";

        /// <summary>
        /// 数据主站国内站
        /// </summary>
        public const string urlOKCoinInternal = "wss://real.okcoin.cn:10440/websocket/okcoinapi";

        /// <summary>
        /// 数据主站OKEX
        /// </summary>
        public const string urlOKEX = "wss://real.okex.com:10440/websocket/okexapi";
        #endregion

        #region 获取OKEX合约行情数据
        /// <summary>
        /// 订阅合约行情 
        /// ① X值为：btc, ltc, eth, etc, bch 
        /// ② Y值为：this_week, next_week, quarter
        /// </summary>
        public const string SubTicker = "ok_sub_futureusd_X_ticker_Y";

        /// <summary>
        /// 订阅K线数据 
        /// ① X值为：btc, ltc, eth, etc, bch 
        /// ② Y值为：this_week, next_week, quarter
        /// ③ Z值为：1min, 3min, 5min, 15min, 30min, 1hour, 2hour, 4hour, 6hour, 12hour, day, 3day, week
        /// </summary>
        public const string SubKLine = "ok_sub_futureusd_X_kline_Y_Z";

        /// <summary>
        /// 订阅合约市场深度(200增量数据返回)
        /// ① X值为：btc, ltc, eth, etc, bch 
        /// ② Y值为：this_week, next_week, quarter
        /// </summary>
        public const string SubDepth = "ok_sub_futureusd_X_depth_Y";

        /// <summary>
        /// 订阅合约市场深度(全量返回)
        /// ① X值为：btc, ltc, eth, etc, bch 
        /// ② Y值为：this_week, next_week, quarter
        /// ③ Z值为：5, 10, 20(获取深度条数) 
        /// </summary>
        public const string SubDepthAll = "ok_sub_futureusd_X_depth_Y_Z";

        /// <summary>
        /// 订阅合约交易信息
        /// ① X值为：btc, ltc, eth, etc, bch 
        /// ② Y值为：this_week, next_week, quarter
        /// </summary>
        public const string SubTrade = "ok_sub_futureusd_X_trade_Y";

        /// <summary>
        /// 订阅合约指数
        /// ① X值为：btc, ltc, eth, etc, bch
        /// </summary>
        public const string SubIndex = "ok_sub_futureusd_X_index";

        /// <summary>
        /// 合约预估交割价格
        /// ① X值为：btc, ltc, eth, etc, bch
        /// </summary>
        public const string SubForecast = "X_forecast_price";
        #endregion

        public const string addChannelStr = "addChannel";
        public const string removeChannelStr = "removeChannel";

        public static string GetFormatChannel(string channel, string X, string Y = "", string Z = "")
        {
            return channel.Replace("_X", '_' + X).Replace("_Y", '_' + Y).Replace("_Z", '_' + Z);
        }

        public static bool IsChannelData(string msg)
        {
            return msg.StartsWith("[{\"binary\":0,\"channel\":\"ok_sub_futureusd") || (msg.StartsWith("[{\"binary\":0,\"channel\":\"") && msg.IndexOf("_forecast_price") == 27);
        }

        public static string GetChannelData(string msg)
        {
            var dataStartIndex = msg.IndexOf("\"data\":") + 7;
            return msg.Substring(dataStartIndex, msg.Length - dataStartIndex - 2);
        }
    }
}
