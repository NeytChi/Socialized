using Xunit;
using NSubstitute;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.Exceptions;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts;

namespace UseCases.InstagramAccounts.Tests
{
    public class LoginSessionManagerTests
    {
        private LoginSessionManager _manager;
        private ILogger _logger;
        private ILoginApi _api;

        public LoginSessionManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _api = Substitute.For<ILoginApi>();

            _manager = new LoginSessionManager(_logger, _api);
        }

        [Fact]
        public void Do_ShouldReturnSuccess_WhenLoginStateIsSuccess()
        {
            // Arrange
            var account = new IGAccount();
            _api.Do(account).Returns(InstagramLoginState.Success);

            // Act
            var result = _manager.Do(account);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(InstagramLoginState.Success, result.State);
        }

        [Fact]
        public void Do_ShouldReturnChallengeRequired_WhenLoginStateIsChallengeRequired()
        {
            // Arrange
            var account = new IGAccount();
            _api.Do(account).Returns(InstagramLoginState.ChallengeRequired);

            // Act
            var result = _manager.Do(account);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(InstagramLoginState.ChallengeRequired, result.State);
        }

        [Theory]
        [InlineData(InstagramLoginState.TwoFactorRequired, "Сесія Instagram аккаунту потребує проходження двох-факторної організації.")]
        [InlineData(InstagramLoginState.InactiveUser, "Сесія Instagram аккаунту не активна.")]
        [InlineData(InstagramLoginState.InvalidUser, "Правильно введені данні для входу в аккаунт.")]
        [InlineData(InstagramLoginState.BadPassword, "Неправильний пароль.")]
        [InlineData(InstagramLoginState.LimitError, "Невідома помилка при спробі зайти(логін) в Instagram аккаунт. Виключення:LimitError.")]
        [InlineData(InstagramLoginState.Exception, "Невідома помилка при спробі зайти(логін) в Instagram аккаунт. Виключення:Exception.")]
        public void Do_ShouldThrowIgAccountException_WhenLoginFails(InstagramLoginState state, string expectedMessage)
        {
            // Arrange
            var account = new IGAccount();
            _api.Do(account).Returns(state);

            // Act & Assert
            var exception = Assert.Throws<IgAccountException>(() => _manager.Do(account));
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
