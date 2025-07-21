using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.DTOs;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ApplicationDbContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _context.Employees
                    .Where(e => e.IsActive)
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToListAsync();

                return employees.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all employees");
                throw;
            }
        }

        public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == id && e.IsActive);

                return employee != null ? MapToDto(employee) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employee with ID {EmployeeId}", id);
                throw;
            }
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
            try
            {
                var employee = new Employee
                {
                    FirstName = createEmployeeDto.FirstName,
                    LastName = createEmployeeDto.LastName,
                    Email = createEmployeeDto.Email,
                    Department = createEmployeeDto.Department,
                    Position = createEmployeeDto.Position,
                    Salary = createEmployeeDto.Salary,
                    HireDate = createEmployeeDto.HireDate,
                    IsActive = true
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Employee created successfully with ID {EmployeeId}", employee.Id);
                return MapToDto(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating employee");
                throw;
            }
        }

        public async Task<EmployeeDto?> UpdateEmployeeAsync(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null || !employee.IsActive)
                {
                    return null;
                }

                employee.FirstName = updateEmployeeDto.FirstName;
                employee.LastName = updateEmployeeDto.LastName;
                employee.Email = updateEmployeeDto.Email;
                employee.Department = updateEmployeeDto.Department;
                employee.Position = updateEmployeeDto.Position;
                employee.Salary = updateEmployeeDto.Salary;
                employee.HireDate = updateEmployeeDto.HireDate;
                employee.IsActive = updateEmployeeDto.IsActive;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Employee updated successfully with ID {EmployeeId}", id);
                return MapToDto(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating employee with ID {EmployeeId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null || !employee.IsActive)
                {
                    return false;
                }

                employee.IsActive = false; // Soft delete
                await _context.SaveChangesAsync();

                _logger.LogInformation("Employee soft deleted successfully with ID {EmployeeId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee with ID {EmployeeId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeesByDepartmentAsync(string department)
        {
            try
            {
                var employees = await _context.Employees
                    .Where(e => e.IsActive && e.Department.ToLower() == department.ToLower())
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToListAsync();

                return employees.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching employees by department {Department}", department);
                throw;
            }
        }

        private static EmployeeDto MapToDto(Employee employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Department = employee.Department,
                Position = employee.Position,
                Salary = employee.Salary,
                HireDate = employee.HireDate,
                IsActive = employee.IsActive,
                FullName = employee.FullName
            };
        }
    }
}