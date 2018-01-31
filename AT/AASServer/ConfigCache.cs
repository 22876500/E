using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AASServer
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
                    _dbName = Program.appConfig.GetValue("DBName", "AAS");
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
                    _connectPort = Program.appConfig.GetValue("ServiceConnectPort", "40808");
                }
                return _connectPort;
            }
        }


        private static string _catsGroups;
        public static string CatsGroups
        {
            get
            {
                if (_catsGroups == null)
                {
                    _catsGroups = Program.appConfig.GetValue("CATSGroups", "");
                }
                return _catsGroups;
            }
        }


        private static string _useLogDetail;
        public static string UseLogDetail
        {
            get
            {
                if (_useLogDetail == null)
                {
                    _useLogDetail = Program.appConfig.GetValue("UseLogDetail", "0");
                }
                return _useLogDetail;
            }
        }


        private static string _useAyersInterface;
        public static string UseAyersInterface
        {
            get
            {
                if (_useAyersInterface == null)
                {
                    _useAyersInterface = Program.appConfig.GetValue("UseAyersInterface", "0");
                }
                return _useAyersInterface;
            }
        }

        private static string _openGroupService;
        public static string OpenGroupService
        {
            get
            {
                if (_openGroupService == null)
                {
                    _openGroupService = Program.appConfig.GetValue("OpenGroupService", "");
                    if (_openGroupService == "")
                    {
                        Program.appConfig.SetValue("OpenGroupService", "0");
                    }
                }
                return _openGroupService;
            }
        }
    }
}
