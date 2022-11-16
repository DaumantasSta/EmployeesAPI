using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using EmployeesAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using EmployeesAPI.Repositories;
using EmployeesAPI.Repositories.Contexts;

namespace EmployeesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger)
        {
            this._employeeRepository = employeeRepository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmployees());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                throw;
            }
        }

        //Get employee by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            try
            {
                var result = await _employeeRepository.GetEmployee(id);

                if (result == null)
                    return NotFound($"Employee with Id = {id} not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                throw;
            }
        }

        //Get employee by boss id
        [HttpGet("byBoss/{bossId}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetByBossId(int bossId)
        {
            try
            {
                var result = await _employeeRepository.GetEmployeesByBoss(bossId);

                if (result.Count() == 0)
                    return NotFound($"Employees with boss id = {bossId} not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                throw;
            }
        }

        //Get employess by name and birthday interval
        [HttpGet("byNameBirthdayInterval/{name}/{firstDate}/{secondDate}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetByNameAndBirthdayDate(string name, DateTime firstDate, DateTime secondDate)
        {
            try
            {
                var result = await _employeeRepository.GetEmployeesByNameAndBirthdayDateInterval(name, firstDate, secondDate);

                if (result == null)
                    return NotFound($"Employees with provided data not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                throw;
            }
        }

        //Get employee count and average salary for particular Role
        [HttpGet("roleInfo/{role}")]
        public async Task<ActionResult<EmployeeRoleInfo>> GetRoleInfo(string role)
        {
            try
            {
                var result = await _employeeRepository.GetRoleInfo(role);

                if (result == null)
                    return NotFound($"Employees that has role = {role} not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> Post([FromBody] Employee employee)
        {
            try
            {
                if (employee == null)
                    return BadRequest();

                var result = await _employeeRepository.AddEmployee(employee);

                if (result == null)
                    return BadRequest();

                return Created(result.EmployeeID.ToString(), result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> Put(int id, [FromBody] Employee employee)
        {
            try
            {
                var result = await _employeeRepository.UpdateEmployee(id, employee);

                return Ok(result);
            }
            catch(NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                throw;
            }
        }

        //Update only employee salary
        [HttpPut("UpdateSalary/{id}/{salary}")]
        public async Task<ActionResult<Employee>> UpdateSalary(int id, decimal salary)
        {
            try
            {
                var result = await _employeeRepository.UpdateEmployeeSalary(id, salary);
                
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured updating salary = {Salary}", salary);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _employeeRepository.DeleteEmployee(id);

                return Accepted();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                throw;
            }
        }
    }
}
