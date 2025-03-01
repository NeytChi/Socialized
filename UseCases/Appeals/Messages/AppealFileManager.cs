using Serilog;
using Core.FileControl;
using Domain.Admins;
using Domain.Appeals.Messages;
using UseCases.Exceptions;
using UseCases.Base;

namespace UseCases.Appeals.Messages
{
    public class AppealFileManager : BaseManager , IAppealFileManager
    {
        private IFileManager FileManager;
        private IAppealFileRepository AppealFilesRepository;

        public AppealFileManager(ILogger logger, 
            IFileManager fileManager,
            IAppealFileRepository appealFilesRepository) : base(logger)
        {
            FileManager = fileManager;
            AppealFilesRepository = appealFilesRepository;
        }
        public HashSet<AppealFile> Create(ICollection<FileDto> upload, long messageId)
        {
            var message = AppealFilesRepository.GetById(messageId);

            return message == null ? 
                throw new NotFoundException($"Сервер не визначив звернення по id={messageId}.") : 
                Create(upload, message);
        }
        public HashSet<AppealFile> Create(ICollection<FileDto> upload, AppealMessage message)
        {
            var files = new HashSet<AppealFile>();
            if (upload != null)
            {
                foreach (var file in upload)
                {
                    var savedFile = FileManager.SaveFileAsync(file.OpenReadStream(), "AppealFiles");
                    var saved = new AppealFile
                    {
                        MessageId = message.Id,
                        RelativePath = savedFile.Result,
                        Message = message
                    };
                    files.Add(saved);
                }
                AppealFilesRepository.Create(files);
                Logger.Information("Були створені файли для повідомлення в зверненні.");
            }
            return files;
        }
    }
}
