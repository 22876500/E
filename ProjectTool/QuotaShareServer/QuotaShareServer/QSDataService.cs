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
        public bool CanSendOrder(string mac, string ip, string stockID, int qty)
        {
            //1.下单前查询接口，查询是否可以下单
            // 如可以下单，锁定额度，
            return false;
        }



    }
}
