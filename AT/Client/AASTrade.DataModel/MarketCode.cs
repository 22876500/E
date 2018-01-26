using System;

namespace DataModel
{
    [Serializable]
    public class MarketCode
    {
        private string _windCode;  //万得代码    
        private string _market;     //交易所名称
        private string _code;       //原始代码
        private string _enName;     //英文名称
        private string _cnName;     //中文名称
        private int _type;          //证券类型

        public string WindCode
        {
            get { return _windCode; }
            set {
                _windCode = value;
            }
        }

        public string Market
        {
            get { return _market; }
            set {
                _market = value;
            }
        }

        public string Code
        {
            get { return _code; }
            set {
                _code = value;
            }
        }

        public string EnName
        {
            get { return _enName; }
            set {
                _enName = value;
            }
        }

        public string CnName
        {
            get { return _cnName; }
            set {
                _cnName = value;
            }
        }

        public int Type
        {
            get { return _type; }
            set {
                _type = value;
            }
        }

        public string TypeCategory
        {
            get { 
                switch (_type & 0xff00)
                {
                    case 0x0000:
                        return "股票";
                    default:
                        return "其他";
                }
            }
        }

        public string TypeName
        {
            get {
                switch (_type & 0x00ff)
                {
                    case 0x10:
                        return "A股";
                    case 0x11:
                        return "中小板股";
                    case 0x12:
                        return "创业板股";
                    case 0x16:
                        return "B股";
                    default:
                        return "其他";
                }
            }
        }
    }
}
