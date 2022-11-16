using EmployeesAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeesAPI.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task<Employee> GetEmployee(int employeeId);
        Task<IEnumerable<Employee>> GetEmployeesByBoss(int bossId);
        Task<IEnumerable<Employee>> GetEmployeesByNameAndBirthdayDateInterval(string name, DateTime from, DateTime to);
        Task<EmployeeRoleInfo> GetRoleInfo(string role);
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> UpdateEmployee(int id, Employee employee);
        Task<Employee> UpdateEmployeeSalary(int employeeId, decimal salary);
        Task DeleteEmployee(int id);
    }
}
