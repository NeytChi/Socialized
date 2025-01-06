using Domain.Admins;
using Domain.Appeals;
using Domain.AutoPosting;
using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals;
using UseCases.Appeals.Commands;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals
{
    public class AppealManagerTests
    {
        [Fact]
        public void Create_WhenTokenAndIdIsValid_ReturnMessage()
        {
            var logger = Substitute.For<ILogger>();
            var appealRepository = Substitute.For<IAppealRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var appealMessageRepository = Substitute.For<IAppealMessageRepository>();
            var categoryRepository = Substitute.For<ICategoryRepository>();
            var command = new CreateAppealCommand { Subject = "Test", UserToken = "1234567890" };
            userRepository.GetByUserTokenNotDeleted(command.UserToken).Returns(new User { Id = 1 });
            var manager = new AppealManager
            (
                logger, 
                appealRepository, 
                userRepository, 
                appealMessageRepository, 
                categoryRepository
            );
            var result = manager.Create(command);

            Assert.Equal(result.Subject, command.Subject);
            Assert.Equal(1, result.State);
        }
        [Fact]
        public void Create_WhenTokenAndIdIsNotValid_ThrowException()
        {
            var logger = Substitute.For<ILogger>();
            var appealRepository = Substitute.For<IAppealRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var appealMessageRepository = Substitute.For<IAppealMessageRepository>();
            var categoryRepository = Substitute.For<ICategoryRepository>();
            var command = new CreateAppealCommand { Subject = "Test", UserToken = "1234567890" };
            userRepository.GetByUserTokenNotDeleted(command.UserToken).ReturnsNull();
            var manager = new AppealManager
            (
                logger,
                appealRepository,
                userRepository,
                appealMessageRepository,
                categoryRepository
            );

            Assert.Throws<NotFoundException>(() => manager.Create(command));
        }
        [Fact]
        public void UpdateAppealToClosed_WhenIdIsValid_Return()
        {
            var appealId = 1;
            var logger = Substitute.For<ILogger>();
            var appealRepository = Substitute.For<IAppealRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var appealMessageRepository = Substitute.For<IAppealMessageRepository>();
            var categoryRepository = Substitute.For<ICategoryRepository>();
            appealRepository.GetBy(appealId).Returns(new Appeal { Id = appealId });
            var manager = new AppealManager
            (
                logger,
                appealRepository,
                userRepository,
                appealMessageRepository,
                categoryRepository
            );

            manager.UpdateAppealToClosed(appealId);
        }
        [Fact]
        public void UpdateAppealToClosed_WhenIdIsNotValid_ThrowException()
        {
            var appealId = 1;
            var logger = Substitute.For<ILogger>();
            var appealRepository = Substitute.For<IAppealRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var appealMessageRepository = Substitute.For<IAppealMessageRepository>();
            var categoryRepository = Substitute.For<ICategoryRepository>();
            appealRepository.GetBy(appealId).ReturnsNull();
            var manager = new AppealManager
            (
                logger,
                appealRepository,
                userRepository,
                appealMessageRepository,
                categoryRepository
            );

            Assert.Throws<NotFoundException>(() => manager.UpdateAppealToClosed(appealId));
        }
        [Fact]
        public void UpdateAppealToAnswered_WhenIdIsValid_Return()
        {
            var appealId = 1;
            var logger = Substitute.For<ILogger>();
            var appealRepository = Substitute.For<IAppealRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var appealMessageRepository = Substitute.For<IAppealMessageRepository>();
            var categoryRepository = Substitute.For<ICategoryRepository>();
            appealRepository.GetBy(appealId).Returns(new Appeal { Id = appealId });
            var manager = new AppealManager
            (
                logger,
                appealRepository,
                userRepository,
                appealMessageRepository,
                categoryRepository
            );

            manager.UpdateAppealToAnswered(appealId);
        }
        [Fact]
        public void UpdateAppealToAnswered_WhenIdIsNotValid_ThrowException()
        {
            var appealId = 1;
            var logger = Substitute.For<ILogger>();
            var appealRepository = Substitute.For<IAppealRepository>();
            var userRepository = Substitute.For<IUserRepository>();
            var appealMessageRepository = Substitute.For<IAppealMessageRepository>();
            var categoryRepository = Substitute.For<ICategoryRepository>();
            appealRepository.GetBy(appealId).ReturnsNull();
            var manager = new AppealManager
            (
                logger,
                appealRepository,
                userRepository,
                appealMessageRepository,
                categoryRepository
            );

            Assert.Throws<NotFoundException>(() => manager.UpdateAppealToAnswered(appealId));
        }
        [Fact]
        public void UpdateAppealToAnswered_WhenStateIsNotRight_ThrowException()
        {
            var appealId = 1;
            var logger = Substitute.For<ILogger>();
            var userRepository = Substitute.For<IUserRepository>();
            var appealMessageRepository = Substitute.For<IAppealMessageRepository>();
            var categoryRepository = Substitute.For<ICategoryRepository>();
            var appealRepository = Substitute.For<IAppealRepository>();
            appealRepository.GetBy(appealId).Returns(new Appeal { State = 1 });
            var firstManager = new AppealManager
            (
                logger,
                appealRepository,
                userRepository,
                appealMessageRepository,
                categoryRepository
            );
            var secondAppealRepository = Substitute.For<IAppealRepository>();
            secondAppealRepository.GetBy(appealId).Returns(new Appeal { State = 2 });
            var secondManager = new AppealManager
            (
                logger,
                secondAppealRepository,
                userRepository,
                appealMessageRepository,
                categoryRepository
            );

            firstManager.UpdateAppealToAnswered(appealId);
            secondManager.UpdateAppealToAnswered(appealId);
        }
    }
}
