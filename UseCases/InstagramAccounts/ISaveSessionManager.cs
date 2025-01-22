using Domain.InstagramAccounts;
using Domain.Users;

namespace UseCases.InstagramAccounts
{
    public interface ISaveSessionManager
    {
        IGAccount Do(User user, string userName, bool challengeRequired);
    }
}
