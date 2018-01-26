//using ClientServerIce.BasicService;
using DataServerIce;

namespace AASTrader.Model.DataModel
{
    public class StockCode : AbstractModel
    {
        private string _code;
        private string _name;
        private string _market;
        private string _pinyin;
        private string _wind;
        private int _type;

        public string Code
        {
            get {
                return _code;
            }
            set {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        public string Name
        {
            get {
                return _name;
            }
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Market
        {
            get {
                return _market;
            }
            set {
                _market = value;
                OnPropertyChanged("Market");
            }
        }

        public string Pinyin
        {
            get {
                return _pinyin;
            }
            set {
                _pinyin = value;
                OnPropertyChanged("Pinyin");
            }
        }

        public string Wind
        {
            get {
                return _wind;
            }
            set {
                _wind = value;
                OnPropertyChanged("Wind");
            }
        }

        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        public bool IsHundred
        {
            get
            {
                int type = _type & 0xff;
                if (type == 0x10 || type == 0x11 || type == 0x12)
                {
                    return true;
                }

                return false;
            }
        }

        public string SearchText
        {
            get {
                return string.Format("{0} {1} {2}", _code.ToLower(), _name, _pinyin.ToLower());
            }
        }

        public StockCode()
        {
            _code = "000000";
            _market = "";
            _name = "";
            _pinyin = "";
            _wind = "";
        }

        public StockCode(DSIceStockCode code)
        {
            _code = code.Code;
            _market = code.Market;
            _name = code.Name;
            _pinyin = code.Pinyin;
            _wind = code.Wind;
            _type = code.Type;
        }

        //public StockCode(CSIceStockCode code)
        //{
        //    _code = code.Code;
        //    _market = code.Market;
        //    _name = code.Name;
        //    _pinyin = code.Pinyin;
        //    _wind = code.Wind;
        //}
    }
}
