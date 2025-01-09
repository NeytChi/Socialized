using Serilog;
using NSubstitute;
using UseCases.Exceptions;
using UseCases.AutoPosts.AutoPostFiles.Commands;
using Domain.AutoPosting;
using UseCases.AutoPosts.AutoPostFiles;
using Microsoft.AspNetCore.Http;
using UseCasesTests.Appeals;
using Core.FileControl;

namespace UseCasesTests.AutoPosts.AutoPostFiles
{
    public class AutoPostFileManagerTests
    {
        private readonly ILogger logger;
        private readonly IAutoPostRepository autoPostRepository;
        private readonly IAutoPostFileRepository autoPostFileRepository;
        private readonly IAutoPostFileSave autoPostFileSave;
        private readonly IFileManager fileManager;
        private readonly AutoPostFileManager autoPostFileManager;

        public AutoPostFileManagerTests()
        {
            logger = Substitute.For<ILogger>();
            autoPostRepository = Substitute.For<IAutoPostRepository>();
            autoPostFileRepository = Substitute.For<IAutoPostFileRepository>();
            autoPostFileSave = Substitute.For<IAutoPostFileSave>();
            fileManager = Substitute.For<IFileManager>();
            autoPostFileManager = new AutoPostFileManager(logger, autoPostRepository, fileManager, autoPostFileRepository, autoPostFileSave);
        }

        [Fact]
        public void Create_WhenFilesAreValid_CreatesAutoPostFiles()
        {
            // Arrange
            var files = new List<CreateAutoPostFileCommand>
            {
                new CreateAutoPostFileCommand { FormFile = new FormFileTest() { } },
                new CreateAutoPostFileCommand { FormFile = new FormFileTest() { } }
            };

            autoPostFileSave.CreateImageFile(Arg.Any<AutoPostFile>(), Arg.Any<IFormFile>()).Returns(true);
            autoPostFileSave.CreateVideoFile(Arg.Any<AutoPostFile>(), Arg.Any<IFormFile>()).Returns(true);

            // Act
            var result = autoPostFileManager.Create(files, 1);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Create_WhenSavingImageFails_ThrowsIgAccountException()
        {
            // Arrange
            var files = new List<CreateAutoPostFileCommand>
            {
                new CreateAutoPostFileCommand { FormFile = new FormFileTest() { } }
            };

            autoPostFileSave.CreateImageFile(Arg.Any<AutoPostFile>(), Arg.Any<IFormFile>()).Returns(false);

            // Act & Assert
            Assert.Throws<IgAccountException>(() => autoPostFileManager.Create(files, 1));
        }

        [Fact]
        public void Create_WhenSavingVideoFails_ThrowsIgAccountException()
        {
            // Arrange
            var files = new List<CreateAutoPostFileCommand>
            {
                new CreateAutoPostFileCommand { FormFile = new FormFileTest() { } }
            };

            autoPostFileSave.CreateVideoFile(Arg.Any<AutoPostFile>(), Arg.Any<IFormFile>()).Returns(false);

            // Act & Assert
            Assert.Throws<IgAccountException>(() => autoPostFileManager.Create(files, 1));
        }
    }
}
