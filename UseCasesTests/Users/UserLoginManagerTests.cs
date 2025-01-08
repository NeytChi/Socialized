using Serilog;
using NSubstitute;
using Domain.Users;
using UseCases.Users;
using UseCases.Users.Commands;
using Core;
using UseCases.Exceptions;
using NSubstitute.ReturnsExtensions;

namespace UseCasesTests.Users
{
    public class UserLoginManagerTests
    {
        ILogger logger = Substitute.For<ILogger>();
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        ProfileCondition profileCondition = new ProfileCondition();
        [Fact]
        public void Login_WhenUserPasswordValidAndEmailIsFound_ReturnUser()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = profileCondition.HashPassword(command.Password);
            var user = new User { Email = command.Email, Password = hashedPassword };
            userRepository.GetByEmail(command.Email).Returns(user);
            var userLoginManager = new UserLoginManager(logger, userRepository);
            
            var result = userLoginManager.Login(command);

            Assert.Equal(command.Email, result.Email);
            Assert.Equal(hashedPassword, result.Password);
        }
        [Fact]
        public void Login_WhenUserPasswordIsNotValid_ThrowValidationException()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = profileCondition.HashPassword("different_password");
            var user = new User { Email = command.Email, Password = hashedPassword };
            userRepository.GetByEmail(command.Email).Returns(user);
            var userLoginManager = new UserLoginManager(logger, userRepository);

            Assert.Throws<ValidationException>(() => userLoginManager.Login(command));
        }
        [Fact]
        public void Login_WhenUserEmailIsNotFound_ThrowNotFoundException()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = profileCondition.HashPassword("different_password");
            var user = new User { Email = command.Email, Password = hashedPassword };
            userRepository.GetByEmail(command.Email).ReturnsNull();
            var userLoginManager = new UserLoginManager(logger, userRepository);

            Assert.Throws<NotFoundException>(() => userLoginManager.Login(command));
        }
        [Fact]
        public void Logout_WhenUserTokenIsFound_Return()
        {
            var tokenForUse = "1234567890";
            var user = new User { TokenForUse = tokenForUse };
            userRepository.GetByUserTokenNotDeleted(tokenForUse).Returns(user);
            var userLoginManager = new UserLoginManager(logger, userRepository);

            userLoginManager.LogOut(tokenForUse);
        }
        [Fact]
        public void Logout_WhenUserTokenIsNotFound_ThrowNotFoundException()
        {
            var tokenForUse = "1234567890";
            var user = new User { TokenForUse = tokenForUse };
            userRepository.GetByUserTokenNotDeleted(tokenForUse).ReturnsNull();
            var userLoginManager = new UserLoginManager(logger, userRepository);

            Assert.Throws<NotFoundException>(() => userLoginManager.LogOut(tokenForUse));
        }
    }
}
