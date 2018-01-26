using AASServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GroupClient
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple),]
    public class GroupService
    {
        private static object SendOrderObject = new object();
        private static object CancelOrderObject = new object();
        private static ConcurrentDictionary<string, ConcurrentBag<string>> dictMacOrderList = new ConcurrentDictionary<string, ConcurrentBag<string>>();

        [OperationContract]
        public string Test(string IP, short Port, string Version, short YybID, string AccountNo, string TradeAccount, string JyPassword, string TxPassword)
        {
            try
            {
                StringBuilder Result = new StringBuilder(1024 * 1024);
                StringBuilder ErrInfo = new StringBuilder(256);

                int ClientID = TdxApi.Logon(IP, Port, Version, YybID, AccountNo, TradeAccount, JyPassword, TxPassword, ErrInfo);

                if (ClientID == -1)
                {
                    return string.Format("{0}|{1}", ClientID, ErrInfo);
                }
                TdxApi.QueryData(ClientID, 5, Result, ErrInfo);
                TdxApi.Logoff(ClientID);
                return string.Format("{0}|{1}", Result, ErrInfo);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [OperationContract]
        public int GetGroupClientID(string groupName)
        {
            if (Program.db.券商帐户.Exists(groupName))
            {
                return Program.db.券商帐户.GetClientID(groupName);
            }
            return -1;
        }

        [OperationContract]
        public bool IsGroupMultythread(string groupName)
        {
            return false;
        }

        #region Query Data Normal
        //仓位查询
        [OperationContract]
        public Queryinfo QueryPosition(string GroupName)
        {
            Queryinfo result = new Queryinfo();
            StringBuilder sbResult = new StringBuilder(128);
            StringBuilder sbError = new StringBuilder(128);

            if (Program.db.券商帐户.Exists(GroupName))
            {
                //var group = Adapter.GroupsDict[GroupName];
                //group.QueryAccountData(1, result);
                Program.db.券商帐户.QueryPosition(GroupName, 1, sbResult, sbError);
                result.Result = sbResult.ToString();
                result.Error = sbError.ToString();
            }
            else
            {
                result.Error = "查询失败:未找到该组合号对应的配置信息！";
            }
            return result;
        }

        [OperationContract]
        public QueryFormatData QueryDataAutoByMac(string groupName, string mac)
        {
            var queryFormatData = new QueryFormatData();

            if (Program.db.券商帐户.Exists(groupName))
            {

                var localQueryData = Program.db.券商帐户.QueryDataLocl(groupName);
                if (localQueryData != null)
                {
                    queryFormatData.ErrWT = localQueryData.SearchTradeErrInfo ?? string.Empty;
                    if ((localQueryData.SearchOperatorError ?? "").Length == 0)
                    {
                        queryFormatData.lstWT = localQueryData.SearchOperatorResult == null ? null : localQueryData.SearchOperatorResult.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    }

                    queryFormatData.ErrCJ = localQueryData.SearchTradeErrInfo ?? string.Empty;
                    if ((localQueryData.SearchTradeErrInfo ?? "").Length > 0)
                    {
                        //处理数据，转为可用数组
                        queryFormatData.lstCJ = localQueryData.SearchTradeResult == null ? null : localQueryData.SearchTradeResult.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    queryFormatData.QueryTime = localQueryData.QueryTime;
                }
                else
                {
                    queryFormatData.ErrCJ = "查询结果为null";
                    queryFormatData.ErrWT = "查询结果为null";
                }
            }
            else
            {
                queryFormatData.ErrCJ = "查询失败，未找到该组合号对应的配置信息！";
                queryFormatData.ErrWT = "查询失败，未找到该组合号对应的配置信息！";
            }

            return queryFormatData;
        }

        public QueryFilteData QueryDataFilted(string mac, string GroupName, bool needTitleInfo)
        {
            return new QueryFilteData() { ErrCJ = "此接口为server自启动，暂不支持此功能!", ErrWT = "此接口为server自启动，暂不支持此功能!" };
        }

        [OperationContract]
        public void UpdateOrderIDList(string mac, string GroupName, string[] orderIDs)
        {
            if (Program.db.券商帐户.Exists(GroupName))
            {
                foreach (var item in orderIDs)
                {
                    if (!dictMacOrderList.ContainsKey(mac))
                    {
                        dictMacOrderList[mac] = new ConcurrentBag<string>();
                    }
                    if (!dictMacOrderList[mac].Contains(item))
                    {
                        dictMacOrderList[mac].Add(item);
                    }
                }
            }
        }
        #endregion

        #region Send/Cancel Order
        [OperationContract]
        public Queryinfo SendOrder(string GroupName, int Category, int PriceType, string Gddm, string Zqdm, decimal Price, decimal Quantity, string Mac)
        {
            Queryinfo response = new Queryinfo();
            lock (SendOrderObject)
            {
                //StringBuilder result = new StringBuilder(1024 * 1024);
                //StringBuilder errInfo = new StringBuilder(256);
                if (Program.db.券商帐户.Exists(GroupName))
                {
                    if (!dictMacOrderList.ContainsKey(Mac))
                    {
                        dictMacOrderList[Mac] = new ConcurrentBag<string>();
                    }
                    string result, errInfo;
                    if (!Tool.IsSendOrderTimeFit())
                    {
                        response.Error = string.Format("下单时限为9:00-15:00, 当前时间超出下单时限", DateTime.Now.ToShortTimeString());
                    }
                    else
                    {
                        Program.logger.LogInfo("开始下单：Group {0}, 证券代码 {1}, 价格 {2}, 下单机器mac {3}", GroupName, Zqdm, Price, Mac);
                        Program.db.券商帐户.SendOrderLocal(GroupName, Category, Gddm, Zqdm, Price, Quantity, Mac, out result, out errInfo);
                        //if (string.IsNullOrEmpty(errInfo))
                        //{
                        //    dictMacOrderList[Mac].Add(result);
                        //}
                        response.Error = errInfo;
                        response.Result = result;
                        Program.logger.LogInfoDetail("下单结果 {0}, 错误信息 {1}", result, errInfo);
                    }
                    
                }
                else
                {
                    return new Queryinfo() { Error = string.Format("下单失败，{0}:未找到该组合号对应的配置信息！", GroupName) };
                }
            }
            return response;
        }

        [OperationContract]
        public Queryinfo CancelOrder(string GroupName,  string ExchangeID, string hth)
        {
            lock (CancelOrderObject)
            {
                string result , errInfo;
                if (!Program.db.券商帐户.Exists(GroupName))
                {
                    result = "";
                    errInfo = "未添加该账户";
                }
                else if (CommonUtils.ExistsGroup(GroupName))
                {
                    result = string.Empty;
                    errInfo = "非本地登录组合号不支持此功能，请联系管理员!";
                }
                else
                {
                    Program.db.券商帐户.CancelOrderLocal(GroupName, hth, ExchangeID, out result, out errInfo);
                }
                var response = new Queryinfo() { Result = result, Error = errInfo };
                return response;
            }
        }

        [OperationContract]
        public Queryinfo SendIMSOrder(string GroupName, string BSFlag, string market, string Zqdm, float price, int qty)
        {
            return new Queryinfo() { Error = "此接口为server自启动，暂不支持此功能!" };
        }

        public Queryinfo CancelImsOrder(string GroupName, string market, string zqdm, string hth)
        {
            return new Queryinfo() { Error = "此接口为server自启动，暂不支持此功能!" };
        }
        #endregion

        [OperationContract]
        public string AccountRepay(string group, decimal amount)
        {
            if (Program.db.券商帐户.Exists(group))
            {
                return Program.db.券商帐户.AccountRepay(group, amount);
            }
            else
            {
                return string.Format("融资融券账户直接还款失败，{0}:未找到该组合号对应的配置信息！", group);
            }
        }

        public class Queryinfo
        {
            public string Result { get; set; }

            public string Error { get; set; }

            public string Other { get; set; }

            public string Other2 { get; set; }
        }

        public class QueryFormatData
        {
            public string[] lstCJ { get; set; }

            public string[] lstWT { get; set; }

            public string ErrCJ { get; set; }

            public string ErrWT { get; set; }

            public TimeSpan QueryTime { get; set; }
        }

        public class GroupQueryData
        {
            /// <summary>
            /// 查询到的成交Result
            /// </summary>
            public string SearchTradeResult { get; set; }

            /// <summary>
            /// 查询到的成交ErroInfo
            /// </summary>
            public string SearchTradeErrInfo { get; set; }

            /// <summary>
            /// 查询到的委托Result
            /// </summary>
            public string SearchOperatorResult { get; set; }

            /// <summary>
            /// 查询到的委托Error
            /// </summary>
            public string SearchOperatorError { get; set; }

            /// <summary>
            /// 查询耗费时间。
            /// </summary>
            public TimeSpan QueryTime { get; set; }
        }

        public class QueryFilteData
        {
            public string[] lstCJ { get; set; }

            public string[] lstWT { get; set; }

            public string strTitleCJ { get; set; }

            public string strTitleWT { get; set; }

            public string ErrCJ { get; set; }

            public string ErrWT { get; set; }

            public TimeSpan QueryTime { get; set; }

        }
    }
}
