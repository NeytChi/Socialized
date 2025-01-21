using Core.FileControl;
using Domain.Admins;
using Domain.Appeals.Messages;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Messages;
using UseCases.Exceptions;

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
            appealFileRepository.GetById(messageId).Returns(new AppealMessage());
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
            appealFileRepository.GetById(messageId).Returns(new AppealMessage());
            var manager = new AppealFileManager(logger, fileManager, appealFileRepository);

            var appealFiles = manager.Create(new List<IFormFile> { new FormFileTest() }, messageId);

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

            Assert.Throws<NotFoundException>(() => manager.Create(new List<IFormFile> { new FormFileTest() }, messageId));
        }
    }
}
