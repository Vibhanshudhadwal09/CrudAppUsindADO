using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace CrudAppUsindADO.Models
{
    public class StudentLogin
    {
      
        public string Email { get; set; }
        public string Password { get; set; }

        [DisplayName("Role")]
        public int SelectedRoleId { get; set; }
        public List<SelectListItem> Roles { get; set; }

        public string Salt { get; set; }

    }

    }

