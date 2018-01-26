using OKCoinClient.OKCoinEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OKCoinClient.ViewModel
{
    public class TradeIndexModel : INotifyPropertyChanged
    {
        #region Members
        const string Coin = OKCoinType.Btc;
        static readonly string[] periods = new[] { OKCoinPeriod.ThisWeek, OKCoinPeriod.NextWeek, OKCoinPeriod.Quarter };

        Dictionary<string, Model.TradeModel> PeriodTradeDict = new Dictionary<string,Model.TradeModel>();
        public Model.IndexModel LatestIndex { get; set; }

        OKCoinAdapter[] TradeAdapters;
        OKCoinAdapter IndexAdapter;

        Thread[] TradeDataThreads;
        Thread IndexDataThread;
        Thread LogDataThread;
        #endregion

        #region Data Binding
        private string _thisWeekSubResult;
        public string ThisWeekSubResult
        {
            get
            {
                return _thisWeekSubResult;
            }
            set
            {
                if (_thisWeekSubResult != value)
                {
                    _thisWeekSubResult = value;
                    NotifyPropertyChange("ThisWeekSubResult");
                }
            }
        }

        private string _nextWeekSubResult;
        public string NextWeekSubResult
        {
            get
            {
                return _nextWeekSubResult;
            }
            set
            {
                if (_nextWeekSubResult != value)
                {
                    _nextWeekSubResult = value;
                    NotifyPropertyChange("NextWeekSubResult");
                }
            }
        }

        private string _quarterSubResult;
        public string QuarterSubResult
        {
            get
            {
                return _quarterSubResult;
            }
            set
            {
                if (_quarterSubResult != value)
                {
                    _quarterSubResult = value;
                    NotifyPropertyChange("QuarterSubResult");
                }
            }
        }

        #endregion

        public TradeIndexModel()
        {
            Start();
        }

        private void Start()
        {
            if (TradeAdapters == null)
            {
                PeriodTradeDict.Clear();
                TradeAdapters = new OKCoinAdapter[periods.Length];
                TradeDataThreads = new Thread[periods.Length];

                for (int i = 0; i < TradeAdapters.Length; i++)
                {
                    PeriodTradeDict.Add(periods[i], null);
                    TradeAdapters[i] = new OKCoinAdapter();
                    TradeAdapters[i].Start();
                    TradeAdapters[i].AddChannel(OKCoinConfig.SubTrade, Coin, periods[i]);

                    TradeDataThreads[i] = new Thread(new ParameterizedThreadStart(TradeDataMain)) { IsBackground = true };
                    TradeDataThreads[i].Start(TradeAdapters[i]);
                }
            }

            if (IndexAdapter == null)
            {
                IndexAdapter = new OKCoinAdapter();
                IndexAdapter.Start();
                IndexAdapter.AddChannel(OKCoinConfig.SubIndex, Coin);

                IndexDataThread = new Thread(new ThreadStart(IndexDataMain)) { IsBackground = true };
                IndexDataThread.Start();
            }

            if (LogDataThread == null)
            {
                LogDataThread = new Thread(new ThreadStart(LogResultMain)) { IsBackground = true };
                LogDataThread.Start();
            }
        }

        private void LogResultMain()
        {
            while (true)
            {
                App.Log.LogInfoFormat("当周升贴水：{0}； 次周升贴水：{1}； 季度升贴水：{2}", ThisWeekSubResult, NextWeekSubResult, QuarterSubResult);
                Thread.Sleep(1000);
            }
        }

        #region Data Deserialize
        private void TradeDataMain(object obj)
        {
            var adapter = obj as OKCoinAdapter;
            if (adapter != null)
            {
                DateTime latestTime = DateTime.MinValue;
                string msg = null;

                while (true)
                {
                    if (adapter.MessageQueue != null && adapter.MessageQueue.Count > 0 && adapter.MessageQueue.TryDequeue(out msg) && OKCoinConfig.IsChannelData(msg))
                    {
                        try
                        {
                            
                            var dataStr = OKCoinConfig.GetChannelData(msg);
                            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<string[][]>(dataStr);
                            if (data != null && data.Length > 0)
                            {
                                var item = data.Last();
                                Model.TradeModel tradeItem = new Model.TradeModel();
                                tradeItem.TradeID = item[0];
                                tradeItem.Price = item[1];
                                tradeItem.TradeQty = item[2];
                                tradeItem.Time = item[3];
                                tradeItem.BSFlag = item[4];
                                UpdateBindDataBuyTradeData(adapter.Channel, tradeItem);
                            }
                        }
                        catch (Exception ex)
                        {
                            App.Log.LogInfoFormat("TradeIndexModel.TradeDataMain Exception:{0}", ex.Message);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }

        private void UpdateBindDataBuyTradeData(string channel, Model.TradeModel latestData)
        {
            if (LatestIndex == null)
                return;

            string subResult = string.Format("{0} = {1} - {2}", Math.Round(double.Parse(latestData.Price) - double.Parse(LatestIndex.futureIndex), 2), latestData.Price, LatestIndex.futureIndex);

            if (channel.Contains(OKCoinEntities.OKCoinPeriod.ThisWeek))
            {
                ThisWeekSubResult = subResult;
                PeriodTradeDict[OKCoinEntities.OKCoinPeriod.ThisWeek] = latestData;
            }
            else if (channel.Contains(OKCoinEntities.OKCoinPeriod.NextWeek))
            {
                NextWeekSubResult = subResult;
                PeriodTradeDict[OKCoinEntities.OKCoinPeriod.NextWeek] = latestData;
            }
            else if (channel.Contains(OKCoinEntities.OKCoinPeriod.Quarter))
            {
                QuarterSubResult = subResult;
                PeriodTradeDict[OKCoinEntities.OKCoinPeriod.Quarter] = latestData;
            }
        }

        private void IndexDataMain()
        {
            string msg;
            while (true)
            {
                if (IndexAdapter.MessageQueue != null && IndexAdapter.MessageQueue.Count > 0 && IndexAdapter.MessageQueue.TryDequeue(out msg))
                {
                    if (OKCoinConfig.IsChannelData(msg))
                    {
                        try
                        {
                            var dataStr = OKCoinConfig.GetChannelData(msg);
                            var IndexItem = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.IndexModel>(dataStr);
                            if (IndexItem != null)
                            {
                                LatestIndex = IndexItem;
                                UpdateBindDataByIndexData(IndexItem);
                            }
                        }
                        catch (Exception ex)
                        {
                            App.Log.LogInfoFormat("TradeIndexModel.IndexDataMain Exception:{0}", ex.Message);
                        }
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void UpdateBindDataByIndexData(Model.IndexModel IndexItem)
        {
            double indexValue = double.Parse(IndexItem.futureIndex);
            if (PeriodTradeDict[OKCoinEntities.OKCoinPeriod.ThisWeek] != null)
            {
                string thisWeekPrice = PeriodTradeDict[OKCoinEntities.OKCoinPeriod.ThisWeek].Price;
                ThisWeekSubResult = string.Format("{0} = {1} - {2}", Math.Round(double.Parse(thisWeekPrice) - indexValue, 2), thisWeekPrice, LatestIndex.futureIndex);
            }
            if (PeriodTradeDict[OKCoinEntities.OKCoinPeriod.NextWeek] != null)
            {
                string nextWeekPrice = PeriodTradeDict[OKCoinEntities.OKCoinPeriod.NextWeek].Price;
                NextWeekSubResult = string.Format("{0} = {1} - {2}", Math.Round(double.Parse(nextWeekPrice) - indexValue, 2), nextWeekPrice, LatestIndex.futureIndex);
            }
            if (PeriodTradeDict[OKCoinEntities.OKCoinPeriod.Quarter] != null)
            {
                string quarterPrice = PeriodTradeDict[OKCoinEntities.OKCoinPeriod.Quarter].Price;
                QuarterSubResult = string.Format("{0} = {1} - {2}", Math.Round(double.Parse(quarterPrice) - indexValue, 2), quarterPrice, LatestIndex.futureIndex);
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
