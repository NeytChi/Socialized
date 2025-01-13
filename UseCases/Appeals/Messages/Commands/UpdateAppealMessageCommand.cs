namespace UseCases.Appeals.Messages.Commands
{
    public class UpdateAppealMessageCommand
    {
        public long MessageId { get; set; }
        public required string Message { get; set; }
    }
}
