using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AASClient;

namespace AASClient.AASServiceReference
{
    public partial class DbDataSet
    {
        public partial class 订单DataTable
        {
            public decimal Get证券仓位(string Zqdm)
            {
                if (this.Any(r => r.证券代码 == Zqdm))
                {
                    return this.Where(r => r.证券代码 == Zqdm).Sum(r => r.已开数量);
                }
                else
                {
                    return 0;
                }
            }


            public decimal Get市值合计()
            {
                if (this.Any())
                {
                    return this.Sum(r => r.已开金额 - r.已平金额);
                }
                else
                {
                    return 0;
                }
            }

            public decimal Get浮动盈亏()
            {
                if (this.Any())
                {
                    return this.Sum(r => r.浮动盈亏);
                }
                else
                {
                    return 0;
                }
            }
        }


        public partial class 订单Row
        {
            //浮动盈亏计算修改为毛利，不减去手续费率，此方法暂时搁置
            public void 刷新浮动盈亏(decimal 手续费率)
            {
                if (this.开仓类别 == 0)
                {
                    decimal 买佣金 = Math.Max(5, Math.Round(this.已开金额 * 手续费率, 2, MidpointRounding.AwayFromZero));
                    decimal 买印花税 = 0;
                    decimal 买过户费 = this.市场代码 == 1 ? Math.Round(this.已开数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;


                    //decimal 卖佣金 = Math.Max(5, Math.Round(this.当前价位 * this.已开数量 * Program.Current平台用户.手续费率, 2, MidpointRounding.AwayFromZero));
                    decimal 卖佣金 = 0;
                    var permition = Program.serverDb.额度分配.FirstOrDefault(_ => _.证券代码 == this.证券代码 && _.交易员 == this.交易员);
                    if (permition != null)
                    {
                        卖佣金 = Math.Max(5, Math.Round(this.当前价位 * this.已开数量 * permition.手续费率, 2, MidpointRounding.AwayFromZero));
                    }
                    //else
                    //{
                    //    卖佣金 = Math.Max(5, Math.Round(this.当前价位 * this.已开数量 * Program.Current平台用户.手续费率, 2, MidpointRounding.AwayFromZero));
                    //}

                    decimal 卖印花税 = Math.Round(this.当前价位 * this.已开数量 * 0.001m, 2, MidpointRounding.AwayFromZero);
                    decimal 卖过户费 = this.市场代码 == 1 ? Math.Round(this.已开数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;


                    decimal 交易费用 = 买佣金 + 买印花税 + 买过户费 + 卖佣金 + 卖印花税 + 卖过户费;



                    this.浮动盈亏 = Math.Round((this.已平金额 + (this.已开数量 - this.已平数量) * this.当前价位) - this.已开金额, 2, MidpointRounding.AwayFromZero) - 交易费用;
                }
                else
                {
                    decimal 卖佣金 = Math.Max(5, Math.Round(this.已开金额 * 手续费率, 2, MidpointRounding.AwayFromZero));
                    decimal 卖印花税 = Math.Round(this.已开金额 * 0.001m, 2, MidpointRounding.AwayFromZero);
                    decimal 卖过户费 = this.市场代码 == 1 ? Math.Round(this.已开数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;




                    decimal 买佣金 = Math.Max(5, Math.Round(this.当前价位 * this.已开数量 * Program.Current平台用户.手续费率, 2, MidpointRounding.AwayFromZero));
                    decimal 买印花税 = 0;
                    decimal 买过户费 = this.市场代码 == 1 ? Math.Round(this.已开数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;

                    decimal 交易费用 = 买佣金 + 买印花税 + 买过户费 + 卖佣金 + 卖印花税 + 卖过户费;


                    this.浮动盈亏 = Math.Round(this.已开金额 - (this.已平金额 + (this.已开数量 - this.已平数量) * this.当前价位), 2, MidpointRounding.AwayFromZero) - 交易费用;
                }
            }

            public void 刷新浮动盈亏()
            {
                if (this.开仓类别 == 0)
                {
                    this.浮动盈亏 = Math.Round((this.已平金额 + (this.已开数量 - this.已平数量) * this.当前价位) - this.已开金额, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    this.浮动盈亏 = Math.Round(this.已开金额 - (this.已平金额 + (this.已开数量 - this.已平数量) * this.当前价位), 2, MidpointRounding.AwayFromZero);
                }
            }
        }



        public partial class 已平仓订单DataTable
        {
            public decimal Get毛利()
            {
                if (this.Any())
                {
                    return this.Sum(r => r.毛利);
                }
                else
                {
                    return 0;
                }
            }



        }
    }



    public partial class JyDataSet
    {
        public partial class 委托DataTable
        {
            public decimal Get交易费用()
            {
                decimal 交易费用 = 0;
                foreach (委托Row 委托Row1 in this)
                {
                    交易费用 += 委托Row1.Get交易费用();
                }
                return 交易费用;
            }

            //public decimal Get交易费用(decimal 手续费率)
            //{
            //    decimal 交易费用 = 0;
            //    foreach (委托Row 委托Row1 in this)
            //    {
            //        交易费用 += 委托Row1.Get交易费用(手续费率);
            //    }
            //    return 交易费用;
            //}

            //public decimal Get交易费用(string Zqdm, decimal 手续费率)
            //{
            //    decimal 交易费用 = 0;
            //    foreach (委托Row 委托Row1 in this.Where(r => r.证券代码 == Zqdm))
            //    {
            //        交易费用 += 委托Row1.Get交易费用(手续费率);
            //    }
            //    return 交易费用;
            //}


            public void Get已买卖股数(string 证券代码, out decimal 已买股数, out decimal 已卖股数)
            {

                已买股数 = 0;
                已卖股数 = 0;

                foreach (委托Row 委托Row1 in this.Where(r => r.证券代码 == 证券代码))
                {
                    if (委托Row1.买卖方向 == 0)
                    {
                        已买股数 += 委托Row1.委托数量 - 委托Row1.撤单数量;
                    }
                    else
                    {
                        已卖股数 += 委托Row1.委托数量 - 委托Row1.撤单数量;
                    }
                }

            }



            public void Get已买卖股数(string 交易员, string 证券代码, out decimal 已买股数, out decimal 已卖股数)
            {

                已买股数 = 0;
                已卖股数 = 0;

                foreach (委托Row 委托Row1 in this.Where(r => r.交易员 == 交易员 && r.证券代码 == 证券代码))
                {
                    if (委托Row1.买卖方向 == 0)
                    {
                        已买股数 += 委托Row1.委托数量 - 委托Row1.撤单数量;
                    }
                    else
                    {
                        已卖股数 += 委托Row1.委托数量 - 委托Row1.撤单数量;
                    }
                }

            }


            public decimal Get已用仓位()
            {

                Dictionary<string, decimal> 证券仓位 = new Dictionary<string, decimal>();

                foreach (委托Row 委托Row1 in this)
                {
                    if (!证券仓位.ContainsKey(委托Row1.证券代码))
                    {
                        证券仓位[委托Row1.证券代码] = 0;
                    }

                    if (委托Row1.买卖方向 == 0)
                    {
                        证券仓位[委托Row1.证券代码] += 委托Row1.委托价格 * (委托Row1.委托数量 - 委托Row1.撤单数量);
                    }
                    else
                    {
                        证券仓位[委托Row1.证券代码] -= 委托Row1.委托价格 * (委托Row1.委托数量 - 委托Row1.撤单数量);
                    }
                }


                decimal 已用仓位 = 0;
                foreach (string ZqdmKey in 证券仓位.Keys)
                {
                    已用仓位 += Math.Abs(证券仓位[ZqdmKey]);
                }
                return 已用仓位;
            }




        }

        public partial class 委托Row
        {
            //public decimal Get交易费用(decimal 手续费率)
            //{

            //    if (this.成交数量 > 0)
            //    {
            //        decimal 成交金额 = this.成交价格 * this.成交数量;

            //        if (this.组合号 == "Ayers")
            //        {

            //            var config = CommonUtils.AyersFeeConfig;
            //            decimal 佣金 = Math.Max(config.Commission * 成交金额, config.CommissionMin);
            //            decimal 印花税 = Math.Max(config.StampTax * 成交金额, 1);
            //            decimal 过户费 = config.TransferFee * 成交金额;
            //            decimal levy = config.TransactionLevy * 成交金额; //交易征费
            //            decimal trading_fee = config.TradingFee * 成交金额;// 交易费

            //            return 佣金 + 印花税 + 过户费 + levy + trading_fee;
            //        }
            //        else
            //        {
            //            decimal 佣金 = Math.Max(5, Math.Round(成交金额 * 手续费率, 2, MidpointRounding.AwayFromZero));
            //            decimal 印花税 = this.买卖方向 == 0 ? 0 : Math.Round(成交金额 * 0.001m, 2, MidpointRounding.AwayFromZero);
            //            decimal 过户费 = this.市场代码 == 1 ? Math.Round(this.成交数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;
            //            return 佣金 + 印花税 + 过户费;
            //        }

            //    }
            //    else
            //    {
            //        return 0;
            //    }
            //}

            public decimal Get交易费用()
            {
                var permition = Program.serverDb.额度分配.FirstOrDefault(_ => _.证券代码 == this.证券代码 && _.交易员 == this.交易员);

                decimal 手续费率 = permition == null ? 0 : permition.手续费率;
                try
                {
                    if (Program.ShareLimitGroups == null)
                    {
                        Program.ShareLimitGroups = Program.AASServiceClient.ShareGroupQuery();
                    }
                    if (Program.ShareLimitGroups != null && Program.ShareLimitGroups.Length > 0)
                    {
                        foreach (var item in Program.ShareLimitGroups)
                        {
                            if (item.GroupTraderList.FirstOrDefault(_ => _.TraderAccount == this.交易员) != null)
                            {
                                var shreLimitItem = item.GroupStockList.FirstOrDefault(_ => _.StockID == this.证券代码 && _.GroupAccount == this.组合号);
                                if (shreLimitItem != null)
                                {
                                    手续费率 = decimal.Parse(shreLimitItem.Commission);
                                    break;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogRunning("自动计算交易费用异常, 异常信息{0}", ex.Message);
                }

                if (this.成交数量 > 0)
                {
                    decimal 成交金额 = this.成交价格 * this.成交数量;

                    decimal 佣金 = Math.Max(5, Math.Round(成交金额 * 手续费率, 2, MidpointRounding.AwayFromZero));
                    decimal 印花税 = this.买卖方向 == 0 ? 0 : Math.Round(成交金额 * 0.001m, 2, MidpointRounding.AwayFromZero);
                    decimal 过户费 = this.市场代码 == 1 ? Math.Round(this.成交数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;
                    return 佣金 + 印花税 + 过户费;

                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
