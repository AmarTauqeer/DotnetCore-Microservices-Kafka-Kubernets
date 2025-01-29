using EmployeeWebApi.Models;

namespace EmployeeWebApi.Services
{
    public interface IDepartmentService
    {
        public Task<Department> CreatAsync(Department department);
        public Task<Department> UpdateAsync(int id, Department department);
        public Task<bool> DeleteAsync(int id);
        public Task<Department> GetByIdAsync(int id);
        public Task<IEnumerable<Department>> GetAll();

    }
}
