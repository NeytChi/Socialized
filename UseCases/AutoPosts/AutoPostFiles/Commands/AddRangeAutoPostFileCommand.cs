using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UseCases.AutoPosts.AutoPostFiles.Commands
{
    public class AddRangeAutoPostFileCommand
    {
        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        [Required(ErrorMessage = "Auto post ID is required")]
        public long AutoPostId { get; set; }

        [Required(ErrorMessage = "Files are required")]
        public required ICollection<CreateAutoPostFileCommand> Files { get; set; }
    }
}
