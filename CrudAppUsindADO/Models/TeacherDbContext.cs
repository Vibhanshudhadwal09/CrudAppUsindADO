using System.Data.SqlClient;

namespace CrudAppUsindADO.Models
{
    public class TeacherDbContext
    {
        string cs = "Data Source=localhost\\MSSQLSERVER08; Initial Catalog=ado_db;Integrated Security=True;";

        public List<Teachers> GetTeachers()
        {
            List<Teachers> TeacherList = new List<Teachers>();
            SqlConnection connection = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("select * from Teachers", connection);

            connection.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                Teachers teacher = new Teachers();
                teacher.TeacherId = Convert.ToInt32(reader.GetValue(0).ToString());
                teacher.TeacherName = reader.GetValue(1).ToString();
                teacher.TeacherPhone = reader.GetValue(2).ToString();
                teacher.Address = reader.GetValue(3).ToString();
                teacher.Email = reader.GetValue(5).ToString();

                TeacherList.Add(teacher);
            }
            connection.Close();
            return TeacherList;
        }

        public bool AddTeacher(Teachers teachers)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Teachers (TeacherId,TeacherName,TeacherPhone,TeacherAddress,Email,Password,Salt) VALUES (@TeacherId,@TeacherName,@TeacherPhone,@TeacherAddress,@Email,@Password,@Salt)", connection))
                    {
                        cmd.Parameters.AddWithValue("@TeacherId", teachers.TeacherId);
                        cmd.Parameters.AddWithValue("@TeacherName", teachers.TeacherName.Trim());
                        cmd.Parameters.AddWithValue("@TeacherPhone", teachers.TeacherPhone.Trim());
                        cmd.Parameters.AddWithValue("@TeacherAddress", teachers.Address.Trim());
                        cmd.Parameters.AddWithValue("@Email", teachers.Email.Trim());
                        cmd.Parameters.AddWithValue("@Password", teachers.HashPassword);
                        cmd.Parameters.AddWithValue("@Salt", teachers.Salt);

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

        public bool EditTeacher(Teachers teachers)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Teachers SET TeacherName=@TeacherName,TeacherPhone=@TeacherPhone,TeacherAddress=@TeacherAddress,Email=@Email WHERE TeacherID=@TeacherID", connection))
                    {
                        cmd.Parameters.AddWithValue("@TeacherId", teachers.TeacherId);
                        cmd.Parameters.AddWithValue("@TeacherName", teachers.TeacherName.Trim());
                        cmd.Parameters.AddWithValue("@TeacherPhone", teachers.TeacherPhone.Trim());
                        cmd.Parameters.AddWithValue("@TeacherAddress", teachers.Address.Trim());
                        cmd.Parameters.AddWithValue("@Email", teachers.Email.Trim());
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
        public bool DeleteTeacher(int id)
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM TeacherStudents WHERE TeacherId=@TeacherId", connection))
                {
                    cmd.Parameters.AddWithValue("@TeacherId", id);
                    cmd.ExecuteNonQuery();
                }
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Teachers WHERE TeacherId=@TeacherId ", connection))
                {
                    cmd.Parameters.AddWithValue("@TeacherID", id);
                    
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

        public List<(int StudentId, string TeachersName, string StudentName)> GetTeacherStudents()
        {
            var result = new List<(int StudentId, string TeachersName, string StudentName)>();
            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = @"SELECT 
                         Employees.ID AS StudentId,
                         Employees.Name AS StudentName,
                         Teachers.TeacherName AS TeacherName
                         FROM 
                         TeacherStudents
                         JOIN 
                         Employees ON TeacherStudents.StudentId = Employees.ID
                         JOIN 
                         Teachers ON TeacherStudents.TeacherId = Teachers.TeacherId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int StudentId = Convert.ToInt32(reader["StudentId"]);
                        string TeacherName = reader["TeacherName"].ToString();
                        string StudentName = reader["StudentName"].ToString();
                        result.Add((StudentId, TeacherName, StudentName));
                    }
                }
            }
            return result;
        }



        public List<Employee> GetAllStudents()
        {
            var students = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = "SELECT ID,Name from employees";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employee = new Employee
                        {
                            Id = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString()
                        };

                        students.Add(employee);
                    }
                }
                return students;
            }
        }

        public bool AssignStudentToTeacher(int teacherId, int studentId)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO TeacherStudents (StudentId,TeacherId) VALUES (@StudentId,@TeacherId)", connection))
                    {
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        cmd.Parameters.AddWithValue("@StudentId", studentId);
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
        public bool RemoveStudentsFromTeacher(int teacherId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM TeacherStudents WHERE TeacherId = @TeacherId", connection))
                    {
                        cmd.Parameters.AddWithValue("@TeacherId", teacherId);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
