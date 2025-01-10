using Domain.InstagramAccounts;

namespace UseCases.InstagramAccounts
{
    public interface IChallengeRequiredAccount
    {
        void Do(IGAccount account, bool replay);
    }
}