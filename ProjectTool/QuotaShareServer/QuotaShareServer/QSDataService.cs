using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace QuotaShareServer
{
    [ServiceContract(SessionMode = SessionMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class QSDataService
    {
        [OperationContract]
        public string Test()
        {
            return "connect success";
        }
        [OperationContract]
        public bool ImportQuota()
        {
            return true;
        }

        //公共券池要提供的接口
        

        [OperationContract]
        public bool LockStockQty(string mac, string stockID, int qty)
        {
            return false;
        }

        public bool FreeStockQty(string mac,  string stockID, int qty)
        {
            return false;
        }

        public List<StockLimit> QueryLimitsTotal()
        {
            var db = new QSDBContext();
            return null;
        }

    }
}
