using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace CrudAppUsindADO.Models
{
    public class StudentLoginDBContext
    {
        string cs = "Data Source=localhost\\MSSQLSERVER08; Initial Catalog=ado_db;Integrated Security=True;";

        public StudentLogin GetUserByEmailAndRole(string email, int roleId)
        {
            StudentLogin user = null;
            string query = roleId
                switch
            {
                1 => "SELECT Email, Password, RolesId,Salt FROM LoginDB WHERE Email = @Email",
                2 => "SELECT Email, Password, RolesId,Salt FROM Employees WHERE Email = @Email",
                3 => "SELECT Email, Password, RolesId,Salt FROM Teachers WHERE Email = @Email",
                _ => null
            };

            if (query == null)
            {
                return null;
            }

            using (SqlConnection connection = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Email", email);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new StudentLogin
                        {
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Salt= reader["Salt"].ToString(),
                            SelectedRoleId = Convert.ToInt32(reader["RolesId"].ToString())
                        };
                    }
                }
            }
            return user;
        }

        public List<SelectListItem> GetRoles()
        {
            List<SelectListItem> roles = new List<SelectListItem>();
            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = "SELECT RolesId, RolesName FROM Roles";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new SelectListItem
                        {
                            Value = reader["RolesId"].ToString(),
                            Text = reader["RolesName"].ToString()
                        });
                    }
                }
            }
            return roles;
        }
    }
}
