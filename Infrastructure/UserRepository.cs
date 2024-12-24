using Domain.Users;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private Context _context;
        public UserRepository(Context context)
        {
            _context = context;
        }
        public User GetByEmail(string email)
        {
            return _context.Users.Where(u => u.Email == email).FirstOrDefault();
        }
        public User GetByEmail(string email, bool deleted)
        {
            return _context.Users.Where(u => u.Email == email && u.IsDeleted == deleted).FirstOrDefault();
        }
        public User GetByEmail(string email, bool deleted = false, bool activate = true)
        {
            return _context.Users.Where(u => u.Email == email && u.IsDeleted == deleted && u.Activate == activate).FirstOrDefault();
        }
        public User GetByUserTokenNotDeleted(string userToken)
        {
            return _context.Users.Where(u => u.TokenForUse == userToken && u.IsDeleted == false).FirstOrDefault();
        }
        public User GetByRecoveryToken(string recoveryToken, bool deleted)
        {
            return _context.Users.Where(u => u.RecoveryToken == recoveryToken && u.IsDeleted == deleted).FirstOrDefault();
        }
        public User GetByHash(string hash, bool deleted, bool activate)
        {
            return _context.Users.Where(u => u.HashForActivate == hash && u.Activate == activate && u.IsDeleted == deleted).FirstOrDefault();
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
