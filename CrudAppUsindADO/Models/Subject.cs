using System.ComponentModel.DataAnnotations;

namespace CrudAppUsindADO.Models
{
    public class Subject
    {
        public int SubjectID { get; set; }
        [Required]
        public string SubjectName { get; set; }
    }
}
