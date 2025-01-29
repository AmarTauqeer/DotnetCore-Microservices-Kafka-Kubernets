using EmployeeWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWebApi.Data
{
    public class EmployeeDB:DbContext
    {
        public EmployeeDB(DbContextOptions<EmployeeDB> options): base(options)
        {
            
        }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

    }
}
