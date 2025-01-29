using Azure.Core;
using Confluent.Kafka;
using EmployeeWebApi.Dtos;
using EmployeeWebApi.kafka;
using EmployeeWebApi.Models;
using EmployeeWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static Confluent.Kafka.ConfigPropertyNames;

namespace DepartmentService.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly ProducerService _producerService;
        public DepartmentController(IDepartmentService departmentService, ProducerService producerService)
        {
            _departmentService = departmentService;
            _producerService = producerService;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Department>>  GetAll()
        {
            return await _departmentService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<Department> GetById(int id)
        {
            return await _departmentService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(DepartmentDto department)
        {
            Department dept = new Department();
            dept.DepartmentName = department.DepartmentName;
            await _departmentService.CreatAsync(dept);
            // produce kafka messages
            var departmentInfo = new
            {
                Created=true,
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName,
            };


            var message = JsonSerializer.Serialize(departmentInfo);

            await _producerService.ProduceAsync("Department", message);

            return Ok("Created");
        }

        [HttpPut]
        public async Task<Department> UpdateDepartment(int id, DepartmentDto department)
        {
            Department dept = new Department();
            dept.DepartmentName = department.DepartmentName;
            // produce kafka messages
            var departmentInfo = new
            {
                Updated = true,
                DepartmentId = id,
                DepartmentName = dept.DepartmentName,
            };


            var message = JsonSerializer.Serialize(departmentInfo);

            await _producerService.ProduceAsync("Department", message);
            return await _departmentService.UpdateAsync(id,dept);
        }

        [HttpDelete("{id}")]
        public async Task<bool> DeleteDepartment(int id)
        {
            // produce kafka messages
            var departmentInfo = new
            {
                Deleted = true,
                DepartmentId = id,
            };


            var message = JsonSerializer.Serialize(departmentInfo);

            await _producerService.ProduceAsync("Department", message);
            return await _departmentService.DeleteAsync(id);
        }
    }
}
