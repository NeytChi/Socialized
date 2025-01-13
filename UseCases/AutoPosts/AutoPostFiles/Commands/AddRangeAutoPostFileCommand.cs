namespace UseCases.AutoPosts.AutoPostFiles.Commands
{
    public class AddRangeAutoPostFileCommand
    {
        public required string UserToken { get; set; }
        public long AutoPostId { get; set; }
        public required ICollection<CreateAutoPostFileCommand> Files { get; set; }
    }
}
