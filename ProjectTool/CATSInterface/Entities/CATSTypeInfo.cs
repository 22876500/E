using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CATSTypeInfo
    {
        //将server中交易类型与CATS系统中交易类型做转换，担保品买入卖出存疑
        public static string GetTradeSide(string side)
        {
            switch (side)
            {
                case "0"://买入
                    return "1";
                case "1"://卖出
                    return "2";
                case "2"://融资买入
                    return "A";
                case "3"://融券卖出
                    return "B";
                case "4"://买券还券
                    return "C";
                case "5"://卖券还款
                    return "D";
                case "7"://担保品买入
                    return "E";
                case "8"://担保品卖出
                    return "2";
                case "6"://现券还券
                default:
                    break;

            }
            return null;
            //1	买入
            //2	卖出
            //A	融资买入
            //B	融券卖出
            //C	买券还券
            //D	卖券还款
            //E	先买券还券，再担保品买入
            //F	ETF申购
            //G	ETF赎回
            //FA	开仓买入
            //FB	开仓卖出
            //FC	平仓买入
            //FD	平仓卖出
            //FG	平今买入
            //FH	平今卖出
            //FI	平昨买入
            //FJ	平昨卖出
        }

        public static int IsTDXBuySide(string side)
        {
            switch (side)
            {
                case "0"://买入
                case "2"://融资买入
                case "4"://买券还券
                case "7"://担保品买入
                    return 0;
                case "1"://卖出
                case "3"://融券卖出
                case "5"://卖券还款
                case "8"://担保品卖出
                    return 1;
                case "6"://现券还券
                default:
                    return 2;
            }
        }

        public static int IsCATSBuySide(string tradeSide)
        {
            switch (tradeSide.Trim())
            {
                case "1"://买入
                case "A"://融资买入
                case "C"://买券还券
                case "E"://担保品买入
                    return 0;
                case "2"://卖出
                case "B"://融券卖出
                case "D"://卖券还款
                    return 1;
                default:
                    return 2;
            }
        }

        public static string GetTradeSideDes(string tradeSide)
        {
            switch (tradeSide.Trim())
            {
                case "1"://买入
                    return "买入";
                case "2"://卖出
                    return "卖出";
                case "A"://融资买入
                    return "融资买入";
                case "B"://融券卖出
                    return "融券卖出";
                case "C"://买券还券
                    return "买券还券";
                case "D"://卖券还款
                    return "卖券还款";
                case "E"://担保品买入
                    return "担保品买入";
                default:
                    return tradeSide.Trim();
            }
        }

        public static string GetAcctTypeDescription(string typeInfo)
        {
            switch (typeInfo)
            {
                case "0":
                    return "股票集中交易";
                case "S0":
                    return "股票模拟";
                case "F0":
                    return "股票深圳快速交易";
                case "SHF0":
                    return "股票上海快速交易";
                case "C":
                    return "信用集中交易";
                case "FC":
                    return "信用深圳快速交易";
                case "SHFC":
                    return "信用上海快速交易";
                case "A":
                    return "中信期货";
                case "SA":
                    return "期货模拟";
                default:
                    return null;
            }
        }

        public static string GetOrderTypeDescription(int market, string typeInfo)
        {
            if (typeInfo == "0")
            {
                return "限价单";
            }

            if (market == 0)//深市
            {
                switch (typeInfo)
                {
                    case "U":
                        return "市价单（最优五档即时成交剩余撤销）";
                    case "S":
                        return "市价单（本方最优价格）";
                    case "T":
                        return "市价单（即时成交剩余撤销）";
                    case "Q":
                        return "市价单（对手方最优价格）";
                    case "V":
                        return "市价单（全额成交或撤单）";
                    default:
                        return null;
                }
            }
            else
            {
                switch (typeInfo)
                {
                    case "U":
                        return "市价单（最优五档即时成交剩余撤销）";
                    case "R":
                        return "市价单（最优五档即时成交剩余转限价）";
                    default:
                        return null;
                }
            }

        }

        public static string GetOrderStatusDescription(string status)
        {
            switch (status)
            {
                case "0":
                    return "已报";
                case "1":
                    return "部分成交";
                case "2":
                    return "全部成交";
                case "3":
                    return "部分撤单";
                case "4":
                    return "全部撤单";
                case "5":
                    return "交易所拒单";
                case "6":
                    return "柜台未接受";
                default:
                    return status;
            }
        }
    }
}
