using Domain.AutoPosting;

namespace UseCases.AutoPosts.AutoPostFiles
{
    public interface IAutoPostFileRepository
    {
        public void Create(AutoPostFile postFile);
        public void Create(ICollection<AutoPostFile> files);
        public void Update(AutoPostFile file);
        public void Update(ICollection<AutoPostFile> posts);
        public AutoPostFile GetBy(long autoPostFileId, bool IsDeleted = false);
        public AutoPostFile GetBy(long autoPostFileId, long autoPostId, bool fileDeleted = false);
        public ICollection<AutoPostFile> GetByRange(long autoPostId, bool fileDeleted = false);
    }
}
