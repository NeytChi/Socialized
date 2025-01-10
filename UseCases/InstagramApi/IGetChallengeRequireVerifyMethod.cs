using Domain.InstagramAccounts;

namespace UseCases.InstagramApi
{
    public interface IGetChallengeRequireVerifyMethod
    {
        public InstagramLoginResult Do(IGAccount account);
    }
}
