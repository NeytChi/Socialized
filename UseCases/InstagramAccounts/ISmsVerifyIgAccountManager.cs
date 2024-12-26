using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramAccounts
{
    public interface ISmsVerifyIgAccountManager
    {
        void SmsVerifySession(SmsVefiryIgAccountCommand command);
    }
}
