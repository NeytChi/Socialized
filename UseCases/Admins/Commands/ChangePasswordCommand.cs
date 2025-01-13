namespace UseCases.Admins.Commands
{
    public class ChangePasswordCommand
    {
        public int RecoveryCode { get; set; }
        public required string Password { get; set; }
    }
}
