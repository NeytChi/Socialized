using Serilog;
using NSubstitute;
using Domain.Packages;
using UseCases.Packages;
using Domain.AutoPosting;
using Domain.InstagramAccounts;
using Domain.Users;
using NSubstitute.ReturnsExtensions;

namespace UseCasesTests.Packages
{
    public class PackageManagerTests
    {
        private readonly ILogger logger;
        private readonly IServiceAccessRepository serviceAccessRepository;
        private readonly IPackageAccessRepository packageAccessRepository;
        private readonly IDiscountRepository discountRepository;
        private readonly IForServerAccessCountingRepository counterRepository;
        private readonly IUserRepository userRepository;
        private readonly PackageManager packageManager;

        public PackageManagerTests()
        {
            logger = Substitute.For<ILogger>();
            serviceAccessRepository = Substitute.For<IServiceAccessRepository>();
            packageAccessRepository = Substitute.For<IPackageAccessRepository>();
            discountRepository = Substitute.For<IDiscountRepository>();
            counterRepository = Substitute.For<IForServerAccessCountingRepository>();
            userRepository = Substitute.For<IUserRepository>();
            packageManager = new PackageManager(logger, serviceAccessRepository, packageAccessRepository, discountRepository, counterRepository, userRepository);
        }

        [Fact]
        public void CreateDefaultServiceAccess_CreatesAndLogsServiceAccess()
        {
            // Arrange
            var userId = 1;
            var package = new PackageAccess { Id = 1, Name = "" };
            packageAccessRepository.GetFirst().Returns(package);
            serviceAccessRepository.When(x => x.Create(Arg.Any<ServiceAccess>())).Do(x => { });
            userRepository.GetBy(userId).Returns(new User { Id = userId });
            // Act
            var result = packageManager.CreateDefaultServiceAccess(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(package.Id, result.Type);
            Assert.True(result.Available);
            Assert.False(result.Paid);
            serviceAccessRepository.Received().Create(Arg.Is<ServiceAccess>(access => access.UserId == userId && access.Type == package.Id));
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Був створений безкоштовний доступ до сервісу")));
        }
        [Fact]
        public void SetPackage_WhenAccessIsNull_CreatesDefaultServiceAccess()
        {
            // Arrange
            var userId = 1;
            var packageId = 1;
            var monthCount = 3;
            var package = new PackageAccess { Id = packageId, Name = "" };
            serviceAccessRepository.GetBy(userId).ReturnsNull();
            packageAccessRepository.GetBy(packageId).Returns(package);
            packageAccessRepository.GetFirst().Returns(package);
            userRepository.GetBy(userId).Returns(new User { Id = userId });
            // Act
            packageManager.SetPackage(userId, packageId, monthCount);

            // Assert
            serviceAccessRepository.Received().Create(Arg.Is<ServiceAccess>(access => access.UserId == userId && access.Type == package.Id));
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Сервісний доступ було оновлено")));
        }

        [Fact]
        public void SetPackage_WhenAccessExists_UpdatesServiceAccess()
        {
            // Arrange
            var userId = 1;
            var packageId = 1;
            var monthCount = 3;
            var package = new PackageAccess { Id = packageId, Name = "" };
            var access = new ServiceAccess { UserId = userId, Type = 2, User = new User() };
            serviceAccessRepository.GetBy(userId).Returns(access);
            packageAccessRepository.GetBy(packageId).Returns(package);

            // Act
            packageManager.SetPackage(userId, packageId, monthCount);

            // Assert
            Assert.Equal(packageId, access.Type);
            Assert.True(access.Available);
            Assert.True(access.Paid);
            serviceAccessRepository.Received().Update(access);
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Сервісний доступ було оновлено")));
        }
        [Fact]
        public void IsServicePackagePersonalize_WhenAccessIsNull_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            serviceAccessRepository.GetByUser(userId).ReturnsNull();

            // Act
            var result = packageManager.IsServicePackagePersonalize(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsServicePackagePersonalize_WhenPackageIsUnlimited_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var access = new ServiceAccess { Type = 1, User = new User() };
            var package = new PackageAccess { IGAccounts = -1, Posts = -1, Stories = -1, Name = "" };
            serviceAccessRepository.GetByUser(userId).Returns(access);
            packageAccessRepository.GetBy(access.Type).Returns(package);

            // Act
            var result = packageManager.IsServicePackagePersonalize(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsServicePackagePersonalize_WhenAccountsExceedLimit_ReturnsFalseAndLogsWarning()
        {
            // Arrange
            var userId = 1;
            var access = new ServiceAccess { Type = 1, User = new User() };
            var package = new PackageAccess { IGAccounts = 1, Posts = 5, Stories = 5 , Name = "" };
            var accounts = new List<IGAccount> { new IGAccount(), new IGAccount() };
            serviceAccessRepository.GetByUser(userId).Returns(access);
            packageAccessRepository.GetBy(access.Type).Returns(package);
            counterRepository.GetAccounts(userId).Returns(accounts);

            // Act
            var result = packageManager.IsServicePackagePersonalize(userId);

            // Assert
            Assert.False(result);
            logger.Received().Warning("Instagram аккаунтів більше ніж доступно по пакету.");
        }

        [Fact]
        public void IsServicePackagePersonalize_WhenPostsExceedLimit_ReturnsFalseAndLogsWarning()
        {
            // Arrange
            var userId = 1;
            var access = new ServiceAccess { Type = 1, User = new User() };
            var package = new PackageAccess { IGAccounts = 1, Posts = 1, Stories = 5, Name = "" };
            var accounts = new List<IGAccount> { new IGAccount() };
            var posts = new List<AutoPost> { new AutoPost(), new AutoPost() };
            serviceAccessRepository.GetByUser(userId).Returns(access);
            packageAccessRepository.GetBy(access.Type).Returns(package);
            counterRepository.GetAccounts(userId).Returns(accounts);
            counterRepository.Get(userId, true).Returns(posts);

            // Act
            var result = packageManager.IsServicePackagePersonalize(userId);

            // Assert
            Assert.False(result);
            logger.Received().Warning("Кількість постів перебільшує кількість доступний по сервісному пакету.");
        }

        [Fact]
        public void IsServicePackagePersonalize_WhenStoriesExceedLimit_ReturnsFalseAndLogsWarning()
        {
            // Arrange
            var userId = 1;
            var access = new ServiceAccess { Type = 1, User = new User() };
            var package = new PackageAccess { IGAccounts = 1, Posts = 5, Stories = 1, Name = "" };
            var accounts = new List<IGAccount> { new IGAccount() };
            var storyPosts = new List<AutoPost> { new AutoPost(), new AutoPost() };
            serviceAccessRepository.GetByUser(userId).Returns(access);
            packageAccessRepository.GetBy(access.Type).Returns(package);
            counterRepository.GetAccounts(userId).Returns(accounts);
            counterRepository.Get(userId, false).Returns(storyPosts);

            // Act
            var result = packageManager.IsServicePackagePersonalize(userId);

            // Assert
            Assert.False(result);
            logger.Received().Warning("Кількість сторіc перебільшує кількість доступний по сервісному пакету.");
        }
    }
}
