using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ConfigCache
    {
        private static readonly object Sync = new object();
        //DataBase
        private static string _dbName;
        public static string DBName {
            get
            {
                if (string.IsNullOrEmpty(_dbName))
                {
                    _dbName = Program.appConfig.GetValue("DBName", "ECoinServer");
                    if (string.IsNullOrEmpty(_dbName))
                    {
                        _dbName = "ECoinServer";
                        Program.appConfig.SetValue("DBName", _dbName);
                    }
                }
                return _dbName;
            }
        }

        //Login Port
        private static string _connectPort;
        public static string ConnectPort
        {
            get
            {
                if (string.IsNullOrEmpty(_connectPort))
                {
                    _connectPort = Program.appConfig.GetValue("ServiceConnectPort", "40008");
                }
                return _connectPort;
            }
        }


        private static string _useLogDetail;
        public static bool UseLogDetail
        {
            get
            {
                if (string.IsNullOrEmpty(_useLogDetail))
                {
                    _useLogDetail = Program.appConfig.GetValue("UseLogDetail", "0");
                }
                return _useLogDetail == "1";
            }
        }

    }
}
