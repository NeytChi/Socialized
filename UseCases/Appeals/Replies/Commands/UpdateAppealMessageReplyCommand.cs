namespace UseCases.Appeals.Replies.Commands
{
    public class UpdateAppealMessageReplyCommand
    {
        public long ReplyId { get; set; }
        public required string Reply { get; set; }
    }
}
