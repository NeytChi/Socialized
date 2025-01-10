using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramAccounts
{
    public interface ILoginSessionManager
    {
        IGAccount Do(IGAccount account, IgAccountRequirements accountRequirements);
    }
}