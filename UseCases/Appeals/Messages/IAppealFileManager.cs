using Domain.Admins;
using UseCases.Base;

namespace UseCases.Appeals.Messages
{
    public interface IAppealFileManager
    {
        HashSet<AppealFile> Create(ICollection<FileDto> upload, AppealMessage message);
        HashSet<AppealFile> Create(ICollection<FileDto> upload, long messageId);
    }
}
