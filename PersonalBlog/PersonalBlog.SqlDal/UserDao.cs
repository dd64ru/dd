using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using log4net;
using PersonalBlog.DalContracts;
using PersonalBlog.Entities;

namespace PersonalBlog.SqlDal
{
    public class UserDao : IUserDao
    {
        private readonly string conStr;

        public UserDao(string connectionString)
        {
            this.conStr = connectionString;
        }

        public bool CanLogin(string login, string password)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_CanLogin";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@Password", password);

                connection.Open();
                return (int)cmd.ExecuteScalar() != 0;
            }
        }

        public int Create(string login, string password, string mainPhoto)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_Create";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", login);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Image", mainPhoto);

                connection.Open();

                return (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_Delete";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Edit(int id, string userImage)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_Edit";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Image", userImage);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public IEnumerable<User> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_GetAll";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yield return new User
                    {
                        Id = (int)reader["Id"],
                        Name = (string)reader["Login"],
                        UserPhoto = (string)reader["MainImage"],
                    };
                }
            }
        }

        public User GetCurrentUser(string login)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_GetCurrentUser";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", login);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                User user = new User();

                while (reader.Read())
                {
                    user.Id = (int)reader["Id"];
                    user.Name = (string)reader["Login"];
                    user.UserPhoto = (string)reader["MainImage"];
                }

                return user;
            }
        }

        public User GetUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_GetUserById";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                User user = new User();

                while (reader.Read())
                {
                    user.Id = (int)reader["Id"];
                    user.Name = (string)reader["Login"];
                    user.UserPhoto = (string)reader["MainImage"];
                }

                return user;
            }
        }

        public bool IsLoginFree(string login)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_IsLoginFree";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", login);

                connection.Open();
                int result = (int)cmd.ExecuteScalar();

                return result == 0;
            }
        }

        public string GetUserLoginById(int userId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "User_GetUserLoginById";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", userId);

                connection.Open();

                return Convert.ToString(cmd.ExecuteScalar());  
            }
        }
    }
}