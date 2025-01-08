using Core;
using Domain.Users;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Exceptions;
using UseCases.Packages;
using UseCases.Users;
using UseCases.Users.Commands;

namespace UseCasesTests.Users
{
    public class UsersManagerTests
    {
        ILogger logger = Substitute.For<ILogger>();
        IUserRepository userRepository = Substitute.For<IUserRepository>();
        IEmailMessanger emailMessanger = Substitute.For<IEmailMessanger>();
        IPackageManager packageManager = Substitute.For<IPackageManager>();

        [Fact]
        public void Create_WhenUserIsAlreadyExistAndNotDeleted_ThrowNotFoundException()
        {
            var command = new CreateUserCommand
            {
                Email = "test@test.com",
                FirstName = "Rick",
                LastName = "Dolton",
                Password = "Pass1234!",
                CountryName = "USA",
                TimeZone = 6,
                Culture = "en_EN"
            };
            var user = new User { Email = command.Email, IsDeleted = false };
            userRepository.GetByEmail(command.Email).Returns(user);
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            Assert.Throws<NotFoundException>(() => userManager.Create(command));
        }
        [Fact]
        public void Create_WhenUserWasDeleted_RestoreUserAndReturn()
        {
            var command = new CreateUserCommand
            {
                Email = "test@test.com",
                FirstName = "Rick",
                LastName = "Dolton",
                Password = "Pass1234!",
                CountryName = "USA",
                TimeZone = 6,
                Culture = "en_EN"
            };
            var user = new User { Email = command.Email, IsDeleted = true };
            userRepository.GetByEmail(command.Email).Returns(user);
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            userManager.Create(command);
        }
        [Fact]
        public void Create_WhenJustNewUser_CreateNewUserAndReturn()
        {
            var command = new CreateUserCommand
            {
                Email = "test@test.com",
                FirstName = "Rick",
                LastName = "Dolton",
                Password = "Pass1234!",
                CountryName = "USA",
                TimeZone = 6,
                Culture = "en_EN"
            };
            userRepository.GetByEmail(command.Email).ReturnsNull();
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            userManager.Create(command);
        }
        [Fact]
        public void RegistrationEmail_WhenUserIsNotFoundByEmail_ThrowNotFoundException()
        {
            var culture = "en_EN";
            var email = "test@test.com";
            userRepository.GetByEmail(email).ReturnsNull();
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            Assert.Throws<NotFoundException>(() => userManager.RegistrationEmail(email, culture));
        }
        [Fact]
        public void RegistrationEmail_WhenUserIsFoundByEmailValid_SendEmailAndReturn()
        {
            var culture = "en_EN";
            var email = "test@test.com";
            var user = new User { Email = email, IsDeleted = true };
            userRepository.GetByEmail(email).Returns(user);
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            userManager.RegistrationEmail(email, culture);
        }
        [Fact]
        public void Activate_WhenUserIsNotFoundByToken_ThrowNotFoundException()
        {
            var hash = "1234567890";
            userRepository.GetByHash(hash, false, false).ReturnsNull();
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            Assert.Throws<NotFoundException>(() => userManager.Activate(hash));
        }
        [Fact]
        public void Activate_WhenUserIsFoundByToken_ActivateAndReturn()
        {
            var hash = "1234567890";
            var user = new User { Email = "test@test.com", IsDeleted = true };
            userRepository.GetByHash(hash, false, false).Returns(user);
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            userManager.Activate(hash);
        }
        [Fact]
        public void Delete_WhenUserIsNotFoundByToken_ThrowNotFoundException()
        {
            var userToken = "1234567890";
            userRepository.GetByUserTokenNotDeleted(userToken).ReturnsNull();
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            Assert.Throws<NotFoundException>(() => userManager.Delete(userToken));
        }
        [Fact]
        public void Delete_WhenUserIsFoundByToken_DeleteAndReturn()
        {
            var userToken = "1234567890";
            var user = new User { Email = "test@test.com", IsDeleted = false };
            userRepository.GetByUserTokenNotDeleted(userToken).Returns(user);
            var userManager = new UsersManager(logger, userRepository, emailMessanger, packageManager);

            userManager.Delete(userToken);
        }
    }
}
