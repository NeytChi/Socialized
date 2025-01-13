namespace UseCases.Users.Commands
{
    public class CheckRecoveryCodeCommand
    {
        public required string UserEmail { get; set; }
        public int RecoveryCode { get; set; }
    }
}
