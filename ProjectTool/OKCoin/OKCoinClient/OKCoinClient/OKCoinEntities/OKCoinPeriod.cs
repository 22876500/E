using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKCoinClient.OKCoinEntities
{
    /// <summary>
    /// 周期类型： this_week, next_week, quarter
    /// </summary>
    public class OKCoinPeriod
    {
        public const string ThisWeek = "this_week";

        public const string NextWeek = "next_week";

        public const string Quarter = "quarter";

        public static string GetPeriod(string str)
        {
            if (str.Contains(ThisWeek))
            {
                return ThisWeek;
            }
            else if (str.Contains(NextWeek))
            {
                return NextWeek;
            }
            else if (str.Contains(Quarter))
            {
                return Quarter;
            }
            else
            {
                return null;
            }
        }

    }
}
