using System.ComponentModel.DataAnnotations;

namespace UseCases.InstagramAccounts.Commands
{
    public class IgAccountRequirements
    {
        [Required(ErrorMessage = "Instagram username is required")]
        public required string InstagramUserName { get; set; }

        [Required(ErrorMessage = "Instagram password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be minimum 6 characters long")]
        public required string InstagramPassword { get; set; }
    }
}
