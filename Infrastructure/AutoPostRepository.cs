using Domain.AutoPosting;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AutoPostRepository : IAutoPostRepository
    {
        private Context _context;
        public AutoPostRepository(Context context)
        {
            _context = context;
        }

        public void Add(AutoPost autoPost)
        {
            _context.AutoPosts.Add(autoPost);
            _context.SaveChanges();
        }
        public void Update(AutoPost autoPost)
        {
            _context.AutoPosts.Update(autoPost);
            _context.SaveChanges();
        }
        public List<AutoPost> GetBy(DateTime executeAt, bool postExecuted = false, bool postDeleted = false)
        {
            return _context.AutoPosts.Where(a => a.Executed == postExecuted && executeAt > a.ExecuteAt && a.Deleted == postDeleted).OrderBy(a => a.ExecuteAt).ToList();
        }
        public AutoPost GetBy(string userToken, long postId, bool postDeleted = false)
        {
            return _context.AutoPosts
                .Join(_context.IGAccounts,
                      autoPost => autoPost.AccountId,
                      account => account.Id,
                      (autoPost, account) => new { autoPost, account })
                .Join(_context.Users,
                      aa => aa.account.UserId,
                      user => user.Id,
                      (aa, user) => new { aa.autoPost, user })
                .Where(au => au.user.TokenForUse == userToken
                             && au.autoPost.Id == postId
                             && au.autoPost.IsDeleted == postDeleted)
                .Select(au => au.autoPost)
                .FirstOrDefault();
        }
        public AutoPost GetBy(string userToken, long postId, bool postDeleted, bool postAutoDeleted, bool postExecuted)
        {
            return _context.AutoPosts
                .Join(_context.IGAccounts,
                      p => p.AccountId,
                      s => s.Id,
                      (p, s) => new { p, s })
                .Join(_context.Users,
                      ps => ps.s.UserId,
                      u => u.Id,
                      (ps, u) => new { ps.p, u })
                .Where(psu => psu.u.TokenForUse == userToken
                               && psu.p.Id == postId
                               && psu.p.Deleted == postDeleted
                               && psu.p.AutoDeleted == postAutoDeleted
                               && psu.p.Executed == postExecuted)
                .Select(psu => psu.p)
                .FirstOrDefault();
        }
        public ICollection<AutoPost> GetBy(GetAutoPostsCommand command)
        {
            return _context.AutoPosts
                .Join(_context.IGAccounts,
                      p => p.AccountId,
                      s => s.Id,
                      (p, s) => new { p, s })
                .Join(_context.Users,
                      ps => ps.s.UserId,
                      u => u.Id,
                      (ps, u) => new { ps.p, ps.s, u })
                .GroupJoin(_context.AutoPostFiles,
                           psu => psu.p.Id,
                           f => f.PostId,
                           (psu, files) => new { psu.p, psu.s, psu.u, files })
                .Where(psuf => psuf.u.TokenForUse == command.UserToken
                               && psuf.s.Id == command.AccountId
                               && psuf.p.Executed == command.PostExecuted
                               && psuf.p.IsDeleted == command.PostDeleted
                               && psuf.p.AutoDeleted == command.PostAutoDeleted
                               && psuf.p.ExecuteAt > command.From
                               && psuf.p.ExecuteAt < command.To)
                .OrderByDescending(psuf => psuf.p.Id)
                .Select(psuf => psuf.p)
                .Skip(command.Since * command.Count)
                .Take(command.Count)
                .ToList();
        }

        public AutoPost GetByWithFiles(long autoPostFileId, bool postDeleted = false)
        {
            return _context.AutoPosts
                .Join(_context.IGAccounts, 
                        p => p.AccountId, 
                        s => s.Id, (p, s) => new { p, s })
                .Join(_context.Users, 
                    ps => ps.s.UserId, 
                    u => u.Id, (ps, u) => new { ps.p, ps.s, u })
                .Where(post => post.p.IsDeleted == postDeleted 
                    && post.p.files.Any(f => f.Id == autoPostFileId))
                .Select(post => post.p)
                .Include(p => p.account)
                .ThenInclude(a => a.User)
                .Include(p => p.files)
                .FirstOrDefault();
        }

        public AutoPost GetByWithUserAndFiles(string userToken, long autoPostId, bool postDeleted = false)
        {
            return _context.AutoPosts
                .Join(_context.IGAccounts,
                      p => p.AccountId,
                      s => s.Id,
                      (p, s) => new { p, s })
                .Join(_context.Users,
                      ps => ps.s.UserId,
                      u => u.Id,
                      (ps, u) => new { ps.p, ps.s, u })
                .GroupJoin(_context.AutoPostFiles,
                           psu => psu.p.Id,
                           f => f.PostId,
                           (psu, files) => new { psu.p, psu.s, psu.u, files })
                .Include(post => post.p.account)
                .Include(post => post.p.account.User)
                .Include(post => post.p.files)
                .Where(post => post.p.Id == autoPostId
                    && post.u.TokenForUse == userToken
                    && post.p.IsDeleted == postDeleted)
                .Select(post => post.p)
                .FirstOrDefault();
        }
    }
}
/*
 public ICollection<AutoPost> GetBy(GetAutoPostsCommand command)  
Structure of output from this method
    new
    {
        post_id = p.postId,
        post_type = p.postType,
        created_at = p.createdAt,
        execute_at = p.executeAt.AddHours(p.timezone),
        auto_delete = p.autoDelete,
        delete_after = p.autoDelete ? p.deleteAfter.AddHours(p.timezone) : p.deleteAfter,
        post_location = p.postLocation,
        post_description = p.postDescription,
        post_comment = p.postComment,
        p.timezone,
        category_id = p.categoryId,
        category_name = p.categoryId == 0 ? ""
            : context.Categories.Where(x => x.categoryId == p.categoryId
                && !x.categoryDeleted).FirstOrDefault().categoryName ?? "",
        category_color = p.categoryId == 0 ? ""
            : context.Categories.Where(x => x.categoryId == p.categoryId
                && !x.categoryDeleted).FirstOrDefault().categoryColor ?? "",
        files = GetPostFilesToOutput(files)
    }
 */