using Domain.AutoPosting;

namespace Infrastructure
{
    public class AutoPostRepository
    {
        private Context Context;
        public AutoPostRepository(Context context)
        {
            Context = context;
        }
        public List<AutoPost> GetBy(DateTime executeAt,
            bool postExecuted = false,
            bool postDeleted = false)
        {
            return Context.AutoPosts.Where(a => a.Executed == postExecuted && executeAt > a.ExecuteAt && a.Deleted == postDeleted).OrderBy(a => a.ExecuteAt).ToList();
        }
        public AutoPost GetBy(string userToken, long postId, bool postDeleted = false)
        {
            return (from autoPost in Context.AutoPosts
                    join account in Context.IGAccounts on autoPost.AccountId equals account.Id
                    join user in Context.Users on account.UserId equals user.Id
                    where user.TokenForUse == userToken
                        && autoPost.Id == postId
                        && autoPost.IsDeleted == postDeleted
                    select autoPost).FirstOrDefault();
        }
        public AutoPost GetBy(string userToken, long postId, bool postDeleted, bool postAutoDeleted, bool postExecuted)
        {
            return (from p in Context.AutoPosts
                    join s in Context.IGAccounts on p.AccountId equals s.Id
                    join u in Context.Users on s.UserId equals u.Id
                    where u.TokenForUse == userToken
                        && p.Id == postId
                        && p.Deleted == postDeleted
                        && p.AutoDeleted == postAutoDeleted
                        && p.Executed == postExecuted
                    select p).FirstOrDefault();
        }
        public ICollection<AutoPost> GetBy(GetAutoPostsCommand command)
        {
            return (from p in Context.AutoPosts
                    join s in Context.IGAccounts on p.AccountId equals s.Id
                    join u in Context.Users on s.UserId equals u.Id
                    join f in Context.AutoPostFiles on p.Id equals f.PostId into files
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