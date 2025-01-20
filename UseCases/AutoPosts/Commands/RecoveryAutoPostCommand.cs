using System.ComponentModel.DataAnnotations;

namespace UseCases.AutoPosts.Commands
{
    public class RecoveryAutoPostCommand : AutoPostCommand
    {
        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        [Required(ErrorMessage = "Auto post ID is required")]
        public long AutoPostId { get; set; }
    }
}

