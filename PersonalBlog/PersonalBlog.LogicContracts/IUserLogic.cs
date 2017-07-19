using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.LogicContracts
{
    public interface IUserLogic
    {
        bool Create(string login, string password, string mainPhoto);

        bool CanLogin(string login, string password);

        bool Edit(string id, string userImage);

        bool Delete(string id);

        bool IsLoginFree(string login);

        User GetCurrentUser(string login);

        IEnumerable<User> GetAll();
    }
}