namespace UseCases.Appeals.Replies.Commands
{
    public class CreateAppealMessageReplyCommand
    {
        public long AppealMessageId { get; set; }
        public required string Reply { get; set; }
    }
}
