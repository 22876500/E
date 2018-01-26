using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ASSetEntity
    {
        /// <summary>
        /// 识别记录是资金还是持仓(F 资金， P 持仓)
        /// </summary>
        public string a_type { get; set; }

        /// <summary>
        /// 账户类型 
        /// </summary>
        public string acct_type { get; set; }

        /// <summary>
        /// 交易帐号
        /// </summary>
        public string acct { get; set; }

        /// <summary>
        /// a_type = F 时，表示资金账户，暂时和acct一样；a_type = P时，标的代码)
        /// </summary>
        public string s1 { get; set; }

        /// <summary>
        /// （a_type = F, 币种；a_type = P, 当前数量 ）
        /// </summary>
        public string s2 { get; set; }
        
        /// <summary>
        /// 当前余额
        /// </summary>
        public string s3 { get; set; }

        /// <summary>
        /// 可用余额
        /// </summary>
        public string s4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string s5 { get; set; }
        public string s6 { get; set; }
        public string s7 { get; set; }
        public string s8 { get; set; }
        public string s9 { get; set; }
        public string s10 { get; set; }
    }
}
