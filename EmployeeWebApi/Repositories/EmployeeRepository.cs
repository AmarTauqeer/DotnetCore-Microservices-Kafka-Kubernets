using EmployeeWebApi.Interfaces;
using EmployeeWebApi.Models;
using EmployeeWebApi.Data;

namespace EmployeeWebApi.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EmployeeDB db) : base(db)
        {
        }
    }
}
