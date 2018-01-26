using OKCoilClientWF.OKCoin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OKCoilClientWF.WebSocketApi
{
    class BuissnesServiceImpl : WebSocketService
    {
        const string thisWeekHeader = "[{\"binary\":0,\"channel\":\"ok_sub_futureusd_btc_ticker_this_week\",";
        const string nextWeekHeader = "[{\"binary\":0,\"channel\":\"ok_sub_futureusd_btc_ticker_next_week\",";
        const string quarterHeader = "[{\"binary\":0,\"channel\":\"ok_sub_futureusd_btc_ticker_quarter\",";


        Regex buyPriceReg = new Regex("buy\":\"([.0-9]+)\"");
        Regex sellPriceReg = new Regex("sell\":\"([.0-9]+)\"");
        //Regex buySellReg = new Regex("buy\":\"([.0-9]+)\",.*sell\":\"([.0-9]+)\"");

        public BuissnesServiceImpl()
        {

        }

        public void onReceive(string msg)
        {
            if (!msg.Contains("{\"event\":\"pong\"}"))
            {
                if (msg.StartsWith(thisWeekHeader))
                {
                    string buyStr = buyPriceReg.Match(msg).Groups[1].Value;
                    string sellStr = sellPriceReg.Match(msg).Groups[1].Value;
                    OKCoilAdapter.Instance.UpdateThisWeek(double.Parse(buyStr), double.Parse(sellStr));
                }
                else if (msg.StartsWith(quarterHeader))
                {
                    string buyStr = Regex.Match(msg, "buy\":\"([.0-9]+)\"").Groups[1].Value;
                    string sellStr = Regex.Match(msg, "sell\":\"([.0-9]+)\"").Groups[1].Value;
                    OKCoilAdapter.Instance.UpdateQuarter(double.Parse(buyStr), double.Parse(sellStr));
                }
                else if (msg.StartsWith(nextWeekHeader))
                {
                    string buyStr = Regex.Match(msg, "buy\":\"([.0-9]+)\"").Groups[1].Value;
                    string sellStr = Regex.Match(msg, "sell\":\"([.0-9]+)\"").Groups[1].Value;
                    OKCoilAdapter.Instance.UpdateNextWeek(double.Parse(buyStr), double.Parse(sellStr));
                }
                else
                {
                    Program.Log.LogInfo(msg);
                }
                //UpdateByDeserialize(msg);
            }
        }

        //private static void UpdateByDeserialize(string msg)
        //{
        //    try
        //    {
        //        var entities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OKCoinResponse>>(msg);
        //        if (entities != null && entities.Count > 0)
        //        {
        //            foreach (var entity in entities)
        //            {
        //                //entity.Time = DateTime.Now;
        //                if (entity.channel == ConfigInfo.BtcThisWeekChannel)
        //                {
        //                    // 订阅合约行情 当月
        //                    OKCoilAdapter.Instance.UpdateThisWeek(entity);
        //                    //onWeekDataReceive(entity);
        //                }
        //                else if (entity.channel == ConfigInfo.BtcQuarterChannel)
        //                {
        //                    //订阅合约行情 当季度
        //                    OKCoilAdapter.Instance.UpdateQuarter(entity);
        //                    //onQuarterDataReceive(entity);
        //                }
        //                else
        //                {
        //                    Program.Log.LogInfoFormat("BuissnesServiceImpl 收到未处理数据{0}", msg);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //Program.Log.LogInfo(msg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Program.Log.LogWarn(string.Format("BuissnesServiceImpl 解析异常：{0}", ex.Message));
        //    }
        //}
    }

    public enum WSChannel { ThisWeekChannel = 0, NextWeekChannel = 1, QuarterChanne = 2 }

    public enum WSChannelChangeType { Add = 0, Remove = 1 }
}
