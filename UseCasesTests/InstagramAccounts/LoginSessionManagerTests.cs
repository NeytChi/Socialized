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
        [Theory]
        [InlineData(InstagramLoginState.TwoFactorRequired)]
        [InlineData(InstagramLoginState.InactiveUser)]
        [InlineData(InstagramLoginState.InvalidUser)]
        [InlineData(InstagramLoginState.BadPassword)]
        [InlineData(InstagramLoginState.LimitError)]
        [InlineData(InstagramLoginState.Exception)]
        public void Do_ThrowsIgAccountException(InstagramLoginState state)
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
