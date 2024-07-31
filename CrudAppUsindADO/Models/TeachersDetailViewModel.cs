namespace CrudAppUsindADO.Models
{
    public class TeachersDetailViewModel
    {
        public Teachers Teacher { get; set; }
        public List<(string TeacherName,string StudentName)> Students { get; set; }
    }
}
