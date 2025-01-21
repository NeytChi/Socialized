using Domain.AutoPosting;
using Domain.InstagramAccounts;

namespace Domain.Packages
{
    public interface IForServerAccessCountingRepository
    {
        ICollection<IGAccount> GetAccounts(long userId, bool accountDeleted = false);
        ICollection<AutoPost> Get(long userId, bool postType);
    }
}
