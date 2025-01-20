using System.ComponentModel.DataAnnotations;

namespace UseCases.Admins.Commands
{
    public class SetupPasswordCommand
    {
        [Required(ErrorMessage = "Token is required")]
        public required string Token { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be minimum 6 characters long")]
        public required string Password { get; set; }
    }
}
