using System;
using System.Collections.Generic;
using PersonalBlog.Entities;

namespace PersonalBlog.DalContracts
{
    public interface IArticleDao
    {
        int Create(string title, DateTime createDate, string mainImage, string text, bool isPublished);

        bool Edit(int id, string title, string mainImage, string text);

        bool Delete(int id);

        void Approve(int id, bool isApproved);

        void SetArticleTag(int articleId, int tagId);

        bool AttachUserToArticle(int userId, int articleId);

        Article GetArticleInfoByCommentId(int commentId);

        IEnumerable<Article> GetAll();

        IEnumerable<Article> GetUnpublished();

        IEnumerable<Article> GetAllUserArticles(int userId);

        IEnumerable<Article> Search(string searchRequest);

        IEnumerable<Article> SearchByTag(string searchRequest);
    }
}