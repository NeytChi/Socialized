namespace UseCases.Users.Commands
{
    public class LoginUserCommand
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
