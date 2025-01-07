using Core.FileControl;
using Domain.Appeals.Messages;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog;
using UseCases.Appeals.Messages;

namespace UseCasesTests.Appeals
{
    public class AppealFileManagerTests
    {
        [Fact]
        public void Create_WhenFilesIsEmpty_ReturnEmptyCollection()
        {
            var messageId = 1;
            var logger = Substitute.For<ILogger>();
            var fileManager = Substitute.For<IFileManager>();
            var appealFileRepository = Substitute.For<IAppealFileRepository>();
            var manager = new AppealFileManager(logger, fileManager, appealFileRepository);

            var appealFiles = manager.Create(new List<IFormFile> { }, messageId);

            Assert.Empty(appealFiles);
        }
        [Fact]
        public void Create_WhenFilesIsExist_ReturnFilesCollection()
        {
            var messageId = 1;
            var logger = Substitute.For<ILogger>();
            var fileManager = Substitute.For<IFileManager>();
            var appealFileRepository = Substitute.For<IAppealFileRepository>();
            var manager = new AppealFileManager(logger, fileManager, appealFileRepository);

            var appealFiles = manager.Create(new List<IFormFile> { new FormFileTest() }, messageId);

            Assert.Equal(1, appealFiles.Count);
        }
    }
}
