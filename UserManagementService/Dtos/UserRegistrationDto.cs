using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Dtos
{
    public class UserRegistrationDto
    {
        public string? Username { get; set; }

        [Required(ErrorMessage ="Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Compare("Password",ErrorMessage = "Password and confirm password don't match!")]
        public string? ConfirmPassword { get; set; }

        public string? ClientUrl { get; set; }
    }
}
