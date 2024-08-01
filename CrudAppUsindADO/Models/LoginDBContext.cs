using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Data.SqlClient;

namespace CrudAppUsindADO.Models
{
    public class LoginDBContext
    {
        string cs = "Data Source=localhost\\MSSQLSERVER08; Initial Catalog=ado_db;Integrated Security=True;";

        public bool AddUser(Login user)

        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO LoginDB (username,Email,Password) VALUES (@username,@Email,@Password)"))
                    {
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password);

                        connection.Open();
                        int i = cmd.ExecuteNonQuery();
                        connection.Close();
                        if (i > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            catch
            {
                return false;
            }
        }

        public Login GetUserByEmail(string email)
        {
            Login user = null;
            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = "SELECT username,Email,Password FROM LoginDB WHERE Email=@Email";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Login
                        {
                            Username = reader["username"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString()
                        };
                    }

                }
            }
            return user;
        }


        public bool VerifyLogin(string email, string password) {

            var user = GetUserByEmail(email);
            return user != null && user.Password == password;
        }
    }
}