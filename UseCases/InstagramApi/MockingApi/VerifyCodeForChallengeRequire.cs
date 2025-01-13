using Domain.InstagramAccounts;

namespace UseCases.InstagramApi.MockingApi
{
    public class VerifyCodeForChallengeRequire : IVerifyCodeForChallengeRequire
    {
        public InstagramLoginResult Do(string code, IGAccount account)
        {
            return new InstagramLoginResult { Success = true, State = InstagramLoginState.Success };
        }

        public InstagramLoginResult Do(bool replay, IGAccount account)
        {
            return new InstagramLoginResult { Success = true, State = InstagramLoginState.Success };
        }
    }
}
