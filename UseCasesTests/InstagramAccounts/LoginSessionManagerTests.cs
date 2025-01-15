using NSubstitute;
using Serilog;
using Domain.InstagramAccounts;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts;
using UseCases.InstagramAccounts.Commands;
using UseCases.Exceptions;

namespace UseCases.Tests
{
    public class LoginSessionManagerTests
    {
        private readonly ILogger _logger;
        private readonly ILoginApi _api;
        private readonly LoginSessionManager _loginSessionManager;

        public LoginSessionManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _api = Substitute.For<ILoginApi>();
            _loginSessionManager = new LoginSessionManager(_logger, _api);
        }

        [Theory]
        [InlineData(InstagramLoginState.Success)]
        [InlineData(InstagramLoginState.ChallengeRequired)]
        public void Do_Success_ReturnsIGAccount(InstagramLoginState state)
        {
            // Arrange
            var accountRequirements = new IgAccountRequirements { InstagramUserName = "username", InstagramPassword = "password" };
            var account = new IGAccount();
            _api.Do(ref account, accountRequirements).Returns(state);

            // Act
            var result = _loginSessionManager.Do(accountRequirements);

            // Assert
            Assert.NotNull(result);
        }

        [InlineData(InstagramLoginState.TwoFactorRequired, "Сесія Instagram аккаунту потребує проходження двох-факторної верифікації.")]
        [InlineData(InstagramLoginState.InactiveUser, "Сесія Instagram аккаунту є не активною.")]
        [InlineData(InstagramLoginState.InvalidUser, "Неправильний логін аккаунту.")]
        [InlineData(InstagramLoginState.BadPassword, "Неправильний пароль.")]
        [InlineData(InstagramLoginState.LimitError, "Невідома помилка при спробі увійти в Instagram аккаунт.")]
        [InlineData(InstagramLoginState.Exception, "Невідома помилка при спробі увійти в Instagram аккаунт.")]
        public void Do_ThrowsIgAccountException(InstagramLoginState state, string expectedMessage)
        {
            // Arrange
            var accountRequirements = new IgAccountRequirements { InstagramUserName = "username", InstagramPassword = "password" };
            var account = new IGAccount();
            _api.Do(ref account, accountRequirements).ReturnsForAnyArgs(state);

            // Act & Assert
            Assert.Throws<IgAccountException>(() => _loginSessionManager.Do(accountRequirements));
        }
    }
}
