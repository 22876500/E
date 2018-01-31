using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace AASServer
{
    public class LimitManageService
    {
        Thread _replyThread;
        ZContext context = null;

        private static LimitManageService _instance = null;
        public static LimitManageService Instance {
            get
            {
                if (_instance == null)
                {
                    _instance = new LimitManageService();
                }
                return _instance;
            }
        }

        private LimitManageService()
        { }

        int _port = -1;
        int Port 
        {
            get
            {
                if (_port < 1)
                {
                    var strPort = Program.appConfig.GetValue("LimitManagePort", "");
                    if (string.IsNullOrEmpty(strPort))
                    {
                        Program.appConfig.SetValue("LimitManagePort", "58000");
                        _port = 58000;
                    }
                    else
                    {
                        _port = int.Parse(strPort);
                    }
                }
                return _port;
            }
        }

        public void Start()
        {
            this.context = ZContext.Create();
            ReplyThreadStart();
        }

        public void Stop()
        {
            this._replyThread.Abort();
        }

        private void ReplyThreadStart()
        {
            _replyThread = new Thread(new ThreadStart(ReplyMain)) { IsBackground = true };
            _replyThread.Start();
        }

        private void ReplyMain()
        {
            using (ZSocket socket = new ZSocket(context, ZSocketType.REP))
            {
                try
                {
                    socket.Bind(string.Format("tcp://*:{0}", Port));
                    Program.logger.LogInfoDetail("额度管理接口开启，Port {0}。", Port);
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("额度管理接口 绑定异常:\r\n  bind info: {0}\r\n  Exception Message: {1}", Port, ex.Message);
                    return;
                }

                while (true)
                {
                    var msgReceive = socket.ReceiveMessage();
                    ZMessage zMsgReply = new ZMessage();
                    string strReceive = null;
                    try
                    {
                        if (msgReceive != null)
                        {
                            strReceive = msgReceive[0].ReadString();
                            SwitchMessage(zMsgReply, strReceive);
                            msgReceive.Dispose();
                        }
                        else
                        {
                            zMsgReply.Add(new ZFrame("无效：消息为空!"));
                        }
                    }
                    catch (Exception ex)
                    {
                        zMsgReply.Add(new ZFrame(string.Format("4|额度管理功能异常, 收到Message:{0}， Exception {1}", strReceive, ex.Message)));
                    }
                    socket.Send(zMsgReply);
                    zMsgReply.Dispose();
                }
            }
        }

        private void SwitchMessage(ZMessage zMsgReply, string message)
        {
            var info = message.Split('|');
            string replyInfo = null;
            switch (info[0])
            {
                case "0":
                    replyInfo = ConnectLimitService(info[1]);

                    break;
                case "1":
                    replyInfo = QueryLimitInfo(info[1]);
                    break;
                case "2"://update
                    replyInfo = UpdateLimitItem(info);
                    break;
                case "3"://delete
                    replyInfo = DeleteLimitItem(info);
                    break;
                case "4"://import
                    replyInfo = AddLimitItem(info);
                    break;
                case "10":
                    replyInfo = KeeyAlive(info[1]);
                    break;
                default:
                    replyInfo = string.Format("逻辑未实现，请联系管理员! 输入信息 {0}", info);
                    break;
            }
            zMsgReply.Add(new ZFrame(replyInfo));
        }

        private string KeeyAlive(string info)
        {
            string replyInfo = string.Empty;
            if (info == CommonUtils.LimitServiceID)
            {
                replyInfo = "10|1|1";
            }
            else
            {
                replyInfo = "10|2|访问ID异常，请联系管理员!";
            }
            return replyInfo;
        }

        public string ConnectLimitService( string info)
        {
            string replyInfo = string.Empty;
            try
            {
                var infoDecrypt = Cryptor.MD5Encrypt(info);
                if (infoDecrypt == "7017A31D24EF0DCCED105D6748D7A8A5")
                {
                    replyInfo = "0|1|" + CommonUtils.LimitServiceID;
                }
                else
                {
                    replyInfo = "0|2|连接字符串不匹配!";
                }
            }
            catch (Exception)
            {
                replyInfo = "0|0|输入数据解析失败!";
            }

            return replyInfo;

        }

        public string QueryLimitInfo(string id)
        {
            string replyInfo = string.Empty;
            if (id == CommonUtils.LimitServiceID)
            {
                try
                {
                    var result = Program.db.额度分配.LimitManagerServiceQeury(id);
                    replyInfo = string.Format("1|1|{0}", result.ToJson());
                }
                catch (Exception ex)
                {
                    replyInfo = string.Format("1|2|{0}", ex.Message);
                }
                
            }
            else
            {
                replyInfo = string.Format("1|0|访问ID异常，请联系管理员");
            }
            return replyInfo;
        }

        public string UpdateLimitItem( string[] info)
        {
            if (info.Length < 3)
            {
                return string.Format("2|0|参数列表异常!");
            }
            string id = info[1];
            //byte market = byte.Parse(info[5]);
            //买模式 买模式1 = (买模式)int.Parse(info[8]);
            //卖模式 卖模式1 = (卖模式)int.Parse(info[9]);
            //decimal limitQty = decimal.Parse(info[10]);
            //decimal commision = decimal.Parse(info[11]);
            //string id, string trader, string groupID, string stockID, decimal qty, byte market, string stockName, string stockFirstLetter, decimal commission, 买模式 买模式1, 卖模式 卖模式1;
            if (id == CommonUtils.LimitServiceID)
            {
                try
                {
                    var dt =info[2].FromJson<DbDataSet.额度分配DataTable>();
                    var row = dt.FirstOrDefault();
                    //修改
                    //Program.db.额度分配.UpdateTradeLimit(info[2], info[3], info[4], market, info[6], info[7], 买模式1, 卖模式1, limitQty, commision);
                    买模式 买模式1 = (买模式)row.买模式;
                    卖模式 卖模式1 = (卖模式)row.卖模式;
                    Program.db.额度分配.UpdateTradeLimit(row.交易员, row.证券代码, row.组合号, row.市场, row.证券名称, row.拼音缩写, 买模式1, 卖模式1, row.交易额度, row.手续费率);
                    var rowUpdated = Program.db.额度分配.Get额度分配(row.交易员, row.证券代码);
                    if (rowUpdated != null)
                    {
                        var dtReturn = new DbDataSet.额度分配DataTable();
                        dtReturn.ImportRow(rowUpdated);
                        return "2|1|" + dtReturn.ToJson();
                    }
                    else
                    {
                        return "2|0|修改失败";
                    }
                }
                catch (Exception ex)
                {
                    return "2|2|修改异常" + ex.Message;
                }
            }
            return "2|0|无效操作!";
        }

        public string DeleteLimitItem(string[] info)
        {
            //string id, string trader, string groupID, string stockID
            string id = info[1];
            string trader = info[2];
            string stockID = info[3];
            //string groupID = info[4];
            if (id == CommonUtils.LimitServiceID)
            {
                try
                {
                    Program.db.额度分配.DeleteTradeLimit(trader, stockID);
                    return "3|1|删除成功";
                }
                catch (Exception ex)
                {
                    return "3|2|删除失败，" + ex.Message;
                }
            }
            return "3|0|无效操作!";
        }

        public string AddLimitItem(string[] info)
        {
            if (info.Length < 3)
            {
                return "5|0|参数列表不足，请联系管理员！";
            }

            string id = info[1];
            if (id == CommonUtils.LimitServiceID)
            {
                try
                {
                    var limits = info[2].FromJson<DbDataSet.额度分配DataTable>();
                    if (limits == null)
                        return "5|2|额度分配表为空!";
                    string msg;
                    int num = Program.db.额度分配.LimitManagerserviceImport(id, limits, out msg);
                    return string.Format("5|1|表中包含数据{0}条，{1}", limits.Count, msg);
                }
                catch (Exception ex)
                {
                    return "5|3|导入异常，Excetption " + ex.Message;
                }
            }
            return "5|4|无效操作";
        }
    }
}
