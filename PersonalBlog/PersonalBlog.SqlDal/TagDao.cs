using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using PersonalBlog.DalContracts;
using PersonalBlog.Entities;

namespace PersonalBlog.SqlDal
{
    public class TagDao : ITagDao
    {
        private readonly string conStr;

        public TagDao(string connectionString)
        {
            this.conStr = connectionString;
        }

        public bool Create(string title)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Tag_Create";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title", title);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Tag_Delete";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool Edit(int id, string title)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Tag_Edit";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Title", title);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public IEnumerable<Tag> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Tag_GetAll";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Tag
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Title"],
                    };
                }
            }
        }

        public IEnumerable<Tag> GetArticleTagsId(int articleId)
        {
            int[] tagsIdArray = this.GetTagsArrayByArticleId(articleId).ToArray();

            List<Tag> tags = new List<Tag>();
            for (int i = 0; i < tagsIdArray.Length; i++)
            {
                tags.Add(this.GetTagById(tagsIdArray[i]));
            }

            return tags;
        }

        private IEnumerable<int> GetTagsArrayByArticleId(int articleId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Tag_GetTagsArrayByArticleId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ArticleId", articleId);

                connection.Open();
                List<int> array = new List<int>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    array.Add((int)reader["TagId"]);
                }

                return array.ToArray();
            }
        }

        private Tag GetTagById(int tagId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Tag_GetTagById";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", tagId);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Tag tag = new Tag();

                while (reader.Read())
                {
                    tag.Id = (int)reader["Id"];
                    tag.Title = (string)reader["Title"];
                }

                return tag;
            }
        }
    }
}