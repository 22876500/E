using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AASServer
{
    /// <summary>
    /// 额度共享配置器
    /// </summary>
    public class ShareLimitAdapter
    {
        #region Members & Properties
        const string FilePath = "/share-limit-config/share-limit-config.xml";

        static object sync = new object();
        #endregion

        #region Properties
        public XDocument Document { get; private set; }

        private List<ShareLimitGroupItem> _shareLimitGroups;
        public List<ShareLimitGroupItem> ShareLimitGroups
        {
            get
            {
                if (_shareLimitGroups == null)
                {
                    lock (sync)
                    {
                        if (_shareLimitGroups == null)
                        {
                            try
                            {
                                var shareLimitGroups = new List<ShareLimitGroupItem>();
                                var groups = Document.Root.Elements("group");
                                foreach (var groupElem in groups)
                                {
                                    var LimitGroupItem = new ShareLimitGroupItem(groupElem.Attribute("name").Value);
                                    LimitGroupItem.GroupStockList = new List<StockLimitItem>();
                                    LimitGroupItem.GroupTraderList = new List<LimitTrader>();

                                    if (groupElem.HasElements)
                                    {
                                        if (groupElem.Element("traders") != null)
                                        {
                                            var traderIEnumarable = groupElem.Element("traders").Elements("trader");
                                            foreach (var traderItem in traderIEnumarable)
                                                LimitGroupItem.GroupTraderList.Add(new LimitTrader() { TraderAccount = traderItem.Attribute("name").Value });
                                        }

                                        if (groupElem.Element("stocks") != null)
                                        {
                                            var stockIEnumerable = groupElem.Element("stocks").Elements("stock");
                                            foreach (var stockItem in stockIEnumerable)
                                            {
                                                var groupName = stockItem.Element("account").Value.Trim();
                                                var stockLimitInfo = new StockLimitItem()
                                                {
                                                    StockID = stockItem.Element("code").Value.Trim(),
                                                    StockName = stockItem.Element("name").Value.Trim(),
                                                    LimitCount = stockItem.Element("limit").Value.Trim(),
                                                    GroupAccount = stockItem.Element("account").Value.Trim(),
                                                    Commission = stockItem.Element("commission").Value.Trim(),
                                                    BuyType = stockItem.Element("buy").Value.Trim(),
                                                    SaleType = stockItem.Element("sale").Value.Trim(),
                                                };
                                                LimitGroupItem.GroupStockList.Add(stockLimitInfo);

                                                var qs = Program.db.券商帐户.FirstOrDefault(_ => _.名称 == groupName);
                                                if (qs != null)
                                                {
                                                    
                                                    if (qs.类型 == "普通" && (stockLimitInfo.BuyType != "0" || stockLimitInfo.SaleType != "0") )
                                                    {
                                                        stockLimitInfo.BuyType = "0";
                                                        stockLimitInfo.SaleType = "0";
                                                        Program.logger.LogInfo("组合号{0}对应股票{1}额度共享配置项异常，普通帐号买卖方式只能为普通买入卖出, 已将买卖方式重置为普通买入普通卖出！", stockLimitInfo.GroupAccount, stockLimitInfo.StockID);
                                                    }
                                                }


                                            }
                                        }
                                    }

                                    shareLimitGroups.Add(LimitGroupItem);
                                }
                                _shareLimitGroups = shareLimitGroups;
                            }
                            catch (Exception ex)
                            {
                                Program.logger.LogInfo("初始化ShareLimitAdapter.ShareLimitGroups 异常： " + ex.Message);
                            }

                        }
                    }
                }
                return _shareLimitGroups;
            }
        }

        private static ShareLimitAdapter _instance;
        public static ShareLimitAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new ShareLimitAdapter();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region Construct
        private ShareLimitAdapter()
        {
            InitDocument();
        }

        private void InitDocument()
        {
            string CurDir = System.Windows.Forms.Application.StartupPath;
            string dir = CurDir + "/share-limit-config";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (File.Exists(CurDir + FilePath))
            {

                var text = File.ReadAllText(CurDir + FilePath, Encoding.Default);
                Document = XDocument.Parse(text);
            }
            else
            {
                InitWithTemplete();
                Document.Save(CurDir + FilePath);
            }
        }

        private void InitWithTemplete()
        {
            Document = new XDocument(
                 new XElement("groups",
                    new XElement("group", new XAttribute("name", "group1"),
                        new XElement("traders"),
                        new XElement("stocks")
                    )
                )
            );
        }
        #endregion

        #region Add Element
        public bool AddShareGroup(string groupName, out string errMsg)
        {
            errMsg = string.Empty;
            var groupItem = Document.Root.Elements("group").First(_ => _.Attribute("name").Value == groupName);
            if (groupItem == null)
            {
                Document.Root.Add(new XElement("group", new XAttribute("name", groupName)));
                Document.Save(System.Windows.Forms.Application.StartupPath + FilePath);
                return true;
            }
            else
            {
                errMsg = string.Format("已存在分组{0}", groupName);
            }
            return false;
        }

        public bool AddGroupStocks(string groupName, StockLimitItem limit, out string errMsg)
        {
            errMsg = string.Empty;
            var groupElem = Document.Root.Elements("group").First(_ => _.Attribute("name").Value == groupName);
            if (groupElem != null)
            {
                var groupItem = this.ShareLimitGroups.First(_ => _.GroupName == groupName);

                var stockRoot = groupElem.Element("stocks");
                if (stockRoot == null)
                {
                    stockRoot = new XElement("stocks");
                    groupElem.Add(stockRoot);
                }

                var existElem = stockRoot.Elements("stock").FirstOrDefault(_ => _.Element("code").Value == limit.StockID);// && _.Element("account").Value == limit.GroupAccount 后续再加可以加不同组合号的功能。
                if (existElem == null)
                {
                    stockRoot.Add(new XElement("stock",
                            new XElement("code", limit.StockID),
                            new XElement("name", limit.StockName),
                            new XElement("limit", limit.LimitCount.ToString()),
                            new XElement("account", limit.GroupAccount),
                            new XElement("commission", limit.Commission),
                            new XElement("buy", limit.BuyType),
                            new XElement("sale", limit.SaleType)
                            ));
                    groupItem.GroupStockList.Add(limit);
                    Document.Save(System.Windows.Forms.Application.StartupPath + FilePath);
                    return true;
                }
                else
                {
                    errMsg = string.Format("已存在组合号{0} 对应股票{1}的配置:{2}！", groupName, limit.StockID, existElem.ToJson());
                }
                

                
            }
            else
            {
                errMsg = string.Format("未找到分组{0}", groupName);
            }
            return false;
        }

        public bool AddTrader(string id, string groupName, out string errMsg)
        {
            errMsg = string.Empty;
            if (!Program.db.平台用户.ExistsUserRole(id, 角色.交易员))
            {
                errMsg = string.Format("不存在交易员{0}", id);
                return false;
            }

            var groupElem = Document.Root.Elements("group").First(_ => _.Attribute("name").Value == groupName);
            if (groupElem != null)
            {
                var tradersElem = groupElem.Element("traders").Elements("trader");
                if (!tradersElem.Any(_ => _.Attribute("name").Value == id))
                {
                    groupElem.Element("traders").Add(new XElement("trader", new XAttribute("name", id)));
                    Document.Save(System.Windows.Forms.Application.StartupPath + FilePath);

                    var groupItem = this.ShareLimitGroups.First(_ => _.GroupName == groupName);
                    groupItem.GroupTraderList.Add(new LimitTrader() { TraderAccount = id });
                    return true;
                }
                else
                {
                    errMsg = string.Format("{0}中已存在交易员{1}", groupName, id);
                }
            }
            else
            {
                errMsg = string.Format("不存在分组{0}", groupName);
            }
            return false;
        }
        #endregion

        #region Update
        public bool UpdateStockLimit(string traderID, string stockID, decimal limit, out string errMsg)
        {
            errMsg = string.Empty;
            //应先检测该用户归属的组，再更新对应信息
            return false;
        }
        #endregion

        #region Remove
        public bool RemoveTrader(string group, string trader)
        {
            var groupElem = Document.Root.Elements("group").FirstOrDefault(_=>_.Attribute("name").Value == group);
            if (groupElem == null)
            {
                return false;
            }
            var node = groupElem.Element("traders").Elements("trader").Where(_ => _.Attribute("name").Value == trader).FirstOrDefault();
            if (node != null)
            {
                var groupItem = this.ShareLimitGroups.First(_ => _.GroupName == group);
                var traderItem = groupItem.GroupTraderList.First(_ => _.TraderAccount == trader);
                groupItem.GroupTraderList.Remove(traderItem);

                node.Remove();
                Document.Save(System.Windows.Forms.Application.StartupPath + FilePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveStock(string group, string stockID, out string errMsg)
        {
            errMsg = string.Empty;
            var groupElem = Document.Root.Elements("group").FirstOrDefault(_ => _.Attribute("name").Value == group);
            if (groupElem == null)
            {
                errMsg = "不存在组合号" + group;
                return false;
            }
            var node = groupElem.Element("stocks").Elements("stock").Where(_ => _.Element("code").Value == stockID).FirstOrDefault();
            if (node != null)
            {
                var groupItem = this.ShareLimitGroups.First(_ => _.GroupName == group);
                var stockItem = groupItem.GroupStockList.FirstOrDefault(_ => _.StockID == stockID);
                groupItem.GroupStockList.Remove(stockItem);

                node.Remove();
                Document.Save(System.Windows.Forms.Application.StartupPath + FilePath);
                return true;
            }
            errMsg = "不存在股票" + stockID;
            return false;
        }
        #endregion

        public ShareLimitGroupItem GetLimitGroup(string trader)
        {
            foreach (var item in ShareLimitGroups)
            {
                var traderItem = item.GroupTraderList.FirstOrDefault(_ => _.TraderAccount == trader);
                //var stockItem = item.GroupStockList.FirstOrDefault(_=> _.StockID == stockID);
                if (traderItem != null)
                {
                    return item;
                }
            }
            return null;
        }

        public decimal Get交易费用(string JyUserName, decimal 手续费率)
        {
            
            decimal 交易费用 = 0;
            foreach (AASServer.DbDataSet.已发委托Row 已发委托Row1 in Program.db.已发委托.Where(r => r.日期 == DateTime.Today && r.交易员 == JyUserName))
            {
                交易费用 += 已发委托Row1.Get交易费用(手续费率);
            }
            return 交易费用;
        }

        public decimal Get交易费用(AASServer.DbDataSet.已发委托Row wtRow, decimal 手续费率)
        {

            if (wtRow.成交数量 > 0)
            {
                decimal 成交金额 = wtRow.成交价格 * wtRow.成交数量;

                if (wtRow.组合号 == AyersMessageAdapter.GroupName)
                {
                    var config = AyersConfig.GetFeeConfig();
                    decimal 佣金 = Math.Max(config.Commission * 成交金额, config.CommissionMin);
                    decimal 印花税 = Math.Ceiling(config.StampTax * 成交金额);
                    decimal 过户费 = config.TransferFee * 成交金额;
                    decimal levy = config.TransactionLevy * 成交金额; //交易征费
                    decimal trading_fee = config.TradingFee * 成交金额;// 交易费

                    return Math.Round(佣金 + 印花税 + 过户费 + levy + trading_fee, 2);
                }
                else
                {
                    decimal 佣金 = Math.Max(5, Math.Round(成交金额 * 手续费率, 2, MidpointRounding.AwayFromZero));
                    decimal 印花税 = wtRow.买卖方向 == 0 ? 0 : Math.Round(成交金额 * 0.001m, 2, MidpointRounding.AwayFromZero);
                    decimal 过户费 = wtRow.市场代码 == 1 ? Math.Round(wtRow.成交数量 * 0.0006m, 2, MidpointRounding.AwayFromZero) : 0;
                    return 佣金 + 印花税 + 过户费;
                }

            }
            else
            {
                return 0;
            }
        }
    }

    public class StockLimitItem
    {
        public StockLimitItem()
        { }

        public string GroupAccount { get; set; }

        public string StockID { get; set; }

        public string StockName { get; set; }

        public string LimitCount { get; set; }

        public string Commission { get; set; }

        public string SaleType { get; set; }

        public string BuyType { get; set; }

        public int GetTradeType(int tradeType)
        {
            if (tradeType == 0)
            {
                if (int.Parse(this.BuyType) == (int)AASServer.买模式.现金买入)
                {
                    return 0;
                }
                else if (int.Parse(this.BuyType) == (int)AASServer.买模式.融资买入)
                {
                    return 2;
                }
                else if (int.Parse(this.BuyType) == (int)AASServer.买模式.担保品买入)
                {
                    return 7;
                }
                else
                {
                    return 4;//买券还券
                }
            }
            else
            {
                if (int.Parse(this.SaleType) == (int)AASServer.卖模式.现券卖出)
                {
                    return 1;
                }
                else if (int.Parse(this.SaleType) == (int)AASServer.卖模式.融券卖出)
                {
                    return 3;
                }
                else if (int.Parse(this.SaleType) == (int)AASServer.卖模式.担保品卖出)
                {
                    return 8;
                }
                else
                {
                    return 5;//卖券还款
                }
            }
        }
    }

    public class LimitTrader
    {

        public string TraderAccount { get; set; }
    }

    public class ShareLimitGroupItem
    {
        public ShareLimitGroupItem()
        { }

        public ShareLimitGroupItem(string name)
        {
            GroupName = name;
        }

        public string GroupName { get; set; }

        public List<LimitTrader> GroupTraderList { get; set; }

        public List<StockLimitItem> GroupStockList { get; set; }

        public void GetShareGroupHasBuyCount(string 证券代码, string trader, List<string> lstSendedOrderID,out decimal 已买股数, out decimal 已卖股数, out decimal traderBuy, out decimal traderSale)
        {
            已买股数 = 0;
            已卖股数 = 0;
            traderBuy = 0;
            traderSale = 0;
            if (GroupTraderList  != null && GroupTraderList.Count > 0)
            {
                var stockItem = GroupStockList.Where(_ => _.StockID == 证券代码).First();
                var users = GroupTraderList.Select(_=> _.TraderAccount).ToList();

                JyDataSet.委托DataTable interFaceData = null;
                if (Program.帐户委托DataTable.ContainsKey(stockItem.GroupAccount))
                    interFaceData = Program.帐户委托DataTable[stockItem.GroupAccount];
                
                var wtList = Program.db.已发委托.Where(r => r.日期 == DateTime.Today && users.Contains(r.交易员) && r.证券代码 == 证券代码);
                foreach (var 已发委托Row1 in wtList)
                {
                    JyDataSet.委托Row interfaceItem = null;
                    if (interFaceData != null)
                        interfaceItem = interFaceData.FirstOrDefault(_ => _.委托编号 == 已发委托Row1.委托编号);

                    decimal cancelCount = interfaceItem == null ? 已发委托Row1.撤单数量 : interfaceItem.撤单数量;

                    if (已发委托Row1.买卖方向 % 2 == 0)
                    {
                        if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                        {
                            已买股数 += 已发委托Row1.成交数量;
                            if (已发委托Row1.交易员 == trader)
                                traderBuy += 已发委托Row1.成交数量;
                        }
                        else
                        {
                            已买股数 += 已发委托Row1.委托数量 - cancelCount;
                            if (已发委托Row1.交易员 == trader)
                                traderBuy += 已发委托Row1.委托数量 - cancelCount;
                        }
                    }
                    else
                    {
                        if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                        {
                            已卖股数 += 已发委托Row1.成交数量;
                            if (已发委托Row1.交易员 == trader)
                                traderSale += 已发委托Row1.成交数量;
                        }
                        else
                        {
                            已卖股数 += 已发委托Row1.委托数量 - cancelCount;
                            if (已发委托Row1.交易员 == trader)
                                traderSale += 已发委托Row1.委托数量 - cancelCount;
                        }
                    }
                    lstSendedOrderID.Add(已发委托Row1.委托编号);
                }
            }
        }

        public void GetShareGroupHasBuyCount(string 证券代码, out decimal 已买股数, out decimal 已卖股数)
        {
            已买股数 = 0;
            已卖股数 = 0;
            if (GroupTraderList != null && GroupTraderList.Count > 0)
            {
                var users = GroupTraderList.Select(_ => _.TraderAccount).ToList();
                var wtList = Program.db.已发委托.Where(r => r.日期 == DateTime.Today && users.Contains(r.交易员) && r.证券代码 == 证券代码);

                JyDataSet.委托DataTable interFaceData = null;
                if (Program.帐户委托DataTable.ContainsKey(this.GroupName))
                    interFaceData = Program.帐户委托DataTable[GroupName];

                foreach (var 已发委托Row1 in wtList)
                {
                    JyDataSet.委托Row interfaceItem = null;
                    if (interFaceData != null)
                        interfaceItem = interFaceData.FirstOrDefault(_ => _.委托编号 == 已发委托Row1.委托编号);

                    decimal cancelCount = interfaceItem == null ? 已发委托Row1.撤单数量 : interfaceItem.撤单数量;

                    if (已发委托Row1.买卖方向 % 2 == 0)
                    {
                        if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                            已买股数 += 已发委托Row1.成交数量;
                        else
                            已买股数 += 已发委托Row1.委托数量 - cancelCount;
                    }
                    else
                    {
                        if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                            已卖股数 += 已发委托Row1.成交数量;
                        else
                            已卖股数 += 已发委托Row1.委托数量 - cancelCount;
                    }
                }
            }
        }

        public Dictionary<string, TraderBuySaleEntity> GetShareGroupBuySaleList(string StockID)
        {
            var dict = new Dictionary<string, TraderBuySaleEntity>();
            var users = GroupTraderList.Select(_ => _.TraderAccount).ToList();
            var wtList = Program.db.已发委托.Where(r => r.日期 == DateTime.Today && users.Contains(r.交易员) && r.证券代码 == StockID);

            JyDataSet.委托DataTable interFaceData = null;
            if (Program.帐户委托DataTable.ContainsKey(this.GroupName))
                interFaceData = Program.帐户委托DataTable[GroupName];

            foreach (var 已发委托Row1 in wtList)
            {
                JyDataSet.委托Row interfaceItem = null;
                if (interFaceData != null)
                    interFaceData.FirstOrDefault(_ => _.委托编号 == 已发委托Row1.委托编号);

                if (!dict.ContainsKey(已发委托Row1.交易员))
                {
                    dict.Add(已发委托Row1.交易员, new TraderBuySaleEntity() { TraderID = 已发委托Row1.交易员 , StockID = StockID});
                }

                decimal cancelCount = interfaceItem == null ? 已发委托Row1.撤单数量 : interfaceItem.撤单数量;

                if (已发委托Row1.买卖方向 % 2 == 0)
                {
                    if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                        dict[已发委托Row1.交易员].BuyCount += 已发委托Row1.成交数量;
                    else
                        dict[已发委托Row1.交易员].BuyCount += 已发委托Row1.委托数量 - cancelCount;
                }
                else
                {
                    if (已发委托Row1.成交数量 > 0 && 已发委托Row1.成交数量 < 已发委托Row1.委托数量 && 已发委托Row1.撤单数量 == 已发委托Row1.成交数量)
                        dict[已发委托Row1.交易员].SoldCount += 已发委托Row1.成交数量;
                    else
                        dict[已发委托Row1.交易员].SoldCount += 已发委托Row1.委托数量 - cancelCount;
                }
            }
            return dict;
        }

        public class TraderBuySaleEntity
        {
            public string TraderID { get; set; }

            public string StockID { get; set; }

            public decimal BuyCount { get; set; }

            public decimal SoldCount { get; set; }
        }
    }

    
}
