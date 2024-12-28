using Domain.Admins;
using Domain.Appeals;

namespace Infrastructure
{
    public class AppealRepository : IAppealRepository
    {
        private Context _context;
        public AppealRepository(Context context) 
        {
            _context = context;
        }
        public Appeal GetBy(int appealId)
        {
            return _context.Appeals.Where(a => a.Id == appealId).FirstOrDefault();
        }
        public Appeal GetBy(long appealId, string userToken, int appealStateIsNot = 4) 
            => _context.Appeals.Join(_context.Users,
                    appeal => appeal.UserId,
                    user => user.Id,
                    (appeal, user) => new { appeal, user })
                .Where(au => au.appeal.Id == appealId
                    && au.appeal.State != appealStateIsNot
                    && au.user.TokenForUse == userToken)
                .Select(au => au.appeal)
                .FirstOrDefault();

        public Appeal[] GetAppealsBy(string userToken, int since = 0, int count = 10, bool IsUserDeleted = false)
        {
            return _context.Appeals
                .Join(_context.Users,
                    appeal => appeal.UserId,
                    user => user.Id,
                    (appeal, user) => new { appeal, user })
                .Where(au => au.user.TokenForUse == userToken
                    && au.user.IsDeleted == IsUserDeleted)
                .OrderBy(au => au.appeal.State)
                .OrderByDescending(au => au.appeal.CreatedAt)
                .Select(au => au.appeal)
                .Skip(since * count).Take(count).ToArray();
        }
        public Appeal[] GetAppealsBy(int since, int count)
        {
            return (from appeal in _context.Appeals
                    orderby appeal.State
                    orderby appeal.CreatedAt descending
                    select appeal)
            .Skip(since * count).Take(count).ToArray();
        }
        public void Create(Appeal appeal)
        {
            _context.Appeals.Add(appeal);
            _context.SaveChanges();
        }
        public void Update(Appeal appeal)
        {
            _context.Appeals.Update(appeal);
            _context.SaveChanges();
        }
        public Appeal GetBy(long appealId)
        {
            return _context.Appeals
                .Join(_context.Users,
                    appeal => appeal.UserId,
                    user => user.Id,
                    (appeal, user) => new { appeal, user })
                .Where(au => au.appeal.Id == appealId)
                .Select(au => au.appeal)
                .FirstOrDefault();
        }
        public Appeal GetBy(long appealId, string userToken)
        {
            return _context.Appeals.Where(a => a.Id == appealId).FirstOrDefault();
        }
        public Appeal[] GetAppealsBy(string userToken, int since = 0, int count = 10)
        {
            return _context.Appeals
                .Join(_context.Users,
                    appeal => appeal.UserId,
                    user => user.Id,
                    (appeal, user) => new { appeal, user })
                .Where(au => au.user.TokenForUse == userToken)
                .Select(au => au.appeal)
                .ToArray();
        }
    }
}
