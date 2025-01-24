using Serilog;
using FfmpegConverter;
using Core.FileControl;
using Domain.AutoPosting;
using UseCases.Base;

namespace UseCases.AutoPosts.AutoPostFiles
{
    public class AutoPostFileSave : BaseManager, IAutoPostFileSave
    {
        private IFileConverter FileConverter;
        private IFileManager FileManager;
        private IFileMover FileMover;

        public AutoPostFileSave(ILogger logger,
            IFileConverter fileConverter,
            IFileManager fileManager,
            IFileMover fileMover) : base(logger)
        {
            FileConverter = fileConverter;
            FileManager = fileManager;
            FileMover = fileMover;
        }
        public bool CreateImageFile(AutoPostFile post, FileDto file)
        {
            var stream = FileConverter.ConvertImage(file.OpenReadStream(), file.ContentType);

            if (stream == null)
            {
                Logger.Error("Сервер не визначив формат картинки для збереження.");
                return false;
            }
            post.Path = FileManager.SaveFile(stream, "auto-posts");
            return true;
        }
        public bool CreateVideoFile(AutoPostFile post, FileDto file)
        {
            string pathFile = FileConverter.ConvertVideo(file.OpenReadStream(), file.ContentType);

            if (pathFile == null)
            {
                Logger.Error("Сервер не визначив формат відео для збереження.");
                return false;
            }
            var stream = FileMover.OpenRead(pathFile + ".mp4");
            if (FileMover.Exists(pathFile))
            {
                FileMover.Delete(pathFile);
            }
            post.Path = FileManager.SaveFile(stream, "auto-posts");
            var thumbnail = FileConverter.GetVideoThumbnail(pathFile + ".mp4");
            post.VideoThumbnail = FileManager.SaveFile(thumbnail, "auto-posts");
            FileMover.Delete(pathFile + ".mp4");
            return true;
        }
    }
}
