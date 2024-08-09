namespace CrudAppUsindADO.Models
{
    public class TeachersDetailViewModel
    {
        public Teachers Teacher { get; set; }
        public List<(int StudentId, string TeachersName, string StudentName)> Students { get; set; }
    }
}
