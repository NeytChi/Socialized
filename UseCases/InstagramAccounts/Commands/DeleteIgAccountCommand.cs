using System.ComponentModel.DataAnnotations;

namespace UseCases.InstagramAccounts.Commands
{
    public class DeleteIgAccountCommand
    {
        [Required(ErrorMessage = "Account ID is required")]
        public long AccountId { get; set; }
    }
}
