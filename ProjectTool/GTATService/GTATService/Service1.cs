using GTATService.Common;
using GTATService.Logger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace GTATService
{
    public partial class Service1 : ServiceBase
    {
        private ZContext contextPub;
        private ZSocket socketPub;

        private string[] ip;
        private string[] port;

        private Dictionary<string, MyZmq> dictMyZmq;
        ConcurrentQueue<ZMessage> dataCache;

        Thread pubDataThread;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.Instance.Info("服务已启动");
            this.StartWork();
        }

        protected override void OnStop()
        {
            LogHelper.Instance.Info("服务已停止");
        }

        public void StartWork()
        {
            if (Init())
            {
                if (Bind())
                {
                    pubDataThread = new Thread(new ThreadStart(PubData)) { IsBackground = true };
                    pubDataThread.Start();

                    this.RecvMessage();
                }
            }
        }

        private bool Init()
        {
            try
            {
                dictMyZmq = new Dictionary<string, MyZmq>();
                dataCache = new ConcurrentQueue<ZMessage>();

                ip = ConfigMain.GetConfigValue(BaseCommon.CONFIGIP).Split(',');
                port = ConfigMain.GetConfigValue(BaseCommon.CONFIGSUBPORT).Split(',');
                if (ip.Length == 0 || port.Length == 0) return false;
                for (int i = 0; i < port.Length; i++)
                {
                    //if (i < ip.Length)
                    //{
                    //    MyZmq mq = new MyZmq();
                    //    mq.Ip = ip[i].Trim();
                    //    mq.Port = int.Parse(port[i].Trim());
                    //    mq.Socket = new ZSocket(new ZContext(), ZSocketType.SUB);
                    //    dictMyZmq.Add(string.Format("{0}:{1}", mq.Ip, mq.Port), mq);
                    //}
                    //else
                    //{
                    //    MyZmq mq = new MyZmq();
                    //    mq.Ip = ip[i - 1].Trim();
                    //    mq.Port = int.Parse(port[i].Trim());
                    //    mq.Socket = new ZSocket(new ZContext(), ZSocketType.SUB);
                    //    dictMyZmq.Add(string.Format("{0}:{1}", mq.Ip, mq.Port), mq);
                    //}
                    MyZmq mq = new MyZmq();
                    mq.Ip = ip[0].Trim();
                    mq.Port = int.Parse(port[i].Trim());
                    mq.Socket = new ZSocket(new ZContext(), ZSocketType.SUB);
                    dictMyZmq.Add(string.Format("{0}:{1}", mq.Ip, mq.Port), mq);
                }
                contextPub = new ZContext();
                socketPub = new ZSocket(contextPub, ZSocketType.PUB);

            }
            catch (Exception ex)
            {
                LogHelper.Instance.Error("Init：" + ex.Message);
                return false;
            }

            return true;
        }
        private bool Bind()
        {
            try
            {
                int i = 1;

                socketPub.Bind(string.Format("tcp://*:{0}", ConfigMain.GetConfigValue(BaseCommon.CONFIGPUBPORT)));
                LogHelper.Instance.Info("pub数据端口：" + ConfigMain.GetConfigValue(BaseCommon.CONFIGPUBPORT));

                foreach (var item in dictMyZmq.Values)
                {
                    item.Socket.Connect(string.Format("tcp://{0}:{1}", item.Ip, item.Port));
                    LogHelper.Instance.Info(string.Format("Sub数据地址{0}：{1}:{2}", i, item.Ip, item.Port));
                    item.Socket.SubscribeAll();
                    Thread.Sleep(100);
                    i++;
                }

            }
            catch (System.Exception ex)
            {
                LogHelper.Instance.Info("异常：" + ex.ToString());
                return false;
            }


            return true;
        }

        private void RecvMessage()
        {
            if (dictMyZmq.Keys.Count == 0) return;
            Thread[] threads = new Thread[dictMyZmq.Keys.Count];
            int i = 0;
            foreach(var item in dictMyZmq.Values)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(SubData)) { IsBackground = true };
                threads[i].Start(item.Socket);
                i++;
            }
            

        }
        private void SubData(Object socketSub)
        {
            ZSocket socket = (ZSocket)socketSub;
            while (true)
            {
                try
                {
                    //LogHelper.Instance.Info("行情数据...");
                    ZMessage recData = new ZMessage();
                    ZError error;
                    
                    bool flag = socket.ReceiveMessage(ref recData, out error);
                    if (flag)
                    {
                        if (recData.Count == 2)
                        {
                            LogHelper.Instance.Info("心跳检测...");
                            continue;
                        }
                        //LogHelper.Instance.Info("行情数据...");
                        //Test test = new Test(DateTime.Now.ToString("HHmmssfff"), recData);
                        dataCache.Enqueue(recData);
                    }
                    else
                    {
                        LogHelper.Instance.Info(error.ToString());
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.Instance.Info("异常：" + ex.ToString());
                    continue;
                }
            }
        }
        private void PubData()
        {
            while (true)
            {
                try
                {
                    ZMessage msg;
                    if (dataCache.Count > 0 && dataCache.TryDequeue(out msg))
                    {
                        //LogHelper.Instance.Info(string.Format("入队时间：{0}, 出队时间：{1}", test.InQueueTime, DateTime.Now.ToString("HHmmssfff")));
                        ZError error;
                        bool flag = socketPub.Send(msg, out error);
                        if (!flag)
                        {
                            LogHelper.Instance.Info(error.ToString());
                        }
                        msg.Dispose();
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }

                catch (System.Exception ex)
                {
                    LogHelper.Instance.Info("异常：" + ex.ToString());
                    continue;
                }
            }
        }
    }
}
