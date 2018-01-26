using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BitMex.ViewModel
{
    public class SubResultMixViewModel : INotifyPropertyChanged
    {
        Adapter.BitMexAdapter AdapterBitMex = new Adapter.BitMexAdapter();
        Adapter.OKCoinAdapter AdapterOKCoinQuarter = new Adapter.OKCoinAdapter();
        Adapter.OKCoinAdapter AdapterOKCoinTrade = new Adapter.OKCoinAdapter();

        Regex buyPriceReg = new Regex("buy\":\"([.0-9]+)\"");
        Regex sellPriceReg = new Regex("sell\":\"([.0-9]+)\"");

        Thread ThreadBitMex;
        Thread ThreadOKCoin;

        public double PriceSellQuarter { get; private set; }

        public double PriceBuyQuarter { get; private set; }

        public double PriceTradeQuarter { get; private set; }

        public double PriceBitMexBuy { get; set; }

        public double PriceBitMexSell { get; set; }

        public double PriceBitMexTrade { get; set; }

        #region Bind Properties
        private string _openPrice;
        public string OpenPrice
        {
            get { return _openPrice; }
            private set
            {
                if (_openPrice != value)
                {
                    _openPrice = value;
                    NotifyPropertyChange("OpenPrice");
                }
            }
        }

        private string _closePrice;
        public string ClosePrice
        {
            get { return _closePrice; }
            private set
            {
                if (_closePrice != value)
                {
                    _closePrice = value;
                    NotifyPropertyChange("ClosePrice");
                }
            }
        }

        private string _subMix;
        public string SubMix
        {
            get { return _subMix; }
            set
            {
                if (_subMix != value)
                {
                    _subMix = value;
                    NotifyPropertyChange("SubMix");
                }
            }
        }

        private double _openPriceMax;
        public double OpenPriceMax
        {
            get
            {
                return _openPriceMax;
            }
            set
            {
                if (_openPriceMax < value)
                {
                    _openPriceMax = value;
                    NotifyPropertyChange("OpenPriceMax");
                }
            }
        }

        private double _openPriceMin;
        public double OpenPriceMin
        {
            get { return _openPriceMin; }
            set
            {
                if (_openPriceMin == 0 || _openPriceMin > value)
                {
                    _openPriceMin = value;
                    NotifyPropertyChange("OpenPriceMin");
                }
            }
        }

        private double _closePriceMax;
        public double ClosePriceMax
        {
            get { return _closePriceMax; }
            set
            {
                if (_closePriceMax < value)
                {
                    _closePriceMax = value;
                    NotifyPropertyChange("ClosePriceMax");
                }
            }
        }

        private double _closePriceMin;
        public double ClosePriceMin
        {
            get { return _closePriceMin; }
            set
            {
                if (_closePriceMin == 0 | _closePriceMin > value)
                {
                    _closePriceMin = value;
                    NotifyPropertyChange("ClosePriceMin");
                }
            }
        }

        private double _subMixMax;
        public double SubMixMax 
        {
            get { return _subMixMax; }
            set {
                if (_subMixMax < value)
                {
                    _subMixMax = value;
                    NotifyPropertyChange("SubMixMax");
                }
            }
        }

        private double _subMixMin;
        public double SubMixMin
        {
            get { return _subMixMin; }
            set
            {
                if (_subMixMin == 0 || _subMixMin> value)
                {
                    _subMixMin = value;
                    NotifyPropertyChange("SubMixMin");
                }
            }
        }
        #endregion


        public SubResultMixViewModel()
        {
            Init();
        }

        void Init()
        {
            AdapterOKCoinQuarter.Start();
            AdapterOKCoinQuarter.AddChannel(Config.OKCoinConfig.SubTicker, "btc", "quarter");

            AdapterOKCoinTrade.Start();
            AdapterOKCoinTrade.AddChannel(Config.OKCoinConfig.SubTrade, "btc", "quarter");

            ThreadOKCoin = new Thread(new ThreadStart(OKCoinMain)) { IsBackground = true };
            ThreadOKCoin.Start();

            ThreadBitMex = new Thread(new ThreadStart(BitMexMain)) { IsBackground = true };
            ThreadBitMex.Start();

            Thread thread = new Thread(new ThreadStart(() => {
                AdapterBitMex.Start(Config.BitMexChannel.OrderBook10 + ":XBTUSD,trade:XBTUSD");
            })) { IsBackground = true };
            thread.Start();
        }

        private void OKCoinMain()
        {
            string msg;
            while (true)
            {
                if (AdapterOKCoinQuarter.MessageQueue.Count > 0 && AdapterOKCoinQuarter.MessageQueue.TryDequeue(out msg))
                {
                    string buyStr = buyPriceReg.Match(msg).Groups[1].Value;
                    string sellStr = sellPriceReg.Match(msg).Groups[1].Value;
                    if (!string.IsNullOrEmpty(buyStr))
                    {
                        PriceBuyQuarter = double.Parse(buyStr);
                    }
                    //更新季度买一卖一
                    if (!string.IsNullOrEmpty(sellStr))
                    {
                        PriceSellQuarter = double.Parse(sellStr);
                    }
                    if (!string.IsNullOrEmpty(sellStr) || !string.IsNullOrEmpty(buyStr))
                    {
                        RefreshOpenClose();
                    }
                }
                else if (AdapterOKCoinTrade.MessageQueue.Count > 0 && AdapterOKCoinTrade.MessageQueue.TryDequeue(out msg))
                {
                    if (Config.OKCoinConfig.IsChannelData(msg))
                    {
                        var dataStr = Config.OKCoinConfig.GetChannelData(msg);
                        var obj = dataStr.FromJson<string[][]>();
                        PriceTradeQuarter = double.Parse(obj.Last()[1]);
                        if (PriceTradeQuarter > 0 && PriceBitMexTrade > 0)
                        {
                            RefreshSubMix();
                        }
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }

            }
        }

        

        private void BitMexMain()
        {
            string msg;
            while (true)
            {
                if (AdapterBitMex.MessageQueue.Count > 0 && AdapterBitMex.MessageQueue.TryDequeue(out msg))
                {
                    if (msg.StartsWith("{\"table\":\"orderBook10\""))
                    {
                        if (msg.Contains("\"data\":"))
                        {
                            var OrderBook10Item = msg.FromJson<BitMexMsg.MsgOrderBook10>();
                            if (OrderBook10Item.action == "update" || OrderBook10Item.action == "insert")
                            {
                                PriceBitMexBuy = OrderBook10Item.data[0].bids[0][0];
                                PriceBitMexSell = OrderBook10Item.data[0].asks[0][0];
                                RefreshOpenClose();
                            }
                        }
                        else {
                            App.Log.LogInfo(msg);
                        }
                    }
                    else if(msg.StartsWith("{\"table\":\"trade\""))
                    {
                        var orderTradeItem = msg.FromJson<BitMexMsg.MsgTrade>();
                        if (orderTradeItem!= null && orderTradeItem.data.Length > 0)
                        {
                            PriceBitMexTrade = double.Parse(orderTradeItem.data.First().price);
                            RefreshSubMix();
                        }
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }

            }
        }

        #region Refresh Data
        private void RefreshSubMix()
        {
            if (PriceBitMexTrade > 0 && PriceTradeQuarter > 0)
            {
                SubMix = string.Format("{0} = {1} - {2}", Math.Round(PriceTradeQuarter - PriceBitMexTrade), PriceTradeQuarter, PriceBitMexTrade);
                if (PriceTradeQuarter != 0 && PriceBitMexTrade != 0)
                {
                    SubMixMax = Math.Round(PriceTradeQuarter - PriceBitMexTrade);
                    SubMixMin = Math.Round(PriceTradeQuarter - PriceBitMexTrade);
                }
            }
        }

        private void RefreshOpenClose()
        {
            OpenPrice = string.Format("{0} = {1} - {2}", Math.Round(PriceBuyQuarter - PriceBitMexBuy), PriceBuyQuarter, PriceBitMexBuy);
            ClosePrice = string.Format("{0} = {1} - {2}", Math.Round(PriceSellQuarter - PriceBitMexSell), PriceSellQuarter, PriceBitMexSell);
            if (PriceBuyQuarter > 0 && PriceBitMexSell > 0)
            {
                OpenPriceMax = Math.Round(PriceBuyQuarter - PriceBitMexBuy);
                OpenPriceMin = Math.Round(PriceBuyQuarter - PriceBitMexBuy);
            }
            if (PriceSellQuarter > 0 && PriceBitMexBuy > 0)
            {
                ClosePriceMax = Math.Round(PriceSellQuarter - PriceBitMexSell);
                ClosePriceMin = Math.Round(PriceSellQuarter - PriceBitMexSell);
            }
        } 
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //根据PropertyChanged事件的委托类，实现PropertyChanged事件：
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
