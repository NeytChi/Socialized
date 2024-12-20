using Domain;

namespace Infrastructure
{
    public class AdminRepository
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
        public void Update(Admin admin)
        {
            _context.Admins.Update(admin);
            _context.SaveChanges();
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
        public Admin GetByRecoveryCode(int recoveryCode)
        {
            return _context.Admins.Where(a => a.RecoveryCode == recoveryCode).FirstOrDefault();
        }
        public Admin[] GetActiveAdmins(int adminId, int since, int count, bool isDeleted = false)
        {
            return _context.Admins.Where(a => a.Id != adminId && a.IsDeleted == isDeleted)
                .Skip(since * count)
                .Take(count)
                .ToArray();
        }
        public dynamic GetUsers(int since, int count, bool isDeleted = false, bool activate = true)
        {
            var users = _context.Users
                .Join(_context.UserProfile,
                      user => user.Id,
                      profile => profile.UserId,
                      (user, profile) => new { user, profile })
                .Where(up => up.user.IsDeleted == isDeleted && up.user.Activate == activate)
                .OrderByDescending(up => up.user.Id)
                .Select(up => new
                {
                    user_email = up.user.Email,
                    registration = up.user.CreatedAt,
                    activity = up.user.LastLoginAt,
                    sessions_count = _context.BusinessAccounts
                        .Count(ba => ba.AccountId == up.user.Id && !ba.IsDeleted)
                })
                .Skip(since * count)
                .Take(count)
                .ToArray();
            return users;
        }
    }
}
