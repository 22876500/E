using DataServerIce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;


namespace AASDataServer.Model
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
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                OnPropertyChanged("Code");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Market
        {
            get
            {
                return _market;
            }
            set
            {
                _market = value;
                OnPropertyChanged("Market");
            }
        }

        public string Pinyin
        {
            get
            {
                return _pinyin;
            }
            set
            {
                _pinyin = value;
                OnPropertyChanged("Pinyin");
            }
        }

        public string Wind
        {
            get
            {
                return _wind;
            }
            set
            {
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

        public string SearchText
        {
            get
            {
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
            _type = 0;
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

        public StockCode(StockCode code)
        {
            Code = code.Code;
            Market = code.Market;
            Name = code.Name;
            Pinyin = code.Pinyin;
            Wind = code.Wind;
            Type = code.Type;
        }

        public DSIceStockCode GetDSIceStockCode()
        {
            DSIceStockCode code = new DSIceStockCode();
            code.Code = _code;
            code.Market = _market;
            code.Name = _name;
            code.Pinyin = _pinyin;
            code.Wind = _wind;
            code.Type = _type;
            return code;
        }
    }
}
