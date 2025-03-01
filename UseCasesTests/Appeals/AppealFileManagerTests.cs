using Core.FileControl;
using Domain.Admins;
using Domain.Appeals.Messages;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Messages;
using UseCases.Base;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals
{
    public class AppealFileManagerTests
    {
        private FileDto File = new FileDto
        {
            FileName = "test",
            Name = "test",
            ContentType = "image",
            ContentDisposition = "form-data",
            Length = 3,
            Headers = new Dictionary<string, string> { { "test", "test" } },
            Stream = new MemoryStream()
        };

        [Fact]
        public void Create_WhenFilesIsEmpty_ReturnEmptyCollection()
        {
            var messageId = 1;
            var logger = Substitute.For<ILogger>();
            var fileManager = Substitute.For<IFileManager>();
            var appealFileRepository = Substitute.For<IAppealFileRepository>();
            appealFileRepository.GetById(messageId).Returns(new AppealMessage());
            var manager = new AppealFileManager(logger, fileManager, appealFileRepository);

            var appealFiles = manager.Create(new List<FileDto> { }, messageId);

            Assert.Empty(appealFiles);
        }
        [Fact]
        public void Create_WhenFilesIsExist_ReturnFilesCollection()
        {
            var messageId = 1;
            var logger = Substitute.For<ILogger>();
            var fileManager = Substitute.For<IFileManager>();
            var appealFileRepository = Substitute.For<IAppealFileRepository>();
            appealFileRepository.GetById(messageId).Returns(new AppealMessage());
            var manager = new AppealFileManager(logger, fileManager, appealFileRepository);

            var appealFiles = manager.Create(new List<FileDto> { File }, messageId);

            Assert.Single(appealFiles);
        }
        [Fact]
        public void Create_WhenFilesIsExist_ThrowNotFoundException()
        {
            var messageId = 1;
            var logger = Substitute.For<ILogger>();
            var fileManager = Substitute.For<IFileManager>();
            var appealFileRepository = Substitute.For<IAppealFileRepository>();
            appealFileRepository.GetById(messageId).ReturnsNull();
            var manager = new AppealFileManager(logger, fileManager, appealFileRepository);

            Assert.Throws<NotFoundException>(() => manager.Create(new List<FileDto> { File }, messageId));
        }
    }
}
