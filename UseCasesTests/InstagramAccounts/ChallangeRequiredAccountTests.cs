using Domain.InstagramAccounts;
using NSubstitute;
using Serilog;
using UseCases.Exceptions;
using UseCases.InstagramApi;
using UseCases.InstagramAccounts;
using Xunit;

public class ChallengeRequiredAccountTests
{
    private readonly IGetChallengeRequireVerifyMethod _mockGetChallengeRequireVerifyMethod;
    private readonly IVerifyCodeForChallengeRequire _mockVerifyCodeForChallengeRequire;
    private readonly ILogger _mockLogger;
    private readonly ChallengeRequiredAccount _challengeRequiredAccount;
    private readonly IGAccount _account;

    public ChallengeRequiredAccountTests()
    {
        _mockGetChallengeRequireVerifyMethod = Substitute.For<IGetChallengeRequireVerifyMethod>();
        _mockVerifyCodeForChallengeRequire = Substitute.For<IVerifyCodeForChallengeRequire>();
        _mockLogger = Substitute.For<ILogger>();
        _challengeRequiredAccount = new ChallengeRequiredAccount(_mockGetChallengeRequireVerifyMethod, _mockVerifyCodeForChallengeRequire, _mockLogger);
        _account = new IGAccount();
    }

    [Fact]
    public void Do_ChallengeFails_ThrowsIgAccountException()
    {
        // Arrange
        _mockGetChallengeRequireVerifyMethod.Do(_account).Returns(new InstagramLoginResult { Success = false });

        // Act & Assert
        Assert.Throws<IgAccountException>(() => _challengeRequiredAccount.Do(_account, false));
    }

    [Fact]
    public void Do_VerifyCodeToSMSFails_ThrowsIgAccountException()
    {
        // Arrange
        _mockGetChallengeRequireVerifyMethod.Do(_account).Returns(new InstagramLoginResult { Success = true });
        _mockVerifyCodeForChallengeRequire.Do(Arg.Any<bool>(), _account).Returns(new InstagramLoginResult { Success = false });

        // Act & Assert
        Assert.Throws<IgAccountException>(() => _challengeRequiredAccount.Do(_account, false));
    }

    [Fact]
    public void Do_AllSucceed_LogsInformation()
    {
        // Arrange
        _mockGetChallengeRequireVerifyMethod.Do(_account).Returns(new InstagramLoginResult { Success = true });
        _mockVerifyCodeForChallengeRequire.Do(Arg.Any<bool>(), _account).Returns(new InstagramLoginResult { Success = true });

        // Act
        _challengeRequiredAccount.Do(_account, false);

        // Assert
        _mockLogger.Received(1).Information("Сесія Instagram аккаунту була пройдена через процедуру підтвердження.");
    }
}
