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
    public class ArticleLogic : IArticleLogic
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IArticleDao articleDao;
        private readonly IUserDao userDao;
        
        public ArticleLogic()
        {
            string conStr = ConfigurationManager.ConnectionStrings["PersonalBlog"].ConnectionString;
            this.articleDao = new ArticleDao(conStr);
            this.userDao = new UserDao(conStr);
        }

        public void Approve(string id, bool isApproved)
        {
            if (int.TryParse(id, out int intId))
            {
                try
                {
                    this.articleDao.Approve(intId, isApproved);
                }
                catch (Exception ex)
                {
                    FatalError(ex);
                }   
            }
            else
            {
                Log.Error("Approving article failed");
            }
        }

        public bool Create(string userLogin, string title, DateTime createDate, string mainImage, string text, string[] tags, bool isPublished)
        {
            if (string.IsNullOrWhiteSpace(userLogin) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(mainImage) || string.IsNullOrWhiteSpace(text) || createDate == null)
            {
                Log.Error("Create article failed");
                return false;
            }

            int[] tagsId = new int[tags.Length];

            for (int i = 0; i < tags.Length; i++)
            {
                tagsId[i] = Convert.ToInt32(tags[i]);
            }

            try
            {
                User user = this.userDao.GetCurrentUser(userLogin);
                int articleId = this.articleDao.Create(title, createDate, mainImage, text, isPublished);

                for (int i = 0; i < tagsId.Length; i++)
                {
                    this.articleDao.SetArticleTag(articleId, tagsId[i]);
                }

                this.articleDao.AttachUserToArticle(user.Id, articleId);

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
                    return this.articleDao.Delete(intId);
                }
                catch (Exception ex)
                {
                    return FatalError(ex);
                } 
            }

            Log.Error("Delete failed");
            return false;
        }

        public bool Edit(string id, string title, string mainImage, string text, string[] tags)
        {
            if (!string.IsNullOrWhiteSpace(title) || !string.IsNullOrWhiteSpace(mainImage) || !string.IsNullOrWhiteSpace(text))
            {
                if (int.TryParse(id, out int intId))
                {
                    int[] tagsId = tags.Select(int.Parse).ToArray();
                    try
                    {
                        this.articleDao.Edit(intId, title, mainImage, text);
                        for (int i = 0; i < tagsId.Length; i++)
                        {
                            this.articleDao.SetArticleTag(intId, tagsId[i]);
                        }

                        return true;
                    }
                    catch (Exception ex)
                    {
                        return FatalError(ex);
                    }
                }
            }

            Log.Error("Edit article failed");
            return false;
        }

        public IEnumerable<Article> GetAll()
        {
            try
            {
                return this.articleDao.GetAll().ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Article>();
            }  
        }

        public IEnumerable<Article> GetAllUserArticles(int userId)
        {
            try
            {
                return this.articleDao.GetAllUserArticles(userId).ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Article>();
            } 
        }

        public Article GetArticleInfoByCommentId(int commentId)
        {
            try
            {
                return this.articleDao.GetArticleInfoByCommentId(commentId);
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new Article();
            }
        }

        public IEnumerable<Article> GetUnpublished()
        {
            try
            {
                return this.articleDao.GetUnpublished().ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Article>();
            }
        }

        public IEnumerable<Article> Search(string searchRequest)
        {
            if (searchRequest == string.Empty)
            {
                return new List<Article>();
            }

            try
            {
                return this.articleDao.Search(searchRequest).ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Article>();
            }
        }

        public IEnumerable<Article> SearchByTag(string searchRequest)
        {
            if (searchRequest == string.Empty)
            { 
                return new List<Article>();
            }

            try
            {
                return this.articleDao.SearchByTag(searchRequest).ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Article>();
            }
        }

        private static bool FatalError(Exception ex)
        {
            Log.Fatal($"Fatal Error, trace: {ex}");
            return false;
        }
    }
}