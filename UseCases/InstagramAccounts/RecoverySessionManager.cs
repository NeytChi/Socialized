using Core;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramAccounts
{
    public class RecoverySessionManager : IRecoverySessionManager
    {
        private ILogger Logger;
        private IIGAccountRepository AccountRepository;
        private ILoginSessionManager LoginSessionManager;
        private IGetStateData GetStateData;
        private ProfileCondition ProfileCondition;
        private IChallengeRequiredAccount ChallengeRequiredAccount;

        public RecoverySessionManager(ILogger logger, 
            IIGAccountRepository accountRepository, 
            ILoginSessionManager loginSessionManager, 
            ProfileCondition profileCondition, 
            IChallengeRequiredAccount challengeRequiredAccount,
            IGetStateData getStateData)
        {
            Logger = logger;
            AccountRepository = accountRepository;
            LoginSessionManager = loginSessionManager;
            GetStateData = getStateData;
            ProfileCondition = profileCondition;
            ChallengeRequiredAccount = challengeRequiredAccount;
        }

        public IGAccount Do(IGAccount account, IgAccountRequirements requirements)
        {
            account.State.Relogin = true;
            account = LoginSessionManager.Do(account, requirements);
            if (account != null && !account.State.Challenger)
            {
                var stateData = GetStateData.AsString(account);
                account.State.SessionSave = ProfileCondition.Encrypt(stateData);
                account.State.Usable = true;
                account.State.Relogin = false;
                account.State.Challenger = false;
                account.IsDeleted = false;
                AccountRepository.Update(account);
                Logger.Information($"Сесія була востановлена, id={account.Id}");
            }
            if (account != null && account.State.Challenger)
            {
                ChallengeRequiredAccount.Do(account, true);
                var stateData = GetStateData.AsString(account);
                account.State.SessionSave = ProfileCondition.Encrypt(stateData);
                account.State.Usable = false;
                account.State.Relogin = false;
                account.State.Challenger = true;
                account.IsDeleted = false;
                AccountRepository.Update(account);
                Logger.Information($"Сесія потребує відновлення, id={account.Id}");
            }
            return account ?? throw new InvalidOperationException("Операція логіну аккаунту була провалена.");
        }
    }
}
