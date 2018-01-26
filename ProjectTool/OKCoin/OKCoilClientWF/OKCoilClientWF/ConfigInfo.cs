using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKCoilClientWF
{
    public static class ConfigInfo
    {
        public const string urlOKCoinInternational = "wss://real.okcoin.com:10440/websocket/okcoinapi";
        public const string urlOKCoinInternal = "wss://real.okcoin.cn:10440/websocket/okcoinapi";
        public const string urlOKEX = "wss://real.okex.com:10440/websocket/okexapi";

        public const string BtcThisWeekChannel = "ok_sub_futureusd_btc_ticker_this_week";
        public const string ltc_this_week_channel = "ok_sub_futureusd_ltc_ticker_this_week";

        public const string BtcNextWeekChannel = "ok_sub_futureusd_btc_ticker_next_week";
        public const string ltc_next_week_channel = "ok_sub_futureusd_ltc_ticker_next_week";

        public const string BtcQuarterChannel = "ok_sub_futureusd_btc_ticker_quarter";
        public const string ltc_quarter_channel = "ok_sub_futureusd_ltc_ticker_quarter";

        //成交订阅模版
        public const string SubTradeTemplete = "ok_sub_futureusd_X_trade_Y";

        public const string addChannelStr = "addChannel";
        public const string removeChannelStr = "removeChannel";
    }
}
