using EmployeeWebApi.Data;
using EmployeeWebApi.Interfaces;
using EmployeeWebApi.Models;

namespace EmployeeWebApi.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(EmployeeDB departmentDbContext) : base(departmentDbContext)
        {
        }
    }
}
