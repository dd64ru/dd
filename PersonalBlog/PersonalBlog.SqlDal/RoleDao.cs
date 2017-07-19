using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using PersonalBlog.DalContracts;
using PersonalBlog.Entities;

namespace PersonalBlog.SqlDal
{
    public class RoleDao : IRoleDao
    {
        private readonly string conStr;

        public RoleDao(string connectionString)
        {
            this.conStr = connectionString;
        }

        public bool AddRoleToUser(int userId, int roleId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Role_AddRoleToUser";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@RoleId", roleId);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public IEnumerable<Role> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Role_GetAll";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    yield return new Role
                    {
                        Id = (int)reader["Id"],
                        Title = (string)reader["Role"],
                    };
                }
            }
        }

        public string[] GetRoles(string username)
        {
            int userId = this.GetUserIdByName(username);
            List<int> rolesId = this.GetRolesByUserId(userId).ToList();
            List<string> rolesTitles = new List<string>();

            for (int i = 0; i < rolesId.Count; i++)
            {
                rolesTitles.Add(this.GetRoleTitleByID(rolesId[i]));
            }

            return rolesTitles.ToArray();
        }

        public bool DeleteUserRole(int userId, string roleTitle)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                int roleId = this.GetRoleIdByTitle(roleTitle);
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Role_DeleteUserRole";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@RoleId", roleId);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public bool AddDefaultRole(int userId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                int defaultRole = 1;

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Role_AddDefaultRole";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@RoleId", defaultRole);

                connection.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        private string GetRoleTitleByID(int roleId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Role_GetRoleTitleByID";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", roleId);

                connection.Open();

                return Convert.ToString(cmd.ExecuteScalar());
            }
        }

        private int GetRoleIdByTitle(string roleTitle)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Role_GetRoleIdByTitle";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Role", roleTitle);

                connection.Open();

                return (int)cmd.ExecuteScalar();
            }
        }

        private IEnumerable<int> GetRolesByUserId(int userId)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Role_GetRolesByUserId";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                List<int> array = new List<int>();
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    array.Add((int)reader["RoleId"]);
                }

                return array.ToArray();
            }
        }

        private int GetUserIdByName(string username)
        {
            using (SqlConnection connection = new SqlConnection(this.conStr))
            {
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "Role_GetUserIdByName";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Login", username);

                connection.Open();

                return (int)cmd.ExecuteScalar();
            }
        }
    }
}