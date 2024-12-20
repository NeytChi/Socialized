namespace UseCases.Users.Commands
{
    public class ChangeUserPasswordCommand
    {
        public string RecoveryToken { get; set; }
        public string UserPassword { get; set; } 
        public string UserConfirmPassword { get; set; } 
    }
}
