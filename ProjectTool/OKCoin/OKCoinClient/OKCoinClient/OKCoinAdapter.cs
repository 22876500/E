using OKCoinClient.OKCoinEntities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;

namespace OKCoinClient
{
    public class OKCoinAdapter : INotifyPropertyChanged
    {
        private static object Sync = new object();
        private static string SelectedUrl = OKCoinConfig.urlOKEX;

        #region Members
        WebSocketBase SocketBase;
        WebSocketImpl SocketService;
        Thread threadReConnect = null; 
        #endregion

        public string Channel { get; private set; }
        public ConcurrentQueue<string> MessageQueue
        {
            get {
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
                SocketBase = new WebSocketBase(SelectedUrl, SocketService as WebSocketService);
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

        //private string SignParams(Dictionary<string, string> paramDict)
        //{
        //    StringBuilder sbSign = new StringBuilder();
            
        //    if (paramDict.Count > 0)
        //    {
        //        var orderedKey = paramDict.Keys.OrderBy(_ => _);
        //        foreach (var item in orderedKey)
        //        {
        //            sbSign.Append(item).Append('=').Append(paramDict[item]).Append('&');
        //        }
        //        sbSign.Append("secret_key=").Append(ConfigInfo.secretKey);
        //    }
        //    //MD5 md5 = new MD5CryptoServiceProvider();
        //    var result = "";
        //    if (sbSign.Length > 0)
        //    {
        //        result = FormsAuthentication.HashPasswordForStoringInConfigFile(sbSign.ToString(), "MD5");
        //    }
        //    return result;
        //}

        //public static string StringToMD5(string str, int i)
        //{
        //    //获取要加密的字段，并转化为Byte[]数组
        //    byte[] data = System.Text.Encoding.Unicode.GetBytes(str.ToCharArray());
        //    //建立加密服务
        //    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //    //加密Byte[]数组
        //    byte[] result = md5.ComputeHash(data);
        //    //将加密后的数组转化为字段
        //    if (i == 16 && str != string.Empty)
        //    {
        //        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
        //    }
        //    else if (i == 32 && str != string.Empty)
        //    {
        //        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        //    }
        //    else
        //    {
        //        switch (i)
        //        {
        //            case 16: return "000000000000000";
        //            case 32: return "000000000000000000000000000000";
        //            default: return "请确保调用函数时第二个参数为16或32";
        //        }

        //    }
        //}
    }
}
