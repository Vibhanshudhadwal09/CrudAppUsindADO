namespace CrudAppUsindADO.Models
{
    public class TeacherCreateViewModel
    {
        public Teachers Teachers { get; set; }
        public List<Employee> AllStudents { get; set; }
        public List<int> SelectedEmployeeIds { get; set; }
    }
}
