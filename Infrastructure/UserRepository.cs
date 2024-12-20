using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UserRepository
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
    }
}
