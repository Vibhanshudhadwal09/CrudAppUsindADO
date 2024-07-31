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
                teacher.TeacherPhone = Convert.ToInt32(reader.GetValue(2).ToString());
                teacher.Address = reader.GetValue(3).ToString();

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
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Teachers (TeacherId,TeacherName,TeacherPhone,TeacherAddress) VALUES (@TeacherId,@TeacherName,@TeacherPhone,@TeacherAddress)", connection))
                    {
                        cmd.Parameters.AddWithValue("@TeacherId", teachers.TeacherId);
                        cmd.Parameters.AddWithValue("@TeacherName", teachers.TeacherName);
                        cmd.Parameters.AddWithValue("@TeacherPhone", teachers.TeacherPhone);
                        cmd.Parameters.AddWithValue("@TeacherAddress", teachers.Address);

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
                    using (SqlCommand cmd = new SqlCommand("UPDATE Teachers SET TeacherName=@TeacherName,TeacherPhone=@TeacherPhone,TeacherAddress=@TeacherAddress WHERE TeacherID=@TeacherID", connection))
                    {
                        cmd.Parameters.AddWithValue("@TeacherId", teachers.TeacherId);
                        cmd.Parameters.AddWithValue("@TeacherName", teachers.TeacherName);
                        cmd.Parameters.AddWithValue("@TeacherPhone", teachers.TeacherPhone);
                        cmd.Parameters.AddWithValue("@TeacherAddress", teachers.Address);
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

        public List<(string TeachersName, string StudentName)> GetTeacherStudents()
        {
            var result = new List<(string TeachersName, string StudentName)>();
            using (SqlConnection connection = new SqlConnection(cs))
            {
                string query = @" SELECT 
                Employees.Name AS StudentName,
                Teachers.TeacherName AS TeacherName
                FROM 
                TeacherStudents
                JOIN 
                Employees ON TeacherStudents.StudentId = Employees.Id
                JOIN 
                Teachers ON TeacherStudents.TeacherId = Teachers.TeacherId;";
                SqlCommand cmd = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string TeacherName = reader["TeacherName"].ToString();
                        string StudentName = reader["StudentName"].ToString();
                        result.Add((TeacherName, StudentName));
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
    }
}
