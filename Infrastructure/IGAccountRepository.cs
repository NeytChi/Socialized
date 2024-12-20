using Domain.InstagramAccounts;

namespace Infrastructure
{
    public class IGAccountRepository
    {
        private Context Context;
        public IGAccountRepository(Context context)
        {
            Context = context;
        }
        public IGAccount GetBy(long accountId)
        {
            return Context.IGAccounts.Where(a => a.Id == accountId).FirstOrDefault();
        }
    }
}
