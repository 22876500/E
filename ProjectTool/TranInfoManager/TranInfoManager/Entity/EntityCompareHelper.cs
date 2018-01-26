using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TranInfoManager.Entity
{
    public static class EntityCompareHelper
    {
        public static Dictionary<MarketDetailDaily, TraderDailyDetail> GetCompareData(List<MarketDetailDaily> m, List<TraderDailyDetail> t, List<MarketDetailDaily> listNotMatched)
        {
            var dict = new Dictionary<MarketDetailDaily, TraderDailyDetail>();
            var dictTrade = new Dictionary<string, List<TraderDailyDetail>>();
            foreach (var item in t)
            {
                if (dictTrade.ContainsKey(item.Sym_Format))
                    dictTrade[item.Sym_Format].Add(item);
                else
                    dictTrade.Add(item.Sym_Format, new List<TraderDailyDetail>() { item });
            }

            foreach (var item in m)
            {
                if (dictTrade.ContainsKey(item.Sym_Format))
                {
                    var tradeItem = dictTrade[item.Sym_Format].FirstOrDefault(_ => Match(_, item));
                    if (tradeItem != null)
                    {
                        dict.Add(item, tradeItem);
                        dictTrade[item.Sym_Format].Remove(tradeItem);
                    }
                    else
                    {
                        listNotMatched.Add(item);
                    }
                }
                else
                {
                    listNotMatched.Add(item);
                }
            }

            return dict;
        }

        #region Compare Daily Calculate
        public static List<CompareDaily> GetCompareDaily(List<MarketDetailDaily> m, List<TraderDailyDetail> t)
        {
            var dictTrade = new Dictionary<string, List<TraderDailyDetail>>();
            var dictResult = new Dictionary<string, CompareDaily>();
            foreach (var item in t)
            {
                if (dictTrade.ContainsKey(item.Sym_Format))
                    dictTrade[item.Sym_Format].Add(item);
                else
                    dictTrade.Add(item.Sym_Format, new List<TraderDailyDetail>() { item });
            }

            foreach (var marketItem in m)
            {
                if (dictTrade.ContainsKey(marketItem.Sym_Format))
                {
                    var tradeItem = dictTrade[marketItem.Sym_Format].FirstOrDefault(_ => Match(_, marketItem));
                    if (tradeItem != null)
                    {
                        dictTrade[marketItem.Sym_Format].Remove(tradeItem);

                        var cKey = GetMixedKey(marketItem, tradeItem);
                        if (dictResult.ContainsKey(cKey))
                            dictResult[cKey].Add(marketItem);
                        else
                            dictResult.Add(cKey, new CompareDaily(marketItem, tradeItem.Trader));
                    }
                }
            }

            var lst = dictResult.Values.OrderBy(_=>_.DATE).ThenBy(_=>_.TRADER).ThenBy(_=>_.Symbol2).ToList();
            lst.ForEach(_ => _.Other += (decimal)0.03);
            AddDailySum(lst);
            
            return lst;
        }

        /// <summary>
        /// 增加小计信息
        /// </summary>
        /// <param name="lstDetail"></param>
        private static void AddDailySum(List<CompareDaily> lstDetail)
        {
            CompareDaily TraderDailySum = null;
            for (int i = 0; i < lstDetail.Count; i++)
            {
                var o = lstDetail[i];
                if (i == 0)
                {
                    TraderDailySum = new CompareDaily() { DATE = o.DATE, TRADER = o.TRADER, Symbol2 = "小计", Comm = o.Comm, ECN = o.ECN, Gross = o.Gross, Shares = o.Shares, Other = o.Other, Seq = 1 };
                }
                else if (lstDetail[i].DATE != lstDetail[i - 1].DATE || lstDetail[i].TRADER != lstDetail[i - 1].TRADER)
                {
                    lstDetail.Insert(i, TraderDailySum);
                    TraderDailySum = new CompareDaily() { DATE = o.DATE, TRADER = o.TRADER, Symbol2 = "小计", Comm = o.Comm, ECN = o.ECN, Gross = o.Gross, Shares = o.Shares, Other = o.Other, Seq = 1 };
                    i++;
                }
                else
                {
                    TraderDailySum.Comm += o.Comm;
                    TraderDailySum.ECN += o.ECN;
                    TraderDailySum.Gross += o.Gross;
                    TraderDailySum.Other += o.Other;
                    TraderDailySum.Shares += o.Shares;
                }
            }
            if (lstDetail.Count > 0 && lstDetail.Last() != TraderDailySum)
                lstDetail.Add(TraderDailySum);
        }

        private static string GetMixedKey(MarketDetailDaily m, TraderDailyDetail t)
        {
            return string.Format("{0}_{1}_{2}", m.TradeDate, t.Trader, m.Sym_Format);
        } 
        #endregion

        public static List<CompareTrader> GetCompareTrader(DateTime st, DateTime et, List<CompareDaily> listCompareDaily)
        {
            var list = new List<CompareTrader>();

            if (listCompareDaily.Count > 0)
            {
                var dict = new Dictionary<string, CompareTrader>();
                foreach (var item in listCompareDaily)
                {
                    if (item.Seq == 1 || item.Symbol2 == "小计" || item.Symbol2 == "合计")
                        continue;

                    if (dict.ContainsKey(item.TRADER))
                    {
                        dict[item.TRADER].Add(item);
                    }
                    else
                    {
                        dict.Add(item.TRADER, new CompareTrader(st, et, item));
                    }
                }
                list.AddRange(dict.Values);
                AddSumInfo(st, et, list);
            }

            return list;
        }

        private static void AddSumInfo(DateTime startDate, DateTime endDate, List<CompareTrader> lst)
        {
            string trader = Regex.Replace(lst.First().Trader, "\\d+$", "");
            var total = new CompareTrader() { StartDate = startDate.Date, EndDate = endDate.Date, Symbol2 = "合计", Trader = trader };
            lst.ForEach(_ => total.Add(_));
            lst.Add(total);
        }

        private static bool Match(TraderDailyDetail t, MarketDetailDaily m)
        {
            if (IsSameStock(t.Sym, m.Symbol2) && IsSameDirection(t.Side, m.Action) && t.Qty == m.ExQuan && t.Exe_Price == m.ExPrice)
            {
                return true;
            }
            return false;
        }

        static Regex NotDigitalChar = new Regex("[^0-9a-zA-Z]");
        private static bool IsSameStock(string a, string b)
        {
            return NotDigitalChar.Replace(a, "") == NotDigitalChar.Replace(b, "");
        }

        public static string GetFormatName(this string a)
        {
            return NotDigitalChar.Replace(a, "");
        }

        private static bool IsSameDirection(string a, string b)
        {
            if (a == b)
            {
                return true;
            }
            if (a.ToUpper().Contains("BOT") && b.ToUpper().Contains("BOT"))
            {
                return true;
            }
            if (a.ToUpper().Contains("SLD") && b.ToUpper().Contains("SLD"))
            {
                return true;
            }
            return false;
        }
    }
}
