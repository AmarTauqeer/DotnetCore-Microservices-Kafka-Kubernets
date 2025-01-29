using EmployeeWebApi.Dtos;
using EmployeeWebApi.Models;
using EmployeeWebApi.Repositories;

namespace EmployeeWebApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IDepartmentRepository _departmentRepository;
        public EmployeeService(IEmployeeRepository repository, IDepartmentRepository departmentRepository)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
        }
        public async Task<Employee> CreatAsync(Employee employee)
        {
            await _repository.CreateAsync(employee);
            return employee;
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

        public async Task<List<EmployeeDto>> GetAll()
        {
            var employees = await _repository.GetAll();
            List<EmployeeDto> employeeDtos = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                EmployeeDto employeeDto = new EmployeeDto();
                // get department name for each employee
                var department = await _departmentRepository.GetById(employee.DepartmentId);
                employeeDto.FirstName = employee.FirstName;
                employeeDto.LastName = employee.LastName;
                employeeDto.City = employee.City;
                employeeDto.Address =employee.Address;
                employeeDto.DepartmentName = department.DepartmentName;
                employeeDto.Phone = employee.Phone;
                employeeDto.EmployeeId = employee.EmployeeId;
                employeeDtos.Add(employeeDto);
            }
            return employeeDtos;
        }

        public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            var employee = await _repository.GetById(id);
            if (employee!=null)
            {
                EmployeeDto employeeDto = new EmployeeDto();
                // get department name for each employee
                var department = await _departmentRepository.GetById(employee.DepartmentId);
                employeeDto.FirstName = employee.FirstName;
                employeeDto.LastName = employee.LastName;
                employeeDto.City = employee.City;
                employeeDto.Address = employee.Address;
                employeeDto.DepartmentName = department.DepartmentName;
                employeeDto.Phone = employee.Phone;
                employeeDto.EmployeeId = employee.EmployeeId;
                return employeeDto;
            }
            return null;
         }

        public async Task<Employee> UpdateAsync(int id, Employee employee)
        {
            var empObj = await _repository.GetById(id);

            if (empObj != null)
            {
                empObj.FirstName = employee.FirstName;
                empObj.LastName = employee.LastName;
                empObj.City = employee.City;
                empObj.Address = employee.Address;
                empObj.DepartmentId= employee.DepartmentId;
                empObj.Phone = employee.Phone;
                await _repository.UpdateAsync(empObj);
                return empObj;
            }
            else
            {
                return null;
            }

        }
    }
}
