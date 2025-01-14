using FfmpegConverter;

namespace UseCases.AutoPosts.AutoPostFiles
{
    public class FileConverterMocking : IFileConverter
    {
        public Stream ConvertImage(Stream streamFile, string contentType)
        {
            return streamFile;
        }

        public bool ConvertImage(string contentType, string pathFile)
        {
            return true;
        }

        public string ConvertVideo(Stream streamVideo, string contentType)
        {
            return string.Empty;
        }

        public bool ConvertVideo(string contentType, string pathFile)
        {
            return false;
        }

        public Stream GetVideoThumbnail(string pathFile)
        {
            return new MemoryStream();
        }
    }
}
