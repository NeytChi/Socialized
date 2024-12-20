using Domain.GettingSubscribes;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class TaskDataRepository
    {
        private Context _context;

        public TaskDataRepository(Context context)
        {
            _context = context;
        }
        public TaskData GetBy(string userToken, long dataId, bool deleted = false)
        {
            return (from d in _context.TaskData
                 join t in _context.TaskGS on d.TaskId equals t.Id
                 join account in _context.IGAccounts on t.AccountId equals account.Id
                 join u in _context.Users on account.UserId equals u.Id
                 where u.TokenForUse == userToken && d.Id == dataId && d.IsDeleted == deleted select d)
                 .FirstOrDefault();
        }
        public List<TaskData> GetBy(long taskId, bool deleted = false)
        {
            return _context.TaskData.Where(d => d.IsDeleted == deleted && d.TaskId == taskId).ToList();
        }
    }
}
