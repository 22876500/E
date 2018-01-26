using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASClient.Model
{
    public class BidAskItem
    {
        
        /// <summary>
        /// 买卖方向 0 买(bid)，1 卖(ask)
        /// </summary>
        public byte Direction {get;set;}

        /// <summary>
        /// 买盘或卖盘的索引(1 ~ 10)
        /// </summary>
        public int Index {get;set;}

        string _description = null;
        public string Description
        {
            get 
            {
                if (_description == null)
                {
                    _description = (Direction == 0 ? "买" : "卖") + CommonUtils.Number_cn(Index) + "价";
                }
                return _description;
            }
        }

        public decimal GetValue(DataModel.MarketData md)
        {
            return Math.Round(((decimal)(Direction == 0 ? md.BidPrice[Index] : md.AskPrice[Index]))/ 10000, 2); 
        }
    }

    public class MarketDataExtend
    {
        public MarketDataExtend(DataModel.MarketData md)
        {
            MData = md;
            this.Time = DateTime.Parse(md.Time.DateTimeFormat());
            this.Count = 1;
        }

        public void RefreshData(DataModel.MarketData md)
        {
            MData = md;
            this.Time = DateTime.Parse(md.Time.DateTimeFormat());
            this.Count++;
        }

        public DataModel.MarketData MData { get; set; }

        public int Count { get; set; }

        public DateTime Time { get; set; }
    }

    public class WarningEntity
    {
        public WarningEntity(DataModel.MarketData md, WarningFormulaOne formula)
        {
            this.MData = md;
            this.wFormula = formula;

            if (!Program.MatchedDataCache[formula.ID].ContainsKey(md.Code))
            {
                Program.MatchedDataCache[formula.ID].TryAdd(md.Code, new MarketDataExtend(md));
            }

            //if (!Program.MatchedCountCache.ContainsKey(this.wFormula.ID))
            //{
            //    Program.MatchedCountCache.TryAdd(wFormula.ID, new ConcurrentDictionary<string, int>());
            //}

            //if (Program.MatchedCountCache[wFormula.ID].ContainsKey(MData.Code))
            //{
            //    Program.MatchedCountCache[wFormula.ID][MData.Code] += 1;
            //}
            //else
            //{
            //    Program.MatchedCountCache[wFormula.ID].TryAdd(MData.Code, 1);
            //}
            //this.WarningCount = Program.MatchedCountCache[wFormula.ID][MData.Code];
            this.WarningCount = Program.MatchedDataCache[this.wFormula.ID][md.Code].Count;
        }

        //private void MatchLargeOrder(DataModel.MarketData md, WarningFormulaOne formula)
        //{
        //    var mdBefore = Program.MarketDataCache[formula.ID][md.Code];
        //    for (int i = 0; i < 10; i++)
        //    {
        //        bool hasBidLarge = false;
        //        if (md.BidVol[i] >= this.wFormula.LargeVolume && md.BidPrice[i] > mdBefore.MData.AskPrice[0])
        //        {
        //            for (int j = 0; j < mdBefore.MData.AskPrice.Length; j++)
        //            {
        //                if (mdBefore.MData.AskPrice[j] < md.AskPrice[0])
        //                {
        //                    hasBidLarge = true;
        //                    this.LargeVolumnFlag = "买单";
        //                    this.LargeVolumnCost += "卖" + CommonUtils.Number_cn(j);
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }
        //            if (hasBidLarge)
        //            {
        //                this.LargeVolume = md.BidVol[i] + "";
        //            }
        //        }
        //        if (!hasBidLarge)
        //        {
        //            bool hasAskLarge = false;
        //            if (md.AskVol[i] >= this.wFormula.LargeVolume && md.AskPrice[i] < mdBefore.MData.BidPrice[0])
        //            {
        //                for (int j = 0; j < mdBefore.MData.BidPrice.Length; j++)
        //                {
        //                    if (mdBefore.MData.BidPrice[j] > md.BidPrice[0])
        //                    {
        //                        hasAskLarge = true;
        //                        this.LargeVolumnFlag = "卖单";
        //                        this.LargeVolumnCost += "买" + CommonUtils.Number_cn(j);
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //            if (hasAskLarge)
        //            {
        //                this.LargeVolume = md.AskVol[i] + "";
        //            }
        //        }
        //    }
        //}

        public string Code { get { return this.MData.Code; } }

        /// <summary>
        /// 预警内容
        /// </summary>
        public string WarningDescription { get { return wFormula.AnnouncementMsg(MData); } }

        /// <summary>
        /// 预警级别
        /// </summary>
        public WarningLevel WarningLevelValue { get { return wFormula.Level; } }

        /// <summary>
        /// 时间
        /// </summary>
        public string DataTime { get { return MData.Time.DateTimeFormat(); } }

        /// <summary>
        /// 现价
        /// </summary>
        public long Match { get { return MData.Match; } }

        /// <summary>
        /// 差价值
        /// </summary>
        public decimal SubValue { get { return wFormula.GetSubValue(this.MData); } }

        /// <summary>
        /// 差价类型
        /// </summary>
        public string SubType { get { return wFormula.GetSubType(); } }

        /// <summary>
        /// 出现次数
        /// </summary>
        public int WarningCount { get; set; }

        /// <summary>
        /// 大单量
        /// </summary>
        public string LargeVolume { get; set; }

        /// <summary>
        /// 大单吃掉档位
        /// </summary>
        public string LargeVolumnCost { get; set; }

        public string LargeVolumnFlag { get; set; }

        public Color Background
        {
            get
            {
                switch (WarningLevelValue)
                {
                    case AASClient.Model.WarningLevel.Yellow:
                        //return Color.Yellow;//245，184，62
                        return Color.FromArgb(255, 243, 203, 18);
                    case AASClient.Model.WarningLevel.Red:
                        return Color.Red;
                    default:
                        return Color.Black;
                }
            }
        }

        WarningFormulaOne wFormula { get; set; }

        DataModel.MarketData MData { get; set; }
    }

    // 公式 ：FirSetting - SecSetting >= ThiSetting */+ Param
    [Serializable]
    public class WarningFormulaOne : IWarningFormula
    {
        public WarningFormulaOne()
        {
            
        }

        public string ID { get; set; }

        /// <summary>
        /// 买/卖N价 - 买/卖m价 (> >= < <=) 买/卖o价 (+ - * /) Param 提示级别为：黄/红
        /// </summary>
        public string 计算方法
        {
            get
            {
                if (FirstSetting == null || SecondSetting == null || ThirdSetting == null)
                {
                    return "";
                }
                string compareMsg = ' ' + CommonUtils.CompareString(CompareTypeInfo) + ' ';
                return FirstSetting.Description + " - " + SecondSetting.Description
                     + compareMsg
                     + ThirdSetting.Description + ' ' + this.ParamCalType.CalculateString() + ' ' + Param;
            }
        }

        public string 预警级别
        {
            get
            {
                switch (Level)
                {
                    case WarningLevel.Yellow:
                        return "黄色预警";
                    case WarningLevel.Red:
                        return "红色预警";
                    default:
                        return "";
                }
            }
        }

        public BidAskItem FirstSetting { get; set; }

        public BidAskItem SecondSetting { get; set; }

        public BidAskItem ThirdSetting { get; set; }

        public decimal Param { get; set; }

        public CalculateType ParamCalType { get; set; }

        public CompareType CompareTypeInfo {get;set;}

        public WarningLevel Level { set; get; }

        public bool IsSubAll { get; set; }

        public decimal Frequency { get; set; }

        public decimal LargeVolume { get; set; }

        public List<string> CodeList { get; set; }//码表缓存

        public string AnnouncementMsg(DataModel.MarketData md)
        {
            string compareMsg = " " + CommonUtils.CompareString(CompareTypeInfo) + " ";
            var msg = string.Format("{0}({1}) - {2}({3}){4}{5}({6}) {7} {8}", 
                FirstSetting.Description,
                FirstSetting.GetValue(md),
                SecondSetting.Description,
                SecondSetting.GetValue(md),
                compareMsg,
                ThirdSetting.Description,
                ThirdSetting.GetValue(md),
                ParamCalType.CalculateString(),
                Param);
            return string.Format(" {0}:{1}", md.Code, msg);
        }

        public bool Match(DataModel.MarketData md)
        {
            if (!IsSubAll && !this.CodeList.Contains(md.Code))
            {
                return false;
            }
            //if (CodeList.Contains(md.Code))
            //{
            //    Program.logger.LogRunning("检测到预警列表中的数据" + md.Code + md.Time);
            //}
            var dt = md.Time.DateTimeFormat();
            DateTime mdTime;
            if (DateTime.TryParse(dt, out mdTime) && (DateTime.Now - mdTime).TotalMinutes > 5)
            {
                return false;
            }

            if (Program.MatchedDataCache.ContainsKey(this.ID))
            {
                if (Program.MatchedDataCache[this.ID].ContainsKey(md.Code))
                {
                    if ((decimal)(mdTime - Program.MatchedDataCache[this.ID][md.Code].Time).TotalSeconds < Frequency)
                    {
                        return false;
                    }
                }
            }
            else
            {
                Program.MatchedDataCache.TryAdd(this.ID, new ConcurrentDictionary<string, MarketDataExtend>());
            }

            if (FirstSetting.GetValue(md) == 0 || SecondSetting.GetValue(md) == 0 || ThirdSetting.GetValue(md) == 0)
            {
                return false;
            }
            

            bool isMatch = false;
            var leftCompareValue = FirstSetting.GetValue(md) - SecondSetting.GetValue(md);
            var rightCompareValue = ParamCalType.Calculate(ThirdSetting.GetValue(md), Param);
            switch (CompareTypeInfo)
            {
                case CompareType.More:
                    isMatch = leftCompareValue > rightCompareValue;
                    break;
                case CompareType.MoreOrEqual:
                    isMatch = leftCompareValue >= rightCompareValue;
                    break;
                case CompareType.Less:
                    isMatch = leftCompareValue < rightCompareValue;
                    break;
                case CompareType.LessOrEqual:
                    isMatch = leftCompareValue <= rightCompareValue;
                    break;
            }

            if (isMatch)
            {
                //匹配，则刷新预警次数，及预警列表。
                if (!Program.Warnings.ContainsKey(this.ID))
                {
                    Program.Warnings.TryAdd(this.ID, new ConcurrentQueue<WarningEntity>());
                }
                Program.Warnings[this.ID].Enqueue(this.GetWarning(md));

                if (Program.MatchedDataCache[this.ID].ContainsKey(md.Code))
                {
                    
                    Program.MatchedDataCache[ID][md.Code].RefreshData(md);
                }
                else
                {
                    Program.MatchedDataCache[this.ID].TryAdd(md.Code, new MarketDataExtend(md));
                }
            }
            //无论是否有预警信息，肯定刷新市场缓存。
            

            return isMatch;
        }

        //价差值
        public decimal GetSubValue(DataModel.MarketData md)
        {
            return Math.Abs(FirstSetting.GetValue(md) - SecondSetting.GetValue(md));
        }

        //价差类型
        public string GetSubType()
        {
            return FirstSetting.Description.Replace("价", string.Empty) + SecondSetting.Description.Replace("价", string.Empty);
        }



        public WarningEntity GetWarning(DataModel.MarketData md)
        {
            return new WarningEntity(md, this);
        }

    }

    public class PublicStock
    {
        public string StockCode { get; set; }

        public string StockName { get; set; }

        public int CanSaleCount { get; set; }

        public byte Market { get; set; }

        public bool HasPossessed { get; set; }
    }

    //public class WarningFormulaDynamic : IWarningFormula
    //{
    //    public string ID { get; set; }

    //    public string 计算方法
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public string 预警级别
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    public bool IsMatchFormula(DataModel.MarketData md)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public string AnnouncementMsg(DataModel.MarketData md)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public decimal Frequency { get; set; }

    //    public decimal GetFrequency()
    //    {
    //        return Frequency;
    //    }

    //    public WarningLevel Level
    //    {
    //        get;
    //        set;
    //    }

    //    public WarningLevel GetLevel()
    //    {
    //        return this.Level;
    //    }

    //    public string GetID()
    //    {
    //        return this.ID;
    //    }
    //}

    
}
