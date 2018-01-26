using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace GroupService
{
    /// <summary>
    /// GroupInterface 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class TradeInterface : System.Web.Services.WebService
    {
        static object SendOrderObject = new object();
        static object CancelOrderObject = new object();
        static Dictionary<string, DateTime> dictLogon = new Dictionary<string, DateTime>();

        #region Group  Login/GetID
        [WebMethod]
        public bool Login(string userName, string password)
        {
            try
            {
                bool isLogon = "admin" == userName && CommonUtils.GetConfig("password") == Cryptor.MD5Encrypt(password);
                if (isLogon)
                {
                    if (dictLogon.ContainsKey(userName))
                    {
                        dictLogon[userName] = DateTime.Now;
                    }
                    else
                    {
                        dictLogon.Add(userName, DateTime.Now);
                        CommonUtils.Log(string.Format("用户'{0}'登录成功 ", Cryptor.MD5Decrypt(userName)));
                    }
                }
                return isLogon;
            }
            catch (Exception ex)
            {
                CommonUtils.Log(ex.Message);
                return false;
            }
        }

        [WebMethod]
        public int GetGroupClientID(string groupName)
        {
            //CommonUtils.Log("GroupClient:客户端确认组合号登录状态，" + groupName);
            if (Adapter.GroupsDict.ContainsKey(groupName))
            {
                var item = Adapter.GroupsDict[groupName];
                return item.ClientID;
            }
            return -1;
        }

        [WebMethod]
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
        #endregion

        #region Query Data Normal
        [WebMethod]
        public Queryinfo QueryData(string GroupName, int Category)
        {
            StringBuilder result = new StringBuilder(1024 * 1024);
            StringBuilder errInfo = new StringBuilder(256);
            if (Adapter.GroupsDict.ContainsKey(GroupName))
            {
                var group = Adapter.GroupsDict[GroupName];
                if (group.ClientID > -1)
                {
                    var dtStart = DateTime.Now;
                    TdxApi.QueryData(group.ClientID, Category, result, errInfo);

                    var logonInfoItem = Adapter.GroupLogonList.First(_ => _.Name == GroupName);
                    logonInfoItem.Times = (DateTime.Now - dtStart).TotalSeconds.ToString();
                }
                else
                {
                    errInfo.Append("查询失败，组合号" + GroupName + " ClientID为-1");
                }
            }
            else
            {
                errInfo.Append("查询失败:未找到该组合号对应的配置信息！");
            }

            return new Queryinfo() { Result = result.ToString(), Error = errInfo.ToString() };
        }

        [WebMethod]
        public QueryDataObj QueryDataAuto(string GroupName)
        {
            StringBuilder result = new StringBuilder(1024 * 1024);
            StringBuilder errInfo = new StringBuilder(256);
            var queryData = new QueryDataObj();

            if (Adapter.GroupsDict.ContainsKey(GroupName))
            {
                var group = Adapter.GroupsDict[GroupName];
                if (group.ClientID > -1)
                {
                    return group.QueryInfo;
                }
                else
                {
                    errInfo.Append("查询失败，组合号" + GroupName + " ClientID为-1");
                }
            }
            else
            {
                errInfo.Append("查询失败，未找到该组合号对应的配置信息！");
            }
            return queryData;
        }

   
        #endregion

        #region Query Data filtered
        [WebMethod]
        public QueryFilteData QueryDataFilted(string mac, string GroupName, bool needTitleInfo)
        {
            if (Adapter.GroupsDict.ContainsKey(GroupName))
            {
                var group = Adapter.GroupsDict[GroupName];
                if (group.ClientID > -1)
                {
                    var queryResult = new QueryFilteData() { };
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
            }
            return null;
        }

        [WebMethod]
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

        [WebMethod]
        public string QueryOrderByID(string GroupName, string orderID)
        {
            if (Adapter.GroupsDict.ContainsKey(GroupName))
            {
                var group = Adapter.GroupsDict[GroupName];
                if (group.ClientID > -1)
                {
                    return group.GetOrder(orderID);
                }
            }
            return null;
        }

        [WebMethod]
        public string[] QueryListByMac(string mac, string GroupName)
        {
            if (Adapter.GroupsDict.ContainsKey(GroupName))
            {
                var group = Adapter.GroupsDict[GroupName];
                if (group.ClientID > -1)
                {
                    return group.GetOrdersDetail(mac);
                }
            }
            return null;
        }
        #endregion

        #region Send/Cancel Order
        [WebMethod]
        public Queryinfo SendOrder(string GroupName, int Category, int PriceType, string Gddm, string Zqdm, float Price, int Quantity)
        {
            Queryinfo response = null;
            lock (SendOrderObject)
            {
                StringBuilder result = new StringBuilder(1024 * 1024);
                StringBuilder errInfo = new StringBuilder(256);

                if (Adapter.GroupsDict.ContainsKey(GroupName))
                {
                    var group = Adapter.GroupsDict[GroupName];
                    if (group.ClientID > -1)
                    {
                        //CommonUtils.Log("组合号：" + GroupName + "下单开始，证券代码：" + Zqdm);
                        DateTime dtStart = DateTime.Now;
                        if (Zqdm.Length == 5)
                        {
                            response = group.SendOrderHK(Category, PriceType, Gddm, Zqdm, Price, Quantity, result, errInfo);
                        }
                        else
                        {
                            response = group.SendOrderNormal(Category, PriceType, Gddm, Zqdm, Price, Quantity, result, errInfo);
                        }
                    }
                    else
                    {
                        errInfo.AppendFormat("{0}: ClientID为-1", GroupName);
                    }
                }
                else
                {
                    errInfo.AppendFormat("下单失败，{0}:未找到该组合号对应的配置信息！", GroupName);
                }
            }
            return response;
        }

        [WebMethod]
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

        #endregion

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
    }
}
