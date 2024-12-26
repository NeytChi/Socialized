using UseCases.Users.Commands;

namespace UseCases.Users
{
    public interface IUsersManager
    {
        void Create(CreateUserCommand command);
        void RegistrationEmail(string userEmail, string culture);
        void Activate(string hash);
        void Delete(string userToken);
    }
}
