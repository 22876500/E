using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitMex.WSocket
{
    class WebSocketImpl : WebSocketService
    {
        public ConcurrentQueue<string> MessageQueue = new ConcurrentQueue<string>();

        public WebSocketImpl()
        {

        }

        public void onReceive(string msg)
        {
            if (!msg.Contains("{\"event\":\"pong\"}"))
            {
                MessageQueue.Enqueue(msg);
            }
        }

    }
}
