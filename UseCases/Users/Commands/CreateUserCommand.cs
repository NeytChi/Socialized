namespace UseCases.Users.Commands
{
    public class CreateUserCommand
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
        public required string CountryName { get; set; }
        public int TimeZone { get; set; }
        public required string Culture { get; set; }
    }
}
