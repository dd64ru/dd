using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.LogicContracts
{
    public interface IRoleLogic
    {
        string[] GetRoles(string username);

        bool DeleteUserRole(string userId, string roleTitle);

        bool AddRoleToUser(string userId, string roleId);

        IEnumerable<Role> GetAll();
    }
}