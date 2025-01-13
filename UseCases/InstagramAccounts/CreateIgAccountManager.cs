using Core;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramAccounts
{
    public class CreateIGAccountManager : BaseManager, IIGAccountManager
    {
        private IIGAccountRepository AccountRepository;
        private IChallengeRequiredAccount ChallengeRequiredAccount;
        private ILoginSessionManager LoginSessionManager;
        private IRecoverySessionManager RecoverySessionManager;
        private ISaveSessionManager SaveSessionManager;
        private ProfileCondition ProfileCondition = new ProfileCondition();

        public CreateIGAccountManager(ILogger logger, 
            IIGAccountRepository accountRepository,
            IChallengeRequiredAccount challengeRequiredAccount,
            ILoginSessionManager loginSessionManager,
            IRecoverySessionManager recoverySessionManager,
            ISaveSessionManager saveSessionManager) : base(logger)
        {
            AccountRepository = accountRepository;
            ChallengeRequiredAccount = challengeRequiredAccount;
            LoginSessionManager = loginSessionManager;
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
            account = LoginSessionManager.Do(command);
            if (account.State.Challenger)
            {
                ChallengeRequiredAccount.Do(account, false);
                return SaveSessionManager.Do(account.UserId, account.Username, true);
            }
            return SaveSessionManager.Do(account.UserId, account.Username, false);
        }
    }
}