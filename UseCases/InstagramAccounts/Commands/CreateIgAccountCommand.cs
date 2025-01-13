namespace UseCases.InstagramAccounts.Commands
{
    public class CreateIgAccountCommand : IgAccountRequirements
    {
        public required string UserToken { get; set; }
    }
}
