using System;
using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.LogicContracts
{
    public interface ICommentLogic
    {
        bool Create(int userId, string articleId, string title, string text, DateTime createDate, bool isPublished);

        bool Delete(string commentId);

        void Approve(string commentId, bool isApproved);

        IEnumerable<Comment> GetArticleComments(int articleId);

        IEnumerable<Comment> GetUnpublished();
    }
}