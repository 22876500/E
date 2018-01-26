using LimitManagement.AASServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace LimitManagement.Entities
{
    public class ServerInfo : INotifyPropertyChanged
    {
        #region Properties
        public string Ip { get; set; }

        public int Port { get; set; }

        public string Remark { get; set; }

        private string ConnectGUID { get; set; }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                NotifyPropertyChange("IsConnected");
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                if (string.IsNullOrEmpty(_status))
                {
                    if (string.IsNullOrEmpty(ConnectGUID))
                    {
                        _status = "未连接";
                    }
                    else
                    {
                        _status = "已连接";
                    }
                }
                return _status;
            }
            set
            {
                _status = value;
                NotifyPropertyChange("Status");
            }
        }
        #endregion

        #region Members
        private Thread _connectThread;
        private ZContext context = new ZContext();
        private ZSocket socket;
        private object Sync = new object();
        private bool isStarted = false;
        #endregion

        public void Start()
        {
            if (!isStarted)
            {
                socket = new ZSocket(context, ZSocketType.REQ);
                socket.Connect(string.Format("tcp://{0}:{1}", Ip, Port));
                if (_connectThread != null)
                {
                    _connectThread.Abort();
                    _connectThread = null;
                }
                _connectThread = new Thread(new ThreadStart(ConnectionCheckMain)) { IsBackground = true };
                _connectThread.Start();
                isStarted = true;
            }
        }

        public void Stop()
        {
            if (isStarted)
            {
                if (socket != null)
                {
                    this.socket.Close();
                    this.socket.Dispose();
                }
                if (_connectThread != null)
                {
                    _connectThread.Abort();
                    _connectThread = null;
                }
                this.Status = "未连接";
                this.IsConnected = false;
                this.ConnectGUID = null;
                isStarted = false;
            }
        }

        #region Keep Connection
        private void ConnectionCheckMain()
        {
            while (true)
            {
                if (!IsConnected || string.IsNullOrEmpty(this.ConnectGUID))
                {
                    lock (Sync)
                    {
                        this.ConnectGUID = GetGuid(socket);
                    }
                    Thread.Sleep(1000);
                }
                else
                {
                    this.IsConnected = true;
                    lock (Sync)
                    {
                        Heartbreak(socket);
                    }
                    Thread.Sleep(3000);
                }
            }
        }

        private string GetGuid(ZSocket socket)
        {
            string guid = null;
            ZMessage msgSend = new ZMessage();
            msgSend.Add(new ZFrame("0|" + ServiceConnectHelper.ConnectionKey));

            try
            {
                socket.Send(msgSend);
                msgSend.Dispose();

                var msgReceive = socket.ReceiveMessage();
                socket.ReceiveTimeout = new TimeSpan(0, 0, 3);
                if (msgReceive != null)
                {
                    var strMsg = msgReceive[0].ReadString();
                    if (strMsg.StartsWith("0|1|"))
                    {
                        guid = strMsg.Substring(4);
                        this.IsConnected = true;
                    }
                    else
                    {
                        Utils.Log("获取连接ID失败！");
                    }
                }
            }
            catch (ZeroMQ.ZException)
            {
                Status = "连接异常，请尝试手动重新连接";
                Thread.CurrentThread.Abort();
            }
            catch (Exception ex)
            {
                Utils.Log("获取连接ID异常！", ex);
                Status = "连接异常！" + ex.Message;
            }
            return guid;
        }

        private void Heartbreak(ZSocket socket)
        {
            var msgSend = new ZMessage();
            msgSend.Add(new ZFrame("10|" + ConnectGUID));

            try
            {
                socket.ReceiveTimeout = new TimeSpan(0, 0, 3);
                socket.Send(msgSend);
                msgSend.Dispose();

                var msgReceive = socket.ReceiveMessage();
                string strReceive = msgReceive[0].ReadString();
                if (!strReceive.Contains("10|1|"))
                {
                    this.ConnectGUID = null;
                    this.IsConnected = false;
                    Status = strReceive;
                }
                else
                {
                    Status = "已连接";
                }
            }
            catch (ZeroMQ.ZException ex)
            {
                this.ConnectGUID = null;
                this.IsConnected = false;
                Status = "连接异常，" + ex.Message;
                Thread.CurrentThread.Abort();
            }
            catch (Exception)
            {
                this.ConnectGUID = null;
                this.IsConnected = false;
                Status = "连接异常";
            }
        } 
        #endregion

        #region 额度分配
        public DbDataSet.额度分配DataTable QueryLimit(out string errMsg)
        {
            errMsg = string.Empty;
            AASServiceReference.DbDataSet.额度分配DataTable dt = null;
            if (!string.IsNullOrEmpty(this.ConnectGUID) && IsConnected)
            {
                lock (Sync)
                {
                    try
                    {
                        string strRequest = string.Format("1|{0}", ConnectGUID);
                        string strReceive = SendRequest(socket, strRequest);
                        if (strReceive.StartsWith("1|1|"))
                            dt = strReceive.Substring(4).FromJson<AASServiceReference.DbDataSet.额度分配DataTable>();
                        else
                            errMsg = strReceive.Substring(4);
                    }
                    catch (Exception ex)
                    {
                        errMsg = string.Format("QueryLimit 异常：{0}", ex.Message);
                    }
                }
            }
            else
            {
                errMsg = string.Format("server {0}({1}) 未连接，无法执行删除命令，请在主界面手动重新连接！", Remark, Ip);
            }

            return dt;
        }

        public string AddLimit(AASServiceReference.DbDataSet.额度分配DataTable dt)
        {
            string errMsg = string.Empty;
            if (!string.IsNullOrEmpty(this.ConnectGUID) && IsConnected)
            {
                lock (Sync)
                {
                    try
                    {
                        var row1 = dt.First();
                        string strRequest = string.Format("4|{0}|{1}", ConnectGUID, dt.ToJson());
                        socket.ReceiveTimeout = new TimeSpan(0, 2, 0);
                        string strReceive = SendRequest(socket, strRequest);
                        return strReceive.Substring(4);
                    }
                    catch (Exception ex)
                    {
                        errMsg = string.Format("Add Limit 异常：{0}", ex.Message);
                    }
                }
            }
            else
            {
                errMsg = string.Format("server {0}({1}) 未连接，无法执行导入命令，请在主界面手动重新连接！", Remark, Ip);
            }

            return errMsg;
        }

        public DbDataSet.额度分配Row UpdateLimit(AASServiceReference.DbDataSet.额度分配DataTable dtUpdate, out string errMsg)
        {
            errMsg = string.Empty;
            DbDataSet.额度分配Row result = null;
            if (!string.IsNullOrEmpty(this.ConnectGUID) && IsConnected)
            {
                lock (Sync)
                {
                    try
                    {
                        string strRequest = string.Format("2|{0}|{1}", ConnectGUID, dtUpdate.ToJson());
                        string strReceive = SendRequest(socket, strRequest);
                        if (strReceive.StartsWith("2|1|"))
                        {
                            var dt = strReceive.Substring(4).FromJson<DbDataSet.额度分配DataTable>();
                            result = dt.First();
                        }
                        else
                            errMsg = strReceive.Substring(4);
                    }
                    catch (Exception ex)
                    {
                        errMsg = string.Format("UpdateLimit 异常：{0}", ex.Message);
                    }
                }
            }
            else
            {
                errMsg = string.Format("server {0}({1}) 未连接，无法执行修改命令，请在主界面手动重新连接！", Remark, Ip);
            }

            return result;
        }

        public string DeleteLimits(DbDataSet.额度分配DataTable table)
        {
            if (!string.IsNullOrEmpty(this.ConnectGUID) && this.IsConnected)
            {
                int successCount = 0;
                int exceptionCount = 0;
                foreach (var item in table)
                {
                    try
                    {
                        string errMsg;
                        if (DeleteLimit(item.交易员, item.证券代码, item.组合号, out errMsg))
                            successCount++;
                        else
                            Utils.LogFormat("删除失败,交易员{0},证券代码{1},组合号{2},服务器{3}({4}),错误信息{5}", item.交易员, item.证券代码, item.组合号, Remark, Ip, errMsg);
                    }
                    catch (Exception ex)
                    {
                        exceptionCount++;
                        Utils.LogFormat("删除异常,交易员{0},证券代码{1},组合号{2},服务器{3}({4}),异常信息{5}", item.交易员, item.证券代码, item.组合号, Remark, Ip, ex.Message);
                    }

                }
                return string.Format("server {0}({1}) 批量删除完毕，总计{2}条，成功{3}条", Remark, Ip, table.Count, successCount);
            }
            else
            {
                return string.Format("server {0}({1}) 未连接，无法执行删除命令，请在主界面手动重新连接！", Remark, Ip);
            }
        }

        public bool DeleteLimit(string trader, string stockID, string group, out string errMsg)
        {
            errMsg = string.Empty;
            bool result = false;
            if (!string.IsNullOrEmpty(this.ConnectGUID) && IsConnected)
            {
                lock (Sync)
                {
                    try
                    {
                        string strRequest = string.Format("3|{0}|{1}|{2}|{3}", ConnectGUID, trader, stockID, group);
                        string strReceive = SendRequest(socket, strRequest);
                        if (strReceive.StartsWith("3|1|"))
                            result = true;
                        else
                            errMsg = strReceive.Substring(4);
                    }
                    catch (Exception ex)
                    {
                        errMsg = string.Format("DeleteLimit 异常：{0}", ex.Message);
                    }
                }
            }
            else
            {
                errMsg = string.Format("server {0}({1}) 未连接，无法执行删除命令，请在主界面手动重新连接！", Remark, Ip);
            }

            return result;
        }
        #endregion

        //public AASServiceReference

        private static string SendRequest(ZSocket socket, string strRequest)
        {
            ZMessage msgRequest = new ZMessage();
            msgRequest.Add(new ZFrame(strRequest));

            socket.Send(msgRequest);
            msgRequest.Dispose();

            ZMessage msgReceive = socket.ReceiveMessage();
            string strReceive = msgReceive[0].ReadString();
            return strReceive;
        }


        #region NotifyPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                //根据PropertyChanged事件的委托类，实现PropertyChanged事件：
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
