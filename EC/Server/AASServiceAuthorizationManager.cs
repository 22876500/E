using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class AASAuthorizationPolicy : System.IdentityModel.Policy.IAuthorizationPolicy
    {
        string id = Guid.NewGuid().ToString();

        private const string ActionOfAdd = "http://www.artech.com/calculator/add";

        private const string ActionOfSubtract = "http://www.artech.com/calculator/subtract";

        private const string ActionOfMultiply = "http://www.artech.com/calculator/multiply";

        private const string ActionOfDivide = "http://www.artech.com/calculator/divide";



        internal const string ClaimType4AllowedOperation = "http://www.artech.com/allowed";


        // this method gets called after the authentication stage，
        // Evaluates whether a user meets the requirements for this authorization policy.
        // 把用户可以访问的函数关联到用户，即给用户添加某些权限
        public bool Evaluate(System.IdentityModel.Policy.EvaluationContext evaluationContext, ref object state)
        {
            //Return Value
            //Type: System.Boolean

            //false if the Evaluate method for this authorization policy must be called if additional claims are added by other authorization policies to evaluationContext; 
            //otherwise, true to state no additional evaluation is required by this authorization policy. 

            //如果其他策略把另外的claims加到evaluationContext中时，必须调用此授权策略的Evaluate函数，则返回false；
            //否则，如果要声明此授权策略不需要另外的评估，返回true


            if (null == state)
            {

                state = false;

            }

            bool hasAddedClaims = (bool)state;

            if (hasAddedClaims)
            {

                return true;

            }

            IList<Claim> claims = new List<Claim>();

            foreach (ClaimSet claimSet in evaluationContext.ClaimSets)
            {

                foreach (Claim claim in claimSet.FindClaims(ClaimTypes.Name, Rights.PossessProperty))
                {

                    string userName = (string)claim.Resource;

                    if (userName.Contains('\\'))
                    {

                        userName = userName.Split('\\')[1];

                        if (string.Compare("Foo", userName, true) == 0)
                        {

                            claims.Add(new Claim(ClaimType4AllowedOperation, ActionOfAdd, Rights.PossessProperty));

                            claims.Add(new Claim(ClaimType4AllowedOperation, ActionOfSubtract, Rights.PossessProperty));

                        }

                        if (string.Compare("Bar", userName, true) == 0)
                        {

                            claims.Add(new Claim(ClaimType4AllowedOperation, ActionOfMultiply, Rights.PossessProperty));

                            claims.Add(new Claim(ClaimType4AllowedOperation, ActionOfDivide, Rights.PossessProperty));

                        }

                    }

                }

            }

            evaluationContext.AddClaimSet(this, new DefaultClaimSet(this.Issuer, claims));

            state = true;

            return true;
        }

        private IPrincipal GetPrincipal(string userName)
        {
            GenericIdentity identity = new GenericIdentity(userName);
            if (string.Compare("Foo", userName, true) == 0)
            {
                return new GenericPrincipal(identity, new string[] { "Administrators" });
            }
            else
            {
                return new GenericPrincipal(identity, new string[] { "Guest" });
            }
        }

        public System.IdentityModel.Claims.ClaimSet Issuer
        {
            get
            {
                return ClaimSet.System;
            }
        }

        public string Id
        {
            get { return this.id; }
        }
    }

    class CustomPrincipal : IPrincipal
    {
        IIdentity identity;
        public CustomPrincipal(IIdentity identity)
        {
            this.identity = identity;
        }

        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }



    public class AASServiceAuthorizationManager : ServiceAuthorizationManager
    {
        //授权用户可以调用服务里的哪些函数,在调用函数前执行CheckAccessCore
        // 判断访问某个函数时，用户的权限里是否有访问这个函数的权限，权限由IAuthorizationPolicy中添加
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // Extract the action URI from the OperationContext. Match this against the claims
            // in the AuthorizationContext.
            //string action = operationContext.RequestContext.RequestMessage.Headers.Action;
            //Program.logger.Log(action);



            //r MessageProperties1 = OperationContext.Current.IncomingMessageProperties;
            //RemoteEndpointMessageProperty RemoteEndpointMessageProperty1 = MessageProperties1[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            //Program.logger.Log("CheckAccessCore:{0} [{1}:{2}]", OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name, RemoteEndpointMessageProperty1.Address, RemoteEndpointMessageProperty1.Port);
            string UserName = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;

            IClient IClient1 = OperationContext.Current.GetCallbackChannel<IClient>();


            if (!Program.ClientUserName.ContainsKey(IClient1))
            {
                ICommunicationObject ICommunicationObject1 = IClient1 as ICommunicationObject;
                ICommunicationObject1.Closed += ICommunicationObject1_Closed;
                ICommunicationObject1.Faulted += ICommunicationObject1_Faulted;


                Program.ClientUserName[IClient1] = UserName;
            }


            //string[] FunctionName = { };
            //if (FunctionName.Contains(function))
            //{

            //}
            //else
            //{
            //    throw new FaultException("无权限");
            //}



            //string action = operationContext.RequestContext.RequestMessage.Headers.Action;//http://tempuri.org/AASService/Login
            //int k = action.LastIndexOf("/");
            //string function = action.Substring(k + 1);

            //using (AASDbContext db = new AASDbContext())
            //{
            //    AASUser AASUserC = db.User.First(r => r.用户名 == UserName);
            //    switch (AASUserC.角色)
            //    {
            //        case 角色.超级管理员:
            //            break;
            //        case 角色.普通管理员:
            //            break;
            //        case 角色.超级风控员:
            //            break;
            //        case 角色.普通风控员:
            //            break;
            //        case 角色.交易员:
            //            break;
            //        default:
            //            throw new FaultException("无权限");
            //    }
            //}


            // Iterate through the various claim sets in the AuthorizationContext.
            //foreach (ClaimSet ClaimSet1 in operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets)
            //{
            //    // Examine only those claim sets issued by System.
            //    if (ClaimSet1.Issuer == ClaimSet.System)
            //    {
            //        // Iterate through claims of type.
            //        foreach (Claim c in ClaimSet1.FindClaims(DataProvider.ClaimType, Rights.PossessProperty))
            //        {
            //            // If the Claim resource matches the action URI then return true to allow access.
            //            Console.WriteLine("正在比较权限：" + action + "和" + c.Resource.ToString());
            //            if (action == c.Resource.ToString())
            //                return true;
            //        }
            //    }
            //}

            // If this point is reached, return false to deny access.true to accept
            return true;
        }



        void ICommunicationObject1_Faulted(object sender, EventArgs e)
        {
            if (Program.ClientUserName.ContainsKey(sender as IClient))
            {
                string UserName;

                Program.ClientUserName.TryRemove(sender as IClient, out UserName);
            }
        }

        void ICommunicationObject1_Closed(object sender, EventArgs e)
        {
            if (Program.ClientUserName.ContainsKey(sender as IClient))
            {
                string UserName;

                Program.ClientUserName.TryRemove(sender as IClient, out UserName);
            }
        }

    }


   
}
