using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GroupClient
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple),]
    public class GroupService
    {

        static object SendOrderObject = new object();
        static object CancelOrderObject = new object();

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
            if (Adapter.GroupsDict.ContainsKey(groupName))
            {
                var item = Adapter.GroupsDict[groupName];
                return item.ClientID;
            }
            return -1;
        }

        [OperationContract]
        public bool IsGroupMultythread(string groupName)
        {
            try
            {
                if (Adapter.GroupsDict.ContainsKey(groupName))
                {
                    return Adapter.GroupsDict[groupName].Multithreading == true;
                }
            }
            catch (Exception ex)
            {
                CommonUtils.Log("GroupService.IsGroupMultythread Function Exception, groupName:" + groupName, ex);
            }

            return false;
        }

        #region Query Data Normal
        [OperationContract]
        public Queryinfo QueryPosition(string GroupName)
        {
            Queryinfo result = new Queryinfo();
            if (Adapter.GroupsDict.ContainsKey(GroupName))
            {
                var group = Adapter.GroupsDict[GroupName];
                group.QueryAccountData(1, result);
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
            //CommonUtils.Log("QueryDataAutoByMac:{0}", groupName);
            StringBuilder result = new StringBuilder(1024 * 1024);
            StringBuilder errInfo = new StringBuilder(256);
            var queryFormatData = new QueryFormatData();

            if (Adapter.GroupsDict.ContainsKey(groupName))
            {
                var group = Adapter.GroupsDict[groupName];

                if (!string.IsNullOrEmpty(group.QueryInfo.SearchOperatorResult))
                {
                    queryFormatData.lstWT = group.FilterDataByOrder(group.QueryInfo.SearchOperatorResult, mac);
                }

                if (!string.IsNullOrEmpty(group.QueryInfo.SearchTradeResult))
                {
                    queryFormatData.lstCJ = group.FilterDataByOrder(group.QueryInfo.SearchTradeResult, mac);
                }
                queryFormatData.ErrWT = group.QueryInfo.SearchOperatorError;
                queryFormatData.ErrCJ = group.QueryInfo.SearchTradeErrInfo;
                queryFormatData.QueryTime = group.QueryInfo.QueryTime;
                return queryFormatData;
            }
            else
            {
                errInfo.Append("查询失败，未找到该组合号对应的配置信息！");
            }
            return null;
        }

        [OperationContract]
        public QueryFilteData QueryDataFilted(string mac, string GroupName, bool needTitleInfo)
        {
            if (Adapter.GroupsDict.ContainsKey(GroupName))
            {
                //CommonUtils.Log(string.Format("1.收到请求！mac{0}, GroupName{1}", mac, GroupName));
                var queryResult = new QueryFilteData();
                try
                {
                    var group = Adapter.GroupsDict[GroupName];
                    queryResult.lstWT = group.GetOrdersDetail(mac);
                    queryResult.lstCJ = group.GetTradesDetail(mac);
                    queryResult.ErrWT = group.QueryInfo.SearchOperatorError;
                    queryResult.ErrCJ = group.QueryInfo.SearchTradeErrInfo;
                    queryResult.QueryTime = group.QueryInfo.QueryTime;

                    if (needTitleInfo)
                    {
                        queryResult.strTitleCJ = group.strTitleCj;
                        queryResult.strTitleWT = group.strTitleWt;
                    }
                    return queryResult;
                }
                catch (Exception ex)
                {
                    queryResult.ErrCJ = string.Format("查询服务异常，Error Message:{0}", ex.Message);
                    queryResult.ErrWT = string.Format("查询服务异常，Error Message:{0}", ex.Message);
                }

            }
            return null;
        }

        [OperationContract]
        public string[] UpdateOrderIDList(string mac, string GroupName, string[] orderIDs)
        {
            if (Adapter.GroupsDict.ContainsKey(GroupName))
            {
                var group = Adapter.GroupsDict[GroupName];
                if (group.ClientID > -1)
                {
                    return group.UpdateOrderID(mac, orderIDs);
                }
            }
            return null;
        }
        #endregion

        #region Send/Cancel Order
        [OperationContract]
        public Queryinfo SendOrder(string GroupName, int Category, int PriceType, string Gddm, string Zqdm, float Price, int Quantity, string Mac)
        {
            Queryinfo response = null;
            lock (SendOrderObject)
            {
                StringBuilder result = new StringBuilder(1024 * 1024);
                StringBuilder errInfo = new StringBuilder(256);

                if (Adapter.GroupsDict.ContainsKey(GroupName))
                {
                    var group = Adapter.GroupsDict[GroupName];
                    if (!Tool.IsSendOrderTimeFit())
                    {
                        return new Queryinfo() { Error = string.Format("下单时限为9:00-15:00, 当前时间超出下单时限", DateTime.Now.ToShortTimeString()) };
                    }
                    else if (group.ClientID == -1)
                    {
                        return new Queryinfo() { Error = string.Format("{0}: ClientID为-1", GroupName) };
                    }
                    else
                    {
                        if (Zqdm.Length == 5)
                        {
                            response = group.SendOrderHK(Category, PriceType, Gddm, Zqdm, Price, Quantity, result, errInfo);
                        }
                        else
                        {
                            response = group.SendOrderNormal(Category, PriceType, Gddm, Zqdm, Price, Quantity, Mac, result, errInfo);
                        }
                        
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
        public Queryinfo CancelOrder(string GroupName, string ExchangeID, string hth)
        {
            lock (CancelOrderObject)
            {
                StringBuilder result = new StringBuilder(1024 * 1024);
                StringBuilder errInfo = new StringBuilder(256);
                if (Adapter.GroupsDict.ContainsKey(GroupName))
                {
                    var group = Adapter.GroupsDict[GroupName];

                    if (group.ClientID > -1)
                    {
                        group.CancelOrder(ExchangeID, hth, result, errInfo);
                    }
                    else
                    {
                        errInfo.AppendFormat("撤单失败，组合号{0} ClientID为-1", GroupName);
                    }
                }
                else
                {
                    errInfo.AppendFormat("GroupService:未找到组合号{0}对应的配置信息！", GroupName);
                }
                var response = new Queryinfo() { Result = result.ToString(), Error = errInfo.ToString() };
                return response;
            }
        }

        /// <summary>
        /// Ims 下单接口
        /// </summary>
        /// <param name="GroupName"></param>
        /// <param name="BSFlag">B 买， S 卖</param>
        /// <param name="market">上海0    深圳1    港股H    沪港通0H      沪股通H0      深港通1H         深股通H1</param>
        /// <param name="Zqdm"></param>
        /// <param name="price"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        [OperationContract]
        public Queryinfo SendIMSOrder(string GroupName, string BSFlag, string market, string Zqdm, float price, int qty)
        {
            Queryinfo response = null;
            lock (SendOrderObject)
            {
                StringBuilder result = new StringBuilder(1024 * 1024);
                StringBuilder errInfo = new StringBuilder(256);
                if (Adapter.GroupsDict.ContainsKey(GroupName))
                {
                    var group = Adapter.GroupsDict[GroupName];
                    if (!Tool.IsSendOrderTimeFit())
                    {
                        errInfo.AppendFormat("下单时限为9:00-15:00, 当前时间{0}超出下单时限", DateTime.Now.ToShortDateString());
                    }
                    else if (group.ClientID > -1)
                    {
                        errInfo.AppendFormat("{0}: ClientID为-1", GroupName);
                    }
                    else
                    {
                        group.SendOrderIMS(BSFlag, market, Zqdm, price, qty, result, errInfo);
                    }
                }
                else
                {
                    errInfo.AppendFormat("下单失败，{0}:未找到该组合号对应的配置信息！", GroupName);
                }
                response = new Queryinfo() { Result = result.ToString(), Error = errInfo.ToString() };
            }
            return response;
        }

        /// <summary>
        /// Ims撤单接口
        /// </summary>
        /// <param name="GroupName">委托编号</param>
        /// <param name="market">上海0    深圳1    港股H    沪港通0H      沪股通H0      深港通1H         深股通H1</param>
        /// <param name="zqdm">证券代码</param>
        /// <param name="hth">委托编号</param>
        /// <returns></returns>
        [OperationContract]
        public Queryinfo CancelImsOrder(string GroupName, string market, string zqdm, string hth)
        {
            lock (CancelOrderObject)
            {
                StringBuilder result = new StringBuilder(1024 * 1024);
                StringBuilder errInfo = new StringBuilder(256);
                if (Adapter.GroupsDict.ContainsKey(GroupName))
                {
                    var group = Adapter.GroupsDict[GroupName];
                    if (!group.IsIMSAccount)
                    {

                    }

                    if (group.ClientID > -1)
                    {
                        group.CancelImsOrder(market , zqdm, hth, result, errInfo);
                    }
                    else
                    {
                        errInfo.AppendFormat("撤单失败，组合号{0} ClientID为-1", GroupName);
                    }
                }
                else
                {
                    errInfo.AppendFormat("GroupService:未找到组合号{0}对应的配置信息！", GroupName);
                }
                var response = new Queryinfo() { Result = result.ToString(), Error = errInfo.ToString() };
                return response;
            }
        }
        #endregion

        /// <summary>
        /// 融资融券账户直接还款
        /// </summary>
        /// <param name="group"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [OperationContract]
        public string AccountRepay(string group, decimal amount)
        {
            if (Adapter.GroupsDict.ContainsKey(group))
            {
                StringBuilder result = new StringBuilder(1024 * 1024);
                StringBuilder errInfo = new StringBuilder(256);
                var groupItem = Adapter.GroupsDict[group];
                groupItem.Repay(Math.Round(amount, 3).ToString(), result, errInfo);
                if (errInfo.Length == 0)
                {
                    return result.ToString();
                }
                else
                {
                    return errInfo.ToString();
                }
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

        public class QueryDataObj
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

        public class QueryFormatData
        {
            public string[] lstCJ { get; set; }

            public string[] lstWT { get; set; }

            public string ErrCJ { get; set; }

            public string ErrWT { get; set; }

            public TimeSpan QueryTime { get; set; }
        }
    }
}
