using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreWebApp.Models
{
    [Table("TblEmployee")]
    public class EmployeeModel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9'-.\s]{1,50}$", ErrorMessage = "Name format is invalid.")]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        public string Office { get; set; }
        [Required]

        public string StartDate { get; set; }
        [RegularExpression(@"^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$", ErrorMessage = "Salary format is invalid.")]
        public string Salary { get; set; }
    }
}
