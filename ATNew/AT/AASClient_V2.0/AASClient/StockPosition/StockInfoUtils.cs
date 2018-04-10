using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AASClient.StockPosition
{
    public class StockInfoUtils
    {
        public static bool GetStockNameMarket(string stockID,out int market, out string stockName)
        {
            market = -1;
            stockName = "";
            string[] details = null;

            var success = GetStocksDetail(stockID, out market, out details);
            if (success && details != null)
            {
                stockName = details[0];
            }
            return success;
        }


        private static bool GetStocksDetail(string stockID, out int market, out string[] details)
        {
            market = -1;
            details = null;
            
            var url = string.Format("http://hq.sinajs.cn/list=sh{0},sz{0}", stockID);
            System.Net.HttpWebRequest request = System.Net.WebRequest.CreateHttp(url);

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader reader = new StreamReader(s, Encoding.Default);
                string text = reader.ReadToEnd();

                var results = text.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (text.EndsWith("=\"\";\n"))
                {//说明深市数据为空
                    details = results[0].Split('"')[1].Split(',');
                    market = 1;
                }
                else
                {
                    details = results[1].Split('"')[1].Split(',');
                    market = 0;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            //var hq_str_sh600228="*ST昌九,11.720,11.830,11.500,11.820,11.480,11.500,11.520,1467713,17089334.000,9000,11.500,15799,11.490,1490,11.480,5500,11.470,2900,11.450,10000,11.520,1900,11.530,2500,11.550,2900,11.570,1700,11.580,2018-03-16,15:00:00,00";
            //var hq_str_sz600228 = "";
            //0：”*ST昌九”，股票名字；
            //1：”11.720″，今日开盘价；
            //2：”11.830″，昨日收盘价；
            //3：”11.500″，当前价格；
            //4：”11.820″，今日最高价；
            //5：”11.480″，今日最低价；
            //6：”11.500″，竞买价，即“买一”报价；
            //7：”11.520″，竞卖价，即“卖一”报价；
            //8：”1467713″，成交的股票数，由于股票交易以一百股为基本单位，所以在使用时，通常把该值除以一百；
            //9：”17089334″，成交金额，单位为“元”，为了一目了然，通常以“万元”为成交金额的单位，所以通常把该值除以一万；
            //10：”9000″，“买一”申请4695股；
            //11：”11.500″，“买一”报价；
            //12：”15799″，“买二”
            //13：”11.490″，“买二”
            //14：”1490″，“买三”
            //15：”11.480″，“买三”
            //16：”5500″，“买四”
            //17：”11.470″，“买四”
            //18：”2900″，“买五”
            //19：”11.450″，“买五”
            //20：”10000″，“卖一”申报3100股，即31手；
            //21：”11.520″，“卖一”报价
            //(22, 23), (24, 25), (26, 27), (28, 29)分别为“卖二”至“卖四的情况”
            //30：”2018-03-16″，日期；
            //31：”15:00:00 00″，时间；
        }
    }
}
