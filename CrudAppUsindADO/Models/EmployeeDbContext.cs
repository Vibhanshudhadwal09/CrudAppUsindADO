using Microsoft.AspNetCore.Identity;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace CrudAppUsindADO.Models
{
    public class EmployeeDbContext
    {
        string cs = "Data Source=localhost\\MSSQLSERVER08; Initial Catalog=ado_db;Integrated Security=True;";
        public List<Employee> GetEmployees()
        {
            List<Employee> EmployeeList = new List<Employee>();
            SqlConnection connection = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("select * from employees", connection);

            connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Employee emp = new Employee();
                emp.Id = Convert.ToInt32(dr.GetValue(0).ToString());
                emp.Name = dr.GetValue(1).ToString();
                emp.City = dr.GetValue(2).ToString();
                emp.Pincode = Convert.ToInt32(dr.GetValue(3).ToString());

                EmployeeList.Add(emp);
            }
            connection.Close();

            return EmployeeList;
        }


        public bool AddEmployee(Employee emp)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO employees (ID, Name, City, Pincode) VALUES (@ID, @Name, @City, @Pincode)", connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", emp.Id);
                        cmd.Parameters.AddWithValue("@Name", emp.Name);
                        cmd.Parameters.AddWithValue("@City", emp.City);
                        cmd.Parameters.AddWithValue("@Pincode", emp.Pincode);

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
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateEmployee(Employee emp)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE employees SET Name=@Name,City=@City,PinCode=@Pincode WHERE ID=@ID", connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", emp.Id);
                        cmd.Parameters.AddWithValue("@Name", emp.Name);
                        cmd.Parameters.AddWithValue("@City", emp.City);
                        cmd.Parameters.AddWithValue("@Pincode", emp.Pincode);

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
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteEmployee(int id)
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM employees WHERE ID=@ID", connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    connection.Open();
                    int i = cmd.ExecuteNonQuery();
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
        public List<(string EmployeeName, string SubjectName)> GetEmployeeSubjects()
        {
            var results = new List<(string EmployeeName, string SubjectName)>();

            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = @"
                    SELECT 
                        Employees.Name AS EmployeeName,
                        Subjects.SubjectName AS SubjectName
                    FROM 
                        EmployeeSubjects
                    JOIN 
                        Employees ON EmployeeSubjects.EmployeeId = Employees.Id
                    JOIN 
                        Subjects ON EmployeeSubjects.SubjectID = Subjects.SubjectID;";

                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string employeeName = reader["EmployeeName"].ToString();
                        string subjectName = reader["SubjectName"].ToString();
                        results.Add((employeeName, subjectName));
                    }
                }
            }

            return results;
        }

    }
}