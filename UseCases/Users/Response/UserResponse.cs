namespace UseCases.Users.Response
{
    public class UserResponse
    {
        public required string Email { get; set; }
        public required string TokenForUse { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
