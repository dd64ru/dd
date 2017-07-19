using System;
using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.LogicContracts
{
    public interface IArticleLogic
    {
        bool Create(string userLogin, string title, DateTime createDate, string mainImage, string text, string[] tags, bool isPublished);

        bool Edit(string id, string title, string mainImage, string text, string[] tags);

        bool Delete(string id);

        void Approve(string id, bool isApproved);

        Article GetArticleInfoByCommentId(int commentId);

        IEnumerable<Article> GetAll();

        IEnumerable<Article> GetUnpublished();

        IEnumerable<Article> GetAllUserArticles(int userId);

        IEnumerable<Article> Search(string searchRequest);

        IEnumerable<Article> SearchByTag(string searchRequest);
    }
}