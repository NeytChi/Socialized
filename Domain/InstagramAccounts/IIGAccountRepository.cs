namespace Domain.InstagramAccounts
{
    public interface IIGAccountRepository
    {
        void Create(IGAccount account);
        void Update(IGAccount account);
        IGAccount Get(long accountId, bool isDeleted = false, bool usable = true);
        IGAccount Get(string userToken, long accountId, bool isDeleted = false);
        IGAccount GetByWithState(long userId, string instagramUsername);
        IGAccount GetByWithState(string userToken, string instagramUsername);
        IGAccount GetByWithState(long accountId, bool accountDeleted = false);
    }
}