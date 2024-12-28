using Domain.Admins;
using Domain.Appeals.Messages;

namespace Infrastructure
{
    public class AppealFileRepository : IAppealFileRepository
    {
        private Context _context;
        public AppealFileRepository(Context context)
        {
            _context = context;
        }
        public ICollection<AppealFile> Create(ICollection<AppealFile> files)
        {
            _context.AppealFiles.AddRange(files);
            _context.SaveChanges();
            return files;
        }
    }
}
