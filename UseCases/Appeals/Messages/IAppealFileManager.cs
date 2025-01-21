using Domain.Admins;
using Microsoft.AspNetCore.Http;

namespace UseCases.Appeals.Messages
{
    public interface IAppealFileManager
    {
        HashSet<AppealFile> Create(ICollection<IFormFile> upload, AppealMessage message);
        HashSet<AppealFile> Create(ICollection<IFormFile> upload, long messageId);
    }
}
