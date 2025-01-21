using NSubstitute;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;
using NSubstitute.ReturnsExtensions;

namespace UseCases.Tests
{
    public class CreateIGAccountManagerTests
    {
        private readonly ILogger _logger;
        private readonly IIGAccountRepository _accountRepository;
        private readonly IChallengeRequiredAccount _challengeRequiredAccount;
        private readonly ILoginSessionManager _loginSessionManager;
        private readonly IRecoverySessionManager _recoverySessionManager;
        private readonly ISaveSessionManager _saveSessionManager;
        private readonly CreateIGAccountManager _createIGAccountManager;

        public CreateIGAccountManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _accountRepository = Substitute.For<IIGAccountRepository>();
            _challengeRequiredAccount = Substitute.For<IChallengeRequiredAccount>();
            _loginSessionManager = Substitute.For<ILoginSessionManager>();
            _recoverySessionManager = Substitute.For<IRecoverySessionManager>();
            _saveSessionManager = Substitute.For<ISaveSessionManager>();

            _createIGAccountManager = new CreateIGAccountManager(
                _logger,
                _accountRepository,
                _challengeRequiredAccount,
                _loginSessionManager,
                _recoverySessionManager,
                _saveSessionManager);
        }

        [Fact]
        public void Create_AccountExists_ReturnsRecoveredAccount()
        {
            // Arrange
            var command = new CreateIgAccountCommand
            {
                UserToken = "userToken",
                InstagramUserName = "existingUser",
                InstagramPassword = "password"
            };
            var existingAccount = new IGAccount();
            _accountRepository.GetByWithState(command.UserToken, command.InstagramUserName).Returns(existingAccount);
            _recoverySessionManager.Do(existingAccount, command).Returns(existingAccount);

            // Act
            var result = _createIGAccountManager.Create(command);

            // Assert
            Assert.Equal(existingAccount, result);
        }

        [Fact]
        public void Create_NewAccount_CallsLoginSessionManager()
        {
            // Arrange
            var command = new CreateIgAccountCommand
            {
                UserToken = "userToken",
                InstagramUserName = "newUser",
                InstagramPassword = "password"
            };
            var newAccount = new IGAccount();
            var state = new SessionState 
            { 
                Account = newAccount, 
                SessionSave = "", 
                TimeAction = new TimeAction { Account = newAccount }
            };
            newAccount.State = state;
            _accountRepository.GetByWithState(command.UserToken, command.InstagramUserName).Returns((IGAccount)null);
            _loginSessionManager.Do(command).ReturnsForAnyArgs(newAccount);

            // Act
            var result = _createIGAccountManager.Create(command);

            // Assert
            _loginSessionManager.Received(1).Do(command);
        }

        [Fact]
        public void Create_NewAccountWithoutChallenge_CallsSaveSessionManager()
        {
            // Arrange
            var command = new CreateIgAccountCommand
            {
                UserToken = "userToken",
                InstagramUserName = "newUser",
                InstagramPassword = "password"
            };
            var newAccount = new IGAccount();
            _accountRepository.GetByWithState(command.UserToken, command.InstagramUserName).Returns((IGAccount)null);
            _loginSessionManager.Do(command).Returns(newAccount);
            newAccount.State = new SessionState 
            { 
                Challenger = false, 
                Account = newAccount, 
                SessionSave = "", 
                TimeAction = new TimeAction { Account = newAccount } 
            };

            // Act
            var result = _createIGAccountManager.Create(command);

            // Assert
            _saveSessionManager.Received(1).Do(newAccount.UserId, newAccount.Username, false);
        }

        [Fact]
        public void Create_NewAccountWithChallenge_CallsChallengeRequiredAccountAndSaveSessionManager()
        {
            // Arrange
            var command = new CreateIgAccountCommand
            {
                UserToken = "userToken",
                InstagramUserName = "newUser",
                InstagramPassword = "password"
            };
            var newAccount = new IGAccount();
            _accountRepository.GetByWithState(command.UserToken, command.InstagramUserName).ReturnsNull();
            _loginSessionManager.Do(command).Returns(newAccount);
            newAccount.State = new SessionState
            {
                Challenger = true,
                Account = newAccount,
                SessionSave = "",
                TimeAction = new TimeAction { Account = newAccount }
            };

            // Act
            var result = _createIGAccountManager.Create(command);

            // Assert
            _challengeRequiredAccount.Received(1).Do(newAccount, false);
            _saveSessionManager.Received(1).Do(newAccount.UserId, newAccount.Username, true);
        }
    }
}
