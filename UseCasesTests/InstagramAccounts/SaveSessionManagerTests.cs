using NSubstitute;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramApi;
using Core;

namespace UseCases.InstagramAccounts.Tests
{
    public class SaveSessionManagerTests
    {
        private SaveSessionManager _manager;
        private ILogger _logger;
        private IIGAccountRepository _accountRepository;
        private IGetStateData _api;
        private ProfileCondition _profileCondition;

        public SaveSessionManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _accountRepository = Substitute.For<IIGAccountRepository>();
            _api = Substitute.For<IGetStateData>();
            _profileCondition = new ProfileCondition();

            _manager = new SaveSessionManager(_logger, _accountRepository, _api, _profileCondition);
        }

        [Fact]
        public void Do_ShouldCreateAndSaveSession_WhenCalled()
        {
            // Arrange
            long userId = 12345;
            string userName = "testuser";
            bool challengeRequired = false;

            _accountRepository.When(x => x.Create(Arg.Any<IGAccount>())).Do(x =>
            {
                var account = x.Arg<IGAccount>();
                account.Id = 1; // Setting the Id after creation
            });

            // Act
            var result = _manager.Do(userId, userName, challengeRequired);

            // Assert
            Assert.Equal(userId, result.UserId);
            Assert.Equal(userName, result.Username);
            Assert.True(result.State.Usable);
            Assert.False(result.State.Challenger);
            Assert.NotNull(result.State.SessionSave);
            _accountRepository.Received().Create(result);
            _accountRepository.Received().Update(result);
            _logger.Received().Information($"Сесія Instagram аккаунту було збережено, id={result.Id}.");
        }

        [Fact]
        public void Do_ShouldHandleChallengeRequired_WhenCalled()
        {
            // Arrange
            long userId = 12345;
            string userName = "testuser";
            bool challengeRequired = true;

            _accountRepository.When(x => x.Create(Arg.Any<IGAccount>())).Do(x =>
            {
                var account = x.Arg<IGAccount>();
                account.Id = 1; // Setting the Id after creation
            });

            // Act
            var result = _manager.Do(userId, userName, challengeRequired);

            // Assert
            Assert.Equal(userId, result.UserId);
            Assert.Equal(userName, result.Username);
            Assert.False(result.State.Usable);
            Assert.True(result.State.Challenger);
            Assert.NotNull(result.State.SessionSave);
            _accountRepository.Received().Create(result);
            _accountRepository.Received().Update(result);
            _logger.Received().Information($"Сесія Instagram аккаунту було збережено, id={result.Id}.");
        }
    }
}
