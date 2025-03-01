using Serilog;
using NSubstitute;
using FfmpegConverter;
using Core.FileControl;
using Domain.AutoPosting;
using UseCases.AutoPosts.AutoPostFiles;
using NSubstitute.ReturnsExtensions;
using UseCases.Base;

namespace UseCasesTests.AutoPosts.AutoPostFiles
{
    public class AutoPostFileSaveTests
    {
        private readonly ILogger logger;
        private readonly IFileConverter fileConverter;
        private readonly IFileManager fileManager;
        private readonly IFileMover fileMover;
        private readonly AutoPostFileSave autoPostFileSave;
        private FileDto file = new FileDto
        {
            FileName = "test",
            Name = "test",
            ContentType = "image",
            ContentDisposition = "form-data",
            Length = 3,
            Headers = new Dictionary<string, string> { { "test", "test" } },
            Stream = new MemoryStream()
        };
        public AutoPostFileSaveTests()
        {
            logger = Substitute.For<ILogger>();
            fileConverter = Substitute.For<IFileConverter>();
            fileManager = Substitute.For<IFileManager>();
            fileMover = Substitute.For<IFileMover>();
            autoPostFileSave = new AutoPostFileSave(logger, fileConverter, fileManager, fileMover);
        }

        [Fact]
        public void CreateImageFile_WhenConvertImageReturnsNull_LogsErrorAndReturnsFalse()
        {
            // Arrange
            var post = new AutoPostFile { Path = "", MediaId = "", VideoThumbnail = "", post = new AutoPost() };
            fileConverter.ConvertImage(file.OpenReadStream(), file.ContentType).ReturnsNull();

            // Act
            var result = autoPostFileSave.CreateImageFile(post, file);

            // Assert
            Assert.False(result);
            logger.Received().Error("Сервер не визначив формат картинки для збереження.");
        }

        [Fact]
        public void CreateImageFile_WhenConvertImageSucceeds_SavesFileAndReturnsTrue()
        {
            // Arrange
            var post = new AutoPostFile { Path = "", MediaId = "", VideoThumbnail = "", post = new AutoPost() };
            var stream = new MemoryStream();
            fileConverter.ConvertImage(file.OpenReadStream(), file.ContentType).Returns(stream);
            fileManager.SaveFileAsync(stream, "auto-posts").Returns("saved/path");

            // Act
            var result = autoPostFileSave.CreateImageFile(post, file);

            // Assert
            Assert.True(result);
            Assert.Equal("saved/path", post.Path);
            fileManager.Received().SaveFileAsync(stream, "auto-posts");
        }
        [Fact]
        public void CreateVideoFile_WhenConvertVideoReturnsNull_LogsErrorAndReturnsFalse()
        {
            // Arrange
            var post = new AutoPostFile { Path = "", MediaId = "", VideoThumbnail = "", post = new AutoPost() };
            fileConverter.ConvertVideo(file.OpenReadStream(), file.ContentType).ReturnsNull();

            // Act
            var result = autoPostFileSave.CreateVideoFile(post, file);

            // Assert
            Assert.False(result);
            logger.Received().Error("Сервер не визначив формат відео для збереження.");
        }

        [Fact]
        public void CreateVideoFile_WhenConvertVideoSucceeds_SavesFileAndThumbnailAndReturnsTrue()
        {
            // Arrange
            var post = new AutoPostFile { Path = "", MediaId = "", VideoThumbnail = "", post = new AutoPost() };
            var videoPath = "video/path";
            var thumbnailStream = new MemoryStream();

            fileConverter.ConvertVideo(file.OpenReadStream(), file.ContentType).Returns(videoPath);
            fileMover.OpenRead(videoPath + ".mp4").ReturnsNull();
            fileConverter.GetVideoThumbnail(videoPath + ".mp4").Returns(thumbnailStream);
            fileManager.SaveFileAsync(Stream.Null, "auto-posts").Returns("saved/video/path");
            fileManager.SaveFileAsync(thumbnailStream, "auto-posts").Returns("saved/thumbnail/path");

            // Act
            var result = autoPostFileSave.CreateVideoFile(post, file);

            // Assert
            Assert.True(result);
        }
    }
}