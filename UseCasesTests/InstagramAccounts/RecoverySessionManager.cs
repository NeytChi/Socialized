using Core;
using Serilog;
using NSubstitute;
using Domain.InstagramAccounts;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;
using NSubstitute.ReturnsExtensions;

namespace UseCases.Tests
{
    public class RecoverySessionManagerTests
    {
        private readonly ILogger _logger;
        private readonly IIGAccountRepository _accountRepository;
        private readonly ILoginSessionManager _loginSessionManager;
        private readonly IGetStateData _getStateData;
        private readonly IChallengeRequiredAccount _challengeRequiredAccount;
        private readonly ProfileCondition _profileCondition;
        private readonly RecoverySessionManager _recoverySessionManager;

        public RecoverySessionManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _accountRepository = Substitute.For<IIGAccountRepository>();
            _loginSessionManager = Substitute.For<ILoginSessionManager>();
            _getStateData = Substitute.For<IGetStateData>();
            _challengeRequiredAccount = Substitute.For<IChallengeRequiredAccount>();
            _profileCondition = new ProfileCondition();

            _recoverySessionManager = new RecoverySessionManager(
                _logger,
                _accountRepository,
                _loginSessionManager,
                _profileCondition,
                _challengeRequiredAccount,
                _getStateData);
        }

        [Fact]
        public void Do_ReloginAndSuccess_SaveSessionState()
        {
            // Arrange
            var account = new IGAccount { Id = 1 };
            account.State = new SessionState
            {
                Challenger = false,
                Account = account,
                SessionSave = "",
                TimeAction = new TimeAction { Account = account }
            };
            var requirements = new IgAccountRequirements { InstagramUserName = "username", InstagramPassword = "password" }; 
            var sessionState = "sessionState";

            _loginSessionManager.Do(account, requirements).Returns(account);
            _getStateData.AsString(account).Returns(sessionState);

            // Act
            var result = _recoverySessionManager.Do(account, requirements);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.State.Usable);
            Assert.False(result.State.Relogin);
            Assert.False(result.State.Challenger);
            _accountRepository.Received(1).Update(account);
            _logger.Received(1).Information($"Сесія була востановлена, id={result.Id}");
        }

        [Fact]
        public void Do_ChallengerAccount_SaveSessionStateNotUsable()
        {
            // Arrange
            var account = new IGAccount { Id = 1 };
            account.State = new SessionState
            {
                Challenger = true,
                Account = account,
                SessionSave = "",
                TimeAction = new TimeAction { Account = account }
            };

            var requirements = new IgAccountRequirements { InstagramUserName = "username", InstagramPassword = "password" }; var sessionState = "sessionState";

            _loginSessionManager.Do(account, requirements).Returns(account);
            _getStateData.AsString(account).Returns(sessionState);

            // Act
            var result = _recoverySessionManager.Do(account, requirements);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.State.Usable);
            Assert.False(result.State.Relogin);
            Assert.True(result.State.Challenger);
            _accountRepository.Received(1).Update(account);
            _challengeRequiredAccount.Received(1).Do(account, true);
            _logger.Received(1).Information($"Сесія потребує відновлення, id={result.Id}");
        }

        [Fact]
        public void Do_LoginFail_ThrowsException()
        {
            // Arrange
            var account = new IGAccount { Id = 1 };
            account.State = new SessionState
            {
                Challenger = false,
                Account = account,
                SessionSave = "",
                TimeAction = new TimeAction { Account = account }
            };
            var requirements = new IgAccountRequirements { InstagramUserName = "username", InstagramPassword = "password" };

            _loginSessionManager.Do(account, requirements).ReturnsNull();

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _recoverySessionManager.Do(account, requirements));
            Assert.Equal("Операція логіну аккаунту була провалена.", exception.Message);
        }
    }
}
