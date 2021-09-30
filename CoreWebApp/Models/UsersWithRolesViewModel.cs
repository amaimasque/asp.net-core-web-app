using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApp.Models
{
    public class UsersWithRolesViewModel
    {
        [Display(Name = "User ID")]
        [Column("Id")]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [Display(Name = "Role")]
        public string SelectedRole { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
