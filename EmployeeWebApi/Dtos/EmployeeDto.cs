namespace EmployeeWebApi.Dtos
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }
        public string? DepartmentName { get; set; }
    }
}
