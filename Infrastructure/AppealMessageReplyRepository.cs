using Domain.Admins;
using Domain.Appeals.Replies;

namespace Infrastructure
{
    public class AppealMessageReplyRepository : IAppealMessageReplyRepository
    {
        private Context _context;
        public AppealMessageReplyRepository(Context context)
        {
            _context = context;
        }
        public void Create(AppealMessageReply reply)
        {
            _context.AppealReplies.Add(reply);
            _context.SaveChanges();
        }
        public AppealMessageReply Get(long id)
        {
            return _context.AppealReplies.Where(a => a.Id == id).FirstOrDefault();
        }
        public void Update(AppealMessageReply reply)
        {
            _context.AppealReplies.Update(reply);
            _context.SaveChanges();
        }
    }
}
