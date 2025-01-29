using System.ComponentModel.DataAnnotations;

namespace UserManagementService.Models.Authentication.Register
{
    public class RegisterUser
    {
        [Required(ErrorMessage ="Username is required!")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string? Password { get; set; }
/*        
        [Compare("Password", ErrorMessage ="Password and confirm password don't match!")]
        public string? ConfirmPassword { get; set; }*/
    }
}
