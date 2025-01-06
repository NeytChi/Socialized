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
        [Fact]
        public void Login_WhenPasswordAndEmailIsValid_ReturnUser()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var logger = Substitute.For<ILogger>();
            var userRepository = Substitute.For<IUserRepository>();
            var profileCondition = new ProfileCondition();
            var hashedPassword = profileCondition.HashPassword(command.Password);
            var user = new User { Email = command.Email, Password = hashedPassword };


            userRepository.GetByEmail(command.Email).Returns(user);
            var userLoginManager = new UserLoginManager(logger, userRepository);
            var result = userLoginManager.Login(command);

            Assert.Equal(command.Email, result.Email);
            Assert.Equal(hashedPassword, result.Password);
        }
        [Fact]
        public void Login_WhenPasswordIsNotValid_ThrowException()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var logger = Substitute.For<ILogger>();
            var userRepository = Substitute.For<IUserRepository>();
            var profileCondition = new ProfileCondition();
            var hashedPassword = profileCondition.HashPassword("different_password");
            var user = new User { Email = command.Email, Password = hashedPassword };


            userRepository.GetByEmail(command.Email).Returns(user);
            var userLoginManager = new UserLoginManager(logger, userRepository);
            Assert.Throws<ValidationException>(() => userLoginManager.Login(command));
        }
        [Fact]
        public void Login_WhenEmailIsNotValid_ThrowException()
        {
            var command = new LoginUserCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var logger = Substitute.For<ILogger>();
            var userRepository = Substitute.For<IUserRepository>();
            var profileCondition = new ProfileCondition();
            var hashedPassword = profileCondition.HashPassword("different_password");
            var user = new User { Email = command.Email, Password = hashedPassword };


            userRepository.GetByEmail(command.Email).ReturnsNull();
            var userLoginManager = new UserLoginManager(logger, userRepository);
            Assert.Throws<NotFoundException>(() => userLoginManager.Login(command));
        }
        [Fact]
        public void Logout_WhenTokenIsValid_Return()
        {
            var tokenForUse = "1234567890";
            var logger = Substitute.For<ILogger>();
            var userRepository = Substitute.For<IUserRepository>();
            var profileCondition = new ProfileCondition();
            var user = new User { TokenForUse = tokenForUse };
            userRepository.GetByUserTokenNotDeleted(tokenForUse).Returns(user);
            var userLoginManager = new UserLoginManager(logger, userRepository);

            userLoginManager.LogOut(tokenForUse);
        }
        [Fact]
        public void Logout_WhenTokenIsNotValid_Return()
        {
            var tokenForUse = "1234567890";
            var logger = Substitute.For<ILogger>();
            var userRepository = Substitute.For<IUserRepository>();
            var profileCondition = new ProfileCondition();
            var user = new User { TokenForUse = tokenForUse };
            userRepository.GetByUserTokenNotDeleted(tokenForUse).ReturnsNull();
            var userLoginManager = new UserLoginManager(logger, userRepository);

            Assert.Throws<NotFoundException>(() => userLoginManager.LogOut(tokenForUse));
        }
    }
}
