namespace UseCases.Users.Commands
{
    public class ChangeOldPasswordCommand
    {
        public required string UserToken { get; set; }
        public required string OldPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
