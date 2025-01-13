namespace UseCases.Admins.Commands
{
    public class CreateAdminCommand
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
    }
}
