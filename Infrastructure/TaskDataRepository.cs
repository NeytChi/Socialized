using Domain.GettingSubscribes;

namespace Infrastructure
{
    public class TaskDataRepository : ITaskDataRepository
    {
        private Context _context;

        public TaskDataRepository(Context context)
        {
            _context = context;
        }
        public void Create(TaskData taskData)
        {
            _context.TaskData.Add(taskData);
            _context.SaveChanges();
        }
        public void Update(TaskData taskData)
        {
            _context.TaskData.Update(taskData);
            _context.SaveChanges();
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
