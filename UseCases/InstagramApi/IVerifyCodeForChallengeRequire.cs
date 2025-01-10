using Domain.InstagramAccounts;

namespace UseCases.InstagramApi
{
    public interface IVerifyCodeForChallengeRequire
    {
        public InstagramLoginResult Do(string code, IGAccount account);
        public InstagramLoginResult Do(bool replay, IGAccount account);
    }
}
