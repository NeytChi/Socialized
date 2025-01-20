using System.ComponentModel.DataAnnotations;

namespace UseCases.Appeals.Commands
{
    public class CreateAppealCommand
    {
        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject cannot be longer than 200 characters")]
        public required string Subject { get; set; }
    }
}
