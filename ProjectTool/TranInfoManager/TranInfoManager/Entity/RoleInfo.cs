using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranInfoManager.Entity
{
    public class RoleInfo
    {
        [Key, Column(Order = 0)]
        public string RoleID { get; set; }

        public string RoleName { get; set; }
    }
}
