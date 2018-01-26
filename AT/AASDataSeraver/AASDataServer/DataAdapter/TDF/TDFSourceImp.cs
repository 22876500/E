using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using AASDataServer.Service;

using TDFAPI;

namespace AASDataServer.DataAdapter.TDF
{
    public class TDFSourceImp : TDFDataSource
    {
        public bool IsOnline { get; set; }

        public bool IsLogin { get; set; }

        public bool IsClose { get; set; }

        public DateTime AliveTime { get; set; }

        public TDFConnectResult ConnectResult { get; set; }

        public TDFLoginResult LoginResult { get; set; }

        private TDFCode[] _codes;
        public TDFCode[] Codes { get { return _codes; } }

        public ConcurrentQueue<TDFMarketData[]> MarketDataCache { get; set; }

        public ConcurrentQueue<TDFTransaction[]> TransactionCache { get; set; }

        public ConcurrentQueue<TDFOrder[]> OrderCache { get; set; }

        public ConcurrentQueue<TDFOrderQueue[]> OrderQueueCache { get; set; }

        public event Action<string> NewSysEvent;

        // 非代理方式连接和登陆服务器
        public TDFSourceImp(TDFOpenSetting_EXT openSetting)
            : base(openSetting)
        {
            IsOnline = false;
            IsLogin = false;
            IsClose = false;
            AliveTime = DateTime.Now;

            MarketDataCache = new ConcurrentQueue<TDFMarketData[]>();
            TransactionCache = new ConcurrentQueue<TDFTransaction[]>();
            OrderCache = new ConcurrentQueue<TDFOrder[]>();
            OrderQueueCache = new ConcurrentQueue<TDFOrderQueue[]>();
        }

        public void ClearCache()
        {
            TDFMarketData[] tmp;
            while (MarketDataCache.TryDequeue(out tmp) == true) ;
            TDFTransaction[] tmp2;
            while (TransactionCache.TryDequeue(out tmp2) == true) ;
            TDFOrder[] tmp3;
            while (OrderCache.TryDequeue(out tmp3) == true) ;
            TDFOrderQueue[] tmp4;
            while (OrderQueueCache.TryDequeue(out tmp4) == true) ;
        }

        //重载 OnRecvSysMsg 方法，接收系统消息通知
        // 请注意：
        //  1. 不要在这个函数里做耗时操作
        //  2. 只在这个函数里做数据获取工作 -- 将数据复制到其它数据缓存区，由其它线程做业务逻辑处理
        public override void OnRecvSysMsg(TDFMSG msg)
        {
            if (msg.MsgID == TDFMSGID.MSG_SYS_CONNECT_RESULT)
            {
                //连接结果
                TDFConnectResult connectResult = msg.Data as TDFConnectResult;
                ConnectResult = connectResult;
                IsOnline = connectResult.ConnResult;
                if (NewSysEvent != null)
                {
                    NewSysEvent("连接状态 " + (IsOnline ? "已连接" : "未连接"));
                }
                App.Logger.Info("TDF：连接状态 "+ (IsOnline ? "已连接" : "未连接"));
            }
            else if (msg.MsgID == TDFMSGID.MSG_SYS_LOGIN_RESULT)
            {
                TDFLoginResult loginResult = msg.Data as TDFLoginResult;
                LoginResult = loginResult;
                IsLogin = loginResult.LoginResult;
                if (NewSysEvent != null)
                {
                    NewSysEvent("登陆状态 " + (IsLogin ? "已登陆" : "未登陆"));
                }
                App.Logger.Info("TDF：登陆状态 " + (IsLogin ? "已登陆" : "未登陆"));
            }
            else if (msg.MsgID == TDFMSGID.MSG_SYS_CODETABLE_RESULT)
            {
                //接收代码表结果
                TDFCodeResult codeResult = msg.Data as TDFCodeResult;
                //客户端请自行保存代码表，本处演示怎么获取代码表内容
                if (codeResult != null)
                {
                    TDFCode[] sh_codes;
                    TDFCode[] sz_codes;
                    GetCodeTable("SH", out sh_codes);
                    GetCodeTable("SZ", out sz_codes);
                    _codes = new TDFCode[sh_codes.Length + sz_codes.Length];
                    sh_codes.CopyTo(_codes, 0);
                    sz_codes.CopyTo(_codes, sh_codes.Length);
                    if (NewSysEvent != null)
                    {
                        NewSysEvent("已获取代码列表," + _codes.Length);
                    }
                }
                else
                {
                    if (NewSysEvent != null)
                    {
                        NewSysEvent("获取代码列表失败");
                    }
                }
                
                App.Logger.Info("TDF：获取代码列表消息");
            }
            else if (msg.MsgID == TDFMSGID.MSG_SYS_QUOTATIONDATE_CHANGE)
            {
                //行情日期变更。
                TDFQuotationDateChange quotationChange = msg.Data as TDFQuotationDateChange;
                if (NewSysEvent != null)
                {
                    NewSysEvent("行情日期变更");
                }
                App.Logger.Info("TDF：行情日期变更");
            }
            else if (msg.MsgID == TDFMSGID.MSG_SYS_MARKET_CLOSE)
            {
                //闭市消息
                TDFMarketClose marketClose = msg.Data as TDFMarketClose;
                IsClose = true;
                if (NewSysEvent != null)
                {
                    NewSysEvent("闭市状态 " + (IsClose ? "闭市" : "未闭市"));
                }
                App.Logger.Info("TDF：闭市状态 " + (IsClose ? "闭市" : "未闭市"));
            }
            else if (msg.MsgID == TDFMSGID.MSG_SYS_HEART_BEAT)
            {
                //心跳消息
                AliveTime = DateTime.Now;
            }
        }
        
        //重载OnRecvDataMsg方法，接收行情数据
        // 请注意：
        //  1. 不要在这个函数里做耗时操作
        //  2. 只在这个函数里做数据获取工作 -- 将数据复制到其它数据缓存区，由其它线程做业务逻辑处理
        public override void OnRecvDataMsg(TDFMSG msg)
        {

            if (msg.MsgID == TDFMSGID.MSG_DATA_MARKET)
            {
                //行情消息
                TDFMarketData[] marketDataArr = msg.Data as TDFMarketData[];
                MarketDataCache.Enqueue(marketDataArr);
            }
            else if(msg.MsgID == TDFMSGID.MSG_DATA_TRANSACTION)
            {
                //逐笔成交
                TDFTransaction[] transactionDataArr = msg.Data as TDFTransaction[];
                TransactionCache.Enqueue(transactionDataArr);
            }
            else if(msg.MsgID == TDFMSGID.MSG_DATA_ORDER)
            {
                //逐笔委托
                TDFOrder[] orderDataArr = msg.Data as TDFOrder[];
                OrderCache.Enqueue(orderDataArr);
            }
            else if (msg.MsgID == TDFMSGID.MSG_DATA_ORDERQUEUE)
            {
                //委托队列
                TDFOrderQueue[] orderQueueDataArr = msg.Data as TDFOrderQueue[];
                OrderQueueCache.Enqueue(orderQueueDataArr);
            }
        }
    }
}
