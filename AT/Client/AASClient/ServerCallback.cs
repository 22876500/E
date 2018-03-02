using AASClient.AASServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AASClient
{
    //WCF的回调函数默认是在UI线程中执行 UseSynchronizationContext=false 表示不在UI中执行,控制台程序没有UI线程，不会出现这种死锁。
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    class ServerCallback : AASServiceCallback
    {
        public void Notify(Notify request)
        {
            Program.交易通知.Enqueue(request);
        }




        public void 风控分配DataTableChanged(风控分配DataTableChanged request)
        {
            Program.风控分配通知.Enqueue(request);
        }

       


        public void 成交DataTableChanged(成交DataTableChanged request)
        {
            Program.成交表通知.Enqueue(request);
        }

        public void 委托DataTableChanged(委托DataTableChanged request)
        {
            Program.委托表通知.Enqueue(request);
        }

        public void 平台用户DataTableChanged(平台用户DataTableChanged request)
        {
            Program.平台用户表通知.Enqueue(request);
        }

        public void 额度分配DataTableChanged(额度分配DataTableChanged request)
        {
            Program.额度分配表通知.Enqueue(request);
        }

        public void 订单DataTableChanged(订单DataTableChanged request)
        {
            Program.订单表通知.Enqueue(request);
        }

        public void 已平仓订单DataTableChanged(已平仓订单DataTableChanged request)
        {
            Program.已平仓订单表通知.Enqueue(request);
        }

        public void 委托Change(AASClient.AASServiceReference.委托Change request)
        {
            var order = Program.jyDataSet.委托.FirstOrDefault(_ => _.委托编号 == request.orderID);
            if (order != null)
            {
                order.状态说明 = request.status;
                order.成交数量 = request.successQty;
                order.成交价格 = request.successPrice;
                order.撤单数量 = request.cancelQty;
            }
            else
            {
                Program.logger.LogInfo(string.Format("Not found Order {0}", request.orderID));
            }
        }

        public void Close()
        {
            Program.AASServiceClient.Close();
        }



       
    }
}
