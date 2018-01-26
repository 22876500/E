using OKCoilClientWF.WebSocketApi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;

namespace OKCoilClientWF.OKCoin
{
    public class OKCoilAdapter
    {
        #region Data
        public ConcurrentQueue<OKCoinPrices> QueuePrices = new ConcurrentQueue<OKCoinPrices>();
        ConcurrentBag<string> bagAddChannel = new ConcurrentBag<string>();

        double quarterBuy { get; set; }
        double quarterSale { get; set; }
        double thisWeekBuy { get; set; }
        double thisWeekSale { get;set; }
        double nextWeekBuy { get; set; }
        double nextWeekSale { get; set; }

        Thread threadReConnect = null;
        Thread ThreadRecordPriceDiff = null;
        #endregion

        private static object Sync = new object();
        private static string SelectedUrl = ConfigInfo.urlOKEX;

        private static OKCoilAdapter _instance;
        public static OKCoilAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new OKCoilAdapter();
                        }
                    }
                }
                return _instance;
            }
        }

        private OKCoilAdapter()
        { }

        WebSocketBase wsb;
        WebSocketService wss;

        public void Start()
        {
            if (wsb == null)
            {
                wss = new BuissnesServiceImpl();

                wsb = new WebSocketBase(SelectedUrl, wss);
                wsb.start();

                //var dict = new Dictionary<string, string>();
                //dict.Add("api_key", ConfigInfo.apiKey);
                //wsb.send("{\"event\":\"login\",\"parameters\":{\"api_key\":\"" + ConfigInfo.apiKey + "\",\"sign\":\"" + SignParams(dict) + "\"}}");//登录
                //wsb.send("{'event':'addChannel','channel':'ok_sub_futureusd_btc_ticker_this_week'}");
                //wsb.send("{'event':'addChannel','channel':'ok_sub_futureusd_btc_ticker_next_week'}");
                //wsb.send("{'event':'addChannel','channel':'ok_sub_futureusd_btc_ticker_quarter'}");
                ChangeChannel(WSChannelChangeType.Add, WSChannel.ThisWeekChannel);
                ChangeChannel(WSChannelChangeType.Add, WSChannel.NextWeekChannel);

                if (threadReConnect == null)
                {
                    threadReConnect = new Thread(new ThreadStart(Reconnect)) { IsBackground = true };
                    threadReConnect.Start();
                }

                if (ThreadRecordPriceDiff == null)
                {
                    ThreadRecordPriceDiff = new Thread(new ThreadStart(LogPriceDiff)) { IsBackground = true };
                    ThreadRecordPriceDiff.Start();
                }
                
            }
        }

        WSChannel SelectedWeek;
        public void ChangeChannel(WSChannelChangeType changeType, WSChannel channel)
        {
            string channelStr = null;
            switch (channel)
            {
                case WSChannel.ThisWeekChannel:
                    channelStr = ConfigInfo.BtcThisWeekChannel;
                    break;
                case WSChannel.NextWeekChannel:
                    channelStr = ConfigInfo.BtcNextWeekChannel;
                    break;
                case WSChannel.QuarterChanne:
                    channelStr = ConfigInfo.BtcQuarterChannel;
                    break;
            }

            string changeStr = null;
            switch (changeType)
            {
                case WSChannelChangeType.Add:
                    changeStr = ConfigInfo.addChannelStr;
                    if (channel != WSChannel.QuarterChanne) SelectedWeek = channel;
                    break;
                case WSChannelChangeType.Remove:
                    changeStr = ConfigInfo.removeChannelStr;
                    break;
            }
            wsb.send("{'event':'" + changeStr + "','channel':'" + channelStr + "'}");
        }

        public void Stop()
        {
            if (wsb != null)
            {
                threadReConnect.Abort();
                threadReConnect = null;

                ThreadRecordPriceDiff.Abort();
                ThreadRecordPriceDiff = null;

                wsb.stop();
                wsb = null;
                wss = null;
            }

        }

        private void Reconnect()
        {
            while (true)
            {
                if (wsb.isReconnect())
                {
                    //wsb.send("{'event':'addChannel','channel':'ok_sub_futureusd_btc_ticker_this_week'}");
                    //wsb.send("{'event':'addChannel','channel':'ok_sub_futureusd_btc_ticker_quarter'}");
                    ChangeChannel(WSChannelChangeType.Add, WSChannel.ThisWeekChannel);
                    ChangeChannel(WSChannelChangeType.Add, WSChannel.NextWeekChannel);
                }
                Thread.Sleep(1000);
            }
        }

        #region Refresh



        private void LogPriceDiff()
        {
            while (true)
            {
                //if (SelectedWeek == WSChannel.ThisWeekChannel)
                //{
                //    Program.Log.LogInfo(string.Format("当周价差记录： 开仓价差 {0}, 平仓价差{1}", quarterBuy - thisWeekSale, quarterSale - thisWeekBuy));
                //}
                //else if (SelectedWeek == WSChannel.NextWeekChannel)
                //{
                //    Program.Log.LogInfo(string.Format("次周价差记录： 开仓价差 {0}, 平仓价差{1}", quarterBuy - nextWeekSale, quarterSale - nextWeekBuy));
                //}
                Program.Log.LogInfo(string.Format("价差记录(次周 -当周)： 开仓价差 {0}, 平仓价差{1}", nextWeekBuy - thisWeekSale, nextWeekSale - thisWeekBuy));
                Thread.Sleep(1000);
            }
        }
        #endregion

        #region Update
        public void UpdateThisWeek(double buy, double sale)
        {
            thisWeekBuy = buy;
            thisWeekSale = sale;
            QueuePrices.Enqueue(new OKCoinPrices() { ThisWeekBuy = thisWeekBuy, ThisWeekSale = thisWeekSale, NextWeekBuy = nextWeekBuy, NextWeekSale = nextWeekSale, QuarterBuy = quarterBuy, QuaterSale = quarterSale });
        }

        public void UpdateNextWeek(double buy, double sale)
        {
            nextWeekBuy = buy;
            nextWeekSale = sale;
            QueuePrices.Enqueue(new OKCoinPrices() { ThisWeekBuy = thisWeekBuy, ThisWeekSale = thisWeekSale, NextWeekBuy = nextWeekBuy, NextWeekSale = nextWeekSale, QuarterBuy = quarterBuy, QuaterSale = quarterSale });
        }

        public void UpdateQuarter(double buy, double sale)
        {
            quarterBuy = buy;
            quarterSale = sale;
            QueuePrices.Enqueue(new OKCoinPrices() { ThisWeekBuy = thisWeekBuy, ThisWeekSale = thisWeekSale, NextWeekBuy = nextWeekBuy, NextWeekSale = nextWeekSale, QuarterBuy = quarterBuy, QuaterSale = quarterSale });
        }
        #endregion

        //private string SignParams(Dictionary<string, string> paramDict)
        //{
        //    StringBuilder sbSign = new StringBuilder();

        //    if (paramDict.Count > 0)
        //    {
        //        var orderedKey = paramDict.Keys.OrderBy(_ => _);
        //        foreach (var item in orderedKey)
        //        {
        //            sbSign.Append(item).Append('=').Append(paramDict[item]).Append('&');
        //        }
        //        sbSign.Append("secret_key=").Append(ConfigInfo.secretKey);
        //    }
        //    //MD5 md5 = new MD5CryptoServiceProvider();
        //    var result = "";
        //    if (sbSign.Length > 0)
        //    {
        //        result = FormsAuthentication.HashPasswordForStoringInConfigFile(sbSign.ToString(), "MD5");
        //    }
        //    return result;
        //}

       
    }
}
