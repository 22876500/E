using BitMex.Config;
using BitMex.WSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BitMex.Adapter
{
    class OKCoinAdapter
    {
        private static object Sync = new object();
        private static string SelectedUrl = OKCoinConfig.urlOKEX;

        #region Members
        WebSocketBaseOKCoin SocketBase;
        WebSocketImpl SocketService;
        Thread threadReConnect = null;
        #endregion

        public string Channel { get; private set; }
        public ConcurrentQueue<string> MessageQueue
        {
            get
            {
                if (SocketService != null)
                {
                    return SocketService.MessageQueue;
                }
                return null;
            }
        }

        public OKCoinAdapter()
        {

        }

        public void Start()
        {
            if (SocketBase == null)
            {
                SocketService = new WebSocketImpl();
                SocketBase = new  WebSocketBaseOKCoin(SelectedUrl, SocketService as WebSocketService);
                SocketBase.start();

                //var dict = new Dictionary<string, string>();
                //dict.Add("api_key", ConfigInfo.apiKey);
                //wsb.send("{\"event\":\"login\",\"parameters\":{\"api_key\":\"" + ConfigInfo.apiKey + "\",\"sign\":\"" + SignParams(dict) + "\"}}");//登录

                threadReConnect = new Thread(new ThreadStart(Reconnect)) { IsBackground = true };
                threadReConnect.Start();
            }
        }

        public void Stop()
        {
            if (SocketBase != null)
            {
                threadReConnect.Abort();
                threadReConnect = null;
                RemoveChannel();
                SocketBase.stop();
                SocketBase = null;
                SocketService = null;
            }
        }

        private void Reconnect()
        {
            while (true)
            {
                if (SocketBase.isReconnect())
                {
                    if (!string.IsNullOrEmpty(Channel))
                    {
                        SocketBase.send("{'event':'addChannel','channel':'" + Channel + "'}");
                    }
                }
                Thread.Sleep(1000);
            }
        }

        #region Channel Changes
        public void AddChannel(string channneTemp, string X, string Y = "", string Z = "")
        {
            string channelStr = OKCoinConfig.GetFormatChannel(channneTemp, X, Y, Z);
            SocketBase.send("{'event':'" + OKCoinConfig.addChannelStr + "','channel':'" + channelStr + "'}");
            if (!string.IsNullOrEmpty(this.Channel))
            {
                RemoveChannel();
            }
            this.Channel = channelStr;
        }

        public void RemoveChannel()
        {
            if (!string.IsNullOrEmpty(this.Channel))
            {
                string removeChannelStr = "{'event':'" + OKCoinConfig.removeChannelStr + "','channel':'" + Channel + "'}";
                this.Channel = null;
                SocketBase.send(removeChannelStr);

            }
        }
        #endregion

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
