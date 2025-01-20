using System.ComponentModel.DataAnnotations;

namespace UseCases.Users.Commands
{
    public class ChangeOldPasswordCommand
    {
        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        [Required(ErrorMessage = "Old password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Old password should be minimum 6 characters long")]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password should be minimum 6 characters long")]
        public required string NewPassword { get; set; }
    }
}
