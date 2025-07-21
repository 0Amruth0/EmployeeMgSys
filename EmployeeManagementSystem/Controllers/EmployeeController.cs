
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Get all active employees
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employees");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employee with ID {EmployeeId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                var employee = await _employeeService.CreateEmployeeAsync(createEmployeeDto);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating employee");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var employee = await _employeeService.UpdateEmployeeAsync(id, updateEmployeeDto);
                if (employee == null)
                {
                    return NotFound($"Employee with ID {id} not found");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating employee with ID {EmployeeId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Delete an employee (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(id);
                if (!result)
                {
                    return NotFound($"Employee with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee with ID {EmployeeId}", id);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Get employees by department
        /// </summary>
        [HttpGet("department/{department}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(string department)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesByDepartmentAsync(department);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employees by department {Department}", department);
                return StatusCode(500, "An error occurred while processing your request");
            }
        }
    }
}