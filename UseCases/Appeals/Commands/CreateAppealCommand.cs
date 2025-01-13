namespace UseCases.Appeals.Commands
{
    public class CreateAppealCommand
    {
        public required string UserToken { get; set; }
        public required string Subject { get; set; }
    }
}
