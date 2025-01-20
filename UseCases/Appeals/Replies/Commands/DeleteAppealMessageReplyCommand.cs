using System.ComponentModel.DataAnnotations;

namespace UseCases.Appeals.Replies.Commands
{
    public class DeleteAppealMessageReplyCommand
    {
        [Required(ErrorMessage = "Reply ID is required")]
        public long ReplyId { get; set; }
    }
}
