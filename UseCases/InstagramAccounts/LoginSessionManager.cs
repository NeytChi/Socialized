using Core;
using Domain.InstagramAccounts;
using Serilog;
using UseCases.Exceptions;
using UseCases.InstagramApi;

namespace UseCases.InstagramAccounts
{
    public class LoginSessionManager : BaseManager
    {
        private ILoginApi Api;
        public ProfileCondition ProfileCondition = new ProfileCondition();

        public LoginSessionManager(ILogger logger, ILoginApi api) : base(logger)
        {
            Api = api;
        }
        public InstagramLoginResult Do(IGAccount account)
        {
            string message = "";
            var result = Api.Do(account);
            switch (result)
            {
                case InstagramLoginState.Success:
                    message = "Сесія Instagram аккаунт був успішно залогінен.";
                    return new InstagramLoginResult { Success = true, State = InstagramLoginState.Success } ;
                case InstagramLoginState.ChallengeRequired:
                    message = "Сесія Instagram аккаунту потребує підтвердження по коду.";
                    return new InstagramLoginResult { Success = false, State = InstagramLoginState.ChallengeRequired };
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
    }
}
