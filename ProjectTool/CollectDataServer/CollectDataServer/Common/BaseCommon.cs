using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectDataServer.Common
{
    public enum RoleType
    {
        超级管理员 = 0,
        普通管理员 = 1,

        超级风控员 = 10,
        普通风控员 = 11,
        初级风控员 = 12,

        交易员 = 20,
    }
    public class BaseCommon
    {
        public const string SERVICEPORT = "ServicePort";
    }
}
