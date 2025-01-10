/*using Core;
using Domain.InstagramAccounts;
using Serilog;
using UseCases.Exceptions;
using UseCases.InstagramApi;

namespace UseCases.InstagramAccounts
{
    public class RestoreInstagramSessionManager : BaseManager, IRestoreInstagramSessionManager
    {
        private IIGAccountRepository AccountRepository;
        private IInstagramApi Api;
        private ProfileCondition ProfileCondition;

        public RestoreInstagramSessionManager(ILogger logger, 
            IInstagramApi api,
            IIGAccountRepository accountRepository,
            ProfileCondition profileCondition) : base(logger)
        {
            AccountRepository = accountRepository;
            Api = api;
            ProfileCondition = profileCondition;
        }
        public RestoreInstagramSessionManager(ILogger logger,
            IInstagramApi api,
            IIGAccountRepository accountRepository) : base(logger)
        {
            AccountRepository = accountRepository;
            Api = api;
            ProfileCondition = new ProfileCondition();
        }
        public IGAccount Do(IGAccount account)
        {
            var decryptedSessionSave = ProfileCondition.Decrypt(account.State.SessionSave);
            var session = Api.LoadStateDataFromString(decryptedSessionSave);
            Logger.Information($"Інстаграм сесія була востановлена з тексту, id={account.Id}.");
            return null;
        }
        public IGAccount Do(long accountId)
        {
            var account = AccountRepository.Get(accountId);
            if (account == null)
            {
                throw new NotFoundException("Сервер не визначив запис Інстаграм аккаунту по id.");
            }
            var decryptedSession = ProfileCondition.Decrypt(account.State.SessionSave);
            var session = Api.LoadStateDataFromString(decryptedSession);
            Logger.Information($"Сессія Інстаграм аккаунту була востановлена, id={account.Id}.");
            return session;
        }
    }
}
*/