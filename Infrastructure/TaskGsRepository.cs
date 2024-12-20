using Domain.GettingSubscribes;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class TaskGsRepository
    {
        private Context _context;

        public TaskGsRepository(Context context) 
        {
            _context = context;
        }
        public TaskGS GetBy(long taskId, bool taskDeleted = false)
        {
            return _context.TaskGS.Where(t => t.Id == taskId && !t.IsDeleted).FirstOrDefault();
        }
        public TaskGS GetBy(string userToken, long taskId, bool taskDeleted = false)
        {
             var task = _context.TaskGS
                .Join(_context.IGAccounts,
                      t => t.AccountId,
                      s => s.Id,
                      (t, s) => new { t, s })
                .Join(_context.Users,
                      ts => ts.s.UserId,
                      u => u.Id,
                      (ts, u) => new { ts.t, u })
                .Where(tu => tu.u.TokenForUse == userToken
                             && tu.t.Id == taskId
                             && tu.t.IsDeleted == taskDeleted)
                .Select(tu => tu.t)
                .FirstOrDefault();
            return task;
        }
    }
}
