using UseCases.Users.Response;
using UseCases.Users.Commands;

namespace UseCases.Users
{
    public interface IUserLoginManager
    {
        UserResponse Login(LoginUserCommand command);
        void LogOut(string userToken);
    }
}
