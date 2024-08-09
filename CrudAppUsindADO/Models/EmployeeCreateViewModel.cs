using System.ComponentModel.DataAnnotations;

namespace CrudAppUsindADO.Models
{
    public class EmployeeCreateViewModel
    {
        public Employee Employee { get; set; }

        public EmployeeImageViewModel Employee1 { get; set; }
        
        public List<Subject> AllSubjects { get; set; }
        
        public List<int> SelectedSubjectIds { get; set; }
         
    }
}
