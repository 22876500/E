using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataComparision.Entity
{
    public class 交割单
    {
        //public string 券商名称 { get; set; }

        public string 组合号 { get; set; }

        public DateTime 交割日期 { get; set; }

        public string 证券代码 { get; set; }

        public string 证券名称 { get; set; }

        public string 买卖标志 { get; set; }

        public decimal 成交数量 { get; set; }

        public decimal 成交价格 { get; set; }

        public decimal 成交金额 { get; set; }

        public string 成交编号 { get; set; }

        public decimal 发生金额 { get; set; }

        public decimal 手续费 { get; set; }

        public decimal 印花税 { get; set; }

        public decimal 过户费 { get; set; }

        public decimal 其他费 { get; set; }

        public string 备注 { get; set; }

        [Key, StringLength(36)]
        public string OrderID { get; set; }

        public int SortSequence { get; set; }

        public int? 序号
        {
            get
            {
                return SortSequence > -1 ? (int?)SortSequence : null;
            }
        }
    
    }

    public class 软件委托
    {
        [Key, Column(Order = 0)]
        public DateTime 成交日期 { get; set; }

        public string 交易员 { get; set; }

        [Key, Column(Order = 1)]
        public string 组合号 { get; set; }

        [Key, Column(Order = 2)]
        public string 证券代码 { get; set; }

        public string 证券名称 { get; set; }

        public string 买卖标志 { get; set; }

        public string 委托时间 { get; set; }

        [Key, Column(Order = 3)]
        public string 委托编号 { get; set; }

        public decimal 委托价格 { get; set; }

        public decimal 委托数量 { get; set; }

        public decimal 成交价格 { get; set; }

        public decimal 成交数量 { get; set; }

        public decimal 撤单数量 { get; set; }

        public string 状态说明 { get; set; }

        public string 券商名称 { get; set; }

        public int SortSequence { get; set; }

        public int? 序号
        {
            get
            {
                return SortSequence > -1 ? (int?)SortSequence : null;
            }
        }
    }

    public class 合计表 //: IStringProperty<合计表>
    {
        [Key, Column(Order = 0)]
        public DateTime 日期 { get; set; }

        [Key, Column(Order = 1)]
        public string 账户 { get; set; }

        public decimal 成交金额 { get; set; }

        public decimal 发生金额 { get; set; }

        public string 剔除备注 { get; set; }

        //public string this[string property]
        //{
        //    get
        //    {
        //        switch (property)
        //        {
        //            case "日期":
        //                return this.日期.ToString();
        //            case "账户":
        //                return this.账户.ToString();
        //            case "成交金额":
        //                return this.成交金额.ToString();
        //            case "发生金额":
        //                return this.发生金额.ToString();
        //            case "剔除备注":
        //                return this.剔除备注.ToString();
        //            default:
        //                return null;
        //        }
        //    }
        //}
    }

    public class 券商
    {
        [Key]
        public string 名称 { get; set; }

        public bool 启用 { get; set; }

        public string 交易服务器 { get; set; }

        public string IP { get; set; }

        public short Port { get; set; }

        public string 版本号 { get; set; }

        public short 营业部代码 { get; set; }

        public string 登录帐号 { get; set; }

        public string 交易帐号 { get; set; }

        public string 交易密码 { get; set; }

        public string TradePsw 
        { 
            get { return Cryptor.MD5Decrypt(交易密码); }
            //set { 交易密码 = Cryptor.MD5Encrypt(value); }
        }

        public string 通讯密码 { get; set; }

        public string CommunicatePsw 
        { 
            get { return Cryptor.MD5Decrypt(通讯密码); }
            //set { 通讯密码 = Cryptor.MD5Encrypt(value); }
        }

    }

    public static class EntityCompareUtil
    {

        public static void CompareData(List<交割单> lstDeli, List<软件委托> lstSoft, List<交割单> delMatched,  List<软件委托> softMatched,  List<交割单> delNotMatched,  List<软件委托> softNotMatched)
        {
            
            List<交割单> specalList = new List<交割单>();
            var dictConnector = new Dictionary<交割单, 软件委托>();
            foreach (var item in lstDeli)
            {
                if (item.备注 == "强制保留" || item.备注 == "手动添加")
                {
                    specalList.Add(item);
                    continue;
                }
                var matchSoftList = lstSoft.Where(_ => _.成交价格 != 0 && _.成交数量 != 0 && IsMatch(item, _)).ToList();
                if (matchSoftList.Count > 0)
                {
                    if (!string.IsNullOrEmpty(item.备注) && item.备注.IndexOf("疑似隔夜仓") > 0)
                    {
                        dictConnector.Add(item, null);
                        continue;
                    }
                    else
                    {
                        var matchtedSoftFilted = matchSoftList.Except(softMatched).FirstOrDefault();
                        if (matchtedSoftFilted != null)
                        {
                            softMatched.Add(matchtedSoftFilted);
                            dictConnector.Add(item, matchtedSoftFilted);
                        }
                        else
                        {
                            delNotMatched.Add(item);
                            //item.备注 += " 疑似隔夜仓 ";
                            //matchtedSoftFilted = matchSoftList.First();
                        }
                    }
                }
                else
                {
                    delNotMatched.Add(item);
                }
            }
            var c = dictConnector.OrderBy(_ => _.Key.SortSequence);
            //softMatched.AddRange(c.Select(_ => _.Value).ToList());

            delMatched.AddRange(c.Select(_ => _.Key).ToList());
            delMatched.AddRange(specalList);

            if (delMatched.Count > 0)
            {
                var totalItem = GetTotal(delMatched);
                delMatched.Add(totalItem);
                if (totalItem.组合号 == "C02")
                {
                    CommonUtils.Log("组合号{0}, 交割单总数{1}, 成交金额合计{2}", totalItem.组合号, (delMatched.Count -1).ToString(), totalItem.成交金额.ToString() );
                }
            }

            softNotMatched.AddRange(lstSoft.Except(softMatched).OrderBy(_ => _.SortSequence));
        }

        public static List<合计表> CompareData(List<交割单> lstDeli, List<软件委托> lstSoft)
        {
            var dictDeliDateGroup = new Dictionary<DateTime, List<string>>();

            foreach (var item in lstDeli)
            {
                if (!dictDeliDateGroup.ContainsKey(item.交割日期))
                    dictDeliDateGroup.Add(item.交割日期, new List<string>() { item.组合号 });
                else if (!dictDeliDateGroup[item.交割日期].Contains(item.组合号))
                    dictDeliDateGroup[item.交割日期].Add(item.组合号);
            }

            var totalList = new List<合计表>();

            foreach (var item in dictDeliDateGroup)
            {
                foreach (var group in item.Value)
                {
                    var Date_Group_DeliList = lstDeli.Where(_ => _.组合号 == group && _.交割日期 == item.Key).OrderBy(_=> _.成交编号).ToList();
                    var Date_Group_SoftList = lstSoft.Where(_ => _.组合号 == group && _.成交日期 == item.Key).ToList();
                    if (Date_Group_DeliList != null && Date_Group_DeliList.Count > 0 && Date_Group_SoftList != null)
                    {
                        var totalItem = GetDateGroupTotal(Date_Group_DeliList, Date_Group_SoftList);
                        //if (group == "C02")
                        //{
                        //    CommonUtils.Log("交割单项总计{0}条，软件委托项总计{1}条，组合号{2},成交金额合计{3}, ", 
                        //        Date_Group_DeliList.Count.ToString(), Date_Group_SoftList.Count.ToString(), group, (totalItem == null ? "" : totalItem.成交金额.ToString()));
                        //}
                        
                        if (totalItem != null)
                            totalList.Add(totalItem);
                    }
                }
            }

            return totalList;
        }

        private static 合计表 GetDateGroupTotal( List<交割单> deliList, List<软件委托> softList)
        {
            var lstMatchedSoft = new List<软件委托>();
            var lstMatchedDeli = new List<交割单>();
            var lstNotMatched = new List<交割单>();
            
            var specalList = new List<交割单>();
            foreach (var Item in deliList)
            {
                if (Item.备注 == "强制保留" || Item.备注 == "手动添加")
                {
                    specalList.Add(Item);
                    continue;
                }

                

                var matchSoftList = softList.Where(_ => _.成交价格 != 0 && _.成交数量 != 0 && IsMatch(Item, _)).ToList();
                if (matchSoftList.Count > 0)
                {

                    if (!string.IsNullOrEmpty(Item.备注) && Item.备注.IndexOf("疑似隔夜仓") > 0)
                    {
                        //CommonUtils.LogInfo("交割单对应项被过滤:" + Item.ToJson());
                        continue;
                    }
                    else
                    {
                        var matchtedSoftFilted = matchSoftList.Except(lstMatchedSoft).FirstOrDefault();
                        //if (Item.组合号 == "C02" && Item.成交编号 == "16267")
                        //{
                        //    if (matchtedSoftFilted == null)
                        //    {
                        //        CommonUtils.LogInfo("交割单16267对应软件委托项为空");
                        //    }
                        //    else
                        //    {
                        //        CommonUtils.LogInfo("交割单16267对应软件委托项为" + matchtedSoftFilted.ToJson());
                        //    }
                        //}
                        if (matchtedSoftFilted != null)
                        {
                            
                            lstMatchedSoft.Add(matchtedSoftFilted);
                            lstMatchedDeli.Add(Item);
                        }
                        else
                        {
                            lstNotMatched.Add(Item);
                        }
                    }
                }
                else if (string.IsNullOrWhiteSpace(Item.买卖标志) || (!Item.买卖标志.Contains("融资借入") && !Regex.IsMatch(Item.买卖标志, "偿还.*(?:本金)")))
                {
                    lstNotMatched.Add(Item);//过滤掉融资借入，偿还本金信息
                }
            }
            lstMatchedDeli.AddRange(specalList);
            //if (lstMatchedDeli.First().组合号 == "C02")
            //{
            //    CommonUtils.Log("匹配到的交割单项总计{0}条数据，成交金额合计为{1}", lstMatchedDeli.Count, lstMatchedDeli.Sum(_=> _.成交金额));
            //    var exceptSoftItems = softList.Where(_ => !lstMatchedSoft.Contains(_) && _.成交数量 > 0);
            //    if (exceptSoftItems != null)
            //    {
            //        //var matchSoftList = deliList.Where(_ => _.成交价格 != 0 && _.成交数量 != 0 && IsMatch(_, exceptSoftItems)).ToList();
            //        CommonUtils.LogInfo("未匹配到的软件委托:" + exceptSoftItems.ToJson());
            //    }
            //    else
            //    {
            //        CommonUtils.LogInfo("未找到没匹配的成交价格数量不为0的委托！");
            //    }
            //}
            
            if (lstMatchedDeli.Count > 0)
            {
                var totalItem = GetTotal(lstMatchedDeli, lstNotMatched);
                return totalItem;
            }
            
            return null;
        }

        private static 交割单 GetTotal(List<交割单> deliResult, string totalRowHeader = "合计")
        {
            
            var first = deliResult.First();
            var deliTotal = new 交割单()
            {
                SortSequence = -1,
                交割日期 = first.交割日期,
                组合号 = first.组合号,
                备注 = totalRowHeader,
            };

            foreach (var item in deliResult)
            {
                deliTotal.成交金额 += item.成交金额;
                deliTotal.发生金额 += item.发生金额;
                deliTotal.过户费 += item.过户费;
                deliTotal.印花税 += item.印花税;
                deliTotal.手续费 += item.手续费;
                deliTotal.其他费 += item.其他费;
            }

            return deliTotal;
        }

        private static 合计表 GetTotal(List<交割单> deliMatch, List<交割单> deliNotMatch)
        {
            StringBuilder sb = new StringBuilder();
            if (deliNotMatch != null && deliNotMatch.Count > 0)
            {
                deliNotMatch = deliNotMatch.OrderBy(_ => _.序号).ToList();
                sb.Append("序号\t证券代码\t证券名称\t买卖标志\t成交价格\t成交数量\t成交金额\t发生金额\t备注");
                foreach (var item in deliNotMatch)
                {
                    sb.AppendFormat("\r\n{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
                        item.序号, item.证券代码, item.证券名称, item.买卖标志, item.成交价格, item.成交数量, item.成交金额, item.发生金额, item.备注);
                }
            }


            var first = deliMatch.First();
            var deliTotal = new 合计表()
            {
                账户 = first.组合号,
                日期 = first.交割日期,
                剔除备注 = sb.ToString(),
            };

            foreach (var item in deliMatch)
            {
                deliTotal.成交金额 += item.成交金额;
                deliTotal.发生金额 += item.发生金额;
            }
            return deliTotal;
        }

        public static bool IsMatch(交割单 itemDel, 软件委托 itemSoft)
        {
            if ((itemDel.证券代码 == itemSoft.证券代码 || itemDel.证券名称 == itemSoft.证券名称)
                && IsSameDirect(itemDel.买卖标志, itemSoft.买卖标志)
                && Math.Abs(itemDel.成交数量) == Math.Abs(itemSoft.成交数量)
                && Math.Abs(itemDel.成交价格 - itemSoft.成交价格) <= (decimal)0.001)
                
            {
                return true;
            }
            return false;
        }

        private static bool IsSameDirect(string s1, string s2)
        {
            if (s1 == null || s2 == null)
            {
                return s1 == s2;
            }
            else if (s1.Contains("买") && s2.Contains("买"))
            {
                return true;
            }
            if (s1.Contains("卖") && s2.Contains("卖"))
            {
                return true;
            }
            return false;
        }

        public static bool IsSame(this 交割单 item1, 交割单 item2)
        {
            return (item1.组合号 == item2.组合号
                && item1.交割日期.Date == item2.交割日期.Date
                && item1.证券代码 == item2.证券代码
                && item1.买卖标志 == item2.买卖标志
                && item1.SortSequence == item2.SortSequence);
        }

        public static bool IsSame(this 软件委托 item1, 软件委托 item2)
        {
            return (item1.组合号 == item2.组合号
                    && item1.成交日期.Date == item2.成交日期.Date
                    && item1.证券代码 == item2.证券代码
                    && item1.交易员 == item2.交易员
                    && item1.委托编号 == item2.委托编号
                    && item1.SortSequence == item2.SortSequence);
        }
    }
}
