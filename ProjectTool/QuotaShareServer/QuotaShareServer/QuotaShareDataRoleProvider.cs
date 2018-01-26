using QuotaShareServer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace QuotaShareServer
{
    public class QuotaShareDataRoleProvider : RoleProvider
    {
        public override string[] GetAllRoles()
        {
            //get all groups
            return System.Enum.GetNames(typeof(RoleType));
        }

        public override string[] GetRolesForUser(string username)
        {
            return new string[] { RoleType.超级管理员.ToString() };
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return RoleType.超级管理员.ToString() == roleName;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
