using EmployeeWebApi.Data;
using EmployeeWebApi.Dtos;
using EmployeeWebApi.kafka;
using EmployeeWebApi.Models;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmployeeWebApi.Controllers
{
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ProducerService _producerService;
        public EmployeeController(IEmployeeService employeeService, ProducerService producerService)
        {
            _employeeService = employeeService;
            _producerService = producerService;
        }

        [HttpGet("all")]
        public async Task<List<EmployeeDto>> GetAll()
        {
            return await _employeeService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<EmployeeDto> GetById(int id)
        {
            return await _employeeService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            await _employeeService.CreatAsync(employee);

            // produce kafka messages
            var Info = new
            {
                Created = true,
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                City = employee.City,
                DepartmentId = employee.DepartmentId
            };


            var message = JsonSerializer.Serialize(Info);

            await _producerService.ProduceAsync("Department", message);
            return Ok("Created");
        }

        [HttpPut]
        public async Task<Employee> UpdateEmployee(int id, Employee employee)
        {
            // produce kafka messages
            var Info = new
            {
                Updated = true,
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                City = employee.City,
                DepartmentId = employee.DepartmentId
            };


            var message = JsonSerializer.Serialize(Info);

            await _producerService.ProduceAsync("Department", message);

            return await _employeeService.UpdateAsync(id, employee);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteEmployee(int id)
        {
            // produce kafka messages
            var info = new
            {
                Deleted = true,
                EmployeeId = id,
            };


            var message = JsonSerializer.Serialize(info);

            await _producerService.ProduceAsync("Department", message);
            return await _employeeService.DeleteAsync(id);
        }
    }
}
