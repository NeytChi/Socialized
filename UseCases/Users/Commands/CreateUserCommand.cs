using System.ComponentModel.DataAnnotations;

namespace UseCases.Users.Commands
{
    public class CreateUserCommand
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be minimum 6 characters long")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Country Name is required")]
        public required string CountryName { get; set; }

        [Range(-12, 12, ErrorMessage = "TimeZone must be between -12 and 12")]
        public int TimeZone { get; set; }

        [Required(ErrorMessage = "Culture is required")]
        public required string Culture { get; set; }
    }
}

