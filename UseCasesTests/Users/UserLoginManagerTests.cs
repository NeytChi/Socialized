using Core;
using Serilog;
using AutoMapper;
using NSubstitute;
using Domain.Users;
using UseCases.Users;
using UseCases.Users.Commands;
using UseCases.Exceptions;
using NSubstitute.ReturnsExtensions;
using UseCases.Users.Response;

namespace UseCasesTests.Users
{
    public class UserLoginManagerTests
    {
        private ILogger logger = Substitute.For<ILogger>();
        private IUserRepository userRepository = Substitute.For<IUserRepository>();
        private IMapper mapper = Substitute.For<IMapper>();

        private ProfileCondition profileCondition = new ProfileCondition();
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
            var userLoginManager = new UserLoginManager(logger, userRepository, mapper);
            mapper.Map<UserResponse>(user).Returns(new UserResponse { Email = command.Email, FirstName = "", LastName = "", TokenForUse = "" });

            var result = userLoginManager.Login(command);

            Assert.Equal(command.Email, result.Email);
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
            var userLoginManager = new UserLoginManager(logger, userRepository, mapper);

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
            var userLoginManager = new UserLoginManager(logger, userRepository, mapper);

            Assert.Throws<NotFoundException>(() => userLoginManager.Login(command));
        }
        [Fact]
        public void Logout_WhenUserTokenIsFound_Return()
        {
            var tokenForUse = "1234567890";
            var user = new User { TokenForUse = tokenForUse };
            userRepository.GetByUserTokenNotDeleted(tokenForUse).Returns(user);
            var userLoginManager = new UserLoginManager(logger, userRepository, mapper);

            userLoginManager.LogOut(tokenForUse);
        }
        [Fact]
        public void Logout_WhenUserTokenIsNotFound_ThrowNotFoundException()
        {
            var tokenForUse = "1234567890";
            var user = new User { TokenForUse = tokenForUse };
            userRepository.GetByUserTokenNotDeleted(tokenForUse).ReturnsNull();
            var userLoginManager = new UserLoginManager(logger, userRepository, mapper);

            Assert.Throws<NotFoundException>(() => userLoginManager.LogOut(tokenForUse));
        }
    }
}
