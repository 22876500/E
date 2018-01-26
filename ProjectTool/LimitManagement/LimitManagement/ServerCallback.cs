using LimitManagement.AASServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LimitManagement
{
    //WCF的回调函数默认是在UI线程中执行 UseSynchronizationContext=false 表示不在UI中执行,控制台程序没有UI线程，不会出现这种死锁。
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    class ServerCallback : AASServiceCallback
    {
        public void Notify(Notify request)
        {
            
        }

        public void 风控分配DataTableChanged(风控分配DataTableChanged request)
        {
            
        }

        public void 成交DataTableChanged(成交DataTableChanged request)
        {
            
        }

        public void 委托DataTableChanged(委托DataTableChanged request)
        {
            
        }

        public void 平台用户DataTableChanged(平台用户DataTableChanged request)
        {
            
        }

        public void 额度分配DataTableChanged(额度分配DataTableChanged request)
        {
            //Program.额度分配表通知.Enqueue(request);
        }

        public void 订单DataTableChanged(订单DataTableChanged request)
        {
            
        }

        public void 已平仓订单DataTableChanged(已平仓订单DataTableChanged request)
        {
            
        }

        public void Close()
        {
            //Program.AASServiceClient.Close();
        }
    }
}
