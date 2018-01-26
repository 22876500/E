using BitMex.BitMexMsg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BitMex.ViewModel
{
    public class SubViewModel : INotifyPropertyChanged
    {
        #region Properties
        private string _openPriceDiff;
        public string OpenPriceDiff
        {
            get
            {
                return _openPriceDiff;
            }
            set
            {
                if (_openPriceDiff != value)
                {
                    _openPriceDiff = value;
                    NotifyPropertyChange("OpenPriceDiff");
                }
            }
        }

        private string _closePriceDiff;
        public string ClosePriceDiff
        {
            get
            {
                return _closePriceDiff;
            }
            set
            {
                if (_closePriceDiff != value)
                {
                    _closePriceDiff = value;
                    NotifyPropertyChange("ClosePriceDiff");
                }
            }
        } 
        #endregion

        Adapter.BitMexAdapter AdapterXBTUSD = new Adapter.BitMexAdapter();
        Adapter.BitMexAdapter AdapterXBTZ17 = new Adapter.BitMexAdapter();

        Thread ThreadXBTUSD;
        Thread ThreadXBTZ17;

        double XBTUSD_LastBuyPrice;
        double XBTUSD_LastSellPrice;

        double XBTZ17_LastBuyPrice;
        double XBTZ17_LastSellPrice;

        public SubViewModel()
        {
            var thread = new Thread(new ThreadStart(()=>{
                AdapterXBTUSD.Start(Config.BitMexChannel.OrderBook10 + ":XBTUSD", Config.BitMexChannel.OrderBook10 + ":XBTZ17");
                ThreadXBTUSD = new Thread(new ThreadStart(DesDataMain)) { IsBackground = true };
                ThreadXBTUSD.Start();
            })) { IsBackground = true };
            thread.Start();

            //Task task = new Task(() => {
                

               
            //});
            //task.Start();

            //Task task1 = new Task(() => {
            //    AdapterXBTZ17.Start(Config.BitMexChannel.OrderBook10 + ":XBTZ17");
            //    ThreadXBTZ17 = new Thread(new ThreadStart(XBTZ17_Main)) { IsBackground = true };
            //    ThreadXBTZ17.Start();
            //});
            //task1.Start();
        }

        private void DesDataMain()
        {
            string msg;
            while (true)
            {
                try
                {
                    if (AdapterXBTUSD.MessageQueue.Count > 0 && AdapterXBTUSD.MessageQueue.TryDequeue(out msg))
                    {
                        if (msg.StartsWith("{\"table\":\"orderBook10\""))
                        {
                            if (msg.Contains("\"data\":"))
                            {
                                var OrderBook10Item = msg.FromJson<BitMexMsg.MsgOrderBook10>();
                                XBTUSD_LastBuyPrice = OrderBook10Item.data[0].bids[0][0];
                                XBTUSD_LastSellPrice = OrderBook10Item.data[0].asks[0][0];
                                OpenPriceDiff = string.Format("{0} = {1} - {2}", XBTZ17_LastBuyPrice - XBTUSD_LastSellPrice, XBTZ17_LastBuyPrice, XBTUSD_LastSellPrice);
                                ClosePriceDiff = string.Format("{0} = {1} - {2}", XBTZ17_LastSellPrice - XBTUSD_LastBuyPrice, XBTZ17_LastSellPrice, XBTUSD_LastBuyPrice);
                            }
                            
                        }
                        else if (true)
                        {
                            
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    App.Log.LogInfoFormat("SubViewModel.XBTUSD_Main 异常：{0}", ex.Message);
                    Thread.Sleep(100);
                }
            }
        }

        private void OrderBookL2_XBTUSD(string msg)
        {
            var lv2 = msg.FromJson<BitMexMsg.MsgOrderBookL2>();
            if (lv2.data != null && lv2.action == "insert")
            {
                double priceBuy = 0;
                double priceSell = 0;
                bool hasGetBuyPrice = false;
                bool hasGetSellPrice = false;
                for (int i = 0; i < lv2.data.Length; i++)
                {
                    var item = lv2.data[i];
                    if (!hasGetSellPrice && item.side == "Sell" && double.TryParse(item.price, out priceSell))
                    {
                        hasGetSellPrice = true;

                    }
                    else if (!hasGetBuyPrice && item.side == "Buy" && double.TryParse(item.price, out priceBuy))
                    {
                        hasGetBuyPrice = true;
                    }
                    if (hasGetSellPrice && hasGetBuyPrice)
                    {
                        break;
                    }
                }
                if (priceBuy > 0 && priceBuy != XBTUSD_LastBuyPrice)
                {
                    XBTUSD_LastBuyPrice = priceBuy;
                }
                if (priceSell > 0 && priceSell != XBTUSD_LastSellPrice)
                {
                    XBTUSD_LastSellPrice = priceSell;
                }
                OpenPriceDiff = string.Format("{0} = {1} - {2}", XBTZ17_LastBuyPrice - XBTUSD_LastSellPrice, XBTZ17_LastBuyPrice, XBTUSD_LastSellPrice);
                ClosePriceDiff = string.Format("{0} = {1} - {2}", XBTZ17_LastSellPrice - XBTUSD_LastBuyPrice, XBTZ17_LastSellPrice, XBTUSD_LastBuyPrice);
                App.Log.LogInfo(msg);
            }
            else if (lv2.data != null && lv2.action == "update")
            {
                App.Log.LogInfo(msg);
            }
        }

        private void XBTZ17_Main()
        {
            string msg;
            while (true)
            {
                try
                {
                    if (AdapterXBTZ17.MessageQueue.Count > 0 && AdapterXBTZ17.MessageQueue.TryDequeue(out msg))
                    {
                        if (msg.StartsWith("{\"table\":") && msg.Contains("data"))
                        {
                            var OrderBook10Item = msg.FromJson<BitMexMsg.MsgOrderBook10>();
                            if (OrderBook10Item.action == "update" || OrderBook10Item.action == "insert")
                            {
                                XBTZ17_LastBuyPrice = OrderBook10Item.data[0].bids[0][0];
                                XBTZ17_LastSellPrice = OrderBook10Item.data[0].asks[0][0];
                                OpenPriceDiff = string.Format("{0} = {1} - {2}", XBTZ17_LastBuyPrice - XBTUSD_LastSellPrice, XBTZ17_LastBuyPrice, XBTUSD_LastSellPrice);
                                ClosePriceDiff = string.Format("{0} = {1} - {2}", XBTZ17_LastSellPrice - XBTUSD_LastBuyPrice, XBTZ17_LastSellPrice, XBTUSD_LastBuyPrice);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    App.Log.LogInfoFormat("SubViewModel.XBTZ17_Main 异常：{0}", ex.Message);
                }
            }
        }

        private void OrderBookL2_XBTZ17(string msg)
        {
            var lv2 = msg.FromJson<BitMexMsg.MsgOrderBookL2>();
            if (lv2.data != null && lv2.action == "insert")
            {
                double priceBuy = 0;
                double priceSell = 0;
                bool hasGetBuyPrice = false;
                bool hasGetSellPrice = false;
                for (int i = 0; i < lv2.data.Length; i++)
                {
                    var item = lv2.data[i];
                    if (!hasGetSellPrice && item.side == "Sell" && double.TryParse(item.price, out priceSell))
                    {
                        hasGetSellPrice = true;

                    }
                    else if (!hasGetBuyPrice && item.side == "Buy" && double.TryParse(item.price, out priceBuy))
                    {
                        hasGetBuyPrice = true;
                    }
                    if (hasGetSellPrice && hasGetBuyPrice)
                    {
                        break;
                    }
                }
                if (priceBuy > 0 && priceBuy != XBTZ17_LastBuyPrice)
                {
                    XBTZ17_LastBuyPrice = priceBuy;
                }
                if (priceSell > 0 && priceSell != XBTZ17_LastSellPrice)
                {
                    XBTZ17_LastSellPrice = priceSell;
                }
                OpenPriceDiff = string.Format("{0} = {1} - {2}", XBTZ17_LastBuyPrice - XBTUSD_LastSellPrice, XBTZ17_LastBuyPrice, XBTUSD_LastSellPrice);
                ClosePriceDiff = string.Format("{0} = {1} - {2}", XBTZ17_LastSellPrice - XBTUSD_LastBuyPrice, XBTZ17_LastSellPrice, XBTUSD_LastBuyPrice);
            }
        }


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
