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
    public class CommentLogic : ICommentLogic
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ICommentDao commentDao;

        public CommentLogic()
        {
            string conStr = ConfigurationManager.ConnectionStrings["PersonalBlog"].ConnectionString;
            this.commentDao = new CommentDao(conStr);
        }

        public void Approve(string commentId, bool isApproved)
        {
            if (int.TryParse(commentId, out int intCommentId))
            {
                try
                {
                    this.commentDao.Approve(intCommentId, isApproved);
                }
                catch (Exception ex)
                {
                    FatalError(ex);
                }  
            }
            else
            {
                Log.Error("Approving comment failed");
            }
        }

        public bool Create(int userId, string articleId, string title, string text, DateTime createDate, bool isPublished)
        {
            if (string.IsNullOrWhiteSpace(articleId) || string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(text) || createDate == null)
            {
                Log.Error("Creating comment failed");
                return false;
            }

            if (int.TryParse(articleId, out int intArticleId))
            {
                try
                {
                    return this.commentDao.Create(userId, intArticleId, title, text, createDate, isPublished);
                }
                catch (Exception ex)
                {
                    return FatalError(ex);
                }
            }

            return false;
        }

        public bool Delete(string commentId)
        {
            if (int.TryParse(commentId, out int intCommentId))
            {
                try
                {
                    return this.commentDao.Delete(intCommentId);
                }
                catch (Exception ex)
                {
                    return FatalError(ex);
                }  
            }

            Log.Error("Delete comment failed");
            return false;
        }

        public IEnumerable<Comment> GetArticleComments(int articleId)
        {
            try
            {
                return this.commentDao.GetArticleComments(articleId).ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Comment>();
            }  
        }

        public IEnumerable<Comment> GetUnpublished()
        {
            try
            {
                return this.commentDao.GetUnpublished().ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Comment>();
            }
        }

        private static bool FatalError(Exception ex)
        {
            Log.Fatal($"Fatal Error, trace: {ex}");
            return false;
        }
    }
}