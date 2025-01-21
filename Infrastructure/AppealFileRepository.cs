using Domain.Admins;
using Domain.Appeals.Messages;

namespace Infrastructure
{
    public class AppealFileRepository : IAppealFileRepository
    {
        private Context Context;
        public AppealFileRepository(Context context)
        {
            Context = context;
        }
        public ICollection<AppealFile> Create(ICollection<AppealFile> files)
        {
            Context.AppealFiles.AddRange(files);
            Context.SaveChanges();
            return files;
        }

        public AppealMessage? GetById(long messageId) => Context.AppealMessages.Find(messageId);
    }
}
