using System;
using System.Collections.Generic;
using System.Linq;
using AASTrader.Model;
using AASTrader.Model.DataModel;
//using Microsoft.Practices.Unity;

namespace AASClient.Manager
{
    public class StockCodeManager : IManager
    {
        private static object sync = new object();
        private static StockCodeManager _instance;

        public static StockCodeManager GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new StockCodeManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private Dictionary<string, StockCode> _codes;



        public Dictionary<string, StockCode> Codes
        {
            get { return _codes; }
        }

        public List<StockCode> CodeList
        {
            get { return _codes.Values.ToList<StockCode>(); }
        }

        public StockCodeManager()
        {
            _codes = new Dictionary<string, StockCode>();
        }

        public StockCode GetStockCode(string code)
        {
            if (_codes.ContainsKey(code))
            {
                return _codes[code];
            }

            return null;
        }

        public string GetStockName(string code)
        {
            if (_codes.ContainsKey(code))
            {
                return _codes[code].Name;
            }

            return "";
        }

        public string GetStockTitle(string code)
        {
            if (_codes.ContainsKey(code))
            {
                return string.Format("{0} [{1}]", _codes[code].Name, code);
            }

            return "";
        }

        public string FindStockCode(string filter)
        {
            List<StockCode> sg = new List<StockCode>();
            string f = filter.ToLower().Trim();
            foreach (StockCode code in CodeList)
            {
                if (code.SearchText.Contains(filter))
                {
                    return code.Code;
                }
            }

            return "";
        }

        public void UpdateCodes(List<StockCode> codes)
        {
            try
            {
                _codes.Clear();
                Dictionary<string, string> pinyin = GetPinyinDict();
                foreach (StockCode code in codes)
                {
                    if (_codes.ContainsKey(code.Code) == false)
                    {
                        //更新拼音
                        if (pinyin.ContainsKey(code.Code))
                        {
                            code.Pinyin = pinyin[code.Code];
                        }
                        _codes.Add(code.Code, code);
                    }
                }
            }
            catch (Exception ex)
            {
                AASClient.Program.logger.LogRunning("StockManager:股票代码更新失败\n{0}", ex.Message);
            }
        }

        #region 拼音字典
        public Dictionary<string, string> GetPinyinDict()
        {
            Dictionary<string, string> codes = new Dictionary<string, string>();
            return codes;
        }
        #endregion

        private static Dictionary<string, string> dict = new Dictionary<string, string>();
        public static string GetNameByCode(string code)
        {
            if (dict.Count < 1)
            {
                var lst = codeNames.Split('\n');
                foreach (var item in lst)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var nv = item.Split(':');
                        if (nv.Length == 2)
                        {
                            dict.Add(nv[0], nv[1]);
                        }
                    }
                }
            }
            if (dict.ContainsKey(code))
            {
                return dict[code];
            }
            else return string.Empty;
        }

        #region Codes
        private static string codeNames = @"";
        #endregion
    }
}
