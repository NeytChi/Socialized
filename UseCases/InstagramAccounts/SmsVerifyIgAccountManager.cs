using Core;
using Serilog;
using UseCases.Exceptions;
using Domain.InstagramAccounts;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramAccounts
{
    public class SmsVerifyIgAccountManager : ISmsVerifyIgAccountManager
    {
        private ILogger Logger;
        private IGetStateData GetStateData;
        private IIGAccountRepository AccountRepository;
        private IRestoreInstagramSessionManager RestoreInstagramSessionManager;
        private IVerifyCodeForChallengeRequire VerifyCodeForChallengeRequire;
        private ProfileCondition ProfileCondition;

        public SmsVerifyIgAccountManager(ILogger logger,
            IGetStateData getStateData, 
            IIGAccountRepository accountRepository,
            IRestoreInstagramSessionManager restoreInstagramSessionManager,
            ProfileCondition profileCondition,
            IVerifyCodeForChallengeRequire verifyCodeForChallengeRequire)
        {
            Logger = logger;
            AccountRepository = accountRepository;
            RestoreInstagramSessionManager = restoreInstagramSessionManager;
            ProfileCondition = profileCondition;
            VerifyCodeForChallengeRequire = verifyCodeForChallengeRequire;
            GetStateData = getStateData;
        }
        public void SmsVerifySession(SmsVefiryIgAccountCommand command)
        {
            var account = AccountRepository.Get(command.UserToken, command.AccountId);
            if (account == null)
            {
                throw new NotFoundException("Сервер не визначив запис Instagram аккаунту по id.");
            }
            if (!account.State.Challenger)
            {
                throw new NotFoundException("Сесія Instagram аккаунту не потребує підтвердження аккаунту.");
            }
            var session = RestoreInstagramSessionManager.Do(account);
            var loginResult = VerifyCodeForChallengeRequire.Do(command.VerifyCode.ToString(), account);
            if (loginResult.State != InstagramLoginState.Success)
            {
                throw new IgAccountException("Код підвердження Instagram аккаунту не вірний.");
            }
            var sessionString = GetStateData.AsString(account);
            account.State.SessionSave = ProfileCondition.Encrypt(sessionString);
            account.State.Usable = true;
            account.State.Relogin = false;
            account.State.Challenger = false;
            AccountRepository.Update(account);
            Logger.Information($"Сесія Instagram аккаунту було веріфікована, id={account.Id}.");
        }
    }
}