using EmployeesAPI.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Models
{
    [EmployeeValidationAttribute]
    public class Employee
    {
        [Required]
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        public DateTime EmploymentDate { get; set; }

        public int BossId { get; set; }

        [Required]
        public string HomeAddress { get; set; }

        [Required]
        public decimal CurrentSalary { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
