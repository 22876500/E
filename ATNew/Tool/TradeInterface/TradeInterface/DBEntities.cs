using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeInterface
{
    public class 券商
    {
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
        }

        public string 通讯密码 { get; set; }

        public string CommunicatePsw
        {
            get { return Cryptor.MD5Decrypt(通讯密码); }
        }

    }

    public class StockItemEntity
    {
        /// <summary>
        /// 证券代码
        /// </summary>
        public string StockID { get; set; }

        public string StockName { get; set; }

        /// <summary>
        /// 现价
        /// </summary>
        public string PriceNow { get; set; }

        /// <summary>
        /// 区间换手率
        /// </summary>
        public string TurnoverRate { get; set; }

        /// <summary>
        /// 区间振幅
        /// </summary>
        public string FluctuationRate { get; set; }

        /// <summary>
        /// 区间成交量
        /// </summary>
        public string FillQty { get; set; }

    }

    public class LimitItemEntity
    {
        public string TraderAccount { get; set; }

        public string TraderName { get; set; }

        public string Group { get; set; }

        public string StockID { get; set; }

        public string TotalQty { get; set; }

    }

    public class TraderStockInfo
    {
        private LimitItemEntity LimitInfo;

        private StockItemEntity StockInfo;

        public TraderStockInfo(LimitItemEntity limit, StockItemEntity stock)
        {
            if (limit.StockID == stock.StockID)
            {
                LimitInfo = limit;
                StockInfo = stock;
            }
            else
            {
                throw new Exception("证券代码不匹配，请联系管理员！");
            }
        }

        public string Trader { get { return LimitInfo.TraderAccount ?? "-"; } }

        /// <summary>
        /// 证券代码
        /// </summary>
        public string StockID { get { return StockInfo.StockID; } }

        public string StockName { get { return StockInfo.StockName; } }

        /// <summary>
        /// 现价
        /// </summary>
        public string PriceNow { get { return StockInfo.PriceNow; } }

        /// <summary>
        /// 区间换手率
        /// </summary>
        public string TurnoverRate { get { return StockInfo.TurnoverRate; } }

        private decimal DecTurnover { get { return string.IsNullOrEmpty(FluctuationRate) ? 0 : decimal.Parse(TurnoverRate); } }

        /// <summary>
        /// 区间振幅
        /// </summary>
        public string FluctuationRate { get { return StockInfo.FluctuationRate; } }
        private decimal DecFluctuation { get { return string.IsNullOrEmpty(FluctuationRate) ? 0 : decimal.Parse(FluctuationRate); } }

        /// <summary>
        /// 区间成交量
        /// </summary>
        public string FillQty { get { return StockInfo.FillQty; } }
        private decimal DecFill { get { return string.IsNullOrEmpty(FillQty) ? 0 : decimal.Parse(FillQty); } }

        /// <summary>
        /// 换手率 * 振幅 * 成交量
        /// </summary>
        public decimal Score
        {
            get
            {
                return Math.Round(DecTurnover * DecFluctuation * Math.Min(DecFill, MarketValueCaculateAdapter.ParamMultiply) / MarketValueCaculateAdapter.ParamDivide, 4);
            }
        }

        public decimal MarketValue
        {
            get
            {
                decimal qty = 0;
                decimal price = 0;

                decimal.TryParse(TotalQty, out qty);

                decimal.TryParse(PriceNow, out price);

                return Math.Round(qty * price);
            }
        }

        public string TotalQty { get { return LimitInfo.TotalQty; } }

        public string Group { get { return LimitInfo.Group; } }
    }
}
