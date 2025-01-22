using Core;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;
using Domain.Users;
using UseCases.Exceptions;

namespace UseCases.InstagramAccounts
{
    public class CreateIGAccountManager : BaseManager, IIGAccountManager
    {
        private IIGAccountRepository AccountRepository;
        private IChallengeRequiredAccount ChallengeRequiredAccount;
        private ILoginSessionManager LoginSessionManager;
        private IRecoverySessionManager RecoverySessionManager;
        private ISaveSessionManager SaveSessionManager;
        private IUserRepository UserRepository;
        private ProfileCondition ProfileCondition = new ProfileCondition();

        public CreateIGAccountManager(ILogger logger, 
            IIGAccountRepository accountRepository,
            IChallengeRequiredAccount challengeRequiredAccount,
            ILoginSessionManager loginSessionManager,
            IRecoverySessionManager recoverySessionManager,
            ISaveSessionManager saveSessionManager,
            IUserRepository userRepository) : base(logger)
        {
            AccountRepository = accountRepository;
            ChallengeRequiredAccount = challengeRequiredAccount;
            LoginSessionManager = loginSessionManager;
            RecoverySessionManager = recoverySessionManager;
            SaveSessionManager = saveSessionManager;
            UserRepository = userRepository;
        }
        public IGAccount Create(CreateIgAccountCommand command)
        {
            var user = UserRepository.GetByUserTokenNotDeleted(command.UserToken);
            if (user == null)
            {
                throw new NotFoundException("Сервер не визначив користувача по токену.");
            }
            var account = AccountRepository.GetByWithState(command.UserToken, command.InstagramUserName);
            if (account != null)
            {
                return RecoverySessionManager.Do(account, command);
            }
            account = LoginSessionManager.Do(command);
            if (account.State.Challenger)
            {
                ChallengeRequiredAccount.Do(account, false);
                return SaveSessionManager.Do(user, account.Username, true);
            }
            return SaveSessionManager.Do(user, account.Username, false);
        }
    }
}