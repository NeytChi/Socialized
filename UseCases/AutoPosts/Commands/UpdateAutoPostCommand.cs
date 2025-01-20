using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UseCases.AutoPosts.AutoPostFiles.Commands;
using UseCases.AutoPosts.Commands;

namespace UseCases.AutoPosts.Commands
{
    public class UpdateAutoPostCommand : AutoPostCommand
    {
        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        [Required(ErrorMessage = "Post ID is required")]
        public long PostId { get; set; }

        public ICollection<UpdateAutoPostFileCommand>? Files { get; set; }
    }
}
