using System.ComponentModel.DataAnnotations;

namespace UseCases.Appeals.Messages.Commands
{
    public class DeleteAppealMessageCommand
    {
        [Required(ErrorMessage = "Message ID is required")]
        public long MessageId { get; set; }
    }
}
