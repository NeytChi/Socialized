namespace UseCases.AutoPosts.Commands
{
    public class DeleteAutoPostCommand
    {
        public required string UserToken { get; set; }
        public long AutoPostId { get; set; }
    }
}
