using Domain.AutoPosting;
using UseCases.AutoPosts.Commands;

namespace UseCases.AutoPosts
{
    public interface IAutoPostManager
    {
        void Create(CreateAutoPostCommand command);
        ICollection<AutoPost> Get(GetAutoPostsCommand command);
        void Update(UpdateAutoPostCommand command);
        void Delete(DeleteAutoPostCommand command);
    }
}
