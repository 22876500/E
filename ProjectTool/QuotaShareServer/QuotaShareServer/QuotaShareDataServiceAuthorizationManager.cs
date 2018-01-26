using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace QuotaShareServer
{
    public class QuotaShareDataServiceAuthorizationManager : ServiceAuthorizationManager
    {
        //授权用户可以调用服务里的哪些函数,在调用函数前执行CheckAccessCore
        // 判断访问某个函数时，用户的权限里是否有访问这个函数的权限，权限由IAuthorizationPolicy中添加
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // Extract the action URI from the OperationContext. Match this against the claims
            // in the AuthorizationContext.
            //string action = operationContext.RequestContext.RequestMessage.Headers.Action;
            //Program.logger.Log(action);



            //MessageProperties MessageProperties1 = OperationContext.Current.IncomingMessageProperties;
            //RemoteEndpointMessageProperty RemoteEndpointMessageProperty1 = MessageProperties1[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            //Program.logger.Log("CheckAccessCore:{0} [{1}:{2}]", OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name, RemoteEndpointMessageProperty1.Address, RemoteEndpointMessageProperty1.Port);
            string UserName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            //IClient IClient1 = OperationContext.Current.GetCallbackChannel<IClient>();


            //if (!Program.ClientUserName.ContainsKey(IClient1))
            //{
            //    ICommunicationObject ICommunicationObject1 = IClient1 as ICommunicationObject;
            //    ICommunicationObject1.Closed += ICommunicationObject1_Closed;
            //    ICommunicationObject1.Faulted += ICommunicationObject1_Faulted;


            //    Program.ClientUserName[IClient1] = UserName;
            //}



            return true;
        }



        void ICommunicationObject1_Faulted(object sender, EventArgs e)
        {
            //if (Program.ClientUserName.ContainsKey(sender as IClient))
            //{
            //    string UserName;

            //    Program.ClientUserName.TryRemove(sender as IClient, out UserName);
            //}
        }

        void ICommunicationObject1_Closed(object sender, EventArgs e)
        {
            //if (Program.ClientUserName.ContainsKey(sender as IClient))
            //{
            //    string UserName;

            //    Program.ClientUserName.TryRemove(sender as IClient, out UserName);
            //}
        }
    }
}
