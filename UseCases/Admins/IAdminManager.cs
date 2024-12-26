using Domain.Users;
using Domain;
using UseCases.Admins.Commands;

namespace UseCases.Admins
{
    public interface IAdminManager
    {
        Admin Create(CreateAdminCommand command);
        Admin Authentication(AuthenticationCommand command);
        void SetupPassword(SetupPasswordCommand command);
        void Delete(DeleteAdminCommand command);
        ICollection<Admin> GetAdmins(long adminId, int since, int count);
        ICollection<User> GetUsers(int since, int count);
        void CreateCodeForRecoveryPassword(string adminEmail);
        void ChangePassword(ChangePasswordCommand command);
    }
}
