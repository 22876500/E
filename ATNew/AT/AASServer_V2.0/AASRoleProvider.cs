using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace AASServer
{
    class AASRoleProvider : RoleProvider
    {
        public override string[] GetAllRoles()
        {
            //get all groups
            return System.Enum.GetNames(typeof(角色));
        }

        public override string[] GetRolesForUser(string username)
        {
            AASServer.DbDataSet.平台用户Row 平台用户Row1 = Program.db.平台用户.Get平台用户( username);
            角色 角色1 = (角色)平台用户Row1.角色;

            return new string[] { 角色1.ToString() };
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            AASServer.DbDataSet.平台用户Row 平台用户Row1 = Program.db.平台用户.Get平台用户(username);
            角色 角色1 = (角色)平台用户Row1.角色;
            return 角色1.ToString() == roleName;
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
