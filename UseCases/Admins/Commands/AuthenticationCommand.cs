using System.ComponentModel.DataAnnotations;

namespace UseCases.Admins.Commands
{
    public class AuthenticationCommand
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be minimum 6 characters long")]
        public required string Password { get; set; }
    }
}
