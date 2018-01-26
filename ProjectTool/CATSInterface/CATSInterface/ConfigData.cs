using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATSInterface
{
    public class ConfigData
    {
        public const int AutoStartHour = 9;
        public const int AutoStopHour = 15;
        public const int AutoCloseHour = 23;

        private static bool? _checkOppo = null;
        public static bool CHECK_OPPO
        {
            get
            {
                if (_checkOppo == null)
                {
                    _checkOppo = Utils.GetConfig("CHECK_OPPO") == "1";
                }
                return _checkOppo == true;
            }
        }
    }
}
