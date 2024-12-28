using Domain.AutoPosting;
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
        public AutoPostFile GetBy(long postId, bool IsDeleted)
        {
            return _context.AutoPostFiles.Where(p => p.Id == postId && p.IsDeleted == IsDeleted).FirstOrDefault();
        }
        public AutoPostFile GetBy(long fileId, long postId, bool fileDeleted = false)
        {
            return _context.AutoPostFiles.Where(f => f.Id == fileId && f.PostId == postId && f.IsDeleted == fileDeleted).FirstOrDefault();
        }
        public AutoPostFile GetBy(string userToken, long autoPostFileId, bool isDeleted = false)
        {
            return _context.AutoPostFiles
                .Join(_context.AutoPosts,
                      file => file.PostId,
                      post => post.Id,
                      (file, post) => new { file, post })
                .Join(_context.IGAccounts,
                      filepost => filepost.post.AccountId,
                      account => account.Id,
                      (filepost, account) => new { filepost, account })
                .Join(_context.Users,
                      fpa => fpa.account.UserId,
                      user => user.Id,
                      (fpa, user) => new { fpa, user })
                .Where(fpau => fpau.user.TokenForUse == userToken
                             && fpau.fpa.filepost.file.Id == autoPostFileId
                             && fpau.fpa.filepost.file.IsDeleted == isDeleted)
                .Select(fpau => fpau.fpa.filepost.file)
                .FirstOrDefault();
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
    }
}
