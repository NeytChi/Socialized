using Domain.InstagramAccounts;

namespace UseCases.InstagramAccounts
{
    public interface ISaveSessionManager
    {
        IGAccount Do(long userId, string userName, bool challengeRequired);
    }
}
