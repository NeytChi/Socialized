using Domain.AutoPosting;
using Microsoft.EntityFrameworkCore;
using UseCases.AutoPosts.AutoPostFiles;

namespace Infrastructure
{
    public class AutoPostFileRepository : IAutoPostFileRepository
    {
        private Context _context;
        public AutoPostFileRepository(Context context)
        {
            _context = context;
        }
        public void Create(AutoPostFile postFile)
        {
            _context.AutoPostFiles.Add(postFile);
            _context.SaveChanges();
        }
        public void Create(ICollection<AutoPostFile> files)
        {
            _context.AutoPostFiles.AddRange(files);
            _context.SaveChanges();
        }
        public void Update(AutoPostFile file)
        {
            _context.AutoPostFiles.Update(file);
            _context.SaveChanges();
        }

        public void Update(ICollection<AutoPostFile> posts)
        {
            _context.AutoPostFiles.UpdateRange(posts);
            _context.SaveChanges();
        }
        public AutoPostFile GetBy(long postId)
        {
            return _context.AutoPostFiles.Where(p => p.Id == postId).FirstOrDefault();
        }
        public AutoPostFile GetBy(long fileId, long postId, bool fileDeleted = false)
        {
            return _context.AutoPostFiles.Where(f => f.Id == fileId && f.PostId == postId && f.IsDeleted == fileDeleted).FirstOrDefault();
        }
        public ICollection<AutoPostFile> GetBy(long postId, bool fileDeleted = false)
        {
            return _context.AutoPostFiles.Where(f => f.Id == postId && f.IsDeleted == fileDeleted).OrderBy(f => f.Order).ToList();
        }
        public List<AutoPost> GetBy(
            DateTime deleteAfter,
            bool autoDeleted = false,
            bool postExecuted = true,
            bool postAutoDeleted = false,
            bool postDeleted = false)
        {
            return _context.AutoPosts.Where(a
                => a.AutoDelete == autoDeleted
                && a.Executed == postExecuted
                && a.AutoDeleted == postAutoDeleted
                && a.DeleteAfter < deleteAfter
                && a.Deleted == postDeleted
            ).OrderBy(a => a.DeleteAfter).ToList();
        }
        public ICollection<AutoPostFile> GetByRange(long autoPostId, bool fileDeleted = false)
        {
            return _context.AutoPostFiles
                .Where(f => f.PostId == autoPostId && f.IsDeleted == fileDeleted).ToArray();
        }

        AutoPostFile IAutoPostFileRepository.GetBy(long autoPostFileId, bool IsDeleted)
        {
            throw new NotImplementedException();
        }

        public AutoPostFile GetBy(string userToken, long autoPostFileId, bool IsDeleted = false)
        {
            throw new NotImplementedException();
        }
    }
}
