using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASClient.TDFData
{
    public class DataSourceConfig
    {
        /// <summary>
        /// 是否启用TDF数据源
        /// </summary>
        public static bool IsUseTDFData = true;

        /// <summary>
        /// 是否启用部分规则(部分重要股票启用tdf数据源，其他启用通达信数据源)
        /// </summary>
        public static bool IsUseVipCodes = false;

        /// <summary>
        /// 是否启用通达信行情
        /// </summary>
        public static bool IsUseTDXData = true;

        

        private static List<string> _vipCodes = new List<string>();
        public static List<string> VipCodes
        {
            get
            {
                return _vipCodes;
            }
        }
    }
}
