using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AASServer
{
    public sealed class TcpConnector: IDisposable
    {
        #region Static Members
        static object sync = new object();
        static TcpConnector _instance;
        public static TcpConnector Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                        {
                            _instance = new TcpConnector();
                        }
                    }
                }
                return _instance;
            }
        }
        static EndPoint endpointInited { get; set; }
        #endregion

        public bool IsEncrypt { get; set; }

        public byte[] Key { get; set; }

        private TcpConnector() {  }

        private bool _connected = false;
        public bool Connected { get {
            return _connected;
        } }
        public Action<string> MessageReceive;
        ConcurrentQueue<Dictionary<string, byte[]>> MsgQueue = new ConcurrentQueue<Dictionary<string, byte[]>>();

        Socket socketClient;
        Thread threadClientReceive;
        Thread threadInvokeReceiveMsg;
        //this.帐户委托DataTable.Deal();

        #region 连接服务端方法
        public bool ClientConnect(string IP, int Port)
        {
            //定义一个套字节监听  包含3个参数(IP4寻址协议,流式连接,TCP协议)
            socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            EndPoint endpoint = null;
            //需要获取文本框中的IP地址
            if (Regex.IsMatch(IP, "[0-9]+[.][0-9]+[.][0-9]+[.][0-9]+"))
            {
                IPAddress ipaddress = IPAddress.Parse(IP);
                IPEndPoint ipEndPoint = new IPEndPoint(ipaddress, Port);
                endpoint = ipEndPoint;
            }
            else
            {
                System.Net.DnsEndPoint dnsEndPoint = new DnsEndPoint(IP, Port);
                endpoint = dnsEndPoint;
            }

            try
            {
                endpointInited = endpoint;
                socketClient.Connect(endpoint);

                //创建一个线程 用于监听服务端发来的消息
                threadClientReceive = new Thread(RecMsg) { IsBackground = true };
                threadClientReceive.Start();

                threadInvokeReceiveMsg = new Thread(InvokeMsg) { IsBackground = true };
                threadInvokeReceiveMsg.Start();

                _connected = true;
            }
            catch (Exception)
            {
                _connected = false;
            }

            return _connected;
        }

        public void Disconnect()
        {
            
            try
            {
                threadClientReceive.Abort();
                threadInvokeReceiveMsg.Abort();
                socketClient.Disconnect(true);
                
            }
            catch { }
        }
        #endregion

        #region 发送信息到服务端的方法
        public void ClientSendMsg(byte[] addHeadBody)
        {
            if (socketClient.Connected)
            {
                //调用客户端套接字发送字节数组
                socketClient.Send(addHeadBody);
                //Program.logger.LogInfoDetail("Tcp消息发送成功，返回值：{0}", i);
            }
            
        }
        #endregion

        #region 接收服务端发来信息的方法
        const string messageEnd = "</message>";
        const string messageStart = "<message";
        private void RecMsg()
        {
            long count = long.MinValue;
            string revStr = string.Empty;
            while (true) //持续监听服务端发来的消息
            {
                if (_connected)
                {
                    try
                    {
                        if (socketClient.Connected)
                        {
                            byte[] arrRecMsg = new byte[1024 * 1024];
                            int length = socketClient.Receive(arrRecMsg);
                            if (length != 0)
                            {
                                if (count == long.MaxValue)
                                {
                                    count = long.MinValue;
                                }
                                count++;
                                string key = string.Format("{0}_{1}", count, length);
                                Dictionary<string, byte[]> dic = new Dictionary<string, byte[]>();
                                dic.Add(key, arrRecMsg);
                                MsgQueue.Enqueue(dic);
                            }
                            //if (this.IsEncrypt)
                            //{

                            //    //revStr = System.Text.Encoding.UTF8.GetString(dec);

                            //    byte[] arrRecMsgDecrypt = null;
                            //    if ((arrRecMsg.Length - 4) % BlowfishECB.BLOCK_SIZE == 0)
                            //    {
                            //        arrRecMsgDecrypt = BlowfishECB.decryptBytes(Key, arrRecMsg.Skip(4).ToArray());
                            //    }
                            //    else
                            //    {
                            //        arrRecMsgDecrypt = BlowfishECB.decryptBytes(Key, arrRecMsg);
                            //        BlowFish b = new BlowFish(Key);
                            //        arrRecMsgDecrypt = b.Decrypt_ECB(arrRecMsg.Skip(4).ToArray());
                            //    }
                            //    revStr = System.Text.Encoding.UTF8.GetString(arrRecMsgDecrypt);
                            //    //Program.logger.LogInfo("Key：" + System.Text.Encoding.Default.GetString(Key));
                            //    //Program.logger.LogInfo("原始报文：" + System.Text.Encoding.Default.GetString(arrRecMsg));

                            //}
                            //else
                            //{
                            //    revStr = System.Text.Encoding.UTF8.GetString(arrRecMsg.Skip(4).Take(length - 4).ToArray());
                            //}
                            
                            //MsgQueue.Enqueue(revStr);
                        }
                        else
                        {
                            Program.logger.LogInfoDetail("连接已断开");
                            Thread.Sleep(5000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Program.logger.LogInfoDetail("Ayers接收线程异常, revStr{0}, ErrorMsg{1}", revStr, ex.Message);
                        Thread.Sleep(60000);
                    }
                }
            }
        }

        private void InvokeMsg()
        {
            string fragmentMsg = string.Empty;
            string revStr = string.Empty;
           
            while (true)
            {
                try
                {
                    Dictionary<string, byte[]> dic = new Dictionary<string, byte[]>();
                    if (MsgQueue.Count > 0 && MsgQueue.TryDequeue(out dic))
                    {
                        foreach (var key in dic.Keys)
                        {
                            byte[] arrRecMsg = new byte[1024 * 1024];
                            int length = int.Parse(key.Split('_')[1]);
                            arrRecMsg = dic[key];
                            if (arrRecMsg == null || arrRecMsg.Length == 0) continue;
                            if (this.IsEncrypt)
                            {

                                byte[] arrRecMsgDecrypt = null;
                                if ((arrRecMsg.Length - 4) % BlowfishECB.BLOCK_SIZE == 0)
                                {
                                    arrRecMsgDecrypt = BlowfishECB.decryptBytes(Key, arrRecMsg.Skip(4).ToArray());
                                }
                                else
                                {
                                    //arrRecMsgDecrypt = BlowfishECB.decryptBytes(Key, arrRecMsg);
                                    BlowFish b = new BlowFish(Key);
                                    arrRecMsgDecrypt = b.Decrypt_ECB(arrRecMsg.Skip(4).ToArray());
                                }
                                revStr = System.Text.Encoding.UTF8.GetString(arrRecMsgDecrypt);
                                //Program.logger.LogInfo("Key：" + System.Text.Encoding.Default.GetString(Key));
                                //Program.logger.LogInfo("原始报文：" + System.Text.Encoding.Default.GetString(arrRecMsg));

                            }
                            else
                            {
                                revStr = System.Text.Encoding.UTF8.GetString(arrRecMsg.Skip(4).Take(length - 4).ToArray());
                            }
                            List<string> messages = new List<string>();

                            if (!string.IsNullOrEmpty(fragmentMsg) && revStr.IndexOf(messageStart) > 0 && revStr.IndexOf(messageEnd) > revStr.IndexOf(messageStart))
                            {
                                int indexEnd = revStr.IndexOf(messageEnd);
                                int indexStart = revStr.IndexOf(messageStart);

                                revStr = fragmentMsg + revStr.Substring(indexStart);
                                fragmentMsg = string.Empty;
                            }

                            #region 消息截取，获取格式正常的消息
                            while (revStr.IndexOf(messageStart) > -1)
                            {
                                int indexEnd = revStr.IndexOf(messageEnd);
                                int indexStart = revStr.IndexOf(messageStart);
                                if (indexEnd > -1)
                                {
                                    if (indexStart < indexEnd)
                                    {
                                        var msgItem = revStr.Substring(indexStart, indexEnd + messageEnd.Length - indexStart);
                                        if (messages.Count > 0 && messages.Last() == msgItem)
                                        {
                                            revStr = revStr.Substring(indexEnd + messageEnd.Length);
                                            continue;
                                        }
                                        else
                                        {
                                            messages.Add(msgItem);
                                            revStr = revStr.Substring(indexEnd + messageEnd.Length);
                                        }
                                    }
                                    else
                                    {

                                        if (indexStart > 0)
                                        {
                                            revStr = revStr.Substring(indexStart);
                                        }
                                        else
                                        {
                                            revStr = revStr.Substring(indexEnd + messageEnd.Length);
                                        }

                                    }
                                }
                                else
                                {
                                    if (indexStart > -1)
                                    {
                                        fragmentMsg = revStr.Substring(indexStart);
                                        revStr = fragmentMsg;
                                    }
                                    break;
                                }
                            }
                            #endregion

                            if (MessageReceive != null)
                            {
                                messages.ForEach(_ => MessageReceive.Invoke(_));
                                messages.Clear();
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Program.logger.LogInfoDetail("Ayers 消息分析线程异常：{0}", ex.Message);
                }
                
            }
        }
        #endregion


        public void Dispose()
        {
            if (socketClient != null)
            {
                socketClient.Dispose();
            }
        }
    }
}
