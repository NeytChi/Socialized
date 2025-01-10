using Serilog;
using Domain.InstagramAccounts;
using UseCases.Exceptions;
using UseCases.InstagramApi;

namespace UseCases.InstagramAccounts
{
    public class ChallengeRequiredAccount : BaseManager, IChallengeRequiredAccount
    {
        private IGetChallengeRequireVerifyMethod GetChallengeRequireVerifyMethod;
        private IVerifyCodeForChallengeRequire VerifyCodeToSMSForChallenge;

        public ChallengeRequiredAccount(IGetChallengeRequireVerifyMethod getChallengeRequireVerifyMethod,
            IVerifyCodeForChallengeRequire verifyCodeForChallengeRequire,
            ILogger logger) : base (logger) 
        {
            GetChallengeRequireVerifyMethod = getChallengeRequireVerifyMethod;
            VerifyCodeToSMSForChallenge = verifyCodeForChallengeRequire;
        }
        public void Do(IGAccount account, bool replay)
        {
            var challenge = GetChallengeRequireVerifyMethod.Do(account);
            if (!challenge.Success)
            {
                throw new IgAccountException("Сервер не може підтвердити Instagram аккаунт.");
            }
            var result = VerifyCodeToSMSForChallenge.Do(replay, account);
            if (!result.Success)
            {
                throw new IgAccountException("Сервер не може запустити верифікації аккаунту через SMS код.");
            }
            Logger.Information("Сесія Instagram аккаунту була пройдена через процедуру підтвердження.");
        }
    }
}
