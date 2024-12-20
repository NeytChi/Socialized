using Domain.AutoPosting;

namespace Infrastructure
{
    public class PostFileRepository
    {
        private Context Context;
        public PostFileRepository(Context context)
        {
            Context = context;
        }
        public AutoPostFile GetBy(long postId)
        {
            return Context.AutoPostFiles.Where(p => p.Id == postId).FirstOrDefault();
        }
        public AutoPostFile GetBy(long fileId, long postId, bool fileDeleted = false)
        {
            return Context.AutoPostFiles.Where(f => f.Id == fileId && f.PostId == postId && f.IsDeleted == fileDeleted).FirstOrDefault();
        }
        public ICollection<AutoPostFile> GetBy(long postId, bool fileDeleted = false)
        {
            return Context.AutoPostFiles.Where(f => f.Id == postId && f.IsDeleted == fileDeleted).OrderBy(f => f.Order).ToList();
        }
        public List<AutoPost> GetBy(
            DateTime deleteAfter,
            bool autoDeleted = false,
            bool postExecuted = true,
            bool postAutoDeleted = false,
            bool postDeleted = false)
        {
            return Context.AutoPosts.Where(a
                => a.AutoDelete == autoDeleted
                && a.Executed == postExecuted
                && a.AutoDeleted == postAutoDeleted
                && a.DeleteAfter < deleteAfter
                && a.Deleted == postDeleted
            ).OrderBy(a => a.DeleteAfter).ToList();
        }
    }
}
