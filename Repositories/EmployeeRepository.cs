using EmployeesAPI.Models;
using EmployeesAPI.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesAPI.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public EmployeeRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
            _apiDbContext.Database.EnsureCreated();
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _apiDbContext.Employee.ToListAsync();
        }

        public async Task<Employee> GetEmployee(int employeeId)
        {
            return await _apiDbContext.Employee.AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeID == employeeId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByBoss(int bossId)
        {
            var employeesByBoss = await _apiDbContext.Employee.Where(x => x.BossId == bossId).ToListAsync();
            return employeesByBoss;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByNameAndBirthdayDateInterval(string name, DateTime from, DateTime to)
        {
            var employees = await _apiDbContext.Employee
                    .Where(x => x.FirstName == name && x.Birthdate > from && x.Birthdate < to).ToListAsync();

            return employees;
        }

        public async Task<EmployeeRoleInfo> GetRoleInfo(string role)
        {
            var employeeCountByRole = _apiDbContext.Employee.Where(x => x.Role == role).Count();

            if (employeeCountByRole == 0)
                return null;

            var averageSalaryByRole = await _apiDbContext.Employee.Where(x => x.Role == role).AverageAsync(x => x.CurrentSalary);
            var employeeRoleInfo = new EmployeeRoleInfo(employeeCountByRole, averageSalaryByRole);

            return employeeRoleInfo;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            if(employee.Role.ToLower() == "ceo")
            {
                var ceoExist = await _apiDbContext.Employee.AnyAsync(x => x.Role.ToLower() == "ceo");

                if (ceoExist)
                    throw new ValidationException($"Only one CEO is allowed");
            }

            var result = await _apiDbContext.Employee.AddAsync(employee);
            await _apiDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Employee> UpdateEmployee(int id, Employee employee)
        {
            var employeeExist = await _apiDbContext.Employee.AnyAsync(x => x.EmployeeID == id);
            
            var ceoExist = await _apiDbContext.Employee.AnyAsync(x => x.Role.ToLower() == "ceo" && x.EmployeeID != id);
            if (ceoExist)
                throw new ValidationException($"Only one CEO is allowed");

            if (!employeeExist)
                throw new NotFoundException($"Employee with ID = {id} not found");

            employee.EmployeeID = id;
            _apiDbContext.Employee.Update(employee);
            await _apiDbContext.SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> UpdateEmployeeSalary(int id, decimal salary)
        {
            if (salary < 0)
                throw new ValidationException($"Salary must be positive number");

            var employee = await GetEmployee(id);

            if (employee == null)
                throw new NotFoundException($"Employee with ID = {id} not found");

            employee.CurrentSalary = salary;
            employee = await UpdateEmployee(id, employee);

            return employee;
        }

        public async Task DeleteEmployee(int id)
        {
            var employeeToDelete = await GetEmployee(id);

            if (employeeToDelete == null)
                throw new NotFoundException($"Employee with Id = {id} not found");

            _apiDbContext.Employee.Remove(employeeToDelete);
            await _apiDbContext.SaveChangesAsync();
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}
