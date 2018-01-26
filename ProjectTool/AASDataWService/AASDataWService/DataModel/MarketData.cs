using System;

namespace AASDataWService.DataModel
{
    [Serializable]
    public class MarketData
    {
        private string _windCode;  //万得代码 
        private string _code;       //原始代码
        private int _actionDay;     //业务发生日
        private int _time;          //业务发生时间
        private int _status;        //状态
        private long _preClose;      //前收盘价
        private long _open;          //开盘价
        private long _high;          //最高价
        private long _low;           //最低价
        private long _match;         //最新价
        private long[] _askPrice;   //申卖价
        private int[] _askVol;     //申卖量
        private long[] _bidPrice;   //申买价
        private int[] _bidVol;       //申买量
        private int _numTrades;     //成交笔数
        private int _volume;       //成交总量
        private long _turnover;     //成交总金额
        private int _totalBidVol;  //委托买入总量
        private int _totalAskVol;  //委托卖出总量
        private long _weightedAvgBidPrice;  //加权平均委买价格
        private long _weightedAvgAskPrice;  //加权平均委卖价格
        private int _IOPV;                  //IOPV净值估值
        private int _yieldToMaturity;       //到期收益率
        private long _highLimited;          //涨停价
        private long _lowLimited;           //跌停价
        private byte[] _prefix;             //证券信息前缀
        private int _syl1;                 //市盈率1
        private int _syl2;                  //市盈率2
        private int _sd2;                   //升跌2

        public string WindCode
        {
            get { return _windCode; }
            set
            {
                _windCode = value;
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
            }
        }

        public int ActionDay
        {
            get { return _actionDay; }
            set
            {
                _actionDay = value;
            }
        }

        public int Time
        {
            get { return _time; }
            set
            {
                _time = value;
            }
        }

        public int Status
        {
            get { return _status; }
            set
            {
                _status = value;
            }
        }

        public long PreClose
        {
            get { return _preClose; }
            set
            {
                _preClose = value;
            }
        }

        public long Open
        {
            get { return _open; }
            set
            {
                _open = value;
            }
        }

        public long High
        {
            get { return _high; }
            set
            {
                _high = value;
            }
        }

        public long Low
        {
            get { return _low; }
            set
            {
                _low = value;
            }
        }

        public long Match
        {
            get { return _match; }
            set
            {
                _match = value;
            }
        }

        public long[] AskPrice
        {
            get { return _askPrice; }
            set
            {
                _askPrice = value;
            }
        }

        public int[] AskVol
        {
            get { return _askVol; }
            set
            {
                _askVol = value;
            }
        }

        public long[] BidPrice
        {
            get { return _bidPrice; }
            set
            {
                _bidPrice = value;
            }
        }

        public int[] BidVol
        {
            get { return _bidVol; }
            set
            {
                _bidVol = value;
            }
        }

        public int NumTrades
        {
            get { return _numTrades; }
            set
            {
                _numTrades = value;
            }
        }

        public int Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
            }
        }

        public long Turnover
        {
            get { return _turnover; }
            set
            {
                _turnover = value;
            }
        }

        public int TotalBidVol
        {
            get { return _totalBidVol; }
            set
            {
                _totalBidVol = value;
            }
        }

        public int TotalAskVol
        {
            get { return _totalAskVol; }
            set
            {
                _totalAskVol = value;
            }
        }

        public long WeightedAvgBidPrice
        {
            get { return _weightedAvgBidPrice; }
            set
            {
                _weightedAvgBidPrice = value;
            }
        }

        public long WeightedAvgAskPrice
        {
            get { return _weightedAvgAskPrice; }
            set
            {
                _weightedAvgAskPrice = value;
            }
        }

        public int IOPV
        {
            get { return _IOPV; }
            set
            {
                _IOPV = value;
            }
        }

        public int YieldToMaturity
        {
            get { return _yieldToMaturity; }
            set
            {
                _yieldToMaturity = value;
            }
        }

        public long HighLimited
        {
            get { return _highLimited; }
            set
            {
                _highLimited = value;
            }
        }

        public long LowLimited
        {
            get { return _lowLimited; }
            set
            {
                _lowLimited = value;
            }
        }

        public byte[] Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
            }
        }

        public int Syl1
        {
            get { return _syl1; }
            set
            {
                _syl1 = value;
            }
        }

        public int Syl2
        {
            get { return _syl2; }
            set
            {
                _syl2 = value;
            }
        }

        public int SD2
        {
            get { return _sd2; }
            set
            {
                _sd2 = value;
            }
        }

        public MarketData()
        {
            _askPrice = new long[10];
            _askVol = new int[10];
            _bidPrice = new long[10];
            _bidVol = new int[10];
            _prefix = new byte[4];
        }
    }
}
