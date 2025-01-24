using Core;
using Domain.Users;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users.Commands;

namespace UseCases.Users
{
    public class UserPasswordRecoveryManager : BaseManager, IUserPasswordRecoveryManager
    {
        private IUserRepository UserRepository;
        private ProfileCondition ProfileCondition = new ProfileCondition();
        private IEmailMessanger EmailMessager;

        public UserPasswordRecoveryManager(ILogger logger,
            IUserRepository userRepository,
            IEmailMessanger emailMessager) : base (logger) 
        {
            UserRepository = userRepository;
            EmailMessager = emailMessager;
        }
        public void RecoveryPassword(string userEmail, string culture)
        {
            Logger.Information($"Початок відновлення паролю, email={userEmail}.");
            var user = UserRepository.GetByEmail(userEmail, false, true);

            if (user == null)
            {
                throw new NotFoundException("Сервер не визначив користувача по email для активації аккаунту.");
            }
            user.RecoveryCode = ProfileCondition.CreateCode(6);
            UserRepository.Update(user);
            EmailMessager.SendRecoveryEmail(user.Email, culture, (int)user.RecoveryCode);
            Logger.Information($"Пароль був востановлений для користувача, id={user.Id}.");
        }
        public string CheckRecoveryCode(CheckRecoveryCodeCommand command)
        {
            Logger.Information($"Початок перевірки коду для зміни паролю, email={command.UserEmail}.");
            var user = UserRepository.GetByEmail(command.UserEmail);
            if (user == null)
            {
                throw new NotFoundException("Сервер не визначив користувача по email для перевірки коду востановлення паролю.");
            }
            if (user.RecoveryCode != command.RecoveryCode)
            {
                throw new ValidationException("Код востановлення паролю не вірний.");
            }
            user.RecoveryToken = ProfileCondition.CreateHash(40);
            user.RecoveryCode = -1;
            UserRepository.Update(user);
            Logger.Information($"Перевірен був код востановлення паролю користувача, id={user.Id}.");
            return user.RecoveryToken;
        }
        public void ChangePassword(ChangeUserPasswordCommand command)
        {
            Logger.Information($"Початок зміни паролю для користувача.");
            var user = UserRepository.GetByRecoveryToken(command.RecoveryToken, false);
            if (user == null)
            {
                throw new NotFoundException("Сервер не визначив користувача по токену востановлення для зміни паролю.");
            }
            if (!command.UserPassword.Equals(command.UserConfirmPassword))
            {
                throw new ValidationException("Паролі не співпадають одне з одним.");
            }
            user.Password = ProfileCondition.HashPassword(command.UserPassword);
            user.RecoveryToken = "";
            UserRepository.Update(user);
            Logger.Information($"Пароль користувача було зміненно, id={user.Id}.");
        }
        public void ChangeOldPassword(ChangeOldPasswordCommand command)
        {
            Logger.Information($"Початок зміни старого паролю на новий для користувача.");
            var user = UserRepository.GetByUserTokenNotDeleted(command.UserToken);
            if (user == null)
            {
                throw new NotFoundException("Сервер не визначив користувача по токену для зміни старого паролю користувача.");
            }
            if (!ProfileCondition.VerifyHashedPassword(user.Password, command.OldPassword))
            {
                throw new ValidationException("Пароль користувача не співпадає з паролем на сервері для заміни старого паролю.");
            }
            user.Password = ProfileCondition.HashPassword(command.NewPassword);
            UserRepository.Update(user);
            Logger.Information($"Старий пароль користувача було зміненно, id={user.Id}.");
        }
    }
}
