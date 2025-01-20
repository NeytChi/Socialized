using System.ComponentModel.DataAnnotations;

namespace UseCases.Users.Commands
{
    public class CheckRecoveryCodeCommand
    {
        [Required(ErrorMessage = "User email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string UserEmail { get; set; }

        [Required(ErrorMessage = "Recovery code is required")]
        [Range(100000, 999999, ErrorMessage = "Recovery code must be a 6-digit number")]
        public int RecoveryCode { get; set; }
    }
}
