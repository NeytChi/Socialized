using Serilog;
using NSubstitute;
using UseCases.Exceptions;
using UseCases.AutoPosts.AutoPostFiles.Commands;
using Domain.AutoPosting;
using UseCases.AutoPosts.AutoPostFiles;
using Core.FileControl;
using UseCases.Base;

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
        private readonly AutoPost autoPost = new AutoPost();
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
                new CreateAutoPostFileCommand { FormFile = File },
                new CreateAutoPostFileCommand { FormFile = File }
            };

            autoPostFileSave.CreateImageFile(Arg.Any<AutoPostFile>(), Arg.Any<FileDto>()).Returns(true);
            autoPostFileSave.CreateVideoFile(Arg.Any<AutoPostFile>(), Arg.Any<FileDto>()).Returns(true);

            // Act
            var result = autoPostFileManager.Create(files, autoPost, 1);

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Create_WhenSavingImageFails_ThrowsIgAccountException()
        {
            // Arrange
            var files = new List<CreateAutoPostFileCommand>
            {
                new CreateAutoPostFileCommand { FormFile = File }
            };

            autoPostFileSave.CreateImageFile(Arg.Any<AutoPostFile>(), Arg.Any<FileDto>()).Returns(false);

            // Act & Assert
            Assert.Throws<IgAccountException>(() => autoPostFileManager.Create(files, autoPost, 1));
        }

        [Fact]
        public void Create_WhenSavingVideoFails_ThrowsIgAccountException()
        {
            // Arrange
            var files = new List<CreateAutoPostFileCommand>
            {
                new CreateAutoPostFileCommand { FormFile = File }
            };

            autoPostFileSave.CreateVideoFile(Arg.Any<AutoPostFile>(), Arg.Any<FileDto>()).Returns(false);

            // Act & Assert
            Assert.Throws<IgAccountException>(() => autoPostFileManager.Create(files, autoPost, 1));
        }
    }
}
