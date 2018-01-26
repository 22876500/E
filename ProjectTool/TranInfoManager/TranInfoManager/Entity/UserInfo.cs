using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranInfoManager.Entity
{
    public class UserInfo
    {
        public string UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        //relation to role
        public string UserRole { get; set; }
    }

}
