using Domain.Users;

namespace Domain.Admins
{
    public interface IAdminRepository
    {
        void Create(Admin admin);
        Admin Update(Admin admin);
        Admin GetByAdminId(long id, bool deleted = false);
        Admin GetByRecoveryCode(int recoveryCode, bool deleted = false);
        Admin GetByEmail(string email, bool deleted = false);
        Admin GetByPasswordToken(string token, bool deleted = false);
        Admin[] GetActiveAdmins(long adminId, int since, int count, bool isDeleted = false);
        ICollection<User> GetUsers(int since, int count, bool isDeleted = false, bool activate = true);
    }
}
