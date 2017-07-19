using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using log4net;
using PersonalBlog.DalContracts;
using PersonalBlog.Entities;

namespace PersonalBlog.SqlDal
{
    public class CommentDao : ICommentDao
    {
        private readonly string conStr;
        
        public CommentDao(string connectionString)
        {
            this.conStr = connectionString;
        }

        public void Approve(int commentId, bool isApproved)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                byte byteIsPublished = 0;
                if (isApproved)
                {
                    byteIsPublished = 1;
                }

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Comment_Approve";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", commentId);
                cmd.Parameters.AddWithValue("@IsPublished", byteIsPublished);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool Create(int userId, int articleId, string title, string text, DateTime createDate, bool isPublished)
        {
            int commentId = this.CreateComment(title, text, createDate, isPublished);
            return this.AddCommentToArticle(userId, articleId, commentId);
        }

        public bool Delete(int commentId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Comment_Delete";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", commentId);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public IEnumerable<Comment> GetArticleComments(int articleId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Comment_GetArticleComments";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ArticleId", articleId);

                List<Comment> comments = new List<Comment>();

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int commentId = (int)reader["CommentId"];
                    int userId = (int)reader["UserId"];

                    comments.Add(this.GetCommentById(commentId, userId));
                }

                return comments;
            }
        }

        public IEnumerable<Comment> GetUnpublished()
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Comment_GetUnpublished";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int commentId = (int)reader["Id"];
                    yield return new Comment
                    {
                        Id = commentId,
                        Title = (string)reader["Title"],
                        CreateDate = (DateTime)reader["CreateDate"],
                        Text = (string)reader["Text"]
                    };
                }
            }
        }

        private Comment GetCommentById(int commentId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Comment_GetCommentById";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CommentId", commentId);
                cmd.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Comment comment = new Comment();

                while (reader.Read())
                {
                    comment.Id = (int)reader["Id"];
                    comment.Title = (string)reader["Title"];
                    comment.Text = (string)reader["Text"];
                    comment.CreateDate = (DateTime)reader["CreateDate"];
                    comment.Author = new User
                    {
                        Id = (int)reader["UserId"],
                        Name = (string)reader["UserLogin"],
                        UserPhoto = (string)reader["UserPhoto"]
                    };
                }

                return comment;
            }
        }

        private int CreateComment(string title, string text, DateTime createDate, bool isPublished)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                byte byteIsPublished = 0;
                if (isPublished)
                {
                    byteIsPublished = 1;
                }

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Comment_CreateComment";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Text", text);
                cmd.Parameters.AddWithValue("@CreateDate", createDate);
                cmd.Parameters.AddWithValue("@IsPublished", byteIsPublished);

                connection.Open();
                return (int)(decimal)cmd.ExecuteScalar();
            }
        }

        private bool AddCommentToArticle(int userId, int articleId, int commentId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Comment_AddCommentToArticle";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ArticleId", articleId);
                cmd.Parameters.AddWithValue("@CommentId", commentId);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }
    }
}