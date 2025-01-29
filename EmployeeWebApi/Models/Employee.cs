using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeWebApi.Models
{
    [Table("employee")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Employee first name is required!")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Employee last name is required!")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "City name is required!")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Address is required!")]
        public string? Address { get; set; }

        public string? Phone { get; set; }

        public DateTimeOffset CreatedDate { get; set; } = DateTime.Now;
        public DateTimeOffset UpdatedDate { get; set; } = DateTime.Now;

        public int DepartmentId { get; set; }
        
     }
}
