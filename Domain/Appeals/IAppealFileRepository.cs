using Domain.Admins;

namespace Domain.Appeals.Messages
{
    public interface IAppealFileRepository
    {
        ICollection<AppealFile> Create(ICollection<AppealFile> files);
    }
}
