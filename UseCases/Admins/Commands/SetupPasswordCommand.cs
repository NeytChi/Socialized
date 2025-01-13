namespace UseCases.Admins.Commands
{
    public class SetupPasswordCommand
    {
        public required string Token { get; set; }
        public required string Password { get; set; }
    }
}
