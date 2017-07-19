using System;
using System.Web.Security;

namespace PersonalBlog.WebUI
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; }

        public override string[] GetRolesForUser(string username)
        {
            return LogicProvider.RoleLogic.GetRoles(username);
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return Array.IndexOf(this.GetRolesForUser(username), roleName) > -1;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
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

        public override string[] GetAllRoles()
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