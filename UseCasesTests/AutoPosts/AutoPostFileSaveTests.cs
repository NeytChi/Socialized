using Serilog;
using NSubstitute;
using FfmpegConverter;
using Core.FileControl;
using Domain.AutoPosting;
using Microsoft.AspNetCore.Http;
using UseCases.AutoPosts.AutoPostFiles;
using NSubstitute.ReturnsExtensions;

namespace UseCasesTests.AutoPosts.AutoPostFiles
{
    public class AutoPostFileSaveTests
    {
        private readonly ILogger logger;
        private readonly IFileConverter fileConverter;
        private readonly IFileManager fileManager;
        private readonly IFileMover fileMover;
        private readonly AutoPostFileSave autoPostFileSave;

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
            var file = Substitute.For<IFormFile>();
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
            var file = Substitute.For<IFormFile>();
            var stream = new MemoryStream();
            fileConverter.ConvertImage(file.OpenReadStream(), file.ContentType).Returns(stream);
            fileManager.SaveFile(stream, "auto-posts").Returns("saved/path");

            // Act
            var result = autoPostFileSave.CreateImageFile(post, file);

            // Assert
            Assert.True(result);
            Assert.Equal("saved/path", post.Path);
            fileManager.Received().SaveFile(stream, "auto-posts");
        }
        [Fact]
        public void CreateVideoFile_WhenConvertVideoReturnsNull_LogsErrorAndReturnsFalse()
        {
            // Arrange
            var post = new AutoPostFile { Path = "", MediaId = "", VideoThumbnail = "", post = new AutoPost() };
            var file = Substitute.For<IFormFile>();
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
            var file = Substitute.For<IFormFile>();
            var videoPath = "video/path";
            FileStream videoStream = null;
            var thumbnailStream = new MemoryStream();

            fileConverter.ConvertVideo(file.OpenReadStream(), file.ContentType).Returns(videoPath);
            fileMover.OpenRead(videoPath + ".mp4").Returns(videoStream);
            fileConverter.GetVideoThumbnail(videoPath + ".mp4").Returns(thumbnailStream);
            fileManager.SaveFile(videoStream, "auto-posts").Returns("saved/video/path");
            fileManager.SaveFile(thumbnailStream, "auto-posts").Returns("saved/thumbnail/path");

            // Act
            var result = autoPostFileSave.CreateVideoFile(post, file);

            // Assert
            Assert.True(result);
            Assert.Equal("saved/video/path", post.Path);
            Assert.Equal("saved/thumbnail/path", post.VideoThumbnail);
            fileManager.Received().SaveFile(videoStream, "auto-posts");
            fileManager.Received().SaveFile(thumbnailStream, "auto-posts");
        }
    }
}