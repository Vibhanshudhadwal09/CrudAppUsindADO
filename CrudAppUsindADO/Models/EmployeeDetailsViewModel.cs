namespace CrudAppUsindADO.Models
{
    public class EmployeeDetailsViewModel
    {
        public Employee Employee { get; set; }
        public List<(string EmployeeName, string SubjectName)> Subjects
        {
            get; set;
        }
    }
}
