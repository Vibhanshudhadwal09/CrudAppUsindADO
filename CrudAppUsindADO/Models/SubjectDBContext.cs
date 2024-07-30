using System.Data;
using System.Data.SqlClient;

namespace CrudAppUsindADO.Models
{
    public class SubjectDBContext
    {
        string cs = "Data Source=localhost\\MSSQLSERVER08; Initial Catalog=ado_db;Integrated Security=True;";

        public List<Subject> GetSubjects()
        {
            List<Subject> SubjectList = new List<Subject>();
            SqlConnection connection = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("Select * from Subjects", connection);

            connection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Subject sub = new Subject();
                sub.SubjectID = Convert.ToInt32(dr.GetValue(0).ToString());
                sub.SubjectName = dr.GetValue(1).ToString();
                SubjectList.Add(sub);

            }
            connection.Close();
            return SubjectList;
        }

        public bool AddSubject(Subject sub)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(cs))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Subjects (SubjectId,SubjectName) VALUES (@SubjectId, @SubjectName)", connection))
                    {
                        cmd.Parameters.AddWithValue("@SubjectId", sub.SubjectID);
                        cmd.Parameters.AddWithValue("@SubjectName", sub.SubjectName);

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

        public bool UpdateSubject(Subject sub)
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE Subjects SET SubjectName=@SubjectName WHERE SubjectId=@SubjectId", connection))
                {
                    cmd.Parameters.AddWithValue("@SubjectId", sub.SubjectID);
                    cmd.Parameters.AddWithValue("@SubjectName", sub.SubjectName);

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

        public bool DeleteSubjects(int id )
        {
            using (SqlConnection connection = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Subjects WHERE SubjectId=@SubjectId", connection))
                {
                    cmd.Parameters.AddWithValue("@SubjectId", id);
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

    }
}
