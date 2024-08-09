using System.ComponentModel.DataAnnotations;

namespace CrudAppUsindADO.Models
{
    public class EmployeeImageViewModel
    {

        [Required(ErrorMessage = "Id is must")]
        public int Id { get; set; }

        [Required(ErrorMessage = " Please Enter  Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please  Enter the city ")]
        [MaxLength(100)]
        public string City { get; set; }

        [Required(ErrorMessage = "Please Enter the Picode")]
        public int Pincode { get; set; }

        [Required(ErrorMessage = "Enter the Email")]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Enter Valid Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter the Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; }

        public IFormFile Photo { get; set; }

    }
}
