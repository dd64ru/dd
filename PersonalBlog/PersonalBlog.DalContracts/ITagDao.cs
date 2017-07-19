using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.DalContracts
{
    public interface ITagDao
    {
        bool Create(string title);

        bool Edit(int id, string title);

        bool Delete(int id);

        IEnumerable<Tag> GetAll();

        IEnumerable<Tag> GetArticleTagsId(int articleId);
    }
}