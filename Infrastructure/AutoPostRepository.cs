using Domain.AutoPosting;
using Microsoft.EntityFrameworkCore;
using UseCases.AutoPosts.AutoPostFiles;

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
            return (from autoPost in _context.AutoPosts
                    join account in _context.IGAccounts on autoPost.AccountId equals account.Id
                    join user in _context.Users on account.UserId equals user.Id
                    where user.TokenForUse == userToken
                        && autoPost.Id == postId
                        && autoPost.IsDeleted == postDeleted
                    select autoPost).FirstOrDefault();
        }
        public AutoPost GetBy(string userToken, long postId, bool postDeleted, bool postAutoDeleted, bool postExecuted)
        {
            return (from p in _context.AutoPosts
                    join s in _context.IGAccounts on p.AccountId equals s.Id
                    join u in _context.Users on s.UserId equals u.Id
                    where u.TokenForUse == userToken
                        && p.Id == postId
                        && p.Deleted == postDeleted
                        && p.AutoDeleted == postAutoDeleted
                        && p.Executed == postExecuted
                    select p).FirstOrDefault();
        }
        public ICollection<AutoPost> GetBy(GetAutoPostsCommand command)
        {
            return (from p in _context.AutoPosts
                    join s in _context.IGAccounts on p.AccountId equals s.Id
                    join u in _context.Users on s.UserId equals u.Id
                    join f in _context.AutoPostFiles on p.Id equals f.PostId into files
                    where u.TokenForUse == command.UserToken
                        && s.Id == command.AccountId
                        && p.Executed == command.PostExecuted
                        && p.IsDeleted == command.PostDeleted
                        && p.AutoDeleted == command.PostAutoDeleted
                        && p.ExecuteAt > command.From
                        && p.ExecuteAt < command.To
                    orderby p.Id descending
                    select p )
                    .Skip(command.Since * command.Count).Take(command.Count).ToList();
        }

        public List<AutoPost> GetBy(DateTime deleteAfter, bool autoDeleted = false, bool postExecuted = true, bool postAutoDeleted = false, bool postDeleted = false)
        {
            throw new NotImplementedException();
        }

        public AutoPost GetByWithFiles(long autoPostFileId, bool postDeleted = false)
        {
            throw new NotImplementedException();
        }

        public AutoPost GetByWithUserAndFiles(string userToken, long autoPostId, bool postDeleted = false)
        {
            throw new NotImplementedException();
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