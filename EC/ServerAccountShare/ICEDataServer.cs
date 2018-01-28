using Ice;
using PortServerIce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AASServer
{
    public class ICEDataServer
    {

        bool _isConnected;
        private string _id;

        Communicator _ic;
        TradeServantPrx _tradeServantPrx;

        private ICEDataServer()
        { 
        
        }

        private static object sync = new object();
        private static ICEDataServer _instance;
        public static ICEDataServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (sync)
                    {
                        if (_instance == null)
                            _instance = new ICEDataServer();
                    }
                }
                return _instance;
            }
        }



        public int Connect()
        {
            try
            {
                if (_ic == null)
                {
                    var icData = new InitializationData();
                    _ic = Ice.Util.initialize(icData);

                    GetTradeServant();
                    //GetQueryServant();

                    _isConnected = true;
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                Program.logger.LogInfo("PortClient:{0},交易接口连接初始化失败!\r\n{1}", _id, ex);
                //App.Logger.Error(string.Format("PortClient:{0},交易接口连接初始化失败!", _id), ex);
            }

            return 1;
        }

        public int Disconnect()
        {
            try
            {
                if (_ic != null)
                {
                    _ic.shutdown();
                    Thread.Sleep(100);
                    _ic.destroy();
                    _tradeServantPrx = null;
                    //_queryServant = null;
                    _ic = null;

                    _isConnected = false;
                    return 0;
                }

            }
            catch (System.Exception ex)
            {
                Program.logger.LogInfo("PortClient:{0},交易接口连接初始化失败!\r\n{1}", _id, ex);
                //App.Logger.Error(string.Format("PortClient:{0},交易接口连接初始化失败!", _id), ex);
            }
            return 1;
        }

        public TradeServantPrx GetTradeServant()
        {
            try
            {
                lock (this)
                {
                    try
                    {
                        if (_tradeServantPrx != null)
                        {
                            _tradeServantPrx.ice_ping();
                            return _tradeServantPrx;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Program.logger.LogInfo(string.Format("PortClient:{0},交易接口连接错误!", _id), ex);
                    }

                    if (_ic != null)
                    {
                        Dictionary<string, string> ctx = new Dictionary<string, string>();
                        ctx.Add("id", _id); //连接ID

                        var endpoint = string.Format("TradeService:tcp -h {0} -p {1}",  "", "");
                        var obj = _ic.stringToProxy(endpoint);
                        var client = TradeServantPrxHelper.checkedCast(obj);
                        client = TradeServantPrxHelper.checkedCast(client.ice_context(ctx));
                        if (client == null)
                        {
                            Program.logger.LogInfo("PortClient: 交易接口连接失败");
                            return null;
                        }

                        client.ice_ping();
                        _tradeServantPrx = client;

                        return _tradeServantPrx;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Program.logger.LogInfo("顶点订单发送失败：{0}", ex.Message);
            }

            return null;
        }

        public int CancelOrder(PSIceAccount account, PSIceOrderCancel cancel, out PSIceOrderCancelOut cancel_out, out PSIceErrorCode error)
        {
            try
            {
                TradeServantPrx prx = GetTradeServant();
                if (prx != null)
                {
                    return prx.CancelOrder(account, cancel, out cancel_out, out error);
                }
            }
            catch (System.Exception ex)
            {
                Program.logger.LogInfo("PortClient:{0},委托撤单调用失败！\r\n{1}", _id, ex);
            }
            error = new PSIceErrorCode(-1, "未知错误");
            cancel_out = null;
            return -1;
        }

        public int SendOrder(PSIceAccount account, PSIceOrder order,out PSIceOrderOut orderout,out PSIceErrorCode error)
        {
            var prx = GetTradeServant();
            try
            {
                if (prx != null)
                {
                    return prx.SendOrder(account, order, out orderout, out error);
                }
            }
            catch (System.Exception ex)
            {
                Program.logger.LogInfo("顶点订单发送失败：{0}", ex.Message);
            }
            error = new PSIceErrorCode(-1, "未知错误");
            orderout = null;
            return 0;
        }

        public PSIceOrder GetPSIceOrder(DbDataSet.额度分配Row rc, PSIceOrderType type, long price, int volume)
        {
            PSIceOrder order = new PSIceOrder();
            
            order.Market = rc.市场 == 0 ? PSIceMarket.SZ : PSIceMarket.SH;
            order.OrderType = type;
            order.Code = rc.证券代码;
            order.Price = price;
            order.Volume = volume;
            //order.BatchId = 0;
            //order.Node = _node;
            return order;
        }

        public PSIceAccount GetPSIceAccount(string accountName)
        {
            PSIceAccount ac = new PSIceAccount();
            return ac;
        }


    }
            

}
