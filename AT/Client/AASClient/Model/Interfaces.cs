using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASClient.Model
{
    public interface IWarningFormula
    {
        string ID { get; set; }

        decimal Frequency { get; set; }

        string 计算方法 { get; }

        string 预警级别 { get; }

        WarningLevel Level { get; set; }

        bool Match(DataModel.MarketData md);

        string AnnouncementMsg(DataModel.MarketData md);


    }
}
