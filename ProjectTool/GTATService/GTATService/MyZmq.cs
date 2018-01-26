using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace GTATService
{
    public class MyZmq
    {
        public string Ip { get; set; }
        public int Port { get; set; }

        public ZSocket Socket { get; set; }
    }
    public class Test
    {
        public Test(string inQueueTime,ZMessage msg)
        {
            InQueueTime = inQueueTime;
            Msg = msg;
        }
        public string InQueueTime { set; get; }

        public ZMessage Msg { set; get; }
    }
}
