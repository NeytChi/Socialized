using Core;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramApi;

namespace UseCases.InstagramAccounts
{
    public class SaveSessionManager : ISaveSessionManager
    {
        private ILogger Logger;
        private IIGAccountRepository AccountRepository;
        private IGetStateData Api;
        private ProfileCondition ProfileCondition;

        public SaveSessionManager(ILogger logger, IIGAccountRepository accountRepository, IGetStateData api, ProfileCondition profileCondition)
        {
            Logger = logger;
            AccountRepository = accountRepository;
            Api = api;
            ProfileCondition = profileCondition;
        }

        public IGAccount Do(long userId, string userName, bool challengeRequired)
        {
            var account = new IGAccount()
            {
                UserId = userId,
                Username = userName,
                CreatedAt = DateTime.UtcNow,
                State = new SessionState
                {
                    Challenger = challengeRequired,
                    Usable = challengeRequired ? false : true
                }
            };
            AccountRepository.Create(account);
            var stateData = Api.AsString(account);
            var encryptData = ProfileCondition.Encrypt(stateData);
            account.State.SessionSave = encryptData;
            AccountRepository.Update(account);
            Logger.Information($"Сесія Instagram аккаунту було збережено, id={account.Id}.");
            return account;
        }
    }
}
