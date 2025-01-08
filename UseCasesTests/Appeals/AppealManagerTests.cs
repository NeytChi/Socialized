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
        ILogger logger = Substitute.For<ILogger>();
        IAppealRepository appealRepository = Substitute.For<IAppealRepository>();
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        IAppealMessageRepository appealMessageRepository = Substitute.For<IAppealMessageRepository>();
        ICategoryRepository categoryRepository = Substitute.For<ICategoryRepository>();

        [Fact]
        public void Create_WhenUserTokenAndIdIsValid_ReturnMessage()
        {
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
        public void Create_WhenUserTokenAndIdIsNotValid_ThrowNotFoundException()
        {
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
        public void UpdateAppealToClosed_WhenAppealIdIsValid_Return()
        {
            var appealId = 1;
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
        public void UpdateAppealToClosed_WhenAppealIdIsNotValid_ThrowNotFoundException()
        {
            var appealId = 1;
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
        public void UpdateAppealToAnswered_WhenAppealIdIsValid_Return()
        {
            var appealId = 1;
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
        public void UpdateAppealToAnswered_WhenAppealIdIsNotValid_ThrowNotFoundException()
        {
            var appealId = 1;
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
        public void UpdateAppealToAnswered_WhenStateIsNotRight_Return()
        {
            var appealId = 1;
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
