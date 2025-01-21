using Core;
using Domain.InstagramAccounts;
using Serilog;
using UseCases.Exceptions;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts.Commands;

namespace UseCases.InstagramAccounts
{
    public class LoginSessionManager : BaseManager, ILoginSessionManager
    {
        private ILoginApi Api;
        public ProfileCondition ProfileCondition = new ProfileCondition();

        public LoginSessionManager(ILogger logger, ILoginApi api) : base(logger)
        {
            Api = api;
        }
        public IGAccount Do(IgAccountRequirements accountRequirements)
        {
            string message = "";

            var account = new IGAccount();
            var state = new SessionState
            {
                SessionSave = "",
                Account = account,
                TimeAction = new TimeAction { Account = account }
            };
            account.State = state;

            var result = Api.Do(ref account, accountRequirements);
            switch (result)
            {
                case InstagramLoginState.Success:
                    message = "Сесія Instagram аккаунт був успішно залогінен.";
                    return account;
                case InstagramLoginState.ChallengeRequired:
                    message = "Сесія Instagram аккаунту потребує підтвердження по коду.";
                    return account;
                case InstagramLoginState.TwoFactorRequired:
                    message = "Сесія Instagram аккаунту потребує проходження двох-факторної організації.";
                    break;
                case InstagramLoginState.InactiveUser:
                    message = "Сесія Instagram аккаунту не активна.";
                    break;
                case InstagramLoginState.InvalidUser:
                    message = "Правильно введені данні для входу в аккаунт.";
                    break;
                case InstagramLoginState.BadPassword:
                    message = "Неправильний пароль.";
                    break;
                case InstagramLoginState.LimitError:
                case InstagramLoginState.Exception:
                default:
                    message = $"Невідома помилка при спробі зайти(логін) в Instagram аккаунт. Виключення:{result.ToString()}.";
                    break;
            }
            throw new IgAccountException(message);
        }
        public IGAccount Do(IGAccount account, IgAccountRequirements accountRequirements)
        {
            return Do(accountRequirements);
        }
    }
}