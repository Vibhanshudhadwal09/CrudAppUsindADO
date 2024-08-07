﻿using System.ComponentModel.DataAnnotations;

namespace CrudAppUsindADO.Models
{
    public class Teachers
    {
    
        public int TeacherId { get; set; }

        [Required(ErrorMessage = " Please Enter  Name")]
        [MaxLength(50)]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Please enter the Phone")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be exactly 10 digits.")]
        [MaxLength(10)]
        public string TeacherPhone  { get; set; }

        [Required(ErrorMessage ="Please enter the Address")]
        public string Address { get; set; }

        [Required(ErrorMessage ="Please Enter the Email")]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Enter Valid Mail")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please Enter the Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string Password { get; set; }
        public string HashPassword { get; set; }
        public string Salt { get; set; }
    }
}
