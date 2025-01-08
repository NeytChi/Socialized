using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Exceptions;
using UseCases.Users;
using UseCases.Users.Commands;

namespace UseCasesTests.Users
{
    public class UserPasswordRecoveryManagerTests
    {
        ILogger logger = Substitute.For<ILogger>();
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        IEmailMessanger emailMessanger = Substitute.For<IEmailMessanger>();

        [Fact]
        public void RecoveryPassword_WhenEmailIsFound_Return()
        {
            var culture = "en_EN";
            var email = "test@test.com";
            var user = new User { Email = email, IsDeleted = false };
            userRepository.GetByEmail(email, false, true).Returns(user);
            var userPasswordRecoveryManager = new UserPasswordRecoveryManager(logger, userRepository, emailMessanger);

            userPasswordRecoveryManager.RecoveryPassword(email, culture);
        }
        [Fact]
        public void RecoveryPassword_WhenEmailIsNotFound_ThrowNotFoundException()
        {
            var culture = "en_EN";
            var email = "test@test.com";
            userRepository.GetByEmail(email, false, true).ReturnsNull();
            var userPasswordRecoveryManager = new UserPasswordRecoveryManager(logger, userRepository, emailMessanger);

            Assert.Throws<NotFoundException>(() => userPasswordRecoveryManager.RecoveryPassword(email, culture));
        }
        [Fact]
        public void CheckRecoveryCode_WhenCodeIsFoundAndEmailIsFound_Return()
        {
            var command = new CheckRecoveryCodeCommand { UserEmail = "test@test.com", RecoveryCode = 1111 };
            var user = new User 
            { 
                Email = command.UserEmail, 
                IsDeleted = false, 
                RecoveryCode = command.RecoveryCode 
            };
            userRepository.GetByEmail(command.UserEmail).Returns(user);
            var userPasswordRecoveryManager = new UserPasswordRecoveryManager(logger, userRepository, emailMessanger);

            var result = userPasswordRecoveryManager.CheckRecoveryCode(command);

            Assert.NotEqual("", result);
        }
        [Fact]
        public void CheckRecoveryCode_WhenEmailIsNotFound_ThrowNotFoundException()
        {
            var command = new CheckRecoveryCodeCommand
            {
                UserEmail = "test@test.com",
                RecoveryCode = 1111
            };
            userRepository.GetByEmail(command.UserEmail).ReturnsNull();
            var userPasswordRecoveryManager = new UserPasswordRecoveryManager(logger, userRepository, emailMessanger);

            Assert.Throws<NotFoundException>(() => userPasswordRecoveryManager.CheckRecoveryCode(command));
        }
        [Fact]
        public void CheckRecoveryCode_WhenCodeIsNotFound_ThrowNotFoundException()
        {
            var command = new CheckRecoveryCodeCommand
            {
                UserEmail = "test@test.com",
                RecoveryCode = 1111
            };
            var user = new User
            {
                Email = command.UserEmail,
                IsDeleted = false,
                RecoveryCode = 2222
            };
            userRepository.GetByEmail(command.UserEmail).Returns(user);
            var userPasswordRecoveryManager = new UserPasswordRecoveryManager(logger, userRepository, emailMessanger);

            Assert.Throws<ValidationException>(() => userPasswordRecoveryManager.CheckRecoveryCode(command));
        }
        [Fact]
        public void ChangePassword_WhenTokenIsFoundAndPasswordIsValid_Return()
        {
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = "1234567890",
                UserPassword = "Pass1234!",
                UserConfirmPassword = "Pass1234!"
            };
            var user = new User { IsDeleted = false, RecoveryToken = command.RecoveryToken };
            userRepository.GetByRecoveryToken(command.RecoveryToken, false).Returns(user);
            var userPasswordRecoveryManager = new UserPasswordRecoveryManager(logger, userRepository, emailMessanger);

            userPasswordRecoveryManager.ChangePassword(command);
        }
        [Fact]
        public void ChangePassword_WhenTokenIsNotFound_ThrowNotFoundException()
        {
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = "1234567890",
                UserPassword = "Pass1234!",
                UserConfirmPassword = "Pass1234!"
            };
            userRepository.GetByRecoveryToken(command.RecoveryToken, false).ReturnsNull();
            var userPasswordRecoveryManager = new UserPasswordRecoveryManager(logger, userRepository, emailMessanger);

            Assert.Throws<NotFoundException>(() => userPasswordRecoveryManager.ChangePassword(command));
        }
        [Fact]
        public void ChangePassword_WhenPasswordIsNotValid_ThrowValidationException()
        {
            var command = new ChangeUserPasswordCommand
            {
                RecoveryToken = "1234567890",
                UserPassword = "Pass1234!",
                UserConfirmPassword = ""
            };
            var user = new User { IsDeleted = false, RecoveryToken = command.RecoveryToken };
            userRepository.GetByRecoveryToken(command.RecoveryToken, false).Returns(user);
            var userPasswordRecoveryManager = new UserPasswordRecoveryManager(logger, userRepository, emailMessanger);

            Assert.Throws<ValidationException>(() => userPasswordRecoveryManager.ChangePassword(command));
        }
    }
}
