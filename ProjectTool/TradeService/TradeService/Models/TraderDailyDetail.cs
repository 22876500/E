using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeService
{
    public class TraderDailyDetail
    {
        public string Time { get; set; }

        /// <summary>
        /// StockName（数据比较依据列）
        /// </summary>
        public string Sym { get; set; }

        public string Sym_Format { get { return Sym.GetFormatName(); } }

        /// <summary>
        /// SLD -> sell, BOT -> Buy （数据比较依据列）
        /// </summary>
        public string Side { get; set; }

        /// <summary>
        /// Quantity（数据比较依据列）
        /// </summary>
        public int Qty { get; set; }

        /// <summary>
        /// Exe_Price （数据比较依据列）
        /// </summary>
        public decimal Exe_Price { get; set; }

        public string Acct { get; set; }

        public DateTime Date { get; set; }
        
        public decimal Lvs_Qty { get; set; }

        /// <summary>
        /// 未确定是否用于比较
        /// </summary>
        public string Dest { get; set; }

        public decimal Price { get; set; }
        
        /// <summary>
        /// Trader Name
        /// </summary>
        public string Trader { get; set; }

        public string Sol { get; set; }

        
        public int Seq { get; set; }
        
        public string Cl_Ord_ID { get; set; }
        
        public string Exch_Cl_Ord_ID { get; set; }
        
        public string Exch_Ord_ID { get; set; }
        
        public string Exch_Ord_ID_2 { get; set; }

        public string Exch_Exec_ID { get; set; }
        
        public string Clear { get; set; }
        public string ECN_Fee { get; set; }
        
        public string Tif { get; set; }
        
        public string AON { get; set; }
        
        public string DNR { get; set; }
        
        public string DNI { get; set; }
        
        public string Acct_Type { get; set; }
        
        public string Specialist { get; set; }
        
        public string Source { get; set; }
        
        public string Poss_Dupe { get; set; }
        
        public string Updated { get; set; }

        
        public DateTime Order_Date { get; set; }
        public string Order_Date_Str { get { return Order_Date.ToString("yyyy/MM/dd"); } }
        
        public string Order_Time { get; set; }

        public string Ord_Rec_ID { get; set; }

        public string Rec_ID { get; set; }
        
        public string Attrib { get; set; }
        
        public decimal Cvr_Qty { get; set; }
        
        public string Comm { get; set; }
        
        public string Creator { get; set; }
        
        public string Batch_ID { get; set; }
        
        public string Strategy { get; set; }
        
        public decimal Alloc_ID { get; set; }
        
        public string Acronym { get; set; }
        
        public string Expiration { get; set; }
        
        public decimal Strike { get; set; }
        
        public string Call_Put { get; set; }
        
        public string Underlying { get; set; }
        
        public string Open_Close { get; set; }
        
        public string Cover_Un { get; set; }
        

        public string Currency { get; set; }
        
        /// <summary>
        /// 空白，不确定数据类型
        /// </summary>
        public string Sett_Cur { get; set; }
        
        /// <summary>
        /// 空白，不确定数据类型
        /// </summary>
        public string FutSettDate { get; set; }

        public decimal SettCurrAmt { get; set; }
        
        public string Basket_Name { get; set; }
        
        public string Basket_ID { get; set; }

        public string Last_Mkt { get; set; }

        public string Inst { get; set; }

        public string ExtendColumn0 { get; set; }

        //两位小数
        public string ExtendColumn1 { get; set; }

        
    }
}
