using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using PersonalBlog.DalContracts;
using PersonalBlog.Entities;
using PersonalBlog.LogicContracts;
using PersonalBlog.SqlDal;

namespace PersonalBlog.Logic
{
    public class RoleLogic : IRoleLogic
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IRoleDao roleDao;

        public RoleLogic()
        {
            string conStr = ConfigurationManager.ConnectionStrings["PersonalBlog"].ConnectionString;
            this.roleDao = new RoleDao(conStr);
        }

        public bool AddRoleToUser(string userId, string roleId)
        {
            if (int.TryParse(userId, out int intUserId) && int.TryParse(roleId, out int intRoleId))
            {
                try
                {
                    return this.roleDao.AddRoleToUser(intUserId, intRoleId);
                }
                catch (Exception ex)
                {
                    return FatalError(ex);
                }
            }

            Log.Error("Add role to user failed");
            return false;
        }

        public bool DeleteUserRole(string userId, string roleTitle)
        {
            if (int.TryParse(userId, out int intUserId))
            {
                try
                {
                    return this.roleDao.DeleteUserRole(intUserId, roleTitle);
                }
                catch (Exception ex)
                {
                    FatalError(ex);
                }     
            }

            Log.Error("Delete role from user failed");
            return false;
        }

        public IEnumerable<Role> GetAll()
        {
            try
            {
                return this.roleDao.GetAll().ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Role>();
            }
        }

        public string[] GetRoles(string username)
        {
            try
            {
                return this.roleDao.GetRoles(username);
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new string[0];
            } 
        }

        private static bool FatalError(Exception ex)
        {
            Log.Fatal($"Fatal Error, trace: {ex}");
            return false;
        }
    }
}