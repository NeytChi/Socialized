using System.ComponentModel.DataAnnotations;

namespace UseCases.Appeals.Messages.Commands
{
    public class UpdateAppealMessageCommand
    {
        [Required(ErrorMessage = "Message ID is required")]
        public long MessageId { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot be longer than 500 characters")]
        public required string Message { get; set; }
    }
}

