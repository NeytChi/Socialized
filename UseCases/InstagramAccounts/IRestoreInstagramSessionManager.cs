using Domain.InstagramAccounts;

namespace UseCases.InstagramAccounts
{
    public interface IRestoreInstagramSessionManager
    {
        IGAccount Do(IGAccount account);
        IGAccount Do(long accountId);
    }
}
