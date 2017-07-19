using System;
using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.DalContracts
{
    public interface ICommentDao
    {
        bool Create(int userId, int articleId, string title, string text, DateTime createDate, bool isPublished);

        bool Delete(int commentId);

        void Approve(int commentId, bool isApproved);

        IEnumerable<Comment> GetArticleComments(int articleId);

        IEnumerable<Comment> GetUnpublished();
    }
}