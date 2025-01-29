using EmployeeWebApi.Dtos;
using EmployeeWebApi.Models;

namespace EmployeeWebApi.Services
{
    public interface IEmployeeService
    {
        public Task<Employee> CreatAsync(Employee employee);
        public Task<Employee> UpdateAsync(int id, Employee employee);
        public Task<bool> DeleteAsync(int id);
        public Task<EmployeeDto> GetByIdAsync(int id);
        public Task<List<EmployeeDto>> GetAll();
    }
}
