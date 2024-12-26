using UseCases.Users.Commands;

namespace UseCases.Users
{
    public interface IUserPasswordRecoveryManager
    {
        void RecoveryPassword(string userEmail, string culture);
        string CheckRecoveryCode(CheckRecoveryCodeCommand command);
        void ChangePassword(ChangeUserPasswordCommand command);
        void ChangeOldPassword(ChangeOldPasswordCommand command);
    }
}
