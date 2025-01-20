using System.ComponentModel.DataAnnotations;

namespace UseCases.Admins.Commands
{
    public class ChangePasswordCommand
    {
        [Required(ErrorMessage = "Recovery code is required")]
        [Range(100000, 999999, ErrorMessage = "Recovery code must be a 6-digit number")]
        public int RecoveryCode { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be minimum 6 characters long")]
        public required string Password { get; set; }
    }
}
