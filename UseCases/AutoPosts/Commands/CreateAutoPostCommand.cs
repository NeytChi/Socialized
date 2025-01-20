using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UseCases.AutoPosts.Commands;
using UseCases.AutoPosts.AutoPostFiles.Commands;

namespace UseCases.AutoPosts.Commands
{
    public class CreateAutoPostCommand : AutoPostCommand
    {
        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        [Required(ErrorMessage = "Files are required")]
        public required ICollection<CreateAutoPostFileCommand> Files { get; set; }
    }
}
