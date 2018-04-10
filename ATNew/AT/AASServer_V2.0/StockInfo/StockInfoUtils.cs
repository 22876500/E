using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASServer.StockInfo
{
    public class StockInfoUtils
    {
        private static ConcurrentDictionary<string, List<string>> DictStockInfo = new ConcurrentDictionary<string, List<string>>();


        public static void Init(string[] codes)
        {
            var codesNotExists = codes.Where(_ => !DictStockInfo.ContainsKey(_)).ToList();
            if (codesNotExists.Count == 0)
                return;

            StringBuilder sbUrl = new StringBuilder(25 + codesNotExists.Count * 9);
            sbUrl.Append("http://hq.sinajs.cn/list=");
            foreach (var item in codesNotExists)
            {
                sbUrl.Append(CommonUtils.GetCodeMarket(item) == 0 ? "sz" : "sh").Append(item).Append(',');
            }
            sbUrl.Remove(sbUrl.Length - 1, 1);

            string text = QueryFromUrl(sbUrl.ToString());
            var results = text.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            DictStockInfo.Clear();

            foreach (var item in results)
            {
                var infos = item.Split('"');
                var stockID = infos[0].Substring(13, 6);
                int market = CommonUtils.GetCodeMarket(stockID);

                if (item.Contains("=\"\""))
                {
                    Program.logger.LogInfoDetail("初始化股票数据异常,股票信息:" + item);
                    market = market == 0 ? 1 : 0;
                    var url = string.Format("http://hq.sinajs.cn/list={0}", (market == 0 ? "sz" : "sh") + stockID);
                    var exceptResult = QueryFromUrl(url);
                    infos = exceptResult.Split('"');
                    Program.logger.LogInfoDetail("股票数据异常修正查询结果：" + exceptResult);
                }

                if (infos.Length > 1)
                {
                    var detailList = infos[1].Split(',').ToList();
                    detailList.Insert(0, market + "");
                    DictStockInfo[stockID] = detailList;
                }
                else
                {
                    Program.logger.LogInfo("股票{0}未查到相关数据!",stockID);
                }
            }
        }

        private static string QueryFromUrl(string url)
        {
            System.Net.HttpWebRequest request = System.Net.WebRequest.CreateHttp(url);
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            Stream s = response.GetResponseStream();
            StreamReader reader = new StreamReader(s, Encoding.Default);
            string text = reader.ReadToEnd();
            return text;
        }

        private static bool GetStockDetail(string stockID, out string[] details)
        {
            details = null;
            var url = string.Format("http://hq.sinajs.cn/list=sh{0},sz{0}", stockID);
            System.Net.HttpWebRequest request = System.Net.WebRequest.CreateHttp(url);

            try
            {
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader reader = new StreamReader(s, Encoding.Default);
                string text = reader.ReadToEnd();

                var results = text.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string market = "";
                if (text.EndsWith("=\"\";\n"))
                {//说明深市数据为空
                    details = results[0].Split('"')[1].Split(',');
                    market = "1";
                }
                else
                {
                    details = results[1].Split('"')[1].Split(',');
                    market = "0";
                }

                var detailList = details.ToList();
                detailList.Insert(0, market);
                DictStockInfo[stockID] = detailList;
                return true;
            }
            catch (Exception)
            {
                return false;

            }
            //var hq_str_sh600228="*ST昌九,11.720,11.830,11.500,11.820,11.480,11.500,11.520,1467713,17089334.000,9000,11.500,15799,11.490,1490,11.480,5500,11.470,2900,11.450,10000,11.520,1900,11.530,2500,11.550,2900,11.570,1700,11.580,2018-03-16,15:00:00,00";
            //var hq_str_sz600228 = "";
            //本文档中将相关市场代码数据存入字典数据中，索引为0的位置，其他数据后移，
            //0：”大秦铁路”，股票名字；
            //1：”27.55″，今日开盘价；
            //2：”27.25″，昨日收盘价；
            //3：”26.91″，当前价格；
            //4：”27.55″，今日最高价；
            //5：”26.20″，今日最低价；
            //6：”26.91″，竞买价，即“买一”报价；
            //7：”26.92″，竞卖价，即“卖一”报价；
            //8：”22114263″，成交的股票数，由于股票交易以一百股为基本单位，所以在使用时，通常把该值除以一百；
            //9：”589824680″，成交金额，单位为“元”，为了一目了然，通常以“万元”为成交金额的单位，所以通常把该值除以一万；
            //10：”4695″，“买一”申请4695股；
            //11：”26.91″，“买一”报价；
            //12：”57590″，“买二”
            //13：”26.90″，“买二”
            //14：”14700″，“买三”
            //15：”26.89″，“买三”
            //16：”14300″，“买四”
            //17：”26.88″，“买四”
            //18：”15100″，“买五”
            //19：”26.87″，“买五”
            //20：”3100″，“卖一”申报3100股，即31手；
            //21：”26.92″，“卖一”报价
            //(22, 23), (24, 25), (26, 27), (28, 29)分别为“卖二”至“卖四的情况”
            //30：”2008 - 01 - 11″，日期；
            //31：”15:05:32″，时间；
        }

        public static bool GetStockName(string stockID, out string stockName)
        {
            stockName = "";
            string[] details = null;

            if (!DictStockInfo.ContainsKey(stockID))
            {
                var success = GetStockDetail(stockID, out details);
                if (success && details != null)
                {
                    stockName = details[0];
                }
                return success;
            }
            else
            {
                stockName = DictStockInfo[stockID][1];
                return true;
            }
            
        }

        internal static decimal GetPriceYesterday(string stockID)
        {
            if (DictStockInfo.ContainsKey(stockID))
            {
                return decimal.Parse(DictStockInfo[stockID][3]);
            }
            else
            {
                string[] detail;

                GetStockDetail(stockID, out detail);
                return decimal.Parse(detail[4]);
            }
        }
    }
}
