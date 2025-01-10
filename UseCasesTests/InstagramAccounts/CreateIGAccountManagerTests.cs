using NSubstitute;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;
using UseCases.InstagramApi;

namespace UseCases.InstagramAccounts.Tests
{
    public class CreateIGAccountManagerTests
    {
        private CreateIGAccountManager _manager;
        private ILogger _logger;
        private IIGAccountRepository _accountRepository;
        private IChallengeRequiredAccount _challengeRequiredAccount;
        private ILoginSessionManager _loginSessionManager;
        private IRestoreInstagramSessionManager _restoreInstagramSessionManager;
        private IRecoverySessionManager _recoverySessionManager;
        private ILoginApi _loginApi;
        private ISaveSessionManager _saveSessionManager;

        public CreateIGAccountManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _accountRepository = Substitute.For<IIGAccountRepository>();
            _challengeRequiredAccount = Substitute.For<IChallengeRequiredAccount>();
            _loginSessionManager = Substitute.For<ILoginSessionManager>();
            _restoreInstagramSessionManager = Substitute.For<IRestoreInstagramSessionManager>();
            _recoverySessionManager = Substitute.For<IRecoverySessionManager>();
            _loginApi = Substitute.For<ILoginApi>();
            _saveSessionManager = Substitute.For<ISaveSessionManager>();

            _manager = new CreateIGAccountManager(_logger, _accountRepository, _loginApi,
                _challengeRequiredAccount, _loginSessionManager, _restoreInstagramSessionManager,
                _recoverySessionManager, _saveSessionManager);
        }

        [Fact]
        public void Create_ShouldReturnRecoveredAccount_WhenAccountExists()
        {
            // Arrange
            var command = new CreateIgAccountCommand { UserToken = "user123", InstagramUserName = "existinguser" };
            var existingAccount = new IGAccount { UserId = 1, Username = "existinguser", State = new SessionState { Challenger = false } };

            _accountRepository.GetByWithState(command.UserToken, command.InstagramUserName).Returns(existingAccount);
            _recoverySessionManager.Do(existingAccount, command).Returns(existingAccount);

            // Act
            var result = _manager.Create(command);

            // Assert
            Assert.Equal(existingAccount, result);
        }

        [Fact]
        public void Create_ShouldCallChallengeRequiredAccount_WhenAccountInChallengeState()
        {
            // Arrange
            var command = new CreateIgAccountCommand { UserToken = "user123", InstagramUserName = "newuser" };
            var newAccount = new IGAccount { UserId = 1, Username = "newuser", State = new SessionState { Challenger = true } };

            _accountRepository.GetByWithState(command.UserToken, command.InstagramUserName).Returns((IGAccount)null);
            _loginSessionManager.Do(null, command).Returns(newAccount);

            // Act
            var result = _manager.Create(command);

            // Assert
            _challengeRequiredAccount.Received().Do(newAccount, false);
        }

        [Fact]
        public void Create_ShouldSaveSession_WhenAccountIsNotInChallengeState()
        {
            // Arrange
            var command = new CreateIgAccountCommand { UserToken = "user123", InstagramUserName = "newuser" };
            var newAccount = new IGAccount { UserId = 1, Username = "newuser", State = new SessionState { Challenger = false } };

            _accountRepository.GetByWithState(command.UserToken, command.InstagramUserName).Returns((IGAccount)null);
            _loginSessionManager.Do(null, command).Returns(newAccount);

            // Act
            var result = _manager.Create(command);

            // Assert
            _saveSessionManager.Received().Do(newAccount.UserId, newAccount.Username, false);
        }
    }
}
