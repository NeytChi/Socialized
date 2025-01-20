using System.ComponentModel.DataAnnotations;

namespace UseCases.Appeals.Replies.Commands
{
    public class CreateAppealMessageReplyCommand
    {
        [Required(ErrorMessage = "Appeal message ID is required")]
        public long AppealMessageId { get; set; }

        [Required(ErrorMessage = "Reply is required")]
        [StringLength(500, ErrorMessage = "Reply cannot be longer than 500 characters")]
        public required string Reply { get; set; }
    }
}
