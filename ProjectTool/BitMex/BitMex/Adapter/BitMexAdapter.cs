using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace BitMex.Adapter
{
    class BitMexAdapter
    {
        WSocket.WebSocketImpl wsl = new WSocket.WebSocketImpl();
        WSocket.WebSocketBaseBitMex wsb;

        public ConcurrentQueue<string> MessageQueue { get { 
            return wsl.MessageQueue; 
        
        } }

        public BitMexAdapter()
        { }

        public void Start(string channel, string args = null)
        {
            if (wsb != null)
            {
                wsb.stop();
            }
            string url = BitmexConfig.Uri  + "?subscribe=" + channel ;
            if (!string.IsNullOrEmpty(args))
            {
                url += "," + args;
            }
            wsb = new WSocket.WebSocketBaseBitMex(url, wsl);
            wsb.start();
        }

        public void Subscribe(string channel,  string args = null)
        {
            wsb.send("{\"op\": \"subscribe\", \"args\": [<SubscriptionTopic>]}".Replace("<SubscriptionTopic>", args ?? channel));
        }

        public void Stop()
        {
            wsb.stop();
            wsb = null;
            wsl = null;
        }

    }
}
