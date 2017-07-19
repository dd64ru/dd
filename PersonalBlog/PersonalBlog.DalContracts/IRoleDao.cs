using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.DalContracts
{
    public interface IRoleDao
    {
        string[] GetRoles(string username);

        bool DeleteUserRole(int userId, string roleTitle);

        bool AddRoleToUser(int userId, int roleId);

        IEnumerable<Role> GetAll();

        bool AddDefaultRole(int userId);
    }
}