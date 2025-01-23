using Domain.Admins;
using Domain.Users;

namespace Infrastructure
{
    public class AdminRepository : IAdminRepository
    {
        private Context _context;
        public AdminRepository(Context context) 
        {
            _context = context;
        }
        public void Create(Admin admin)
        {
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }
        public Admin Update(Admin admin)
        {
            _context.Admins.Update(admin);
            _context.SaveChanges();
            return admin;
        }
        public Admin GetByAdminId(long adminId, bool deleted = false)
        {
            return _context.Admins.Where(a => a.Id == adminId && a.IsDeleted == deleted).FirstOrDefault();
        }
        public Admin GetByEmail(string email, bool deleted = false)
        {
            return _context.Admins.Where(a => a.Email == email && a.IsDeleted == deleted).FirstOrDefault();
        }
        public Admin GetByPasswordToken(string passwordToken, bool deleted = false)
        {
            return _context.Admins.Where(a => a.TokenForStart == passwordToken && a.IsDeleted == deleted).FirstOrDefault();
        }
        public Admin GetByRecoveryCode(int recoveryCode, bool deleted = false)
        {
            return _context.Admins.Where(a => a.RecoveryCode == recoveryCode && a.IsDeleted == deleted).FirstOrDefault();
        }
        public Admin[] GetActiveAdmins(long adminId, int since, int count, bool isDeleted = false)
        {
            return _context.Admins.Where(a => a.Id != adminId && a.IsDeleted == isDeleted)
                .Skip(since * count)
                .Take(count)
                .ToArray();
        }
        public ICollection<User> GetUsers(int since, int count, bool isDeleted = false, bool activate = true)
        {
            var users = _context.Users
                .Where(u => u.IsDeleted == isDeleted && u.Activate == activate)
                .OrderByDescending(u => u.Id)
                .Skip(since * count)
                .Take(count)
                .ToArray();
            return users;
        }
    }
}
