using Core;
using Domain.Admins;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Admins;
using UseCases.Admins.Commands;
using UseCases.Exceptions;

namespace UseCasesTests.Admins
{
    public class AdminManagerTests
    {
        ILogger logger = Substitute.For<ILogger>();
        IAdminRepository repository = Substitute.For<IAdminRepository>();
        IAdminEmailManager emailManager = Substitute.For<IAdminEmailManager>();
        ProfileCondition profileCondition = new ProfileCondition();

        [Fact]
        public void Create_WhenSameEmailIsNotFound_ReturnNewAdmin()
        {
            var command = new CreateAdminCommand
            {
                Email = "test@test.com",
                FirstName = "Rick",
                LastName = "Dolton",
                Password = "password"
            };
            repository.GetByEmail(command.Email, false).ReturnsNull();
            var adminManager = new AdminManager(logger, repository, emailManager);

            var result = adminManager.Create(command);

            Assert.Equal(result.Email, command.Email);
            Assert.Equal(result.FirstName, command.FirstName);
            Assert.Equal(result.LastName, command.LastName);
        }
        [Fact]
        public void Create_WhenSameEmailIsFound_ThrowNotFoundException()
        {
            var command = new CreateAdminCommand
            {
                Email = "test@test.com",
                FirstName = "Rick",
                LastName = "Dolton",
                Password = "password"
            };
            repository.GetByEmail(command.Email, false).Returns(new Admin());
            var adminManager = new AdminManager(logger, repository, emailManager);

            Assert.Throws<NotFoundException>(() => adminManager.Create(command));
        }
        [Fact]
        public void SetupPassword_WhenAdminTokenIsFound_Return()
        {
            var command = new SetupPasswordCommand { Token = "1234567890", Password = "password" };
            repository.GetByPasswordToken(command.Token, false).Returns(new Admin { });
            var adminManager = new AdminManager(logger, repository, emailManager);
            
            adminManager.SetupPassword(command);
        }
        [Fact]
        public void SetupPassword_WhenTokenIsNotFound_ThrowNotFoundException()
        {
            var command = new SetupPasswordCommand { Token = "1234567890", Password = "password" };
            repository.GetByPasswordToken(command.Token, false).ReturnsNull();
            var adminManager = new AdminManager(logger, repository, emailManager);

            Assert.Throws<NotFoundException>(() => adminManager.SetupPassword(command));
        }
        [Fact]
        public void Authentication_WhenEmailIsFoundAndPasswordIsValid_ReturnAdmin()
        {
            var command = new AuthenticationCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = profileCondition.HashPassword(command.Password);
            var admin = new Admin { Id = 1, Email = command.Email, Password = hashedPassword };
            repository.GetByEmail(command.Email).Returns(admin);
            var adminManager = new AdminManager(logger, repository, emailManager);

            var result = adminManager.Authentication(command);

            Assert.Equal(admin.Id, result.Id);
            Assert.Equal(command.Email, result.Email);
        }
        [Fact]
        public void Authentication_WhenEmailIsNotFound_ThrowNotFoundException()
        {
            var command = new AuthenticationCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            repository.GetByEmail(command.Email).ReturnsNull();
            var adminManager = new AdminManager(logger, repository, emailManager);

            Assert.Throws<NotFoundException>(() => adminManager.Authentication(command));
        }
        [Fact]
        public void Authentication_WhenPasswordIsNotValid_ThrowValidationExceptionException()
        {
            var command = new AuthenticationCommand
            {
                Email = "test@test.com",
                Password = "password"
            };
            var hashedPassword = profileCondition.HashPassword("different_password");
            var admin = new Admin { Id = 1, Email = command.Email, Password = hashedPassword };
            repository.GetByEmail(command.Email).Returns(admin);
            var adminManager = new AdminManager(logger, repository, emailManager);

            Assert.Throws<ValidationException>(() => adminManager.Authentication(command));
        }
        [Fact]
        public void Delete_WhenIdIsFound_Return()
        {
            var command = new DeleteAdminCommand { AdminId = 1 };
            repository.GetByAdminId(command.AdminId, false).Returns(new Admin { Id = 1 });
            var adminManager = new AdminManager(logger, repository, emailManager);

            adminManager.Delete(command);
        }
        [Fact]
        public void Delete_WhenIdIsNotFound_ThrowNotFoundException()
        {
            var command = new DeleteAdminCommand { AdminId = 1 };
            repository.GetByAdminId(command.AdminId, false).ReturnsNull();
            var adminManager = new AdminManager(logger, repository, emailManager);

            Assert.Throws<NotFoundException>(() => adminManager.Delete(command));
        }
        [Fact]
        public void CreateCodeForRecoveryPassword_WhenEmailIsFound_Return()
        {
            var email = "test@test.com";
            repository.GetByEmail(email, false).Returns(new Admin { Email = email });
            var adminManager = new AdminManager(logger, repository, emailManager);

            adminManager.CreateCodeForRecoveryPassword(email);
        }
        [Fact]
        public void CreateCodeForRecoveryPassword_WhenEmailIsNotFound_Return()
        {
            var email = "test@test.com";
            repository.GetByEmail(email, false).ReturnsNull();
            var adminManager = new AdminManager(logger, repository, emailManager);

            Assert.Throws<NotFoundException>(() => adminManager.CreateCodeForRecoveryPassword(email));
        }
    }
}
