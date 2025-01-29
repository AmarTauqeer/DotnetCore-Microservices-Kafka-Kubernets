using EmployeeWebApi.Models;
using EmployeeWebApi.Repositories.Common;

namespace EmployeeWebApi.Repositories
{
    public interface IEmployeeRepository: IRepository<Employee>
    {
    }
}
