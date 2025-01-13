using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramApi.MockingApi
{
    public class LoginApi : ILoginApi
    {
        public InstagramLoginState Do(ref IGAccount iGAccount, IgAccountRequirements accountRequirements)
        {
            return InstagramLoginState.Success;
        }
    }
}
