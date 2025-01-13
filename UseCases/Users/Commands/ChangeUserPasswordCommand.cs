namespace UseCases.Users.Commands
{
    public class ChangeUserPasswordCommand
    {
        public required string RecoveryToken { get; set; }
        public required string UserPassword { get; set; } 
        public required string UserConfirmPassword { get; set; } 
    }
}
