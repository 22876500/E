using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class AASUserNamePasswordValidator : System.IdentityModel.Selectors.UserNamePasswordValidator
    {

        public override void Validate(string username, string Password)
        {
            string[] Data = Password.Split('\t');

       



            if (!Program.db.平台用户.ExistsUser(username ,Data[0]))
            {
                throw new FaultException("用户名或密码错误");
            }

            if (!Program.db.MAC地址分配.IsMacValid(username, Data[1]))
            {
                throw new FaultException("mac地址错误");
            }
            
        }
    }
}
