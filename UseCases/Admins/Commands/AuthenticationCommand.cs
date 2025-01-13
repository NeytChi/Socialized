namespace UseCases.Admins.Commands
{
    public class AuthenticationCommand
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
