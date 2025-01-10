using Domain.InstagramAccounts;

namespace UseCases.InstagramApi
{
    public interface IGetStateData
    {
        string AsString(IGAccount account);
    }
}
