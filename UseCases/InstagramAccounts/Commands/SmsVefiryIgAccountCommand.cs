using System.ComponentModel.DataAnnotations;

namespace UseCases.InstagramAccounts.Commands
{
    public class SmsVefiryIgAccountCommand
    {
        [Required(ErrorMessage = "Account ID is required")]
        public long AccountId { get; set; }

        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        [Required(ErrorMessage = "Verification code is required")]
        [Range(100000, 999999, ErrorMessage = "Verify code must be a 6-digit number")]
        public int VerifyCode { get; set; }
    }
}
