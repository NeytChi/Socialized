using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramAccounts
{
    public interface IIGAccountManager
    {
        IGAccount Create(CreateIgAccountCommand command);
    }
}
