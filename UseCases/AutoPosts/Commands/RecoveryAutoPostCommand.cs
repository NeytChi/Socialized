namespace UseCases.AutoPosts.Commands
{
    public class RecoveryAutoPostCommand : AutoPostCommand
    {
        public required string UserToken { get; set; }
        public long AutoPostId { get; set; }
    }
}
