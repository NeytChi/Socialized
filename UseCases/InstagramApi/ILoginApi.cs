using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramApi
{
    public interface ILoginApi
    {
        InstagramLoginState Do(ref IGAccount iGAccount, IgAccountRequirements accountRequirements);
    }
}
