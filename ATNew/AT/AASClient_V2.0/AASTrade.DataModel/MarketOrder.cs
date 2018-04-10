using System;

namespace DataModel
{
    [Serializable]
    public class MarketOrder
    {
        private string _windCode;  //万得代码
        private string _code;       //原始代码
        private int _actionDay;     //业务发生日
        private int _time;          //业务发生时间
        private int _order;         //委托号
        private long _price;         //委托价格
        private int _volume;         //委托数量
        private byte _orderKind;     //委托类别
        private byte _functionCode;  //委托代码


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

        public int Order
        {
            get { return _order; }
            set {
                _order = value;
            }
        }

        public long Price
        {
            get { return _price; }
            set
            {
                _price = value;
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

        public byte OrderKind
        {
            get { return _orderKind; }
            set
            {
                _orderKind = value;
            }
        }

        public byte FunctionCode
        {
            get { return _functionCode; }
            set
            {
                _functionCode = value;
            }
        }

    }
}
