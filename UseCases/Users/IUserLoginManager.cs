using Domain.Users;
using UseCases.Users.Commands;

namespace UseCases.Users
{
    public interface IUserLoginManager
    {
        User Login(LoginUserCommand command);
        void LogOut(string userToken);
    }
}
