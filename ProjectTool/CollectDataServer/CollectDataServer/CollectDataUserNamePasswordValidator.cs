using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CollectDataServer
{
    class CollectDataUserNamePasswordValidator : UserNamePasswordValidator
    {
        public override void Validate(string username, string Password)
        {
            string[] Data = Password.Split('\t');

            //CommonUtils.Log("login : username {0}, password {1}", username, Password);
            if (username != "")
            {
                throw new FaultException("用户名或密码错误");
            }
            if (Password != "")
            {
                throw new FaultException("用户名或密码错误");
            }

            //if (!Program.db.平台用户.ExistsUser(username, Data[0]))
            //{
            //    throw new FaultException("用户名或密码错误");
            //}

            //if (!Program.db.MAC地址分配.IsMacValid(username, Data[1]))
            //{
            //    throw new FaultException("mac地址错误");
            //}

        }
    }
}
