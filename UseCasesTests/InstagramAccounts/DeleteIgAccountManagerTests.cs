using NSubstitute;
using Serilog;
using Domain.InstagramAccounts;
using Domain.GettingSubscribes;
using UseCases.Exceptions;

namespace UseCases.InstagramAccounts.Tests
{
    public class DeleteIgAccountManagerTests
    {
        private DeleteIgAccountManager _manager;
        private ILogger _logger;
        private IIGAccountRepository _accountRepository;
        private ITaskGettingSubscribesRepository _taskGettingSubscribesRepository;

        public DeleteIgAccountManagerTests()
        {
            _logger = Substitute.For<ILogger>();
            _accountRepository = Substitute.For<IIGAccountRepository>();
            _taskGettingSubscribesRepository = Substitute.For<ITaskGettingSubscribesRepository>();

            _manager = new DeleteIgAccountManager(_logger, _accountRepository, _taskGettingSubscribesRepository);
        }

        [Fact]
        public void Delete_ShouldThrowNotFoundException_WhenAccountDoesNotExist()
        {
            // Arrange
            long accountId = 12345;
            _accountRepository.GetByWithState(accountId).Returns((IGAccount)null);

            // Act & Assert
            var exception = Assert.Throws<NotFoundException>(() => _manager.Delete(accountId));
            Assert.Equal("Сервер не визначив запис Instagram аккаунту по id.", exception.Message);
        }

        [Fact]
        public void Delete_ShouldMarkAccountAsDeleted_WhenAccountExists()
        {
            // Arrange
            long accountId = 12345;
            var account = new IGAccount { UserId = accountId, IsDeleted = false };

            _accountRepository.GetByWithState(accountId).Returns(account);

            // Act
            _manager.Delete(accountId);

            // Assert
            Assert.True(account.IsDeleted);
            _accountRepository.Received().Update(account);
            _logger.Received().Information($"Instagram аккаунт був видалений, id={accountId}.");
        }

        [Fact]
        public void StopTasksGettingSubscribes_ShouldMarkTasksAsDeletedAndStopped()
        {
            // Arrange
            long accountId = 12345;
            var tasks = new List<TaskGS>
            {
                new TaskGS { Deleted = false, Stopped = false, Updated = false },
                new TaskGS { Deleted = false, Stopped = false, Updated = false }
            };

            _taskGettingSubscribesRepository.GetBy(accountId).Returns(tasks);

            // Act
            _manager.StopTasksGettingSubscribes(accountId);

            // Assert
            foreach (var task in tasks)
            {
                Assert.True(task.Deleted);
                Assert.True(task.Stopped);
                Assert.True(task.Updated);
            }
            _taskGettingSubscribesRepository.Received().Update(tasks);
            _logger.Received().Information($"Всі задачі були закриті по Instagram аккаунту, id={accountId}.");
        }
    }
}
