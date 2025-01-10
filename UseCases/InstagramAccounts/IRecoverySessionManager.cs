using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramAccounts
{
    public interface IRecoverySessionManager
    {
        IGAccount Do(IGAccount account, IgAccountRequirements requirements);
    }
}
