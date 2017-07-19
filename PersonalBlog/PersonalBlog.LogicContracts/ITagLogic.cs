using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.LogicContracts
{
    public interface ITagLogic
    {
        bool Create(string title);

        bool Edit(string id, string title);

        bool Delete(string id);

        IEnumerable<Tag> GetAll();

        IEnumerable<Tag> GetArticleTags(int id);
    }
}