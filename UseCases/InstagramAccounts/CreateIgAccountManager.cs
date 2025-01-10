using Core;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;
using UseCases.InstagramApi;

namespace UseCases.InstagramAccounts
{
    public class CreateIGAccountManager : BaseManager, IIGAccountManager
    {
        private IIGAccountRepository AccountRepository;
        private IChallengeRequiredAccount ChallengeRequiredAccount;
        private ILoginSessionManager LoginSessionManager;
        private IRestoreInstagramSessionManager RestoreInstagramSessionManager;
        private IRecoverySessionManager RecoverySessionManager;
        private ILoginApi LoginApi;
        private ISaveSessionManager SaveSessionManager;
        private ProfileCondition ProfileCondition = new ProfileCondition();

        public CreateIGAccountManager(ILogger logger, 
            IIGAccountRepository accountRepository,
            ILoginApi api,
            IChallengeRequiredAccount challengeRequiredAccount,
            ILoginSessionManager loginSessionManager,
            IRestoreInstagramSessionManager restoreInstagramSessionManager,
            IRecoverySessionManager recoverySessionManager,
            ISaveSessionManager saveSessionManager) : base(logger)
        {
            AccountRepository = accountRepository;
            ChallengeRequiredAccount = challengeRequiredAccount;
            LoginApi = api;
            LoginSessionManager = loginSessionManager;
            RestoreInstagramSessionManager = restoreInstagramSessionManager;
            RecoverySessionManager = recoverySessionManager;
            SaveSessionManager = saveSessionManager;
        }
        public IGAccount Create(CreateIgAccountCommand command)
        {
            command.InstagramUserName = command.InstagramUserName.Trim();

            var account = AccountRepository.GetByWithState(command.UserToken, command.InstagramUserName);
            if (account != null)
            {
                return RecoverySessionManager.Do(account, command);
            }
            account = LoginSessionManager.Do(account, command);
            if (account.State.Challenger)
            {
                ChallengeRequiredAccount.Do(account, false);
                return SaveSessionManager.Do(account.UserId, account.Username, true);
            }
            return SaveSessionManager.Do(account.UserId, account.Username, false);
        }
    }
}