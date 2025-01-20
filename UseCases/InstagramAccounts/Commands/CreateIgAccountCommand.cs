using System.ComponentModel.DataAnnotations;

namespace UseCases.InstagramAccounts.Commands
{
    public class CreateIgAccountCommand : IgAccountRequirements
    {
        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }
    }
}
