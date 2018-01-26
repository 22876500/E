

using System;
using System.Text.RegularExpressions;
namespace AASServer
{


    public partial class JyDataSet
    {
        partial class 成交DataTable
        {
        }
    
        partial class 委托DataTable
        {
            public void Deal()
            {
                foreach (JyDataSet.委托Row 委托Row1 in this)
                {
                    委托Row1.Deal();
                }
            }
        }


        partial class 委托Row
        {
            public void Deal()
            {
                AASServer.DbDataSet.已发委托Row 已发委托Row1 = Program.db.已发委托.Get已发委托(DateTime.Today, this.组合号, this.委托编号);

                if (已发委托Row1 == null)
                {
                    return;
                }


                if (this.状态说明 == "废单")
                {
                    bool needUpdate = false;
                    
                    if (已发委托Row1.状态说明 != "废单")
                    {
                        Program.db.已发委托.Update(已发委托Row1.日期, 已发委托Row1.组合号, 已发委托Row1.委托编号, "废单");
                        needUpdate = true;
                    }
                    if (needUpdate)
                    {
                        Program.废单通知.Enqueue(this);
                    }
                }
                
                if (this.状态说明.Contains("未通知废单"))
                {
                    if (Regex.IsMatch(this.状态说明, "Please Input Valid Value For Quantity [(]Lot Size [0-9]+"))
                    {
                        this.状态说明 = string.Format("废单，请按一手股数{0}的整数倍交易!", Regex.Match(this.状态说明, "[0-9]+").Value);
                    }
                    else
                    {
                        this.状态说明 = this.状态说明.Replace("未通知废单", "废单");
                    }
                    Program.db.已发委托.Update(已发委托Row1.日期, 已发委托Row1.组合号, 已发委托Row1.委托编号, this.状态说明);
                    Program.废单通知.Enqueue(this);
                }


                if (this.撤单数量 > 已发委托Row1.撤单数量)
                {
                    Program.db.已发委托.Update(已发委托Row1.日期, 已发委托Row1.组合号, 已发委托Row1.委托编号, this.撤单数量);
                }




                if (this.成交数量 > 已发委托Row1.成交数量)
                {
                    decimal 未处理成交数量 = this.成交数量 - 已发委托Row1.成交数量;
                    if (this.成交价格 == 0)
                    {
                        this.成交价格 = this.委托价格;
                    }
                    
                    decimal 未处理成交金额 = this.成交价格 * this.成交数量 - 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                    
                    decimal 未处理成交价格 = Math.Round(未处理成交金额 / 未处理成交数量, 3, MidpointRounding.AwayFromZero);
                    

                    AASServer.JyDataSet.成交DataTable 成交DataTable1 = new JyDataSet.成交DataTable();
                    JyDataSet.成交Row 成交Row1 = 成交DataTable1.New成交Row();
                    成交Row1.交易员 = this.交易员;
                    成交Row1.组合号 = this.组合号;
                    成交Row1.成交时间 = DateTime.Now.ToString("HH:mm:ss");
                    成交Row1.证券代码 = this.证券代码;
                    成交Row1.证券名称 = this.证券名称;
                    成交Row1.成交价格 = 未处理成交价格;
                    成交Row1.成交数量 = 未处理成交数量;
                    成交Row1.成交金额 = 未处理成交金额;
                    成交Row1.成交编号 = this.委托编号;
                    成交Row1.委托编号 = this.委托编号;
                    成交Row1.买卖方向 = this.买卖方向;
                    成交Row1.市场代码 = this.市场代码;


                    if (!Program.db.订单.Exists(成交Row1))
                    {
                        Program.db.订单.Add(成交Row1);
                    }
                    else
                    {
                        Program.db.订单.Update(成交Row1);
                    }



                    Program.db.已发委托.Update(DateTime.Today, this.组合号, this.委托编号, this.成交价格, this.成交数量);



                    Program.db.交易日志.Add(成交Row1);




                    Program.成交通知.Enqueue(成交Row1);


                }
            }


            public decimal 计算撤单数量()
            {
                if (this.状态说明.IndexOf("废单") != -1 ||
                    this.状态说明.IndexOf("撤") != -1 ||
                    this.状态说明.IndexOf("申报无效") != -1)
                {
                    return this.委托数量 - this.成交数量;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
