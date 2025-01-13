using UseCases.AutoPosts.AutoPostFiles.Commands;

namespace UseCases.AutoPosts.Commands
{
    public class CreateAutoPostCommand : AutoPostCommand
    {
        public required string UserToken { get; set; }
        public required ICollection<CreateAutoPostFileCommand> Files { get; set; }
    }
}
