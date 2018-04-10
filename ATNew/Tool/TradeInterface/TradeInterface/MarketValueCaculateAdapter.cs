using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeInterface
{
    public class MarketValueCaculateAdapter 
    {

        public static decimal ParamMultiply
        {
            get
            {
                var configItem = CommonUtils.GetConfig("ParamMultiply");
                if (string.IsNullOrEmpty(configItem))
                {
                    configItem = "200000000";
                    CommonUtils.SetConfig("ParamMultiply", configItem);
                }

                return decimal.Parse(configItem);
            }
            set
            {
                CommonUtils.SetConfig("ParamMultiply", value.ToString());
            }
        }

        public static decimal ParamDivide
        {
            get
            {
                var configItem = CommonUtils.GetConfig("ParamDivide");
                if (string.IsNullOrEmpty(configItem))
                {
                    configItem = "1000000000";
                    CommonUtils.SetConfig("ParamDivide", configItem);
                }
                return decimal.Parse(configItem);
            }

            set
            {
                CommonUtils.SetConfig("ParamDivide", value.ToString());
            }
        }

        private static List<StockItemEntity> _stockEntityList;
        public static List<StockItemEntity> StockEntityList
        {
            get
            {
                //读文件
                //if (_stockEntityList == null)
                //{
                //    try
                //    {
                //        if (File.Exists(CommonUtils.CurrentPath + "\\stock.txt"))
                //        {
                //            string txt = File.ReadAllText(CommonUtils.CurrentPath + "\\stock.txt", Encoding.UTF8);
                //            _stockEntityList = txt.FromJson<List<StockItemEntity>>();
                //        }
                        
                //    }
                //    catch (Exception ex)
                //    {
                //        CommonUtils.Log("MarketValueCaculateAdapter.StockEntityList 读异常,", ex);
                //    }
                //}
                return _stockEntityList;
            }
            set 
            {
                _stockEntityList = value;
                //写文件
                //try
                //{
                //    File.WriteAllText("stock.txt", value.ToJson(), Encoding.UTF8);
                //}
                //catch (Exception ex)
                //{
                //    CommonUtils.Log("MarketValueCaculateAdapter.StockEntityList 写异常,", ex);
                //}
            }
        }

        public static List<LimitItemEntity> LimitEntityList { get; set; }

        public static List<TraderStockInfo> CalculateList { get; set; }

        public static void CalculateTraderStockList()
        {
            if (LimitEntityList == null || LimitEntityList.Count == 0 || StockEntityList == null || StockEntityList.Count == 0)
            {
                return;
            }
            if (CalculateList == null)
            {
                CalculateList = new List<TraderStockInfo>();
            }
            CalculateList.Clear();

            foreach (var stockItem in StockEntityList)
            {
                var relatedList = LimitEntityList.Where(_ => _.StockID == stockItem.StockID);
                foreach (var limitItem in relatedList)
                {
                    var calItem = new TraderStockInfo(limitItem, stockItem);
                    CalculateList.Add(calItem);
                }
            }
        }
    }
}
