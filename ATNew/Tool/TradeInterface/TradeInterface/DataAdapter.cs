using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeInterface
{
    static class DataAdapter
    {
        /// <summary>
        /// 通达信接口-交易数据查询
        /// </summary>
        /// <param name="o">券商</param>
        /// <param name="tradeDataType">表示查询信息的种类，0资金  1股份   2当日委托  3当日成交     4可撤单   5股东代码  6融资余额   7融券余额  8可融证券</param>
        /// <returns></returns>
        public static DataTable QueryTradeData(券商 o, int tradeDataType, bool isCheckPort)
        {
            DataTable dt = null;
            StringBuilder ErrInfo = new StringBuilder(256);
            StringBuilder result = new StringBuilder(1024 * 1024);
            if (isCheckPort && o.营业部代码 == 8888)
            {
                o.营业部代码 = 24;
                o.IP = "124.74.242.150";
                o.Port = 443;
            }
            //var ClientID = TdxApi.LogonEx(o.IP, o.Port, o.版本号, o.营业部代码, o.登录帐号, o.交易帐号, o.TradePsw, o.CommunicatePsw, "39.104.93.151", "00-16-3E-00-1C-12", ErrInfo);
            var ClientID = TdxApi.Logon(o.IP, o.Port, o.版本号, o.营业部代码, o.登录帐号, o.交易帐号, o.TradePsw, o.CommunicatePsw, ErrInfo);
            if (ErrInfo.Length > 0)
            {
                CommonUtils.ShowMsg(ErrInfo.ToString());
            }
            else
            {
                TdxApi.QueryData(ClientID, tradeDataType, result, ErrInfo);
                if (ErrInfo.Length == 0)
                {
                    dt = CommonUtils.ChangeDataStringToTable(result.ToString());
                }
                else
                {
                    CommonUtils.ShowMsg(ErrInfo.ToString());
                }
                TdxApi.Logoff(ClientID);
            }
            return dt;
        }
    }
}
