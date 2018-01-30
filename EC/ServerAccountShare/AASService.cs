using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Server
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IClient))]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class AASService
    {

        [OperationContract]
        public Server.DbDataSet.平台用户DataTable QuerySingleUser(string ClientVersion)
        {
            if (ClientVersion != Program.Version)
            {
                throw new FaultException("客户端版本不正确");
            }

            return Program.db.平台用户.QuerySingleUser(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
            //双工通信，调用客户端实现的回调函数。
            //ICalculatorCallback callback = OperationContext.Current.GetCallbackChannel<ICalculatorCallback>();
            //callback.showResult(a + b, a, b);
            //UserNamePasswordValidator的认证方式，Validator中可以知道相应的UserName和Password，
            //在Service中直接使用OperationContext.Current.ServiceSecurityContext.PrimaryIdentity即可获取当前登录用户信息。


            //获取远程端口的IP和端口号。 可以利用来进行心跳检测
            //MessageProperties MessageProperties1 = OperationContext.Current.IncomingMessageProperties;
            //RemoteEndpointMessageProperty RemoteEndpointMessageProperty1 = MessageProperties1[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;


            //IClient IClient1 = OperationContext.Current.GetCallbackChannel<IClient>();

            //ICommunicationObject ICommunicationObject1 = IClient1 as ICommunicationObject;
            //ICommunicationObject1.Closed += ICommunicationObject1_Closed;
            //ICommunicationObject1.Faulted += ICommunicationObject1_Faulted;

            //Program.client[OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name] = IClient1;
            //Program.userName[IClient1] = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;




            //Program.logger.Log("{0}登录成功[{1}:{2}]", OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name, RemoteEndpointMessageProperty1.Address, RemoteEndpointMessageProperty1.Port);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [OperationContract]
        public string Decrypt(string string1)
        {
            return Cryptor.MD5Decrypt(string1);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [OperationContract]
        public string[] Get交易服务器(string QS)
        {
            string FileName = QS + "交易服务器.txt";
            if (File.Exists(FileName))
            {
                return File.ReadAllLines(FileName, Encoding.Default);
            }
            else
            {
                return new string[] { "未找到交易服务器文件:127.0.0.1:7708" };
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "交易员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        [OperationContract]
        public string[] Get行情服务器()
        {
         

            string FileName = "行情服务器.txt";
            if (File.Exists(FileName))
            {
                return File.ReadAllLines(FileName, Encoding.Default);
            }
            else
            {
                return new string[] { "未找到行情服务器文件:127.0.0.1:7708" };
            }
        }

        [OperationContract]
        public string GetDataServerIP()
        {
            string fileName = "数据分发服务器.txt";
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName, Encoding.Default);
            }
            else
            {
                return string.Empty;
            }
        }


        [OperationContract]
        public string QueryClientIP()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return endpoint.Address + ":" + endpoint.Port;
        }
        
        #region 用户管理
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        [OperationContract]
        public Server.DbDataSet.平台用户DataTable QueryUser()
        {
            Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
            if (平台用户Row.角色 == (int)角色.普通管理员)
            {
                if (平台用户Row.分组 == (int)分组.ALL)
                {
                    return Program.db.平台用户.QueryUserInRoles(new int[] { (int)角色.普通管理员, (int)角色.普通风控员, (int)角色.交易员 }, 平台用户Row.分组);
                }
                else
                {
                    return Program.db.平台用户.QueryUserInRoles(new int[] { (int)角色.普通风控员, (int)角色.交易员 }, 平台用户Row.分组);
                }
            }
            else
            {
                return Program.db.平台用户.QueryUserInRoles(new int[] { (int)角色.超级管理员, (int)角色.普通管理员, (int)角色.超级风控员, (int)角色.普通风控员, (int)角色.交易员 }, 平台用户Row.分组);
            }
        }



        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void ResetPassword(string UserName, string Password)
        {
            Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
            if (平台用户Row.角色 == (int)角色.普通管理员)
            {
                if (平台用户Row.用户名 == UserName)
                {
                    throw new FaultException("普通管理员不能重置自己的密码");
                }
                else
                {
                    Program.db.平台用户.ResetPassword(UserName, Password);

                }
            }
            else
            {
                Program.db.平台用户.ResetPassword(UserName, Password);
            }
        }





        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void AddUser(string UserName, string Password, 角色 Role, decimal 仓位限制, decimal 亏损限制, decimal 手续费率, bool 允许删除碎股订单, 分组 Region)
        {
            if (Role!= 角色.交易员)
            {
                仓位限制 = 0;
                亏损限制 = 0;
                手续费率 = 0;
            }


            if (Role!= 角色.普通风控员 && Role!= 角色.超级风控员)
            {
                允许删除碎股订单 = false;
            }


            if (Role == 角色.超级风控员)
            {
                允许删除碎股订单 = true;
            }



             Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
             if (平台用户Row.角色 == (int)角色.普通管理员 && 平台用户Row.分组 != (int)分组.ALL)
             {
                 if (Role == 角色.超级管理员 || Role == 角色.超级风控员 || Role == 角色.普通管理员)
                 {
                     throw new FaultException("无权限添加此角色的平台用户");
                 }
                 else
                 {
                     Program.db.平台用户.AddUser(UserName, Password, Role, 仓位限制, 亏损限制, 手续费率, 允许删除碎股订单, Region);
                 }
             }
             else
             {
                 Program.db.平台用户.AddUser(UserName, Password, Role, 仓位限制, 亏损限制, 手续费率, 允许删除碎股订单, Region);
             }
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void UpdateUser(string UserName, decimal 仓位限制, decimal 亏损限制, decimal 手续费率, bool 允许删除碎股订单, 分组 分组1)
        {
            Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
            Server.DbDataSet.平台用户Row 被修改用户Row = Program.db.平台用户.Get平台用户(UserName);


            if (被修改用户Row.角色 != (int)角色.交易员)
            {
                仓位限制 = 0;
                亏损限制 = 0;
                手续费率 = 0;
            }


            if (被修改用户Row.角色 != (int)角色.普通风控员 && 被修改用户Row.角色 != (int)角色.超级风控员)
            {
                允许删除碎股订单 = false;
            }


            if (被修改用户Row.角色 == (int)角色.超级风控员)
            {
                允许删除碎股订单 = true;
            }


            if (平台用户Row.角色 == (int)角色.普通管理员 && 平台用户Row.分组 != (int)分组.ALL)
            {
                if (被修改用户Row.角色 == (int)角色.超级管理员 || 被修改用户Row.角色 == (int)角色.超级风控员 || 被修改用户Row.角色 == (int)角色.普通管理员)
                {
                    throw new FaultException("无权限修改此角色的平台用户");
                }
                else
                {
                    Program.db.平台用户.UpdateUser(UserName, 仓位限制, 亏损限制, 手续费率, 允许删除碎股订单, 分组1);
                }
            }
            else
            {
                Program.db.平台用户.UpdateUser(UserName, 仓位限制, 亏损限制, 手续费率, 允许删除碎股订单, 分组1);
            }
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void DeleteUser(string UserName)
        {
            Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
            Server.DbDataSet.平台用户Row 被删除用户Row = Program.db.平台用户.Get平台用户(UserName);

            if (平台用户Row.角色 == (int)角色.普通管理员 && 平台用户Row.分组 != (int)分组.ALL)
            {
                if (被删除用户Row.角色 == (int)角色.超级管理员 || 被删除用户Row.角色 == (int)角色.超级风控员 || 被删除用户Row.角色 == (int)角色.普通管理员)
                {
                    throw new FaultException("无权限删除此角色的平台用户");
                }
                else
                {
                    Program.db.平台用户.DeleteUser(UserName);
                    foreach (IClient IClient1 in Program.ClientUserName.Keys)
                    {
                        if (Program.ClientUserName[IClient1] == UserName)
                        {
                            IClient1.Close();
                        }
                    }
                    Program.db.额度分配.DeleteAllTradeLimitByUser(UserName);
                }
            }
            else
            {
                Program.db.平台用户.DeleteUser(UserName);
                foreach (IClient IClient1 in Program.ClientUserName.Keys)
                {
                    if (Program.ClientUserName[IClient1] == UserName)
                    {
                        IClient1.Close();
                    }
                }
                Program.db.额度分配.DeleteAllTradeLimitByUser(UserName);
            }
        }
        #endregion


        #region 风控分配管理
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public Server.DbDataSet.平台用户DataTable QueryFK()
        {
            Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
            return Program.db.平台用户.QueryUserInRoles(new int[] { (int)角色.普通风控员 }, 平台用户Row.分组);
        }



        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public Server.DbDataSet.平台用户DataTable QueryJyBelongFK(string FKUserName)
        {
            return Program.db.QueryJyBelongFK(FKUserName);
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public Server.DbDataSet.平台用户DataTable QueryJyNotBelongFK(string FKUserName)
        {
            return Program.db.QueryJyNotBelongFK(FKUserName);
        }



        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void AddTraderToRC(string RC, string[] Traders)
        {
            foreach (string Trader in Traders)
            {
                Program.db.风控分配.Add(Trader, RC);
            }
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void DeleteTraderFromRC(string RC, string[] Traders)
        {
            foreach (string Trader in Traders)
            {
                Program.db.风控分配.Delete(Trader, RC);
            }
        }


        #endregion

        #region 电子币账户管理
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public Server.DbDataSet.电子币帐户DataTable QueryECoinQsAccount()
        {
            return Program.db.电子币帐户.QueryQsAccount();
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void EnableECoinQSAccount(string Name, bool Enabled)
        {
            Program.db.电子币帐户.EnableQSAccount(Name, Enabled);
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void AddECoinQSAccount(bool Enabled, string Name,  string 交易平台,  string 登录帐号, string apiKey, string secretKey)
        {
            Program.db.电子币帐户.AddQSAccount(Enabled, Name, 交易平台,  登录帐号, apiKey, secretKey);
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void UpdateECoinQSAccount(string Name, string 交易平台,string 登录帐号,  string apiKey, string secretKey)
        {
            Program.db.电子币帐户.UpdateQSAccount(Name, 交易平台, 登录帐号,  apiKey, secretKey);
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void DeleteECoinQSAccount(string Name)
        {
            Program.db.电子币帐户.DeleteQSAccount(Name);
        }

        [OperationContract]
        public List<Adapter.AccounCoin> QueryAccountCoin(string 交易员, List<string> list = null)
        {
            var relation = Program.db.交易账户关联.GetRelation(交易员);
            if (relation != null && Program.db.电子币帐户.Exists(relation.电子币账户))
            {
                return Program.db.电子币帐户.QueryAccountCoin(relation.电子币账户, list);
            }
            else
            {
                return null;
            }
        }

        [OperationContract]
        public decimal QueryCoinQty(string 交易员, string Coin)
        {
            var relation = Program.db.交易账户关联.GetRelation(交易员);
            if (relation != null && Program.db.电子币帐户.Exists(relation.电子币账户))
            {
                return Program.db.电子币帐户.QueryAccountCoin(relation.电子币账户, Coin);
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 券商帐户管理
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public Server.DbDataSet.券商帐户DataTable QueryQsAccount()
        {
            return Program.db.券商帐户.QueryQsAccount();
        }
        
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void EnableQSAccount(string Name, bool Enabled)
        {
            Program.db.券商帐户.EnableQSAccount(Name, Enabled);
        }



        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void AddQSAccount(bool Enabled, string Name, string QS, string Type, string 交易服务器, string Version, short YybID, string Account, string TradeAccount, string JyPassword, string TxPassword, string SHGDDM, string SZGDDM, int 查询间隔时间)
        {
            //if (Program.db.恒生帐户.Exists(Name))
            //{
            //    throw new FaultException("帐户名称已被恒生帐户使用");
            //}


            Program.db.券商帐户.AddQSAccount( Enabled,  Name,  QS,  Type,  交易服务器,  Version,  YybID,  Account,  TradeAccount,  JyPassword,  TxPassword,SHGDDM,  SZGDDM,  查询间隔时间);
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void UpdateQSAccount(string Name, string 交易服务器, string Version, string 交易密码, string 通讯密码, int 查询间隔时间)
        {
            Program.db.券商帐户.UpdateQSAccount(Name,  交易服务器,  Version,  交易密码,  通讯密码,  查询间隔时间);
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void DeleteQSAccount(string Name)
        {
            Program.db.券商帐户.DeleteQSAccount(Name);
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public string Test(string IP, short Port, string Version, short YybID, string AccountNo, string TradeAccount, string JyPassword, string TxPassword)
        {
            if (!CommonUtils.UseOpenTdx)
            {
                return "|未执行OpenTdx, 此版本不支持Test接口，请联系管理员!";
            }
            StringBuilder Result = new StringBuilder(1024 * 1024);
            StringBuilder ErrInfo = new StringBuilder(256);

            int ClientID = -1; 
            try
            {
                //ClientID = TdxApi.Logon(IP, Port, Version, YybID, AccountNo, TradeAccount, JyPassword, TxPassword, ErrInfo);
                if (ClientID > -1)
                {
                    //TdxApi.QueryData(ClientID, 5, Result, ErrInfo);
                    //TdxApi.Logoff(ClientID);
                }
                else
                {
                    ErrInfo.AppendFormat(" 券商帐号登录失败！");
                }
            }
            catch (Exception ex)
            {
                ErrInfo.AppendFormat(" 券商帐号测试异常:{0}", ex.Message);
            }
             
            if (ClientID == -1)
            {
                return string.Format(" {0}|{1}", ClientID, ErrInfo);
            }
            return string.Format("{0}|{1}", Result, ErrInfo);

        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        public string AccountRepay(string group, decimal amount)
        {
            if (Program.db.券商帐户.Exists(group))
            {
                return Program.db.券商帐户.AccountRepay(group, amount);
            }
            else
            {
                return string.Format("融资融券账户直接还款失败，{0}:未找到该组合号信息！", group);
            }
        }
        #endregion

        #region 交易额度管理
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public Server.DbDataSet.平台用户DataTable QueryJY()
        {
            Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
            return Program.db.平台用户.QueryUserInRoles(new int[] { (int)角色.交易员 }, 平台用户Row.分组);
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public Server.DbDataSet.额度分配DataTable QueryTradeLimit()
        {
            return Program.db.额度分配.QueryTradeLimit();
        }




        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void AddTradeLimit(string 交易员, string 证券代码, string 组合号, byte 市场代码, string 证券名称, string 拼音缩写, 买模式 买模式1, 卖模式 卖模式1, decimal 交易额度, decimal 手续费率)
        {
            Program.db.额度分配.AddTradeLimit(交易员, 证券代码, 组合号, 市场代码, 证券名称, 拼音缩写, 买模式1, 卖模式1, 交易额度, 手续费率);
            
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void UpdateTradeLimit(string 交易员, string 证券代码, string 组合号, byte 市场代码, string 证券名称, string 拼音缩写, 买模式 买模式1, 卖模式 卖模式1, decimal 交易额度, decimal 手续费率)
        {
            Program.db.额度分配.UpdateTradeLimit(交易员, 证券代码, 组合号, 市场代码, 证券名称, 拼音缩写, 买模式1, 卖模式1, 交易额度, 手续费率);
           
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void DeleteTradeLimit(string 交易员, string 证券代码)
        {
            Program.db.额度分配.DeleteTradeLimit(交易员, 证券代码);
            
        }
        #endregion

        #region MAC地址管理
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public Server.DbDataSet.平台用户DataTable Query普通用户()
        {
            Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
            return Program.db.平台用户.QueryUserInRoles(new int[] { (int)角色.普通管理员, (int)角色.超级风控员, (int)角色.普通风控员, (int)角色.交易员 }, 平台用户Row.分组);
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public Server.DbDataSet.MAC地址分配DataTable QueryMACBelongUser(string UserName)
        {
            return Program.db.MAC地址分配.QueryMACBelongUser(UserName);
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void AddMAC(string UserName, string MAC)
        {
            Program.db.MAC地址分配.AddMAC( UserName,  MAC);
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        public void DeleteMAC(string UserName, string MAC)
        {
            Program.db.MAC地址分配.DeleteMAC(UserName,  MAC);
        }

        #endregion

        #region 交易员函数
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "交易员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public void FectchAllTable(string UserName)
        {


            Program.成交表Changed[UserName] = true;
            Program.委托表Changed[UserName] = true;
            Program.平台用户表Changed[UserName] = true;
            Program.额度分配表Changed[UserName] = true;
            Program.订单表Changed[UserName] = true;
            Program.已平仓订单表Changed[UserName] = true;
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "交易员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public string SendOrder(string UserName, string 证券代码, int 买卖方向, decimal 委托数量, decimal 委托价格)
        {
            try
            {
                var limits = Program.db.额度分配.GetAllLimi(UserName, 证券代码);
                if (limits == null || limits.Count() <= 0)
                {
                    return string.Format("|无此证券交易额度");
                }
                
                object sync = Program.db.平台用户.GetSendOrderSyncObj(UserName);
                lock (sync)
                {
                    DbDataSet.平台用户Row AASUser1 = Program.db.平台用户.Get平台用户(UserName);
                    if (limits.Count() > 1)
                    {
                        return SendMultyAccountOrder(UserName, 证券代码, 买卖方向, 委托数量, 委托价格, AASUser1);
                    }
                    else
                    {
                        return SendOrderMain(UserName, 证券代码, 买卖方向, 委托数量, 委托价格, AASUser1);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("服务器下单异常:{0} {1}", ex.Message, ex.StackTrace);

                return string.Format("|{0}", ex.Message);
            }
        }

        private static string SendOrderMain(string UserName, string 证券代码, int 买卖方向, decimal 委托数量, decimal 委托价格, DbDataSet.平台用户Row AASUser1)
        {
            DbDataSet.额度分配Row TradeLimit1 = Program.db.额度分配.Get额度分配(UserName, 证券代码);
            string zqName = TradeLimit1 == null ? 证券代码 : TradeLimit1.证券名称;

            decimal 已用仓位 = Program.db.已发委托.Get已用仓位(UserName);
            decimal 当日委托交易费用 = Program.db.已发委托.Get交易费用(AASUser1);

            //统计此股的已买股数 已卖股数
            List<string> lstSendedID = new List<string>();
            decimal 已买股数 = 0;
            decimal 已卖股数 = 0;
            Program.db.已发委托.Get已买卖股票(UserName, 证券代码, lstSendedID, out 已买股数, out 已卖股数);

            decimal 开仓数量 = Tool.Get开仓数量From已买卖数量(买卖方向, 委托数量, 已买股数, 已卖股数);

            if (开仓数量 > 0)
            {
                //仓位限制
                decimal 欲下仓位 = 委托价格 * 开仓数量;
                if (已用仓位 + 欲下仓位 > AASUser1.仓位限制)
                {
                    return string.Format("|仓位超限, 已用仓位{0:f2},欲下仓位{1:f2}, 超过设定值{2:f2}", 已用仓位, 欲下仓位, AASUser1.仓位限制);
                }

                //亏损限制  
                decimal 当日亏损 = Program.db.已平仓订单.Get当日已平仓亏损(UserName) + 当日委托交易费用;
                if (当日亏损 >= AASUser1.亏损限制)
                {
                    return string.Format("|用户亏损{0:f2}超过设定值{1:f2}", 当日亏损, AASUser1.亏损限制);
                }
            }

            if (TradeLimit1 == null)
            {
                return string.Format("|无此证券交易额度");
            }

            if (CommonUtils.OrderCacheQueue.Count > 0)
            {
                var cache = CommonUtils.OrderCacheQueue.Where(_ => _.Trader == UserName && _.Zqdm == 证券代码 && !_.IsReturnedResult
                    && !lstSendedID.Contains(_.OrderID) && (DateTime.Now - _.SendTime).TotalSeconds < 20).ToList();
                if (cache.Count > 0)
                {
                    var buy = cache.Where(_ => _.Category % 2 == 0).Sum(_ => _.Quantity);
                    var sale = cache.Where(_ => _.Category % 2 == 1).Sum(_ => _.Quantity);
                    已买股数 += buy;
                    已卖股数 += sale;
                }
            }

            if (买卖方向 == 0)
            {
                if (已买股数 + 委托数量 > TradeLimit1.交易额度)
                {
                    return string.Format("|买数量超限, 已买数量{0:f0}, 欲买数量{1:f0}, 超过设定值{2:f0}", 已买股数, 委托数量, TradeLimit1.交易额度);
                }
            }
            else
            {
                if (已卖股数 + 委托数量 > TradeLimit1.交易额度)
                {
                    return string.Format("|卖数量超限, 已卖数量{0:f0}, 欲卖数量{1:f0}, 超过设定值{2:f0}", 已卖股数, 委托数量, TradeLimit1.交易额度);
                }
            }

            OrderCacheEntity orderCacheObj = new OrderCacheEntity()
            {
                Category = 买卖方向,
                Zqdm = 证券代码,
                ZqName = zqName,
                Price = 委托价格,
                Quantity = 委托数量,
                Trader = UserName,
                GroupName = TradeLimit1.组合号,
                SendTime = DateTime.Now,
                Sender = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name,
                IsReturnedResult = false,
            };
            CommonUtils.OrderCacheQueue.Enqueue(orderCacheObj);
            string 委托编号;
            string ErrInfo;
            bool hasOrderNo;

            if (Program.db.券商帐户.Exists(TradeLimit1.组合号))
            {
                Program.db.券商帐户.SendOrder(TradeLimit1.组合号, TradeLimit1.Get券商帐户买卖类别(买卖方向), TradeLimit1.市场, 证券代码, 委托价格, 委托数量, orderCacheObj, out 委托编号, out ErrInfo, out hasOrderNo);
            }
            else
            {
                return "|帐户不存在";
            }

            if (ErrInfo == string.Empty)
            {
                if (hasOrderNo)
                {
                    orderCacheObj.OrderID = 委托编号;
                    orderCacheObj.IsReturnedResult = true;

                    Program.db.已发委托.Add(DateTime.Today, TradeLimit1.组合号, 委托编号, UserName, "委托成功", TradeLimit1.市场, 证券代码, zqName, 买卖方向, 0m, 0m, 委托价格, (decimal)委托数量, 0m);
                    string Msg = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name == UserName ? "下单成功" : string.Format("风控员{0}下单成功", OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
                    Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), UserName, TradeLimit1.组合号, 证券代码, zqName, 委托编号, 买卖方向, 委托数量, 委托价格, Msg);
                    if (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name != UserName)
                    {
                        风控操作 风控操作1 = new 风控操作
                        {
                            风控员 = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name,
                            交易员 = UserName,
                            证券代码 = 证券代码,
                            证券名称 = zqName,
                            委托编号 = 委托编号,
                            买卖方向 = 买卖方向,
                            委托数量 = 委托数量,
                            委托价格 = 委托价格,
                            操作内容 = Msg
                        };
                        Program.风控操作.Enqueue(风控操作1);
                    }
                }

                return string.Format("{0}|", 委托编号);

            }
            else if (string.IsNullOrEmpty(orderCacheObj.ClientGUID))
            {
                orderCacheObj.IsReturnedResult = true;
            }
            else if (ErrInfo.Contains("尝试其他交易服务器") || ErrInfo.Contains("超时"))
            {
                orderCacheObj.IsTimeOutError = "1";
            }
            return string.Format("|{0}", ErrInfo);
        }

        private static string SendMultyAccountOrder(string UserName, string 证券代码, int 买卖方向, decimal 委托数量, decimal 委托价格, DbDataSet.平台用户Row AASUser1)
        {
            var limits = Program.db.额度分配.GetAllLimi(UserName, 证券代码);
            if (limits == null ||limits.Count() > 0)
            {
                return string.Format("|无此证券交易额度");
            }
            //如何根据获取的列表进行判断下单信息？
            //先按常规走，判断剩余额度是否足够。再判断是否需要多帐号下单，
            //1.判断根据
            decimal 当日委托交易费用 = Program.db.已发委托.Get交易费用(AASUser1);
            decimal 已用仓位 = Program.db.已发委托.Get已用仓位(UserName);
            string zqName = limits.FirstOrDefault() == null ? "" : limits.FirstOrDefault().证券名称 ;

            List<string> lstSendedID = new List<string>();
            decimal 已买股数 = 0;
            decimal 已卖股数 = 0;
            
            Dictionary<string, decimal> buyDict, saleDict;
            Program.db.已发委托.GetBuySaleNum(UserName, 证券代码, lstSendedID, out buyDict, out saleDict);
            已买股数 = buyDict.Values.Sum();
            已卖股数 = saleDict.Values.Sum();

            decimal 开仓数量 = Tool.Get开仓数量From已买卖数量(买卖方向, 委托数量, 已买股数, 已卖股数);


            if (开仓数量 > 0)
            {
                //仓位限制
                decimal 欲下仓位 = 委托价格 * 开仓数量;
                if (已用仓位 + 欲下仓位 > AASUser1.仓位限制)
                {
                    return string.Format("|仓位超限, 已用仓位{0:f2},欲下仓位{1:f2}, 超过设定值{2:f2}", 已用仓位, 欲下仓位, AASUser1.仓位限制);
                }

                //亏损限制  
                decimal 当日亏损 = Program.db.已平仓订单.Get当日已平仓亏损(UserName) + 当日委托交易费用;
                if (当日亏损 >= AASUser1.亏损限制)
                {
                    return string.Format("|用户亏损{0:f2}超过设定值{1:f2}", 当日亏损, AASUser1.亏损限制);
                }
            }

            //已买股数应该做成字典
            if (CommonUtils.OrderCacheQueue.Count > 0)
            {
                var cache = CommonUtils.OrderCacheQueue.Where(_ => _.Trader == UserName && _.Zqdm == 证券代码 && !_.IsReturnedResult
                    && !lstSendedID.Contains(_.OrderID) && (DateTime.Now - _.SendTime).TotalSeconds < 20).ToList();
                if (cache.Count > 0)
                {
                    foreach (var item in cache)
                    {
                        if (item.Category % 2 == 0)
                        {
                            已买股数 += item.Quantity;
                            if (buyDict.ContainsKey(item.GroupName))
                                buyDict[item.GroupName] += item.Quantity;
                            else
                                buyDict.Add(item.GroupName, item.Quantity);
                        }
                        else
                        {
                            已卖股数 += item.Quantity;
                            if (saleDict.ContainsKey(item.GroupName))
                                saleDict[item.GroupName] += item.Quantity;
                            else
                                saleDict.Add(item.GroupName, item.Quantity);
                        }
                    }
                }
            }

            var totalLimit = limits.Sum(_=> _.交易额度);
            if (买卖方向 == 0)
            {
                if (已买股数 + 委托数量 > totalLimit)
                {
                    return string.Format("|买数量超限, 已买数量{0:f0}, 欲买数量{1:f0}, 超过设定值{2:f0}", 已买股数, 委托数量, totalLimit);
                }
            }
            else
            {
                if (已卖股数 + 委托数量 > totalLimit)
                {
                    return string.Format("|卖数量超限, 已卖数量{0:f0}, 欲卖数量{1:f0}, 超过设定值{2:f0}", 已卖股数, 委托数量, totalLimit);
                }
            }
            
            //按额度排序，可以直接按剩余下单额度下单则直接下单病返回，没有的话循环所有额度下单，直至下单。
            Dictionary<string, decimal> limitLeft = new Dictionary<string, decimal>();
            if (买卖方向 == 0)
            {
                foreach (var item in limits)
                {
                    limitLeft.Add(item.组合号, item.交易额度 - (buyDict.ContainsKey(item.组合号) ? buyDict[item.组合号] : 0));
                }
            }
            else
            {
                foreach (var item in limits)
                {
                    limitLeft.Add(item.组合号, item.交易额度 - (saleDict.ContainsKey(item.组合号) ? saleDict[item.组合号] : 0));
                }
            }

            string resultLast = string.Empty;
            decimal wtQtyLeft = 委托数量;
            var orderedLimit = limitLeft.OrderByDescending(_ => _.Value);
            foreach (var item in orderedLimit)
            {
                var TradeLimitItem = limits.FirstOrDefault(_=> _.组合号 == item.Key);

                if (item.Value >= wtQtyLeft)
                {
                    resultLast = SendGroupOrder(UserName, 证券代码, 买卖方向, wtQtyLeft, 委托价格, zqName, TradeLimitItem);
                    break;
                }
                else 
                {
                    resultLast = SendGroupOrder(UserName, 证券代码, 买卖方向, item.Value, 委托价格, zqName, TradeLimitItem);
                    wtQtyLeft -= item.Value;
                }
            }

            return resultLast;
        }

        private static string SendGroupOrder(string UserName, string 证券代码, int 买卖方向, decimal 委托数量, decimal 委托价格, string zqName, Server.DbDataSet.额度分配Row TradeLimit1)
        {
            OrderCacheEntity orderCacheObj = new OrderCacheEntity()
            {
                Category = 买卖方向,
                Zqdm = 证券代码,
                ZqName = zqName,
                Price = 委托价格,
                Quantity = 委托数量,
                Trader = UserName,
                GroupName = TradeLimit1.组合号,
                SendTime = DateTime.Now,
                Sender = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name,
                IsReturnedResult = false,
            };
            CommonUtils.OrderCacheQueue.Enqueue(orderCacheObj);
            string 委托编号;
            string ErrInfo;
            bool hasOrderNo = true;

            if (Program.db.券商帐户.Exists(TradeLimit1.组合号))
            {
                Program.db.券商帐户.SendOrder(TradeLimit1.组合号, TradeLimit1.Get券商帐户买卖类别(买卖方向), TradeLimit1.市场, 证券代码, 委托价格, 委托数量, orderCacheObj, out 委托编号, out ErrInfo, out hasOrderNo);
            }
            else
            {
                return "|帐户不存在";
            }

            if (ErrInfo == string.Empty)
            {
                if (hasOrderNo)
                {
                    orderCacheObj.OrderID = 委托编号;
                    orderCacheObj.IsReturnedResult = true;

                    Program.db.已发委托.Add(DateTime.Today, TradeLimit1.组合号, 委托编号, UserName, "委托成功", TradeLimit1.市场, 证券代码, zqName, 买卖方向, 0m, 0m, 委托价格, (decimal)委托数量, 0m);
                    string Msg = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name == UserName ? "下单成功" : string.Format("风控员{0}下单成功", OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
                    Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), UserName, TradeLimit1.组合号, 证券代码, zqName, 委托编号, 买卖方向, 委托数量, 委托价格, Msg);

                    if (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name != UserName)
                    {
                        风控操作 风控操作1 = new 风控操作();
                        风控操作1.风控员 = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                        风控操作1.交易员 = UserName;
                        风控操作1.证券代码 = 证券代码;
                        风控操作1.证券名称 = zqName;
                        风控操作1.委托编号 = 委托编号;
                        风控操作1.买卖方向 = 买卖方向;
                        风控操作1.委托数量 = 委托数量;
                        风控操作1.委托价格 = 委托价格;
                        风控操作1.操作内容 = Msg;
                        Program.风控操作.Enqueue(风控操作1);
                    }
                }

                return string.Format("{0}|", 委托编号);

            }
            else if (string.IsNullOrEmpty(orderCacheObj.ClientGUID))
            {
                orderCacheObj.IsReturnedResult = true;
            }
            return string.Format("|{0}", ErrInfo);
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "交易员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public string SendECoinOrder(string UserName, string 证券代码, int 买卖方向, decimal 委托数量, decimal 委托价格)
        {
            var relation = Program.db.交易账户关联.GetRelation(UserName);
            string accountName = string.Empty;
            if (relation != null)
            {
                accountName = relation.电子币账户;
            }
            else
            {
                var limit = Program.db.额度分配.Get额度分配(UserName, 证券代码);
                accountName = limit == null ? "" : limit.组合号;
            }

            string OrderID, ErrInfo, logMsg, returnMsg;

            object userSync = Program.db.平台用户.GetSendOrderSyncObj(UserName);
            lock (userSync)
            {
                #region 仓位验证
                var dtNotFinished = Program.db.已发委托.GetNotFinished已发委托(DateTime.Today, accountName, UserName, 买卖方向);
                var kv = CommonUtils.GetBinanceCoinInfo(证券代码);
                if (买卖方向 == 0)
                {
                    var countCoinBasic = Program.db.可用资金.Get可用资金(UserName, kv.Value); //买入则验证资金币种是否足够
                    var basicCoinOccupied = dtNotFinished == null ? 0 : dtNotFinished.Where(_ => _.证券代码.EndsWith(kv.Value)).Sum(_ => _.委托价格 * (_.委托数量 - _.成交数量 - _.撤单数量));
                    if ((countCoinBasic - basicCoinOccupied) < 委托数量 * 委托价格)
                    {
                        return string.Format("|币种{0}现有数量{0}, 尚未成交委托占用数量{1}, 欲使用{2}*{3}={4} 超出限制，仓位验证失败!", kv.Value, basicCoinOccupied, 委托价格, 委托数量, 委托价格 * 委托数量);
                    }
                }
                else
                {
                    var countCoinTrade = Program.db.可用资金.Get可用资金(UserName, kv.Key);//卖出则验证本币种是否足够
                    var coinTradeOccupied = dtNotFinished == null ? 0 : dtNotFinished.Where(_ => _.证券代码.EndsWith(kv.Key)).Sum(_ => _.委托数量);
                    if ((countCoinTrade - coinTradeOccupied) < 委托数量)
                    {
                        return string.Format("|币种{0}现有数量{0}, 未成交卖单占用数{1}, 欲卖出{2} 超出限制，仓位验证失败!", kv.Key, coinTradeOccupied, 委托数量);
                    }
                }
                #endregion
                
                string sender = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                OrderCacheEntity orderCacheObj = AddCache(UserName, 证券代码, 买卖方向, 委托数量, 委托价格, accountName, sender);

                #region 下单账户验证
                if (Program.db.电子币帐户.Exists(accountName))
                {
                    Program.db.电子币帐户.SendOrder(accountName, 买卖方向, 证券代码, 委托价格, 委托数量, UserName, out OrderID, out ErrInfo);
                }
                else
                {
                    return "|帐户不存在";
                }
                #endregion

                if (ErrInfo == string.Empty)
                {
                    orderCacheObj.OrderID = OrderID;
                    orderCacheObj.IsReturnedResult = true;

                    Program.db.已发委托.Add(DateTime.Today, accountName, OrderID, UserName, "委托成功", 0, 证券代码, 证券代码, 买卖方向, 0m, 0m, 委托价格, (decimal)委托数量, 0m);
                    logMsg = sender == UserName ? "下单成功" : string.Format("风控员{0}下单成功", sender);
                    if (sender != UserName)
                    {
                        风控操作 风控操作1 = new 风控操作();
                        风控操作1.风控员 = sender;
                        风控操作1.交易员 = UserName;
                        风控操作1.证券代码 = 证券代码;
                        风控操作1.证券名称 = 证券代码;
                        风控操作1.委托编号 = OrderID;
                        风控操作1.买卖方向 = 买卖方向;
                        风控操作1.委托数量 = 委托数量;
                        风控操作1.委托价格 = 委托价格;
                        风控操作1.操作内容 = logMsg;
                        Program.风控操作.Enqueue(风控操作1);
                    }

                    returnMsg = string.Format("{0}|", OrderID);

                }
                else
                {
                    logMsg = ErrInfo;
                    returnMsg = string.Format("|{0}", ErrInfo);
                }
                Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), UserName, accountName, 证券代码, "", OrderID, 买卖方向, 委托数量, 委托价格, logMsg);
            }
            
            return returnMsg;
        }

        private static OrderCacheEntity AddCache(string UserName, string 证券代码, int 买卖方向, decimal 委托数量, decimal 委托价格, string 组合号, string sender)
        {
            OrderCacheEntity orderCacheObj = new OrderCacheEntity()
            {
                Category = 买卖方向,
                Zqdm = 证券代码,
                Price = 委托价格,
                Quantity = 委托数量,
                Trader = UserName,
                GroupName = 组合号,
                SendTime = DateTime.Now,
                Sender = sender,
            };
            CommonUtils.OrderCacheQueue.Enqueue(orderCacheObj);
            return orderCacheObj;
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "交易员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public string CancelOrder(string UserName, string 组合号, string 证券代码, string 证券名称, byte 市场代码, string 委托编号, int 买卖方向, decimal 委托数量, decimal 委托价格)
        {
            try
            {
                string Result;
                string ErrInfo;
                if (Program.db.电子币帐户.Exists(组合号))
                {
                    Program.db.电子币帐户.CancelOrder(证券代码, 组合号, 委托编号, out Result, out ErrInfo);
                }
                else if (Program.db.券商帐户.Exists(组合号))
                {
                    Program.db.券商帐户.CancelOrder(证券代码, 组合号, 市场代码, 委托编号, out Result, out ErrInfo);
                }
                else
                {
                    return "|帐户不存在";
                }
                
                if (ErrInfo == string.Empty)
                {
                    string Msg = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name == UserName ? "撤单成功" : string.Format("风控员{0}撤单成功", OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);
                    Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), UserName, 组合号, 证券代码, 证券名称, 委托编号, 买卖方向, 委托数量, 委托价格, Msg);
                    
                    if (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name != UserName)
                    {
                        风控操作 风控操作1 = new 风控操作();
                        风控操作1.风控员 = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                        风控操作1.交易员 = UserName;
                        风控操作1.证券代码 = 证券代码;
                        风控操作1.证券名称 = 证券名称;
                        风控操作1.委托编号 = 委托编号;
                        风控操作1.买卖方向 = 买卖方向;
                        风控操作1.委托数量 = 委托数量;
                        风控操作1.委托价格 = 委托价格;
                        风控操作1.操作内容 = Msg;
                        Program.风控操作.Enqueue(风控操作1);
                    }

                    return "撤单完成|";
                }
                else
                {
                    return string.Format("|{0}", ErrInfo);

                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("服务器撤单异常:{0} {1}", ex.Message, ex.StackTrace);

                return string.Format("|{0}", ex.Message);
            }

        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "交易员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public string CancelECoinOrder(string UserName, string 组合号, string 证券代码, string 委托编号, int 买卖方向, decimal 委托数量, decimal 委托价格)
        {
            try
            {
                string Result, ErrInfo, logMsg, returnMsg;
                if (Program.db.电子币帐户.Exists(组合号))
                {
                    Program.db.电子币帐户.CancelOrder(证券代码, 组合号, 委托编号, out Result, out ErrInfo);
                }
                else
                {
                    return "|帐户不存在";
                }

                if (ErrInfo == string.Empty)
                {
                    string operatorName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

                    logMsg = operatorName == UserName ? "撤单成功" : string.Format("风控员{0}撤单成功", operatorName);
                    returnMsg = "撤单完成|";

                    if (operatorName != UserName)
                    {
                        风控操作 风控操作1 = new 风控操作();
                        风控操作1.风控员 = operatorName;
                        风控操作1.交易员 = UserName;
                        风控操作1.证券代码 = 证券代码;
                        风控操作1.证券名称 = 证券代码;
                        风控操作1.委托编号 = 委托编号;
                        风控操作1.买卖方向 = 买卖方向;
                        风控操作1.委托数量 = 委托数量;
                        风控操作1.委托价格 = 委托价格;
                        风控操作1.操作内容 = logMsg;
                        Program.风控操作.Enqueue(风控操作1);
                    }
                }
                else
                {
                    logMsg = ErrInfo;
                    returnMsg = string.Format("|{0}", ErrInfo);
                }

                Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), UserName, 组合号, 证券代码, "", 委托编号, 0, 0, 0, logMsg);
                return returnMsg;
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("服务器撤单异常:{0} {1}", ex.Message, ex.StackTrace);
                return string.Format("|{0}", ex.Message);
            }
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "交易员")]
        public void RefreshOrderTable(string userName)
        {
            var orderTable = Program.db.订单.Query订单BelongJy(userName);
            Tool.Send订单TableChangedNoticeToTrader(userName, orderTable);
        }
        #endregion

        #region 风控函数
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public void Fectch名下交易员Table()
        {
            string FKUserName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;


            Program.风控分配表Changed[FKUserName] = true;
        }

        
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public void CloseOrder(string 交易员, string 组合号, string 证券代码, decimal 平仓数量, decimal 平仓价格)
        {
            Server.DbDataSet.平台用户Row 平台用户Row = Program.db.平台用户.Get平台用户(OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name);



            if (!平台用户Row.允许删除碎股订单)
            {
                throw new FaultException("无权限平仓碎股");
            }




            Server.DbDataSet.订单Row 订单Row1 = Program.db.订单.Get订单(交易员, 组合号, 证券代码);
            if (订单Row1 == null)
            {
                return;
            }

            //AASServer.DbDataSet.额度分配Row TradeLimit1 = Program.db.额度分配.Get额度分配(交易员, 证券代码);
            //if (TradeLimit1 == null)
            //{
            //    throw new FaultException("无此证券交易额度");
            //}


            string 委托编号 = Guid.NewGuid().ToString();






            Program.db.已发委托.Add(DateTime.Today, 组合号, 委托编号, 交易员, "风控平仓", 订单Row1.市场代码, 证券代码, 订单Row1.证券名称, 订单Row1.平仓类别, 平仓价格, 平仓数量, 平仓价格, 平仓数量, 0m);


            Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), 交易员, 组合号, 证券代码, 订单Row1.证券名称, 委托编号, 订单Row1.平仓类别, 平仓数量, 平仓价格, "风控平仓");





            Server.JyDataSet.成交DataTable 成交DataTable1 = new JyDataSet.成交DataTable();

            JyDataSet.成交Row 成交Row1 = 成交DataTable1.New成交Row();
            成交Row1.交易员 = 交易员;
            成交Row1.组合号 = 组合号;
            成交Row1.成交时间 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            成交Row1.证券代码 = 证券代码;
            成交Row1.证券名称 = 订单Row1.证券名称;
            成交Row1.成交价格 = 平仓价格;
            成交Row1.成交数量 = 平仓数量;
            成交Row1.成交金额 = 平仓价格 * 平仓数量;
            成交Row1.成交编号 = 委托编号;
            成交Row1.委托编号 = 委托编号;
            成交Row1.买卖方向 = 订单Row1.平仓类别;
            成交Row1.市场代码 = 订单Row1.市场代码;



 
            Program.db.订单.Update(成交Row1);


            //Program.db.已发委托.Update(DateTime.Today, 委托Row1.组合号, 委托Row1.委托编号, New成交价格, New成交数量);

            Program.db.交易日志.Add(成交Row1);

            Program.成交通知.Enqueue(成交Row1);         
        }


        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public Server.DbDataSet.交易日志DataTable QueryJyLogBelongFK(string UserName)
        {
            Server.DbDataSet.平台用户DataTable 名下交易员DataTable = Program.db.QueryJyBelongFK(UserName);
            List<string> JyList = new List<string>();
            foreach (Server.DbDataSet.平台用户Row 平台用户Row1 in 名下交易员DataTable)
            {
                JyList.Add(平台用户Row1.用户名);
            }

            return Program.db.交易日志.QueryTodayJyLog(JyList);

        }



        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级风控员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通风控员")]
        public Server.JyDataSet.业绩统计DataTable Query业绩BelongFK(string UserName, DateTime StartDate, DateTime EndDate)
        {
            Server.JyDataSet.业绩统计DataTable 业绩统计DataTable1 = new JyDataSet.业绩统计DataTable();
            StringBuilder sb = new StringBuilder();
            try
            {
                Server.DbDataSet.平台用户DataTable 名下交易员DataTable = Program.db.QueryJyBelongFK(UserName);
                List<string> JyList = new List<string>();
                foreach (Server.DbDataSet.平台用户Row 平台用户Row1 in 名下交易员DataTable)
                {
                    JyList.Add(平台用户Row1.用户名);
                }

                sb.AppendLine("1.名下交易员统计成功！");

                Server.DbDataSet.已发委托DataTable 已发委托DataTable1 = new DbDataSet.已发委托DataTable();
                Server.DbDataSet.已平仓订单DataTable 已平仓订单DataTable1 = new DbDataSet.已平仓订单DataTable();

                已平仓订单DataTable1.Load(JyList, StartDate, EndDate);
                已发委托DataTable1.Load(JyList, StartDate, EndDate);

                sb.AppendLine("2.已平仓订单及已发委托，统计成功！");




                foreach (Server.DbDataSet.已发委托Row 已发委托Row1 in 已发委托DataTable1.Where(r => r.成交数量 != 0))
                {
                    decimal 交易费用 = 0;
                    Server.DbDataSet.额度分配Row AASPermition = Program.db.额度分配.Get额度分配(已发委托Row1.交易员, 已发委托Row1.证券代码);

                    var group = ShareLimitAdapter.Instance.GetLimitGroup(已发委托Row1.交易员);
                    if (group != null)
                    {
                        var stockLimitItem = group.GroupStockList.FirstOrDefault(_ => _.GroupAccount == 已发委托Row1.组合号 && _.StockID == 已发委托Row1.证券代码);
                        if (stockLimitItem != null)
                        {
                            交易费用 += ShareLimitAdapter.Instance.Get交易费用(已发委托Row1, decimal.Parse(stockLimitItem.Commission));
                        }
                        else
                        {
                            Program.logger.LogInfo("业绩统计逻辑异常，额度共享逻辑未找到组合号{0}下证券代码{1}对应的配置项！");
                        }
                    }
                    else
                    {
                        if (AASPermition == null)
                        {
                            Server.DbDataSet.平台用户Row AASUser1 = Program.db.平台用户.Get平台用户(已发委托Row1.交易员);
                            交易费用 = 已发委托Row1.Get交易费用(AASUser1.手续费率);
                        }
                        else
                        {
                            交易费用 = 已发委托Row1.Get交易费用(AASPermition.手续费率);
                        }
                    }
                    

                    if (!业绩统计DataTable1.Any(r => r.交易员 == 已发委托Row1.交易员 && r.组合号 == 已发委托Row1.组合号 && r.证券代码 == 已发委托Row1.证券代码))
                    {
                        #region 生成业绩统计Row
                        Server.JyDataSet.业绩统计Row 业绩统计RowNew = 业绩统计DataTable1.New业绩统计Row();
                        业绩统计RowNew.交易员 = 已发委托Row1.交易员;
                        业绩统计RowNew.组合号 = 已发委托Row1.组合号;
                        业绩统计RowNew.证券代码 = 已发委托Row1.证券代码;
                        业绩统计RowNew.证券名称 = 已发委托Row1.证券名称;



                        if (已发委托Row1.买卖方向 == 0)
                        {
                            #region 买单
                            业绩统计RowNew.买入数量 = 已发委托Row1.成交数量;
                            业绩统计RowNew.买入金额 = 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                            业绩统计RowNew.买入均价 = 已发委托Row1.成交价格;


                            业绩统计RowNew.卖出数量 = 0;
                            业绩统计RowNew.卖出金额 = 0;
                            业绩统计RowNew.卖出均价 = 0;

                            #endregion

                        }
                        else
                        {
                            #region 卖单
                            业绩统计RowNew.买入数量 = 0;
                            业绩统计RowNew.买入金额 = 0;
                            业绩统计RowNew.买入均价 = 0;

                            业绩统计RowNew.卖出数量 = 已发委托Row1.成交数量;
                            业绩统计RowNew.卖出金额 = 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                            业绩统计RowNew.卖出均价 = 已发委托Row1.成交价格;


                            #endregion
                        }


                        业绩统计RowNew.毛利 = 0;
                        业绩统计RowNew.交易费用 = 交易费用;
                        业绩统计RowNew.净利润 = 业绩统计RowNew.毛利 - 业绩统计RowNew.交易费用;
                        业绩统计DataTable1.Add业绩统计Row(业绩统计RowNew);

                        #endregion
                    }
                    else
                    {
                        #region 修改业绩统计Row
                        Server.JyDataSet.业绩统计Row 业绩统计Row1 = 业绩统计DataTable1.First(r => r.交易员 == 已发委托Row1.交易员 && r.组合号 == 已发委托Row1.组合号 && r.证券代码 == 已发委托Row1.证券代码);


                        if (已发委托Row1.买卖方向 == 0)
                        {
                            #region 买单
                            业绩统计Row1.买入数量 += 已发委托Row1.成交数量;
                            业绩统计Row1.买入金额 += 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                            业绩统计Row1.买入均价 = Math.Round(业绩统计Row1.买入金额 / 业绩统计Row1.买入数量, 3, MidpointRounding.AwayFromZero);
                            #endregion
                        }
                        else
                        {
                            #region 卖单
                            业绩统计Row1.卖出数量 += 已发委托Row1.成交数量;
                            业绩统计Row1.卖出金额 += 已发委托Row1.成交价格 * 已发委托Row1.成交数量;
                            业绩统计Row1.卖出均价 = Math.Round(业绩统计Row1.卖出金额 / 业绩统计Row1.卖出数量, 3, MidpointRounding.AwayFromZero);
                            #endregion
                        }



                        业绩统计Row1.毛利 += 0;
                        业绩统计Row1.交易费用 += 交易费用;
                        业绩统计Row1.净利润 = 业绩统计Row1.毛利 - 业绩统计Row1.交易费用;
                        #endregion
                    }
                }

                sb.AppendLine("3.业绩统计生成或修改，成功！");

                foreach (Server.JyDataSet.业绩统计Row 业绩统计Row1 in 业绩统计DataTable1)
                {
                    if (已平仓订单DataTable1.Any(r => r.交易员 == 业绩统计Row1.交易员 && r.组合号 == 业绩统计Row1.组合号 && r.证券代码 == 业绩统计Row1.证券代码))
                    {
                        业绩统计Row1.毛利 = 已平仓订单DataTable1.Where(r => r.交易员 == 业绩统计Row1.交易员 && r.组合号 == 业绩统计Row1.组合号 && r.证券代码 == 业绩统计Row1.证券代码).Sum(r => r.毛利);
                        业绩统计Row1.净利润 = 业绩统计Row1.毛利 - 业绩统计Row1.交易费用;
                    }
                }
                sb.AppendLine("4.毛利及净利润计算完毕！");
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo(sb.ToString() + System.Environment.NewLine + ex.Message);
            }


            return 业绩统计DataTable1;

        }
        #endregion
        
        #region 额度共享
        [OperationContract]
        public string ShareLimitDocumentQuery()
        {
            try
            {
                return string.Format("0|{0}", ShareLimitAdapter.Instance.Document.ToString());
            }
            catch (Exception ex)
            {
                return string.Format("1|{0}", ex.Message);
            }
        }

        [OperationContract]
        public List<ShareLimitGroupItem> ShareGroupQuery()
        {
            try
            {
                return ShareLimitAdapter.Instance.ShareLimitGroups;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public string AddTrader(string group, string trader)
        {
            string errMsg = null;
            try
            {
                bool isSuccess = ShareLimitAdapter.Instance.AddTrader(trader, group, out errMsg);
                if (isSuccess)
                {
                    return string.Format("0|");
                }
                else
                {
                    return string.Format("1|{0}", errMsg);
                }
            }
            catch (Exception ex)
            {
                return "1|" + ex.Message;
            }
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public bool RemoveGroupTrader(string group, string trader)
        {
            try
            {
                return ShareLimitAdapter.Instance.RemoveTrader(group, trader);

            }
            catch (Exception)
            {
                return false;
            }
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public string AddStock(string group, StockLimitItem limitItem)
        {
            string errMsg = null;
            try
            {
                bool isSuccess = ShareLimitAdapter.Instance.AddGroupStocks(group, limitItem, out errMsg);
                if (isSuccess)
                    return "0|添加成功";
            }
            catch (Exception ex)
            {
                errMsg = "1|" + ex.Message;
            }
            return errMsg;
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public string UpdateStock(string group, StockLimitItem stockLimit)
        {
            return string.Format("0|");
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public string RemoveStock(string group, string stockID)
        {
            string errMsg = null;
            try
            {
                if (ShareLimitAdapter.Instance.RemoveStock(group, stockID, out errMsg))
                    return "0|删除成功";
            }
            catch (Exception ex)
            {
                errMsg = "1|" + ex.Message;
            }
            return errMsg;
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public List<LimitTrader> QueryNotGroupedTrader()
        {
            List<LimitTrader> tradersNotGrouped = null;
            try
            {
                tradersNotGrouped = new List<LimitTrader>();
                List<LimitTrader> traders = new List<LimitTrader>();
                ShareLimitAdapter.Instance.ShareLimitGroups.ForEach(_ => traders.AddRange(_.GroupTraderList));
                var groupedTrader = traders.Select(_ => _.TraderAccount).ToList();
                Program.db.平台用户.Where(_ => _.角色 == 4 && !groupedTrader.Contains(_.用户名)).ToList().ForEach(_ => tradersNotGrouped.Add(new LimitTrader() { TraderAccount = _.用户名 }));
                return tradersNotGrouped;
            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("AASServer.NotGroupedTraderQuery Exception:" + ex.Message);
            }
            return tradersNotGrouped;
        }
        #endregion

        #region 可用资金
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void Add可用资金(string userName,string coinType,decimal limit)
        {
            Program.db.可用资金.Add(userName, coinType, limit);
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void Delete可用资金(string userName, string coinType)
        {
            if (Program.db.可用资金.Exists(userName, coinType))
            {
                Program.db.可用资金.Delete(userName, coinType);
            }
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public void Update可用资金(string userName, string coinType, decimal limit)
        {
            if (Program.db.可用资金.Exists(userName, coinType))
            {
                Program.db.可用资金.Update(userName, coinType, limit);
            }
        }

        [OperationContract]
        public DbDataSet.可用资金DataTable Query可用资金(string userName, string coinType)
        {
            return Program.db.可用资金.Query可用资金(userName, coinType);
        }

        [OperationContract]
        public DbDataSet.可用资金DataTable QueryAll可用资金()
        {
            return Program.db.可用资金.QueryAll可用资金();
        }
        #endregion

        #region 交易账户关联
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public string AddAccountRelation(string trader, string account)
        {
            if (Program.db.交易账户关联.Exists(trader))
            {
                return "0|已存在该交易员关联账户";
            }
            else
            {
                Program.db.交易账户关联.AddRelation(trader, account);
                return "1|";
            }
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public string DeleteAccountRelation(string trader)
        {
            if (Program.db.交易账户关联.Exists(trader))
            {
                Program.db.交易账户关联.DeleteRelation(trader);
                return "1|";
            }
            else 
            {
                return "0|不存在该交易员的交易账户配置";
            }
        }

        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Role = "超级管理员")]
        [PrincipalPermission(SecurityAction.Demand, Role = "普通管理员")]
        public string UpdateAccountRelation(string trader, string account)
        {

            if (Program.db.交易账户关联.Exists(trader))
            {
                Program.db.交易账户关联.UpdateRelation(trader, account);
                return "1|";
            }
            else
            {
                return "0|不存在该交易员的交易账户配置";
            }
            
        }

        [OperationContract]
        public DbDataSet.交易账户关联DataTable QueryAccountRelation()
        {
            return Program.db.交易账户关联.QeuryAccountRelation();
        }
        
        #endregion


    }



    public interface IClient
    {
        [OperationContract(IsOneWay = true)]
        void Notify(string 操作员, string 证券代码, string 证券名称, string 委托编号, int 买卖方向, decimal 委托数量, decimal 委托价格, string 信息);
        
        [OperationContract(IsOneWay = true)]
        void 风控分配DataTableChanged(Server.DbDataSet.平台用户DataTable 名下交易员DataTable);
        
        [OperationContract(IsOneWay = true)]
        void 成交DataTableChanged(string UserName, Server.JyDataSet.成交DataTable TableChanged);

        [OperationContract(IsOneWay = true)]
        void 委托DataTableChanged(string UserName, Server.JyDataSet.委托DataTable TableChanged);

        [OperationContract(IsOneWay = true)]
        void 平台用户DataTableChanged(string UserName, Server.DbDataSet.平台用户DataTable TableChanged);

        [OperationContract(IsOneWay = true)]
        void 额度分配DataTableChanged(string UserName, Server.DbDataSet.额度分配DataTable TableChanged);

        [OperationContract(IsOneWay = true)]
        void 订单DataTableChanged(string UserName, Server.DbDataSet.订单DataTable TableChanged);

        [OperationContract(IsOneWay = true)]
        void 已平仓订单DataTableChanged(string UserName, Server.DbDataSet.已平仓订单DataTable TableChanged);
        
        [OperationContract(IsOneWay = true)]
        void Close();//当帐号被管理员删除是调用
    }
}
