using EmployeeWebApi.Models;
using EmployeeWebApi.Repositories;

namespace EmployeeWebApi.Services
{
    public class DeptService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        public DeptService(IDepartmentRepository repository)
        {
            _repository = repository;
        }
        public async Task<Department> CreatAsync(Department department)
        {
            await _repository.CreateAsync(department);
            return department;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var isfound = await _repository.GetById(id);
            if (isfound != null)
            {
                return await _repository.DeleteAsync(isfound);
            }
            return false;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _repository.GetById(id);

        }

        public async Task<Department> UpdateAsync(int id, Department department)
        {
            var dept = await _repository.GetById(id);

            if (dept!=null)
            {
                dept.DepartmentName = department.DepartmentName;
                await _repository.UpdateAsync(dept);
                return dept;
            }else
            {
                return null;
            }
            
        }
    }
}
