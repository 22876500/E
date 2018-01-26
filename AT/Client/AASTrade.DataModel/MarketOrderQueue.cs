using System;

namespace DataModel
{
    [Serializable]
    public class MarketOrderQueue
    {
        private string _windCode;  //万得代码
        private string _code;       //原始代码
        private int _actionDay;     //业务发生日
        private int _time;          //业务发生时间
        private int _side;          //买卖方向
        private long _price;         //委托价格
        private int _orders;        //订单数量
        private int _items;          //明细个数
        private int[] _volume;      //订单明细

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

        public int Side
        {
            get { return _side; }
            set {
                _side = value;
            }
        }

        public long Price
        {
            get { return _price; }
            set {
                _price = value;
            }
        }

        public int Orders
        {
            get { return _orders; }
            set {
                _orders = value;
            }
        }

        public int Items
        {
            get { return _items; }
            set {
                _items = value;
            }
        }

        public int[] Volume
        {
            get { return _volume; }
            set {
                _volume = value;
            }
        }

        public MarketOrderQueue()
        { 
            _volume = new int[200];
        }
    }
}
