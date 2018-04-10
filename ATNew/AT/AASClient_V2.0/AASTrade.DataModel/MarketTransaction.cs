using System;

namespace DataModel
{
    [Serializable]
    public class MarketTransaction
    {
        private string _windCode;  //万得代码
        private string _code;       //原始代码
        private int _actionDay;     //业务发生日
        private int _time;          //业务发生时间
        private int _index;         //成交编号
        private long _price;         //成交价格
        private int _volume;         //成交数量
        private int _turnover;       //成交金额
        private int _flag;           //买卖方向
        private byte _orderKind;     //成交类别
        private byte _functionCode;  //成交代码
        private int _askOrder;      //卖方委托序号
        private int _bidOrder;      //买方委托序号


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

        public int Index
        {
            get { return _index; }
            set {
                _index = value;
            }
        }

        public long Price
        {
            get { return _price; }
            set {
                _price = value;
            }
        }

        public int Volume
        {
            get { return _volume; }
            set {
                _volume = value;
            }
        }

        public int Turnover
        {
            get { return _turnover; }
            set {
                _turnover = value;
            }
        }

        public int Flag
        {
            get { return _flag; }
            set {
                _flag = value;
            }
        }

        public byte OrderKind
        {
            get { return _orderKind; }
            set {
                _orderKind = value;
            }
        }

        public byte FunctionCode
        {
            get { return _functionCode; }
            set {
                _functionCode = value;
            }
        }

        public int AskOrder
        {
            get { return _askOrder; }
            set {
                _askOrder = value;
            }
        }

        public int BidOrder
        {
            get { return _bidOrder; }
            set { 
                _bidOrder = value; 
            }
        }
    }
}
