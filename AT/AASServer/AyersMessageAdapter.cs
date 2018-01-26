using AASServer.AyersEntity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AASServer
{
    public class AyersMessageAdapter
    {
        #region Members
        public const string GroupName = "Ayers";
        private const string configPath = "/config/config.xml";
        private static object sync = new object();
        private Thread AliveThread;
        private Thread ConnectThread;
        #endregion

        #region Properties

        private static AyersMessageAdapter _instance;
        public static AyersMessageAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new AyersMessageAdapter();
                        }
                    }
                }
                return _instance;
            }
        }

 
        public AyersAccount Account { get; set; }

        public bool IsConnected { get; private set; }

        private bool IsLogon { get; set; }

        private BlowFish BlowFishInstance { get; set; }
        #endregion

        private AyersMessageAdapter()
        {
        }

        public void Start()
        {
            var appConfig = Program.appConfig.GetValue("UseAyersInterface", "");
            if (appConfig != "1") return;
            if (ConnectThread == null)
            {
                ConnectThread = new Thread(new ThreadStart(connectMain)) { IsBackground = true };
                ConnectThread.Start();
            }
        }

        private void connectMain()
        {
            int retrySecond = 60000;
            while (true)
            {
                if (Account == null)
                {
                    if (System.IO.File.Exists(System.Windows.Forms.Application.StartupPath + configPath))
                        InitAccount();
                    else
                        Program.logger.LogInfo("Ayers配置项不存在，将不启用接口！");
                }

                if (!IsConnected)
                {
                    if (TcpConnector.Instance.ClientConnect(Account.Server_Ip, Account.PortUsing))
                    {
                        TcpConnector.Instance.MessageReceive += new Action<string>(ReceiveMessage);
                        IsConnected = true;
                    }
                    else
                    {
                        Program.logger.LogInfo("Ayers服务器连接失败，配置信息：IP {0},Port {1}，将于{2}秒后重新尝试！", Account.Server_Ip, Account.PortUsing, retrySecond/1000);
                    }
                }

                if (IsConnected && !IsLogon)
                {
                    Logon();
                }
                Thread.Sleep(retrySecond);
            }
        }

        public void InitAccount()
        {
            string logConfig = File.ReadAllText(System.Windows.Forms.Application.StartupPath + configPath);
            var doc = XDocument.Parse(logConfig);
            this.Account = new AyersAccount();
            Account.Server_Ip = doc.Root.Element("server_ip").Value;
            Account.Port_No_Encryption = int.Parse(doc.Root.Element("port_no_encryption").Value);
            Account.Port_Encryption = int.Parse(doc.Root.Element("port_encryption").Value);
            Account.Encryption_Key = doc.Root.Element("encryption_key").Value;
            Account.Message_Compression = doc.Root.Element("message_compression").Value;
            Account.Api_Login_ID = doc.Root.Element("api_login_id").Value;
            Account.Api_Login_Psw = doc.Root.Element("api_login_psw").Value;
            Account.Site_ID = doc.Root.Element("site").Value;
            Account.Station_ID = doc.Root.Element("station").Value;
            Account.Type = doc.Root.Element("type").Value;
            Account.Client_ID_First = doc.Root.Element("client_first").Element("id").Value;
            Account.Client_Psw_First = doc.Root.Element("client_first").Element("password").Value;
            Account.Client_ID_Second = doc.Root.Element("client_second").Element("id").Value;
            Account.Client_Psw_Second = doc.Root.Element("client_second").Element("password").Value;
            Account.Client_ID_Using = doc.Root.Element("client_using").Value;
            if (doc.Root.Element("port_using") != null)
            {
                Account.PortUsing = int.Parse(doc.Root.Element("port_using").Value);
            }
            Program.logger.LogInfoDetail("AyersMessageAdapter.InitAccount Completed.");
        }

        public void Stop()
        {
            lock (sync)
            {
                try
                {
                    string errMsg;
                    Logout(out errMsg);
                }
                catch (Exception) { }

                if (this.IsLogon)
                {
                    AliveThread.Abort();
                    AliveThread = null;
                    IsConnected = false;
                }
                if (this.IsConnected)
                {
                    ConnectThread.Abort();
                    ConnectThread = null;
                    this.IsConnected = false;
                }
            }
        }

        #region Message Send

        static DateTime sendLoginTime = DateTime.MinValue;
        public void Logon()
        {
            lock (sync)
            {
                if (!IsLogon && sendLoginTime == DateTime.MinValue || (DateTime.Now - sendLoginTime).TotalSeconds > 15)
                {
                    sendLoginTime = DateTime.Now;
                    XDocument xDoc = new XDocument(
                           new XElement("message",
                               new XAttribute("type", "login"),
                               new XAttribute("msgnum", AyersConfig.MsgNum++),
                               new XAttribute("language", "gb"),

                               new XElement("site", Account.Site_ID),
                               new XElement("station", Account.Station_ID),
                               new XElement("type", Account.Type),
                               new XElement("user", Account.Api_Login_ID),
                               new XElement("password", Account.Api_Login_Psw)
                           )
                       );
                    string errMsg;

                    
                    if (SendMessage(xDoc, out errMsg))
                    {
                        Program.logger.LogInfo("Ayers登录请求发送成功, 用户名：{0}, Message {1}", Account.Api_Login_ID, xDoc.ToString());
                    }
                    else
                    {
                        Program.logger.LogInfo("Ayers登录请求发送失败,  ErrorMessage：{0}",errMsg);

                    }
                }
            }

            
        }

        public bool Logout(out string errMsg)
        {
            if (IsConnected)
            {
                XDocument xDoc = new XDocument(
                    new XElement("message",
                        new XAttribute("type", "logout"),
                        new XAttribute("msgnum", AyersConfig.MsgNum++),

                        new XElement("site", Account.Site_ID),
                        new XElement("type", Account.Type)
                    )
                );
                return SendMessage(xDoc, out errMsg);
            }
            else
            {
                errMsg = "Ayers 接口未连接!";
                return false;
            }
            //<message type=”logout” msgnum=”5486”> <site>BTST</site> <type>DEFAULT</type> <user>BTST</user> <session_id>q34342342d</session_id> </message> 
        }

        #region Order Add/Update/Cancel
        public bool AddOrder(bool side, string stockCode, decimal price, int qty, OrderCacheEntity orderCache, out string ErrMsg, string order_type = "I", string exchange_code = "HKEX")
        {
            if (!AyersMessageAdapter.Instance.IsConnected)
            {
                //AyersMessageAdapter.Instance.Start();
                ErrMsg = "Ayers服务器未能连接！";
                return false;
            }
            else if (!AyersMessageAdapter.Instance.IsLogon)
            {
                ErrMsg = "Ayers服务器未登录！";
                return false;
            }

            string reference = CommonUtils.GetAyersReference();
            XDocument xDoc = new XDocument(
                new XElement("message",
                    new XAttribute("type", "order_action:Add"),
                    new XAttribute("msgnum", AyersConfig.MsgNum++),
                    new XElement("bs_flag", side ? "B" : "S"),
                    new XElement("client_acc_code", Account.Client_Acc_ID),
                    new XElement("exchange_code", exchange_code),
                    new XElement("product_code", stockCode),
                    new XElement("order_type", order_type),
                    new XElement("price", price),
                    new XElement("qty", qty),
                    new XElement("reference", reference),
                    new XElement("ip_address", Account.Server_Ip)
                )
            );
            orderCache.MsgNum = xDoc.Root.Attribute("msgnum").Value;
            orderCache.ClientGUID = reference;

            return SendMessage(xDoc, out ErrMsg);
        }

        public bool UpdateOrder(decimal price, int qty, string order_no, out string errMsg)
        {
            if (!AyersMessageAdapter.Instance.IsConnected)
            {
                //AyersMessageAdapter.Instance.Start();
                errMsg = "服务器未能连接";
                return false;
            }
            else if (!AyersMessageAdapter.Instance.IsLogon)
            {
                errMsg = "Ayers服务器未登录！";
                return false;
            }

            try
            {
                XDocument xDoc = new XDocument(
                   new XElement("message",
                       new XAttribute("type", "order_action:Update"),
                       new XAttribute("msgnum", AyersConfig.MsgNum++),
                       new XElement("price", price),
                       new XElement("qty", qty),
                       new XElement("order_no", order_no),
                       new XElement("ip_address", Account.Server_Ip)));
                SendMessage(xDoc, out errMsg);

                return true;
            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("订单更新失败,Message:{0}, StackTrace:{1}", ex.Message, ex.StackTrace);
                errMsg = string.Format("更新失败，订单号{0},异常信息{1}", order_no, ex.Message);
                return false;
            }

            //<message type=”order_action:Update” msgnum=”5712”>
            //    <price>37</price>
            //    <qty>2000</qty>
            //    <order_no>32545</order_no>
            //    <ip_address>202.139.231.23</ip_address>
            //</message> 

            //<message type=”response” msgnum=”5864”>
            //    <status>0</status>
            //    <information>order_status=NEW</information>
            //</message>
        }

        ConcurrentQueue<AyersCancelRecord> QueueCancelRecord = new ConcurrentQueue<AyersCancelRecord>();

        public bool CancelOrder(string order_no, string sender, out string errMsg)
        {
            if (!Instance.IsConnected)
            {
                //Instance.Start();
                errMsg = "撤单失败，未能连接到Ayers服务器！";
                return false;
            }
            else if (!AyersMessageAdapter.Instance.IsLogon)
            {
                errMsg = "Ayers服务器未登录！";
                return false;
            }

            errMsg = string.Empty;
            try
            {
                XDocument xDoc = new XDocument(
                    new XElement("message",
                        new XAttribute("type", "order_action:Cancel"),
                        new XAttribute("msgnum", AyersConfig.MsgNum++),
                        new XElement("order_no", order_no),
                        new XElement("ip_address", Account.Server_Ip)));
                QueueCancelRecord.Enqueue(new AyersCancelRecord() { MsgNum = xDoc.Root.Attribute("msgnum").Value, OrderID = order_no, Sender = sender });

                SendMessage(xDoc, out errMsg);
                return true;
            }
            catch (Exception ex)
            {
                errMsg = string.Format("撤单异常，订单号{0}，异常信息:{1}", order_no, ex.Message);
                return false;
            }

            //<message type=”order_action:Cancel” msgnum=”5713”>
            //    <order_no>15487</order_no>
            //    <ip_address>202.139.231.23</ip_address>
            //</message> 
            //<message type=”response” msgnum=”5864”>
            //    <status>0</status>
            //    <information>order_status=CAN</information>
            //</message> 
        }

        #endregion

        #region Cash In/Out
        /// <summary>
        ///  Client Cash Withdrawal Request : to request AyersGTS that to withdraw cash from client 
        /// </summary>
        //public bool CashOut(string ccy, string amount, string remark, string bank_code, string bank_acc)
        //{

        //    if (IsConnected)
        //    {
        //        XDocument xDoc = new XDocument(
        //            new XElement("message",
        //                new XAttribute("type", "cash_out"),
        //                new XAttribute("msgnum", AyersConfig.MsgNum++),

        //                new XElement("client_acc_code", Account.Client_Acc_ID),
        //                new XElement("password", Account.Client_Acc_Psw),
        //                new XElement("ccy", ccy),
        //                new XElement("amount", amount),
        //                new XElement("remark", remark),
        //                new XElement("bank_code", bank_code),
        //                new XElement("bank_acc", bank_acc)
        //            )
        //        );
        //        string errMsg;
        //        if (SendMessage(xDoc, out errMsg))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //    //<message type="cash_out" msgnum=”5713”>
        //    //  <client_acc_code>CC1</client_acc_code>
        //    //  <password>1234</password>
        //    //  <ccy>HKD</ccy>
        //    //  <amount>1000</amount>
        //    //  <remark>Deposit through HSBC e-banking</remark>
        //    //  <bank_code>HSBC</bank_code>
        //    //  <bank_acc>168-998877-876</bank_acc>
        //    //</message> 
        //}

        //public bool CachIn(string ccy, string amount, string remark)
        //{
        //    string errMsg;
        //    if (IsConnected)
        //    {
        //        XDocument xDoc = new XDocument(
        //            new XAttribute("type", "cash_in"),
        //            new XAttribute("msgnum", AyersConfig.MsgNum++),

        //            new XElement("client_acc_code", Account.Client_Acc_ID),
        //            new XElement("password", Account.Client_Acc_Psw),
        //            new XElement("ccy", ccy),
        //            new XElement("amount", amount),
        //            new XElement("remark", remark)
        //        );

        //        if (SendMessage(xDoc, out errMsg))
        //        {
        //            return true;
        //        }

        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //    }
        //    return false;
        //    //<message type=”cash_in” msgnum=”5713”>
        //    //  <client_acc_code>CC1</client_acc_code>
        //    //  <password>1234</password>
        //    //  <ccy>HKD</ccy>
        //    //  <amount>1000</amount>
        //    //  <remark>Deposit through HSBC e-banking</remark>
        //    //</message> 
        //}
        #endregion

        #region Interface Prepared
        //public bool Portfolio(out string errMsg)
        //{
        //    if (IsConnected)
        //    {
        //        XDocument xDoc = new XDocument(
        //            new XElement("message",
        //                new XAttribute("type", "portfolio"),
        //                new XAttribute("msgnum", AyersConfig.MsgNum++),

        //                new XElement("client_acc_code", Account.Client_Acc_ID),
        //                new XElement("password", Account.Client_Acc_Psw)
        //            )
        //        );
        //        return SendMessage(xDoc, out errMsg);
        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //        return false;
        //    }

        //}

        //public bool OsOrder(out string errMsg)
        //{
        //    if (IsConnected)
        //    {
        //        XDocument xDoc = new XDocument(
        //            new XElement("message",
        //                new XAttribute("type", "os_order"),
        //                new XAttribute("msgnum", AyersConfig.MsgNum++),
        //                new XElement("client_acc_code", Account.Client_Acc_ID),
        //                new XElement("password", Account.Client_Acc_Psw),
        //                new XElement("input_channel", string.Empty)
        //            )
        //        );
        //        return SendMessage(xDoc, out errMsg);
        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //        return false;
        //    }
        //    //<message type=”os_order” msgnum=”5711”> <client_acc_code>CC1</client_acc_code> <password>123456</password> <input_channel></input_channel> </message> 
        //}

        //public bool OrderEnq(string orderID, out string errMsg, string reference = null)
        //{
        //    if (IsConnected)
        //    {
        //        XDocument xDoc = null;
        //        if (string.IsNullOrEmpty(orderID))
        //        {
        //            xDoc = new XDocument(
        //                new XElement("message",
        //                    new XAttribute("type", "order_enq"),
        //                    new XAttribute("msgnum", AyersConfig.MsgNum++),
        //                    new XElement("reference", reference),
        //                    new XElement("client_acc_code", Account.Client_Acc_ID),
        //                    new XElement("password", Account.Client_Acc_Psw)
        //                )
        //            );
        //        }
        //        else
        //        {
        //            xDoc = new XDocument(
        //                new XElement("message",
        //                    new XAttribute("type", "order_enq"),
        //                    new XAttribute("msgnum", AyersConfig.MsgNum++),
        //                    new XElement("order_no", orderID),
        //                    new XElement("client_acc_code", Account.Client_Acc_ID),
        //                    new XElement("password", Account.Client_Acc_Psw)
        //                )
        //            );
        //        }
        //        return SendMessage(xDoc, out errMsg);
        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //        Program.logger.LogInfo(errMsg);
        //        return false;
        //    }
        //    //<message type=”order_enq” msgnum=”5711”> <order_no>23446</order_no> <client_acc_code>CC1</client_acc_code> <password>123456</password> </message> 

        //}

        //public bool MultiOrder(DateTime from, DateTime to, out string errMsg)
        //{
        //    errMsg = string.Empty;
        //    if (IsConnected)
        //    {

        //        var xDoc = new XDocument(
        //                       new XElement("message",
        //                           new XAttribute("type", "multi_order"),
        //                           new XAttribute("msgnum", AyersConfig.MsgNum++),

        //                           new XElement("client_acc_code", Account.Client_Acc_ID),
        //                           new XElement("from_trade_date", from.ToString("yyyy-MM-dd")),
        //                           new XElement("to_trade_date", from.ToString("yyyy-MM-dd"))
        //                       )
        //                   );
        //        try
        //        {
        //            return SendMessage(xDoc, out errMsg);
        //        }
        //        catch (Exception ex)
        //        {
        //            errMsg = ex.Message;
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //        return false;
        //    }
        //    //<message type=”multi_order” msgnum=”5711”> <client_acc_code>CC1</client_acc_code> <from_trade_date>2010-02-03</from_trade_date> <to_trade_date>2010-02-04</to_trade_date> </message>
        //}

        //public bool OrderAmendEnq(string orderNO, out string errMsg)
        //{
        //    if (IsConnected)
        //    {
        //        var xDoc = new XDocument(
        //                       new XElement("message",
        //                           new XAttribute("type", "order_amend_enq"),
        //                           new XAttribute("msgnum", AyersConfig.MsgNum++),
        //                           new XElement("order_no", orderNO)
        //                       )
        //                   );
        //        try
        //        {
        //            return SendMessage(xDoc, out errMsg);
        //        }
        //        catch (Exception ex)
        //        {
        //            errMsg = ex.Message;
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //        return false;
        //    }
        //    //order_amend_enq
        //}

        //public bool OrderTradeEnq(string orderNO, out string errMsg)
        //{
        //    if (IsConnected)
        //    {
        //        var xDoc = new XDocument(
        //                       new XElement("message",
        //                           new XAttribute("type", "order_trades_enq"),
        //                           new XAttribute("msgnum", AyersConfig.MsgNum++),
        //                           new XElement("order_no", orderNO)
        //                       )
        //                   );
        //        try
        //        {
        //            return SendMessage(xDoc, out errMsg);
        //        }
        //        catch (Exception ex)
        //        {
        //            errMsg = ex.Message;
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //        return false;
        //    }
        //}

        //public bool ClientInfoEnq(out string errMsg)
        //{
        //    if (IsConnected)
        //    {
        //        var xDoc = new XDocument(
        //                       new XElement("message",
        //                           new XAttribute("type", "client_info_enq"),
        //                           new XAttribute("msgnum", AyersConfig.MsgNum++),
        //                           new XElement("client_acc_code", Account.Client_Acc_ID)
        //                       )
        //                   );
        //        try
        //        {
        //            return SendMessage(xDoc, out errMsg);
        //        }
        //        catch (Exception ex)
        //        {
        //            errMsg = ex.Message;
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //        return false;
        //    }
        //}

        //public bool ClientAuthentication(string ip, out string errMsg)
        //{
        //    if (IsConnected)
        //    {
        //        XDocument xDoc = new XDocument(
        //            new XElement("message",
        //                 new XAttribute("type", "client_auth"),
        //                new XAttribute("msgnum", AyersConfig.MsgNum++),

        //                new XElement("type", "INTERNET"),
        //                new XElement("client_acc_code", Account.Client_Acc_ID),
        //                new XElement("pwd", Account.Client_Acc_Psw),
        //                new XElement("log_login", "Y"),//Y = log this authentication inside Ayers N = doesn’t log this authentication request (default)
        //                new XElement("log_login_remark", "MANGO"),//Only used when log_login = Y The remark will show in AyersGTS “Login/Logout History Report” 
        //                new XElement("require_trading_group", "N"),//Request for Trading Group Code in reply msg, Y – Yes, N – No 
        //                new XElement("require_price_entitlement", "N"),//Request for Price Entitlement Details in reply msg, Y – Yes, N – No 
        //                new XElement("master_user", "Y"),
        //                new XElement("ip_address", ip),
        //                new XElement("acc_type", "C")
        //            )
        //        );
        //        try
        //        {
        //            return SendMessage(xDoc, out errMsg);
        //        }
        //        catch (Exception ex)
        //        {
        //            errMsg = ex.Message;
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        errMsg = "Ayers 接口未连接!";
        //        return false;
        //    }
        //    //<message type=”client_auth” msgnum=”1234”>
        //    //    <type>INTERNET</type>
        //    //    <client_acc_code>JOHNLEE</client_acc_code >
        //    //    <pwd>abc1234</pwd>
        //    //    <log_login>Y</log_login>
        //    //    <log_login_remark>MANGO</log_login_remark>
        //    //    <require_trading_group>N</require_trading_group>
        //    //    <require_price_entitlement>N</require_price_entitlement>
        //    //    <master_user>Y</master_user>
        //    //    <ip_address>127.1.2.3</ip_address >
        //    //    <acc_type>C</acc_type>
        //    //</message> 
        //}

        //public bool ChgPsw(string newPsw, out string errMsg)
        //{ 
        //     if (IsConnected)
        //     {
        //         XDocument xDoc = new XDocument(
        //             new XElement("message",
        //                 new XAttribute("type", "chg_pwd"),
        //                 new XAttribute("msgnum", AyersConfig.MsgNum++),
        //                 new XElement("type", "INTERNET"),
        //                 new XElement("client_acc_code", Account.Client_Acc_ID),
        //                 new XElement("old_pwd", Account.Client_Acc_Psw),
        //                 new XElement("new_pwd", newPsw)
        //             )
        //         );
        //         return SendMessage(xDoc, out errMsg);
        //     }
        //     else
        //     {
        //         errMsg = "Ayers 接口未连接!";
        //         return false;
        //     }
        //    //<message type=”chg_pwd” msgnum =”1234”> <type>INTERNET</type> <client_acc_code>CC1</client_acc_code> <old_pwd>1234</old_pwd> <new_pwd>123456</new_pwd> </message>   
        //}
        #endregion

        #region Ayers Account Config
        public bool UpdateAccount(string Server_Ip, int Port_No_Encryption, int Port_Encryption, string Encryption_Key, string Message_Compression,
            string Api_Login_ID, string Api_Login_Psw, string Site_ID, string Station_ID, string Type,
            string Client_ID, string Client_ID_Psw, string ClientID_Bak, string ClientID_Psw_Bak, string Client_ID_Using, int Port_Using)
        {
            bool success = false;
            lock (sync)
            {
                try
                {
                    var filePath = System.Windows.Forms.Application.StartupPath + configPath;
                    if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "/config/"))
                        Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "/config/");
                    XDocument xDoc = new XDocument(
                        new XElement("Config",
                            new XElement("server_ip", Server_Ip),
                            new XElement("port_no_encryption", Port_No_Encryption),
                            new XElement("port_encryption", Port_Encryption),
                            new XElement("encryption_key", Encryption_Key),
                            new XElement("message_compression", Message_Compression),
                            new XElement("api_login_id", Api_Login_ID),
                            new XElement("api_login_psw", Api_Login_Psw),
                            new XElement("site", Site_ID),
                            new XElement("station", Station_ID),
                            new XElement("type", Type),

                            new XElement("client_first",
                                new XElement("id", Client_ID),
                                new XElement("password", Client_ID_Psw)),
                            new XElement("client_second",
                                new XElement("id", ClientID_Bak),
                                new XElement("password", ClientID_Psw_Bak)),
                            new XElement("client_using", Client_ID_Using),
                            new XElement("Port_Using", Port_Using)
                        )
                    );

                    File.WriteAllText(filePath, xDoc.ToString(), Encoding.UTF8);
                    InitAccount();
                    if (this.IsLogon)
                    {
                        this.Stop();
                        this.Start();
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfo("Ayers帐号信息保存失败,Message:{0},Stack:{1}", ex.Message, ex.StackTrace);
                }
            }
            return success;
        }

        public string GetCongif()
        {
            string message = null;
            try
            {
                if (ConfigCache.UseAyersInterface == "1")
                {
                    var filePath = System.Windows.Forms.Application.StartupPath + configPath;
                    if (File.Exists(filePath))
                    {
                        message = "1|" + File.ReadAllText(filePath);
                    }
                    else
                    {
                        message = "0|未找到默认配置文件!";
                    }
                }
                else
                {
                    message = "0|服务端配置不支持Ayers接口!";
                }
            }
            catch (Exception ex)
            {
                message = "0|" + ex.Message;
            }
            return message;
        }
        #endregion

        private void AliveThreadStart()
        {
            bool isTested = false;
            while (true)
            {
                XDocument xDoc = new XDocument(
                    new XElement("message",
                        new XAttribute("type", "order_action:keep_alive"),
                        new XAttribute("msgnum", AyersConfig.MsgNum++)
                    )
                );
                string errMsg;
                if (!SendMessage(xDoc, out errMsg))
                {
                    Program.logger.LogInfo(errMsg);
                }
                SendMessage(xDoc, out errMsg);
                if (!isTested)
                {
                    isTested = true;
                    TestAmount();
                }
                Thread.Sleep(60000);
            }
        }

        ConcurrentQueue<XDocument> Requests = new ConcurrentQueue<XDocument>();
        bool SendMessage(XDocument xDoc, out string errMsg,bool isEncry = false)
        {
            
            errMsg = string.Empty;
            string msgNum = xDoc.Root.Attribute("msgnum").Value;
            string strMsg = Regex.Replace(xDoc.ToString(), ">\\s*<", "><").Replace("&amp;", "&");//.Replace("\r", "").Replace("\n", "");
            Requests.Enqueue(xDoc);
            string msgBody = null;
            byte[] byteArr = null;
            if (Account.PortUsing == Account.Port_Encryption)
            {
                var key = Encoding.UTF8.GetBytes(Account.Encryption_Key);

                BlowfishECB b = new BlowfishECB(key, 0, key.Length);
                byteArr = BlowfishECB.encryptBytes(key, Encoding.UTF8.GetBytes(strMsg));
                if (TcpConnector.Instance.Key == null)
                {
                    TcpConnector.Instance.IsEncrypt = true;
                    TcpConnector.Instance.Key = key;
                }
            }
            else
            {
                msgBody = strMsg;
                byteArr = Encoding.UTF8.GetBytes(msgBody);
            }
            
            byte[] byteMain = new byte[byteArr.Length + 4];
            BitConverter.GetBytes(byteArr.Length).CopyTo(byteMain, 0);
            byteArr.CopyTo(byteMain, 4);

            

            try
            {
                TcpConnector.Instance.ClientSendMsg(byteMain);
                return true;
            }
            catch (Exception ex)
            {
                //errMsg = string.Format("Ayers接口消息发送异常：{0}", ex.Message);
                Program.logger.LogInfo("Ayers接口消息发送异常：{0}, StackTrace：{1}", ex.Message, ex.StackTrace);
                if (ex.Message.Contains("远程主机强迫关闭了一个现有的连接"))
                {
                    Program.logger.LogInfo("Ayers接口连接断开，将自动重新登录！");
                    this.IsConnected = TcpConnector.Instance.ClientConnect(Account.Server_Ip, Account.PortUsing);
                    if (IsConnected)
                    {
                        Logon();
                    }
                }
                return false;
            }
        }
        #endregion

        #region Message Receive
        private void ReceiveMessage(string obj)
        {
            string type = string.Empty;
            try
            {
                XDocument doc = XDocument.Parse(obj);
                XDocument msgSend = null;

                type = doc.Root.Attribute("type").Value;
                if (doc.Root.Attribute("msgnum") != null)
                {
                    string msgNum = doc.Root.Attribute("msgnum").Value;
                    msgSend = Requests.First(_ => _.Root.Attribute("msgnum").Value == msgNum);
                    type = msgSend.Root.Attribute("type").Value;
                   
                }
                
                 switch (type)
                {
                    case "login":
                        NotificationLogin(doc);
                        break;
                    case "order_action:Add"://add
                        NotificationAdd(doc, msgSend);
                        break;
                    case "order_action:Cancel"://cancel
                        NotificationCancel(doc, msgSend);
                        break;
                    case "order_action:Update"://update
                        NotificationUpdate(doc, msgSend);
                        break;
                    case "order_action:keep_alive"://keep-alive
                        NotificationAlive(doc, msgSend);
                        break;
                    case "order_notification":// notification-order
                        NotificationOrderChanges(doc);
                        break;
                    case "trade_notification":// notification-trade
                        NotificationTradeChanges(doc);
                        break;
                    case "order_recovery":
                        NotificationRecover(doc);
                        break;
                     case "order_enq":
                        NotificationSingleOrderQuery(doc);
                        break;
                     case "multi_order":
                        NotificationMultiOrderQuery(doc);
                        break;
                     case "chg_pwd":
                        NotificationChgPwd(doc, msgSend);
                        break;
                     case "duplicate_login_notification":
                    default:
                        Program.logger.LogInfoDetail("Ayers接口收到未处理消息：{0}", doc.ToString());
                        break;
                }
                if (Program.帐户委托DataTable.ContainsKey(GroupName))
                {
                    Program.帐户委托DataTable[GroupName].Deal();
                }
                if (type != "login" && type != "order_action:keep_alive" && type != "order_recovery")
                {
                    Program.logger.LogInfo(string.Format(" request:{0} \r\n response:{1}", msgSend == null ? string.Empty : Regex.Replace(msgSend.ToString(), ">\\s*<", "><"), obj));
                }
            }
            catch (System.Xml.XmlException ex)
            {
                Program.logger.LogInfo("Ayers接口消息解析异常，消息类型{0}，收到消息：{1}，XmlException ：{2}", type, obj, ex.Message);
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("Ayers接口消息处理异常，消息类型{0}，收到消息：{1}，异常信息：{2}", type, obj, ex.Message);
            }
        }

        #region Basic （Login & Alive）
        private void NotificationLogin(XDocument docReceive)
        {
            //<message type="response" msgnum="0"><status>0</status><alert_change_pwd>False</alert_change_pwd><force_change_pwd>False</force_change_pwd><pwd_expiry_date>2022-11-05</pwd_expiry_date><last_login_time>2017-06-30 17:19:31</last_login_time><require_activation>False</require_activation><recovery_order_count>4</recovery_order_count></message>
            var status = docReceive.Root.Element("status").Value;
            if (status == "0")
            {
                if (!this.IsLogon)
                {
                    lock (sync)
                    {
                        if (!this.IsLogon)
                        {
                            this.IsLogon = true;
                            if (AliveThread == null)
                            {
                                this.AliveThread = new Thread(AliveThreadStart) { IsBackground = true };
                                this.AliveThread.Start();
                            }
                            Program.logger.LogInfo("Ayers服务器登录成功！");
                        }
                    }
                }
            }
            else
            {
                //Program.logger.LogInfo("登录失败, {0}", docReceive.Root.Element("error").Value);
                Program.logger.LogInfo("登录失败, {0}", docReceive.ToString());
            }
        }

        private void NotificationAlive(XDocument doc, XDocument msgSend)
        {
            var status = doc.Root.Element("status").Value;
            if (status == "0")
            {
                this.IsLogon = true;
                //Program.logger.LogInfoDetail("Ayers keep_alive success, Time: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                Program.logger.LogInfoDetail(doc.Root.Element("error").Value);
            }
        } 
        #endregion

        #region Add/Update/Cancel/Recover
        private void NotificationAdd(XDocument docReceive, XDocument docRequest)
        {
            string msgNum = docReceive.Root.Attribute("msgnum").Value;
            string status = docReceive.Root.Element("status").Value;
            var order = CommonUtils.OrderCacheQueue.FirstOrDefault(_ => _.MsgNum == msgNum);

            if (status == "0")
            {
                var elemInfo = docReceive.Root.Element("information").Value.Split(',');
                var orderNo = elemInfo[0].Split('=')[1];
                var orderStatus = elemInfo[1].Split('=')[1];
                if (order != null)
                {
                    order.OrderID = orderNo;
                    var TradeLimit1 = Program.db.额度分配.FirstOrDefault(_ => _.组合号 == GroupName && _.交易员 == order.Trader && _.证券代码 == order.Zqdm);

                    Program.db.已发委托.Add(DateTime.Today, GroupName, orderNo, order.Trader, "委托成功", order.Market, order.Zqdm, order.ZqName, order.Category, 0m, 0m, order.Price, order.Quantity, 0m);
                    string Msg = order.Sender == order.Trader ? "下单成功" : string.Format("风控员{0}下单成功", order.Sender);
                    Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), order.Trader, GroupName, order.ZqName, order.ZqName, orderNo, order.Category, order.Quantity, order.Price, Msg);

                    Program.AddConsignmentCache(order.Trader, order.Zqdm, order.Category, order.Quantity, order.Price, orderNo, TradeLimit1.组合号, TradeLimit1.证券名称, TradeLimit1.市场);

                    if (order.Sender == order.Trader)
                    {
                        风控操作 风控操作1 = new 风控操作();
                        风控操作1.风控员 = order.Sender;
                        风控操作1.交易员 = order.Trader;
                        风控操作1.证券代码 = order.Zqdm;
                        风控操作1.证券名称 = order.ZqName;
                        风控操作1.委托编号 = orderNo;
                        风控操作1.买卖方向 = order.Category;
                        风控操作1.委托数量 = order.Quantity;
                        风控操作1.委托价格 = order.Price;
                        风控操作1.操作内容 = Msg;
                        Program.风控操作.Enqueue(风控操作1);
                    }

                    JyDataSet.委托Row row = Program.帐户委托DataTable[GroupName].New委托Row();
                    row.交易员 = order.Trader;
                    row.组合号 = GroupName;
                    row.委托时间 = DateTime.Now.ToString("HH:mm:ss");
                    row.证券代码 = order.Zqdm;
                    row.证券名称 = order.ZqName;
                    row.委托价格 = order.Price;
                    row.委托数量 = order.Quantity;
                    row.成交价格 = 0;
                    row.成交数量 = 0;
                    row.撤单数量 = 0;
                    row.状态说明 = "委托成功";
                    row.委托编号 = orderNo;
                    row.买卖方向 = order.Category;
                    row.市场代码 = 2;

                    Program.帐户委托DataTable[GroupName].Add委托Row(row);
                }

            }
            else
            {
                string Msg = string.Format("废单({0},{1})", docReceive.Root.Element("error").Value, docReceive.Root.Element("errorCode").Value);
                Program.db.已发委托.Add(DateTime.Today, GroupName, order.ClientGUID, order.Trader, Msg, order.Market, order.Zqdm, order.ZqName, order.Category, 0m, 0m, order.Price, order.Quantity, 0m);
                
                Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), order.Trader, GroupName, order.ZqName, order.ZqName, order.ClientGUID, order.Category, order.Quantity, order.Price, Msg);

                Program.logger.LogInfo("Ayers下单错误:{0}, 下单消息：{1}, 回复消息：{2}", docReceive.Root.Element("error").Value, docRequest.ToString(), docReceive.ToString());

                JyDataSet.委托Row row = Program.帐户委托DataTable[GroupName].New委托Row();
                row.交易员 = order.Trader;
                row.组合号 = GroupName;
                row.委托时间 = DateTime.Now.ToString("HH:mm:ss");
                row.证券代码 = order.Zqdm;
                row.证券名称 = order.ZqName;
                row.委托价格 = order.Price;
                row.委托数量 = order.Quantity;
                row.成交价格 = 0;
                row.成交数量 = 0;
                row.撤单数量 = order.Quantity;
                row.状态说明 = "未通知" + Msg;
                row.委托编号 = order.ClientGUID;
                row.买卖方向 = order.Category;
                row.市场代码 = 2;

                Program.帐户委托DataTable[GroupName].Add委托Row(row);
            }
            if (order != null)
            {
                order.IsReturnedResult = true;
            }
        }

        private void NotificationUpdate(XDocument revDoc, XDocument docRequest)
        {
            var returnInfo = revDoc.Element("information");
            var orderID = docRequest.Element("order_no").Value;
            var order = Program.帐户委托DataTable[GroupName].First(_ => _.委托编号 == orderID);
            if (returnInfo != null && returnInfo.Value.Contains("order_status="))
            {
                //<message type="order_action:Update" msgnum="21"><price>98.50</price><qty>500</qty><order_no>250411</order_no><ip_address>PNS2.AYERS.COM.HK</ip_address></message>
                //<message type="response" msgnum="21"><status>0</status><information>order_status=NEW</information></message>
                var status = returnInfo.Value.Replace("order_status=", string.Empty);

                if (status.ToUpper() == "NEW" || status.ToUpper() == "WA")
                {
                    order.状态说明 = "订单修改成功";
                    //return "订单修改成功";
                }
                else
                {
                    order.状态说明 = "订单修改失败";
                    //return "订单修改失败：" + returnInfo;
                }

            }
            else if (revDoc.Element("error") != null && revDoc.Element("errorCode") != null)
            {
                order.状态说明 = revDoc.Element("error").Value + revDoc.Element("errorCode").Value;
//<message type="response" msgnum="25">
//    <status>1</status>
//    <error>Please Input Valid Value For Price</error>
//    <errorCode>Error.InvalidValue,=Price</errorCode>
//</message>
            }
        }

        private void NotificationCancel(XDocument docReceive, XDocument docRequest)
        {
            string msgNum = docReceive.Root.Attribute("msgnum").Value;
            string status = docReceive.Root.Element("status").Value;
            string orderID = docRequest.Root.Element("order_no").Value;
            var order = Program.帐户委托DataTable[GroupName].First(_ => _.委托编号 == orderID);

            var info = docReceive.Root.Element("information");
            if (status == "0" && info != null)
            {
                order.状态说明 = "撤单提交成功 " + GetStatusComment(info.Value.Replace("order_status=", ""));
                order.撤单数量 = order.委托数量 - order.成交数量;

                var recordLast = QueueCancelRecord.FirstOrDefault(_ => _.MsgNum == msgNum);
                if (recordLast != null)
                {
                    string Msg = recordLast.Sender == order.交易员 ? "撤单提交成功" : string.Format("风控员{0}撤单提交成功", recordLast.Sender);
                    Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), order.交易员, GroupName, order.证券代码, order.证券名称, orderID, order.买卖方向, order.委托数量, order.委托价格, Msg);

                    if (recordLast.Sender != order.交易员)
                    {
                        风控操作 风控操作1 = new 风控操作();
                        风控操作1.风控员 = recordLast.Sender;
                        风控操作1.交易员 = order.交易员;
                        风控操作1.证券代码 = order.证券代码;
                        风控操作1.证券名称 = order.证券名称;
                        风控操作1.委托编号 = order.委托编号;
                        风控操作1.买卖方向 = order.买卖方向;
                        风控操作1.委托数量 = order.委托数量;
                        风控操作1.委托价格 = order.委托价格;
                        风控操作1.操作内容 = Msg;
                        Program.风控操作.Enqueue(风控操作1);
                    }
                }
                else
                {
                    Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), order.交易员, GroupName, order.证券代码, order.证券名称, orderID, order.买卖方向, order.委托数量, order.委托价格, "撤单成功(未找到撤单命令缓存)");
                }
            }
            else
            {
                var errorInfo = docReceive.Root.Element("error");
                Program.logger.LogInfo("Ayers撤单错误:{0}, 撤单消息：{1}, 回复消息：{2}", docReceive.Root.Element("error").Value, docRequest.ToString(), docReceive.ToString());
                if (errorInfo != null)
                {
                    var msg = errorInfo.Value;
                    Program.db.交易日志.Add(DateTime.Today, DateTime.Now.ToString("HH:mm:ss"), order.交易员, GroupName, order.证券代码, order.证券名称, orderID, order.买卖方向, order.委托数量, order.委托价格, msg);
                }
            }
        }

        #region Order Recover Notification
        private void NotificationRecover(XDocument doc)
        {
            string referenceID = doc.Root.Element("reference").Value;
            string orderID = doc.Root.Element("order_no").Value;
            string code = doc.Root.Element("product_code").Value;
            decimal exec_qty = decimal.Parse(doc.Root.Element("exec_qty").Value);
            decimal exec_price = decimal.Parse(doc.Root.Element("exec_price").Value);
            decimal outstand_qty = decimal.Parse(doc.Root.Element("outstand_qty").Value);
            string action = doc.Root.Element("last_order_action_code").Value;
            DateTime date = DateTime.Parse(doc.Root.Element("create_time").Value);

            if (!string.IsNullOrEmpty(orderID))
            {
                var order = Program.帐户委托DataTable[GroupName].FirstOrDefault(_ => (_.委托编号 == orderID || _.委托编号 == referenceID) && _.证券代码 == code);
                if (order != null)
                {
                    Program.logger.LogInfoDetail("order_recovery ：{0}", doc.ToString());
                    RecoverWT(doc, referenceID, orderID, exec_qty, exec_price, outstand_qty, date, order);

                    //委托重新修覆盖后，成交及现有仓位刷新都应重新计算。另外，要修改下单逻辑，试试返回referenceID
                    RecoverCj(doc, order);
                }
            }
            else
            {
                Program.logger.LogInfo("Ayers 恢复订单信息异常：恢复信息中Order_ID为空，对应数据：{0}", doc.ToString());
            }
        }

        private void RecoverWT(XDocument doc, string referenceID, string orderID, decimal exec_qty, decimal exec_price, decimal outstand_qty, DateTime date, JyDataSet.委托Row order)
        {
            order.成交数量 = exec_qty;
            order.成交价格 = exec_price;
            order.撤单数量 = order.委托数量 - order.成交数量 - outstand_qty;
            order.状态说明 = GetStatus(doc);
            order.委托时间 = date.ToString("HH:mm:ss");
            if (order.委托编号 == referenceID)//如果以本地GUID为编号，则修改为服务端交易Order_ID
                order.委托编号 = orderID;
        }

        private void RecoverCj(XDocument doc, JyDataSet.委托Row order)
        {
            if (doc.Root.Element("trades") != null)
            {
                var trades = doc.Root.Element("trades").Elements("trade");
                foreach (var item in trades)
                {
                    //添加成交项。
                    var tradeID = item.Element("trade_id").Value;
                    var price = decimal.Parse(item.Element("price").Value);
                    var qty = decimal.Parse(item.Element("qty").Value);
                    var createTime = DateTime.Parse(item.Element("create_time").Value);
                    var createDate = createTime.Date;

                    if (!Program.帐户委托DataTable.ContainsKey(GroupName))
                    {
                        var dt = new JyDataSet.成交DataTable();
                        Program.帐户成交DataTable[GroupName] = dt;
                    }

                    if (!Program.帐户成交DataTable[GroupName].Any(_ => _.交易员 == order.交易员 && _.成交编号 == tradeID))
                    {
                        JyDataSet.成交Row row = Program.帐户成交DataTable[GroupName].New成交Row();
                        row.交易员 = order.交易员;
                        row.组合号 = GroupName;
                        row.证券代码 = order.证券代码;
                        row.证券名称 = order.证券名称;
                        row.委托编号 = order.委托编号;
                        row.买卖方向 = order.买卖方向;
                        row.市场代码 = order.市场代码;

                        row.成交编号 = tradeID;
                        row.成交时间 = createTime.ToString("HH:mm:ss");
                        row.成交价格 = price;
                        row.成交数量 = qty;
                        row.成交金额 = price * qty;

                        Program.帐户成交DataTable[GroupName].Add成交Row(row);
                    }
                }
            }
        } 
        #endregion

        private string GetStatus(XDocument doc)
        {
            string status = string.Empty;
            var reject_reason = doc.Root.Element("reject_reason");
            if (reject_reason != null && !string.IsNullOrEmpty(reject_reason.Value))
            {
                status = reject_reason.Value;
            }
            else
            {
                XElement order_status = doc.Root.Element("order_status");
                if (order_status != null && !string.IsNullOrEmpty(order_status.Value))
                {
                    status = GetStatusComment(order_status.Value);
                    if (order_status.Value == "Q" && doc.Root.Element("order_sub_status") != null)
                    {
                        status += " " + doc.Root.Element("order_sub_status").Value;
                    }
                }

                XElement order_type = doc.Root.Element("order_type");
                if (order_type != null && !string.IsNullOrEmpty(order_type.Value))
                {
                    if (status.Length > 0)
                    {
                        status += " " + GetOrderType(order_type.Value);
                    }
                    else
                    {
                        status = GetOrderType(order_type.Value);
                    }
                }
            }
            return status;
        }

        private string GetStatusComment(string status)
        {
            //NEW – pending new order //已发
            //WA – waiting for approval //等待成交
            //PRO – sending to exchange //等待修改
            //Q – Queued (see order_sub_status for detail) //排队中
            //REJ – rejected //拒绝：需要获取被拒绝原因
            //PEX – partially filled //部分成交
            //FEX – fully filled     //全部成交
            //CAN – cancelled order  //取消
            switch (status.ToUpper())
            {
                case "ADD":
                case "NEW":
                    return "下单完成";
                case "WA":
                    return "等待回复";
                case "PRO":
                    return "等待修改";
                case "Q":
                    return "Queued";
                case "REJ":
                    return "订单被拒绝";
                case "PEX":
                    return "部分成交";
                case "FEX":
                    return "成交";
                case "CAN":
                    return "撤单完成";
                default:
                    return status;
            }
        }

        private string GetOrderType(string order_type)
        {
            switch (order_type.ToUpper())
            {
                case "L":
                    return "Price Limit";
                case "E":
                    return "Enhanced Limit";
                case "S":
                    return "Special Limit";
                case "I":
                    return "Auction Limit(竞价限价)";
                case "A":
                    return "Auction Market";
                default:
                    return order_type;
            }
        }
        #endregion

        #region Changes
        private void NotificationOrderChanges(XDocument docReceive)
        {
           var orderID = docReceive.Root.Element("order_no").Value;
            var order = Program.帐户委托DataTable[GroupName].FirstOrDefault(_ => _.委托编号 == orderID);
            if (order != null)
            {
                decimal exec_qty = decimal.Parse(docReceive.Root.Element("exec_qty").Value);//成交数量

                decimal qty = decimal.Parse(docReceive.Root.Element("qty").Value);//数量

                if (exec_qty > 0)
                {
                    decimal exec_price = decimal.Parse(docReceive.Root.Element("exec_price").Value);//成交价格
                    order.成交数量 = exec_qty;
                    order.成交价格 = exec_price;
                }

                XElement elemOutland = docReceive.Root.Element("outstand_qty");//未成交数量
                if (elemOutland != null)
                {
                    decimal outstand_qty = decimal.Parse(elemOutland.Value);
                    order.撤单数量 = qty - exec_qty - outstand_qty;
                }

                order.状态说明 = GetStatus(docReceive);

                var status = docReceive.Root.Element("order_status").Value;
                switch (status.ToUpper())
                {
                    
                    case "PRO":
                    case "REJ":
                        order.撤单数量 = qty - exec_qty;
                        break;
                    default:
                        break;
                }

                if (string.IsNullOrEmpty(order.委托时间))
                {
                    order.委托时间 = DateTime.Parse(docReceive.Root.Element("create_time").Value).ToString("HH:mm:ss");
                }

                Program.帐户委托DataTable[GroupName].Deal();
                
            }
            else
            {
                var client_acc_node = docReceive.Root.Element("client_acc_code");
                if (client_acc_node != null && client_acc_node.Value == Account.Client_ID_Using)
                {
                    Program.logger.LogInfoDetail("NotificationOrderChanges：未找到对应委托{0}", docReceive.ToString());
                }
            }
        }

        //成交刷新
        private void NotificationTradeChanges(XDocument doc)
        {
            var orderID = doc.Root.Element("order_no").Value;

            var order = Program.帐户委托DataTable[GroupName].FirstOrDefault(_ => _.委托编号 == orderID);
            if (order != null)
            {
                Process_Order(doc, order);

                Process_Trade(doc, order);
            }
            else
            {
                Program.logger.LogInfoDetail("NotificationTradeChanges：未找到对应订单{0}", doc.ToString());
            }
        }

        private static void Process_Order(XDocument doc, JyDataSet.委托Row order)
        {
            Program.logger.LogInfoDetail("Trade Nitification Change, process Order: {0}", doc);
            decimal exec_qty = decimal.Parse(doc.Root.Element("exec_qty").Value);//成交数量

            decimal wtQty = decimal.Parse(doc.Root.Element("qty").Value);//数量

            if (exec_qty > 0)
            {
                decimal exec_price = decimal.Parse(doc.Root.Element("exec_price").Value);//成交价格
                order.成交数量 = exec_qty;
                order.成交价格 = exec_price;
            }

            XElement elemOutland = doc.Root.Element("outstand_qty");//未成交数量
            if (elemOutland != null)
            {
                decimal outstand_qty = decimal.Parse(elemOutland.Value);
                order.撤单数量 = wtQty - exec_qty - outstand_qty;
            }

            XElement last_order_action_code = doc.Root.Element("last_order_action_code");
            if (last_order_action_code != null && !string.IsNullOrEmpty(last_order_action_code.Value))
            {
                order.状态说明 = last_order_action_code.Value;
            }

            var wtTime = DateTime.Parse(doc.Root.Element("create_time").Value).ToString("HH:mm:ss");
            if (order.委托时间 != wtTime)
            {
                order.委托时间 = wtTime;
            }
        }

        private static void Process_Trade(XDocument doc, JyDataSet.委托Row order)
        {
            try
            {
                var trades = doc.Root.Element("trades").Elements("trade");
                foreach (var item in trades)
                {
                    //添加成交项。
                    var tradeID = item.Element("trade_id").Value;
                    var price = decimal.Parse(item.Element("price").Value);
                    var qty = decimal.Parse(item.Element("qty").Value);
                    var createTime = DateTime.Parse(item.Element("create_time").Value);
                    var createDate = createTime.Date;

                    if (!Program.帐户委托DataTable.ContainsKey(GroupName))
                    {
                        var dt = new JyDataSet.成交DataTable();
                        Program.帐户成交DataTable[GroupName] = dt;
                    }

                    if (!Program.帐户成交DataTable[GroupName].Any(_ => _.交易员 == order.交易员 && _.成交编号 == tradeID))
                    {
                        JyDataSet.成交Row row = Program.帐户成交DataTable[GroupName].New成交Row();
                        row.交易员 = order.交易员;
                        row.组合号 = GroupName;
                        row.证券代码 = order.证券代码;
                        row.证券名称 = order.证券名称;
                        row.委托编号 = order.委托编号;
                        row.买卖方向 = order.买卖方向;
                        row.市场代码 = order.市场代码;

                        row.成交编号 = tradeID;
                        row.成交时间 = createTime.ToString("HH:mm:ss");
                        row.成交价格 = price;
                        row.成交数量 = qty;
                        row.成交金额 = price * qty;

                        Program.帐户成交DataTable[GroupName].Add成交Row(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.logger.LogInfoDetail("Function: NotificationTradeChanges, LV2 Function - Process_Trade Error:Message {0}, StackTrace:{1}", ex.Message, ex.StackTrace);
            }
            
        }

        #endregion


        private void NotificationSingleOrderQuery(XDocument doc)
        {
            //throw new NotImplementedException();
            NotificationOrderChanges(doc);
            //Program.logger.LogInfoDetail("Ayers接口收到单订单查询结果：{0}", doc.ToString());
        }

        private void NotificationMultiOrderQuery(XDocument doc)
        {
            //throw new NotImplementedException();
            Program.logger.LogInfoDetail("Ayers接口收到多订单查询结果：{0}", doc.ToString());
        }

        private void NotificationChgPwd(XDocument doc, XDocument docSend)
        {
            //<message type="chg_pwd" msgnum="2">
            //    <type>INTERNET</type>
            //    <client_acc_code>VICAPI2</client_acc_code>
            //    <old_pwd>12345678</old_pwd>
            //    <new_pwd>88888888</new_pwd>
            //</message>
            //<message type="response" msgnum="2"><status>0</status></message>
            var status = doc.Root.Element("status").Value;
            if (status == "0")
            {
                var pwdNew = docSend.Root.Element("new_pwd").Value;
                if (Account.Client_Acc_ID == Account.Client_ID_First)
                {
                    Account.Client_Psw_First = pwdNew;
                }
                else
                {
                    Account.Client_Psw_Second = pwdNew;
                }
                //var filePath = System.Windows.Forms.Application.StartupPath + configPath;
                //File.WriteAllText(filePath, Account.xml)
                //Program.logger.LogInfoDetail("Ayers keep_alive success, Time: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                Program.logger.LogInfoDetail(doc.Root.Element("error").Value);
            }
        }

        private void TestAmount()
        {
            //string errMsg;
            try
            {
                //ClientAuthentication("113.138.17.4", out errMsg);
                //OrderEnq("250475", out errMsg);
                //MultiOrder(DateTime.Today, DateTime.Now, out errMsg);
                //OrderAmendEnq("250424", out errMsg);
                //Portfolio(out errMsg);
                //OsOrder(out errMsg);
                //OrderTradeEnq("250424", out errMsg);
                //ClientInfoEnq(out errMsg);
                //CancelOrder("250475", "JYY01", out errMsg);
                //ChgPsw("12345678", out errMsg);
                // 可能的交易密码：88888888，12345678
            }
            catch (Exception ex)
            {
                Program.logger.LogInfo("TestAmount exception msg {0}", ex.Message);
            }

        }
        #endregion


        internal void Load()
        {
            using (var db = new AASDbContext())
            {
                var today = DateTime.Today;
                var lstWT = db.已发委托.Where(_ => _.日期 == today && _.组合号 == "Ayers").ToList();
                var lstCJ = db.已处理成交.Where(_ => _.日期 == today && _.组合号 == "Ayers").ToList();

                var wtDataTable = new JyDataSet.委托DataTable();
                foreach (var wtRow in lstWT)
                {
                    if (wtRow != null)
                    {
                        JyDataSet.委托Row 委托Row1 = wtDataTable.New委托Row();
                        委托Row1.交易员 = wtRow.交易员;
                        委托Row1.组合号 = GroupName;
                        委托Row1.证券代码 = wtRow.证券代码;
                        委托Row1.证券名称 = wtRow.证券名称;
                        委托Row1.委托价格 = wtRow.委托价格;
                        委托Row1.委托数量 = wtRow.委托数量;
                        委托Row1.委托编号 = wtRow.委托编号;
                        委托Row1.买卖方向 = wtRow.买卖方向;
                        委托Row1.市场代码 = wtRow.市场代码;
                        委托Row1.委托时间 = wtRow.日期.ToString("HH:mm:ss");
                        委托Row1.成交价格 = wtRow.成交价格;
                        委托Row1.成交数量 = wtRow.成交数量;
                        委托Row1.状态说明 = wtRow.状态说明;
                        委托Row1.撤单数量 = wtRow.撤单数量;
                        wtDataTable.Add委托Row(委托Row1);
                    }
                }
                Program.帐户委托DataTable[GroupName] = wtDataTable;

                var cjDataTable = new JyDataSet.成交DataTable();
                Program.帐户成交DataTable[GroupName] = cjDataTable;
            }
        }



    }
}
