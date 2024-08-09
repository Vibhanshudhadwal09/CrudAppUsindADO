using Microsoft.AspNetCore.Identity;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Transactions;
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
                emp.Email = dr.GetValue(4).ToString();
                emp.ImagePath = dr.GetValue(7).ToString();

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


                    if (EmployeeExist(emp.Id))
                    {
                        return false;
                    }

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO employees (ID, Name, City, Pincode,Email,Password,Image,Salt) VALUES (@ID, @Name, @City, @Pincode,@Email,@Password,@Image,@Salt)", connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", emp.Id);
                        cmd.Parameters.AddWithValue("@Name", emp.Name.Trim());
                        cmd.Parameters.AddWithValue("@City", emp.City.Trim());
                        cmd.Parameters.AddWithValue("@Pincode", emp.Pincode);
                        cmd.Parameters.AddWithValue("@Email", emp.Email.Trim());
                        cmd.Parameters.AddWithValue("@Password", emp.HashPassword);
                        cmd.Parameters.AddWithValue("@Salt", emp.Salt);
                        cmd.Parameters.AddWithValue("@Image", emp.ImagePath);
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
        public List<Subject> GetAllSubjects()
        {
            var subjects = new List<Subject>();
            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = "SELECT SubjectId,SubjectName from Subjects";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var subject = new Subject
                        {
                            SubjectID = Convert.ToInt32(reader["SubjectID"]),
                            SubjectName = reader["SubjectName"].ToString(),
                        };
                        subjects.Add(subject);
                    }
                }
                return subjects;
            }
        }

        public bool AssignSubjectToEmployee(int employeeId, int subjectId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO EmployeeSubjects (EmployeeId,SubjectId) VALUES (@EmployeeID,@SubjectId)", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", employeeId);
                        cmd.Parameters.AddWithValue("@SubjectId", subjectId);
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
            catch (Exception ex) { return false; }
        }

        public bool UpdateEmployee(Employee emp)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE employees SET Name=@Name,City=@City,PinCode=@Pincode,Email=@Email WHERE ID=@ID", connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", emp.Id);
                        cmd.Parameters.AddWithValue("@Name", emp.Name);
                        cmd.Parameters.AddWithValue("@City", emp.City);
                        cmd.Parameters.AddWithValue("@Pincode", emp.Pincode);
                        cmd.Parameters.AddWithValue("@Email", emp.Email);

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
               connection.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM EmployeeSubjects WHERE EmployeeId=@EmployeeId", connection))
                {
                    cmd.Parameters.AddWithValue("@EmployeeId", id);
                    cmd.ExecuteNonQuery();
                }
               
                using (SqlCommand cmd=new SqlCommand("Delete From TeacherStudents  where StudentId=@StudentId ",connection))
                {
                    cmd.Parameters.AddWithValue("@StudentId", id);
                    cmd.ExecuteNonQuery();
                }
               
                using (SqlCommand cmd = new SqlCommand("DELETE FROM employees WHERE ID=@ID", connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

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

        public bool DeleteEmployeeSubjects(int employeeId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM EmployeeSubjects WHERE EmployeeId=@EmployeeId", connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeId", employeeId);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public bool EmployeeExist(int id)
        {
            string query = "Select COUNT(*) FROM employees WHERE ID=@ID";
            using(SqlConnection connection = new SqlConnection(cs))
            {
                using(SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    connection.Open();
                    int count=(int)cmd.ExecuteScalar();
                    connection.Close();
                    return count > 0;
                }
            }
        }


    }
}