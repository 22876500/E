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
   
    //先认证再授权

    //自定义认证，判断用户名密码是否正确
    class AASServiceAuthenticationManager : ServiceAuthenticationManager
    {
        public override System.Collections.ObjectModel.ReadOnlyCollection<System.IdentityModel.Policy.IAuthorizationPolicy> Authenticate(System.Collections.ObjectModel.ReadOnlyCollection<System.IdentityModel.Policy.IAuthorizationPolicy> authPolicy, Uri listenUri, ref System.ServiceModel.Channels.Message message)
        {
            // check the user and password against a database;
            // if not match
            // throw new AuthenticationException("Incorrect credentials!");


            return base.Authenticate(authPolicy, listenUri, ref message);
        }
    }
    
}
