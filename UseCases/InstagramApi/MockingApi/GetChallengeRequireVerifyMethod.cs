using Domain.InstagramAccounts;

namespace UseCases.InstagramApi.MockingApi
{
    public class GetChallengeRequireVerifyMethod : IGetChallengeRequireVerifyMethod
    {
        public InstagramLoginResult Do(IGAccount account)
        {
            return new InstagramLoginResult { Success = true, State = InstagramLoginState.Success };
        }
    }
}
