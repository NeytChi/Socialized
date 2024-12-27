using Domain.InstagramAccounts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class IGAccountRepository : IIGAccountRepository
    {
        private Context Context;
        public IGAccountRepository(Context context)
        {
            Context = context;
        }

        public void Create(IGAccount account)
        {
            Context.IGAccounts.Add(account);
            Context.SaveChanges();
        }
        public void Update(IGAccount account)
        {
            Context.IGAccounts.Update(account);
            Context.SaveChanges();
        }
        public IGAccount Get(long accountId, bool isDeleted = false, bool usable = true)
        {
            return Context.IGAccounts.Where(a => a.Id == accountId && a.IsDeleted == isDeleted).FirstOrDefault();
        }

        public IGAccount Get(string userToken, long accountId, bool isDeleted = false)
        {
            return Context.IGAccounts
                    .Include(a => a.User)
                    .Where(a => a.Id == accountId 
                        && a.IsDeleted == isDeleted
                        && a.User.TokenForUse == userToken).FirstOrDefault();
        }

        public IGAccount GetBy(long accountId)
        {
            return Context.IGAccounts.Where(a => a.Id == accountId).FirstOrDefault();
        }

        public IGAccount GetByWithState(long userId, string instagramUsername)
        {
            return Context.IGAccounts
                    .Include(a => a.User)
                    .Include(a => a.State)
                    .Where(a => a.Username == instagramUsername
                        && a.User.Id == userId).FirstOrDefault();
        }

        public IGAccount GetByWithState(string userToken, string instagramUsername)
        {
            return Context.IGAccounts
                .Include(a => a.User)
                .Include(a => a.State)
                .Where(a => a.Username == instagramUsername
                    && a.User.TokenForUse == userToken).FirstOrDefault();
        }

        public IGAccount GetByWithState(long accountId, bool accountDeleted = false)
        {
            return Context.IGAccounts
                .Include(a => a.State)
                .Where(a => a.Id == accountId
                    && a.IsDeleted == accountDeleted).FirstOrDefault();
        }
    }
}
