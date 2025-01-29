using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmployeeWebApi.Models
{
    [Table("department")]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        [Required(ErrorMessage ="Department name is required!")]
        public string? DepartmentName { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTime.Now;
        public DateTimeOffset UpdatedDate { get; set; } = DateTime.Now;
    }
}
