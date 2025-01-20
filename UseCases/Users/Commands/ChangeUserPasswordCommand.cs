using System.ComponentModel.DataAnnotations;

namespace UseCases.Users.Commands
{
    public class ChangeUserPasswordCommand
    {
        [Required(ErrorMessage = "Recovery token is required")]
        public required string RecoveryToken { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be minimum 6 characters long")]
        public required string UserPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("UserPassword", ErrorMessage = "Password and confirmation password do not match")]
        public required string UserConfirmPassword { get; set; }
    }
}
