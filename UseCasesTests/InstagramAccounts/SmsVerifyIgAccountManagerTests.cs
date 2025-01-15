using Xunit;
using NSubstitute;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.Exceptions;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;
using Core;

namespace UseCases.InstagramAccounts.Tests
{
    public class SmsVerifyIgAccountManagerTests
    {
        private SmsVerifyIgAccountManager _manager;
        private ILogger _logger;
        private IGetStateData _getStateData;
        private IIGAccountRepository _accountRepository;
        private IVerifyCodeForChallengeRequire _verifyCodeForChallengeRequire;
        private ProfileCondition _profileCondition;

        public SmsVerifyIgAccountManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _getStateData = Substitute.For<IGetStateData>();
            _accountRepository = Substitute.For<IIGAccountRepository>();
            _verifyCodeForChallengeRequire = Substitute.For<IVerifyCodeForChallengeRequire>();
            _profileCondition = new ProfileCondition();

            _manager = new SmsVerifyIgAccountManager(_logger, _getStateData, _accountRepository,
                                                     _profileCondition, _verifyCodeForChallengeRequire);
        }

        [Fact]
        public void SmsVerifySession_ShouldThrowNotFoundException_WhenAccountDoesNotExist()
        {
            // Arrange
            var command = new SmsVefiryIgAccountCommand { UserToken = "user123", AccountId = 123 };

            _accountRepository.Get(command.UserToken, command.AccountId).Returns((IGAccount)null);

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => _manager.SmsVerifySession(command));
            Assert.Equal("Сервер не визначив запис Instagram аккаунту по id.", exception.Message);
        }

        [Fact]
        public void SmsVerifySession_ShouldThrowNotFoundException_WhenAccountNotInChallengeState()
        {
            // Arrange
            var command = new SmsVefiryIgAccountCommand { UserToken = "user123", AccountId = 123 };
            var account = new IGAccount { State = new SessionState { Challenger = false } };

            _accountRepository.Get(command.UserToken, command.AccountId).Returns(account);

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => _manager.SmsVerifySession(command));
            Assert.Equal("Сесія Instagram аккаунту не потребує підтвердження аккаунту.", exception.Message);
        }

        [Fact]
        public void SmsVerifySession_ShouldThrowIgAccountException_WhenVerificationFails()
        {
            // Arrange
            var command = new SmsVefiryIgAccountCommand { UserToken = "user123", AccountId = 123, VerifyCode = 12345 };
            var account = new IGAccount { State = new SessionState { Challenger = true } };

            _accountRepository.Get(command.UserToken, command.AccountId).Returns(account);
            _verifyCodeForChallengeRequire.Do(command.VerifyCode.ToString(), account).Returns(new InstagramLoginResult { State = InstagramLoginState.Exception });

            // Act & Assert
            var exception = Assert.Throws<IgAccountException>(() => _manager.SmsVerifySession(command));
            Assert.Equal("Код підвердження Instagram аккаунту не вірний.", exception.Message);
        }

        [Fact]
        public void SmsVerifySession_ShouldVerifyAndSaveSession_WhenVerificationSuccessful()
        {
            // Arrange
            var command = new SmsVefiryIgAccountCommand { UserToken = "user123", AccountId = 123, VerifyCode = 12345 };
            var account = new IGAccount { Id = 123, State = new SessionState { Challenger = true } };

            _accountRepository.Get(command.UserToken, command.AccountId).Returns(account);
            _verifyCodeForChallengeRequire.Do(command.VerifyCode.ToString(), account).Returns(new InstagramLoginResult { State = InstagramLoginState.Success });
            _getStateData.AsString(account).Returns("sessionData");

            // Act
            _manager.SmsVerifySession(command);

            // Assert
            Assert.True(account.State.Usable);
            Assert.False(account.State.Relogin);
            Assert.False(account.State.Challenger);
            Assert.Equal(_profileCondition.Encrypt("sessionData"), account.State.SessionSave);
            _accountRepository.Received().Update(account);
            _logger.Received().Information($"Сесія Instagram аккаунту було веріфікована, id={account.Id}.");
        }
    }
}

