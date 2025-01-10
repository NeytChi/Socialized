using NSubstitute;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts.Commands;
using Core;

namespace UseCases.InstagramAccounts.Tests
{
    public class RecoverySessionManagerTests
    {
        private RecoverySessionManager _manager;
        private ILogger _logger;
        private IIGAccountRepository _accountRepository;
        private IRestoreInstagramSessionManager _restoreInstagramSessionManager;
        private ILoginSessionManager _loginSessionManager;
        private IGetStateData _getStateData;
        private IChallengeRequiredAccount _challengeRequiredAccount;
        private ProfileCondition _profileCondition;

        public RecoverySessionManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _accountRepository = Substitute.For<IIGAccountRepository>();
            _restoreInstagramSessionManager = Substitute.For<IRestoreInstagramSessionManager>();
            _loginSessionManager = Substitute.For<ILoginSessionManager>();
            _getStateData = Substitute.For<IGetStateData>();
            _challengeRequiredAccount = Substitute.For<IChallengeRequiredAccount>();
            _profileCondition = new ProfileCondition();

            _manager = new RecoverySessionManager(_logger, _accountRepository, _restoreInstagramSessionManager,
                                                  _loginSessionManager, _profileCondition, _challengeRequiredAccount, _getStateData);
        }

        [Fact]
        public void Do_ShouldRestoreAndSaveSession_WhenLoginSuccessful()
        {
            // Arrange
            var account = new IGAccount { Id = 123, State = new SessionState { Challenger = false, Relogin = true } };
            var requirements = new IgAccountRequirements();

            _restoreInstagramSessionManager.Do(account).Returns(account);
            _loginSessionManager.Do(account, requirements).Returns(account);
            _getStateData.AsString(account).Returns("stateData");

            // Act
            var result = _manager.Do(account, requirements);

            // Assert
            Assert.True(result.State.Usable);
            Assert.False(result.State.Relogin);
            Assert.False(result.State.Challenger);
            Assert.Equal(_profileCondition.Encrypt("stateData"), result.State.SessionSave);
            _accountRepository.Received().Update(account);
            _logger.Received().Information($"Сесія була востановлена, id={account.Id}");
        }

        [Fact]
        public void Do_ShouldHandleChallengeRequired_WhenAccountInChallengeState()
        {
            // Arrange
            var account = new IGAccount { Id = 123, State = new SessionState { Challenger = true, Relogin = true } };
            var requirements = new IgAccountRequirements();

            _restoreInstagramSessionManager.Do(account).Returns(account);
            _loginSessionManager.Do(account, requirements).Returns(account);
            _getStateData.AsString(account).Returns("stateData");

            // Act
            var result = _manager.Do(account, requirements);

            // Assert
            Assert.False(result.State.Usable);
            Assert.False(result.State.Relogin);
            Assert.True(result.State.Challenger);
            Assert.Equal(_profileCondition.Encrypt("stateData"), result.State.SessionSave);
            _challengeRequiredAccount.Received().Do(account, true);
            _accountRepository.Received().Update(account);
            _logger.Received().Information($"Сесія була востановлена, id={account.Id}");
        }
    }
}

