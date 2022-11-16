using EmployeesAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeesAPI.Data
{
    public class EmployeeValidationAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var emp = (Employee)value;

            //FirstName and LastName validation
            if(emp.FirstName == emp.LastName)
            {
                return new ValidationResult(ErrorMessage ?? "FirstName and LastName must be different");
            }

            //Employee age validation (18-70)
            var age = DateTime.Today.Year - emp.Birthdate.Year;
            if (age < 18 || age > 70)
            {
                return new ValidationResult(ErrorMessage ?? "Employee age invalid (18-70)");
            }

            //EmploymentDate Validation
            DateTime from = new DateTime(2000, 01, 01);
            if (emp.EmploymentDate < from || emp.EmploymentDate > DateTime.UtcNow)
            {
                return new ValidationResult(ErrorMessage ?? "Make sure your date is greater than 2000-01-01 and is not future");
            }

            //Salary validation
            if (emp.CurrentSalary < 0)
            {
                return new ValidationResult(ErrorMessage ?? "Salary should be greater than 0");
            }

            //CEO role has no boss
            if (emp.Role.ToLower() == "ceo" && emp.BossId > 0)
            {
                return new ValidationResult(ErrorMessage ?? "CEO cannot have boss");
            }

            return ValidationResult.Success;
        }
    }

}
