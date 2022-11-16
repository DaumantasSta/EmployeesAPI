namespace EmployeesAPI.Models
{
    public class EmployeeRoleInfo
    {
        public int EmployeesCount { get; set; }
        public decimal AverageSalary { get; set; }

        public EmployeeRoleInfo(int EmployeesCount, decimal AverageSalary)
        {
            this.EmployeesCount = EmployeesCount;
            this.AverageSalary = AverageSalary;
        }

    }
}
