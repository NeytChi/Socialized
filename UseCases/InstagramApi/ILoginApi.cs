using Domain.InstagramAccounts;

namespace UseCases.InstagramApi
{
    public interface ILoginApi
    {
        InstagramLoginState Do(IGAccount iGAccount);
    }
}
