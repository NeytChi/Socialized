using Serilog;
using NSubstitute;
using UseCases.Exceptions;
using UseCases.AutoPosts.Commands;
using Domain.InstagramAccounts;
using Domain.AutoPosting;
using UseCases.AutoPosts;
using UseCases.AutoPosts.AutoPostFiles;
using UseCases.AutoPosts.AutoPostFiles.Commands;
using NSubstitute.ReturnsExtensions;

namespace UseCasesTests.AutoPosts
{
    public class AutoPostManagerTests
    {
        private readonly ILogger logger;
        private readonly IAutoPostRepository autoPostRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAutoPostFileManager autoPostFileManager;
        private readonly IIGAccountRepository iGAccountRepository;
        private readonly AutoPostManager autoPostManager;

        public AutoPostManagerTests()
        {
            logger = Substitute.For<ILogger>();
            autoPostRepository = Substitute.For<IAutoPostRepository>();
            categoryRepository = Substitute.For<ICategoryRepository>();
            autoPostFileManager = Substitute.For<IAutoPostFileManager>();
            iGAccountRepository = Substitute.For<IIGAccountRepository>();
            autoPostManager = new AutoPostManager(logger, autoPostRepository, categoryRepository, autoPostFileManager, iGAccountRepository);
        }

        [Fact]
        public void Create_WhenAccountIsNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var command = new CreateAutoPostCommand { UserToken = "token", AccountId = 1, Files = new List<CreateAutoPostFileCommand> { } };
            iGAccountRepository.Get(command.UserToken, command.AccountId).ReturnsNull();

            // Act & Assert
            Assert.Throws<NotFoundException>(() => autoPostManager.Create(command));
        }

        [Fact]
        public void Create_WhenAccountIsFound_CreatesAutoPost()
        {
            // Arrange
            var command = new CreateAutoPostCommand { UserToken = "token", AccountId = 1, Files = new List<CreateAutoPostFileCommand>(), ExecuteAt = DateTime.UtcNow.AddDays(1), TimeZone = 3, AutoDelete = false };
            var account = new IGAccount { Id = 1 };
            iGAccountRepository.Get(command.UserToken, command.AccountId).Returns(account);
            var postFile = new AutoPostFile { Path = "file.jpg", MediaId = "", post = new AutoPost(), VideoThumbnail = "" };
            var postFiles = new List<AutoPostFile> { postFile };
            autoPostFileManager.Create(command.Files, postFile.post, 1).Returns(postFiles);

            // Act
            autoPostManager.Create(command);

            // Assert
            autoPostRepository.Received().Add(Arg.Any<AutoPost>());
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Був створений новий автопост")));
        }
        [Fact]
        public void Get_ReturnsAutoPosts()
        {
            // Arrange
            var command = new GetAutoPostsCommand { AccountId = 1, UserToken = "token" };
            var autoPosts = new List<AutoPost> { new AutoPost { Id = 1 } };
            autoPostRepository.GetBy(command).Returns(autoPosts);

            // Act
            var result = autoPostManager.Get(command);

            // Assert
            Assert.Equal(autoPosts, result);
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Отримано список авто-постів"))); 
        }
        [Fact]
        public void Update_WhenPostIsNotFound_ThrowsNotFoundException()
        { 
            // Arrange
            var command = new UpdateAutoPostCommand { UserToken = "token", PostId = 1 }; 
            autoPostRepository.GetBy(command.UserToken, command.PostId).ReturnsNull(); 
            
            // Act & Assert
            Assert.Throws<NotFoundException>(() => autoPostManager.Update(command)); 
        }
        [Fact]
        public void Update_WhenPostIsFound_UpdatesAutoPost()
        {
            // Arrange
            var command = new UpdateAutoPostCommand
            {
                UserToken = "token",
                PostId = 1,
                ExecuteAt = DateTime.UtcNow,
                TimeZone = 3,
                Location = "New Location",
                Description = "New Description",
                Comment = "New Comment"
            };
            var post = new AutoPost { Id = 1, TimeZone = 0 };
            autoPostRepository.GetBy(command.UserToken, command.PostId).Returns(post);

            // Act
            autoPostManager.Update(command);

            // Assert
            Assert.Equal(command.ExecuteAt.AddHours(-command.TimeZone), post.ExecuteAt);
            Assert.Equal(command.TimeZone, post.TimeZone);
            Assert.Equal(command.Location, post.Location);
            Assert.Equal(command.Description, post.Description);
            Assert.Equal(command.Comment, post.Comment);
            autoPostRepository.Received().Update(post);
        }
        [Fact]
        public void Delete_WhenPostIsNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var command = new DeleteAutoPostCommand { UserToken = "token", AutoPostId = 1 };
            autoPostRepository.GetBy(command.UserToken, command.AutoPostId).ReturnsNull();

            // Act & Assert
            Assert.Throws<NotFoundException>(() => autoPostManager.Delete(command));
        }

        [Fact]
        public void Delete_WhenPostIsFound_DeletesAutoPost()
        {
            // Arrange
            var command = new DeleteAutoPostCommand { UserToken = "token", AutoPostId = 1 };
            var post = new AutoPost { Id = 1, Deleted = false };
            autoPostRepository.GetBy(command.UserToken, command.AutoPostId).Returns(post);

            // Act
            autoPostManager.Delete(command);

            // Assert
            Assert.True(post.Deleted);
            autoPostRepository.Received().Update(post);
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Авто пост був видалений")));
        }
    }
}