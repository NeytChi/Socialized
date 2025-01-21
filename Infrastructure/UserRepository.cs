using Domain.Users;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private Context Context;
        public UserRepository(Context context) => Context = context;

        public User? GetBy(long userId)
        {
            return Context.Users.Find(userId);
        }
        public User? GetByEmail(string email)
        {
            return Context.Users.Where(u => u.Email == email).FirstOrDefault();
        }
        public User? GetByEmail(string email, bool deleted)
        {
            return Context.Users.Where(u => u.Email == email && u.IsDeleted == deleted).FirstOrDefault();
        }
        public User? GetByEmail(string email, bool deleted = false, bool activate = true)
        {
            return Context.Users.Where(u => u.Email == email && u.IsDeleted == deleted && u.Activate == activate).FirstOrDefault();
        }
        public User? GetByUserTokenNotDeleted(string userToken)
        {
            return Context.Users.Where(u => u.TokenForUse == userToken && u.IsDeleted == false).FirstOrDefault();
        }
        public User? GetByRecoveryToken(string recoveryToken, bool deleted)
        {
            return Context.Users.Where(u => u.RecoveryToken == recoveryToken && u.IsDeleted == deleted).FirstOrDefault();
        }
        public User? GetByHash(string hash, bool deleted, bool activate)
        {
            return Context.Users.Where(u => u.HashForActivate == hash && u.Activate == activate && u.IsDeleted == deleted).FirstOrDefault();
        }

        public void Create(User user)
        {
            Context.Users.Add(user);
            Context.SaveChanges();
        }

        public void Update(User user)
        {
            Context.Users.Update(user);
            Context.SaveChanges();
        }
        public void Delete(User user)
        {
            Update(user);
        }
    }
}
