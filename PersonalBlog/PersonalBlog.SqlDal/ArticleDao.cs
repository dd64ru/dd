using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using PersonalBlog.DalContracts;
using PersonalBlog.Entities;

namespace PersonalBlog.SqlDal
{
    public class ArticleDao : IArticleDao
    {
        private readonly string conStr;
        
        public ArticleDao(string connectionString)
        {
            this.conStr = connectionString;
        }

        public void Approve(int id, bool isApproved)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                byte byteIsPublished = 0;
                if (isApproved)
                {
                    byteIsPublished = 1;
                }

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_Approve";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsPublished", byteIsPublished);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int Create(string title, DateTime createDate, string mainImage, string text, bool isPublished)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                byte byteIsPublished = 0;
                if (isPublished)
                {
                    byteIsPublished = 1;
                }

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_Create";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@CreateDate", createDate);
                cmd.Parameters.AddWithValue("@MainImage", mainImage);
                cmd.Parameters.AddWithValue("@Text", text);
                cmd.Parameters.AddWithValue("@IsPublished", byteIsPublished);

                connection.Open();

                return (int)(decimal)cmd.ExecuteScalar(); 
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_Delete";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Edit(int id, string title, string mainImage, string text)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_Edit";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@MainImage", mainImage);
                cmd.Parameters.AddWithValue("@Text", text);

                connection.Open();
                cmd.ExecuteNonQuery();

                return true;
            }
        }

        public IEnumerable<Article> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_GetAll";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int articleId = (int)reader["Id"];
                    yield return new Article
                    {
                        Id = articleId,
                        Title = (string)reader["Title"],
                        CreateDate = (DateTime)reader["CreateDate"],
                        MainImage = (string)reader["MainImage"],
                        Text = (string)reader["Text"],
                        Author = (string)reader["Login"]
                    };
                }
            }
        }

        public IEnumerable<Article> GetAllUserArticles(int userId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                List<int> articlesId = this.GetAllUserArticlesId(userId).ToList();
                List<Article> articlesArray = new List<Article>();

                for (int i = 0; i < articlesId.Count(); i++)
                {
                    articlesArray.Add(this.GetArticle(articlesId[i]));
                }

                return articlesArray.ToArray();
            }
        }

        public IEnumerable<Article> GetUnpublished()
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_GetUnpublished";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Article
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        CreateDate = (DateTime)reader["CreateDate"],
                        MainImage = (string)reader["MainImage"],
                        Text = (string)reader["Text"],
                        Author = (string)reader["Login"]
                    };
                }
            }
        }

        public IEnumerable<Article> Search(string searchRequest)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_Search";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchString", searchRequest);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Article
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                        CreateDate = (DateTime)reader["CreateDate"],
                        MainImage = (string)reader["MainImage"],
                        Text = (string)reader["Text"],
                        Author = (string)reader["Login"]
                    };
                }
            }
        }

        public IEnumerable<Article> SearchByTag(string searchRequest)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_SearchByTag";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SearchString", searchRequest);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int articleId = (int)reader["Id"];
                    yield return new Article
                    {
                        Id = articleId,
                        Title = (string)reader["Title"],
                        CreateDate = (DateTime)reader["CreateDate"],
                        MainImage = (string)reader["MainImage"],
                        Text = (string)reader["Text"],
                        Author = (string)reader["Login"]
                    };
                }
            }
        }

        public Article GetArticleInfoByCommentId(int commentId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_GetArticleInfoByCommentId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CommentId", commentId);

                connection.Open();

                int articleId = (int)cmd.ExecuteScalar();

                return this.GetArticle(articleId);
            }
        }

        public void SetArticleTag(int articleId, int tagId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_SetArticleTag";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ArticleId", articleId);
                cmd.Parameters.AddWithValue("@TagId", tagId);

                connection.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public bool AttachUserToArticle(int userId, int articleId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_AttachUserToArticle";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ArticleId", articleId);

                connection.Open();

                return cmd.ExecuteNonQuery() == 1;
            }
        }

        private IEnumerable<int> GetAllUserArticlesId(int userId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_GetAllUserArticlesId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                List<int> array = new List<int>();
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    array.Add((int)reader["ArticleId"]);
                }

                return array.ToArray();
            }
        }

        private Article GetArticle(int articleId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Article_GetArticle";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", articleId);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Article article = new Article();

                while (reader.Read())
                {
                    article.Id = (int)reader["Id"];
                    article.Title = (string)reader["Title"];
                    article.CreateDate = (DateTime)reader["CreateDate"];
                    article.MainImage = (string)reader["MainImage"];
                    article.Text = (string)reader["Text"];
                }

                return article;
            }
        }
    }
}