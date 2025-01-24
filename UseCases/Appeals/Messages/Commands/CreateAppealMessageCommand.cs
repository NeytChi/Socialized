using UseCases.Base;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Appeals.Messages.Commands
{
    public class CreateAppealMessageCommand
    {
        [Required(ErrorMessage = "Appeal ID is required")]
        public long AppealId { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot be longer than 500 characters")]
        public required string Message { get; set; }

        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        public ICollection<FileDto>? Files { get; set; }
    }
}
