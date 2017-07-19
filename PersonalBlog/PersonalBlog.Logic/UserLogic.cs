using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using log4net;
using PersonalBlog.DalContracts;
using PersonalBlog.Entities;
using PersonalBlog.LogicContracts;
using PersonalBlog.SqlDal;

namespace PersonalBlog.Logic
{
    public class UserLogic : IUserLogic
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IUserDao userDao;
        private readonly IRoleDao roleDao;

        public UserLogic()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PersonalBlog"].ConnectionString;
            this.userDao = new UserDao(connectionString);
            this.roleDao = new RoleDao(connectionString);
        }

        public bool CanLogin(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                Log.Error("Log in failed");
                return false;
            }

            string hashPassword = this.HashString(password);

            try
            {
                return this.userDao.CanLogin(login.ToLowerInvariant(), hashPassword);
            }
            catch (Exception ex)
            {
                return FatalError(ex);
            }
        }

        public bool Create(string login, string password, string mainPhoto)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || !this.IsLoginFree(login))
            {
                Log.Error("User registation failed");
                return false;
            }

            if (mainPhoto == string.Empty)
            {
                mainPhoto = "No photo";
            }

            string hashPassword = this.HashString(password);

            try
            {
                int userId = this.userDao.Create(login.ToLowerInvariant(), hashPassword, mainPhoto);
                this.roleDao.AddDefaultRole(userId);
                return true;
            }
            catch (Exception ex)
            {
                return FatalError(ex);
            } 
        }

        public bool Delete(string id)
        {
            if (int.TryParse(id, out int intId))
            {
                try
                {
                    return this.userDao.Delete(intId);
                }
                catch (Exception ex)
                {
                    return FatalError(ex);
                }
            }

            Log.Error("Delete failed");
            return false;
        }

        public bool Edit(string id, string userImage)
        {
            if (int.TryParse(id, out int intId) && !string.IsNullOrWhiteSpace(userImage))
            {
                try
                {
                    return this.userDao.Edit(intId, userImage);
                }
                catch (Exception ex)
                {
                    return FatalError(ex);
                }   
            }

            Log.Error("Edit failed");
            return false;
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return this.userDao.GetAll().ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<User>();
            } 
        }

        public User GetCurrentUser(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                Log.Error("User is null");
                return null;
            }

            try
            {
                return this.userDao.GetCurrentUser(login);
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new User();
            }   
        }

        public bool IsLoginFree(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                Log.Error("Is login free null");
                return false;
            }

            try
            {
                return this.userDao.IsLoginFree(login.ToLowerInvariant());
            }
            catch (Exception ex)
            {
                return FatalError(ex);
            }
        }

        private static bool FatalError(Exception ex)
        {
            Log.Fatal($"Fatal Error, trace: {ex}");
            return false;
        }

        private string HashString(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}