using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.DalContracts
{
    public interface IUserDao
    {
        int Create(string login, string password, string mainPhoto);

        bool CanLogin(string login, string password);

        bool Edit(int id, string userImage);

        bool Delete(int id);

        bool IsLoginFree(string login);

        User GetCurrentUser(string login);

        IEnumerable<User> GetAll();

        User GetUserById(int id);

        string GetUserLoginById(int userId);
    }
}