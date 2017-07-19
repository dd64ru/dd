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
    public class TagLogic : ITagLogic
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITagDao tagDao;

        public TagLogic()
        {
            string conStr = ConfigurationManager.ConnectionStrings["PersonalBlog"].ConnectionString;
            this.tagDao = new TagDao(conStr);
        }

        public bool Create(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Log.Error("Creating tag failed");
                return false;
            }

            try
            {
                return this.tagDao.Create(title);
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
                    return this.tagDao.Delete(intId);
                }
                catch (Exception ex) 
                {
                    return FatalError(ex);
                }
            }

            Log.Error("Deleting tag failed");
            return false;
        }

        public bool Edit(string id, string title)
        {
            if (int.TryParse(id, out int intId) && !string.IsNullOrWhiteSpace(title))
            {
                try
                {
                    return this.tagDao.Edit(intId, title);
                }
                catch (Exception ex)
                {
                    return FatalError(ex);
                }   
            }

            Log.Error("Editing tag failed");
            return false;
        }

        public IEnumerable<Tag> GetAll()
        {
            try
            {
                return this.tagDao.GetAll().ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Tag>();
            } 
        }

        public IEnumerable<Tag> GetArticleTags(int id)
        {
            try
            {
                return this.tagDao.GetArticleTagsId(id).ToArray();
            }
            catch (Exception ex)
            {
                FatalError(ex);
                return new List<Tag>();
            }
        }

        private static bool FatalError(Exception ex)
        {
            Log.Fatal($"Fatal Error, trace: {ex}");
            return false;
        }
    }
}