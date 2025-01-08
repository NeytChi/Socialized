using Domain.Appeals;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Serilog;
using UseCases.Appeals.Messages;
using UseCases.Appeals.Messages.Commands;
using UseCases.Exceptions;

namespace UseCasesTests.Appeals
{
    public class AppealMessageManagerTests
    {
        private ILogger logger = Substitute.For<ILogger>();
        IAppealRepository appealRepository = Substitute.For<IAppealRepository>();
        IAppealMessageRepository appealMessageRepository = Substitute.For<IAppealMessageRepository>();
        IAppealFileManager appealFileManager = Substitute.For<IAppealFileManager>();

        [Fact]
        public void Create_WhenAppealIdAndUserTokenIsNotFound_ThrowNotFoundException()
        {
            var manager = new AppealMessageManager(logger, appealRepository, appealMessageRepository, appealFileManager);
            var command = new CreateAppealMessageCommand
            {
                AppealId = 1,
                Message = "",
                UserToken = "",
                Files = new List<IFormFile> { new FormFileTest() }
            };

            Assert.Throws<NotFoundException>(() => manager.Create(command));
        }
        [Fact]
        public void Create_WhenAppealIdAndTokenIsFound_ReturnMessage()
        {
            var command = new CreateAppealMessageCommand
            {
                AppealId = 1, Message = "test", UserToken = "",
                Files = new List<IFormFile> { new FormFileTest() }
            };
            appealRepository.GetBy(command.AppealId, command.UserToken)
                .Returns(new Domain.Admins.Appeal { Id = command.AppealId });
            var manager = new AppealMessageManager(logger, appealRepository, appealMessageRepository, appealFileManager);
            
            var result = manager.Create(command);

            Assert.Equal(command.AppealId, result.AppealId);
            Assert.Equal(command.Message, result.Message);
        }
        [Fact]
        public void Update_WhenAppealIdIsNotFound_ThrowNotFoundException()
        {
            var command = new UpdateAppealMessageCommand
            {
                MessageId = 1,
                Message = "update test",
            };
            appealMessageRepository.GetBy(command.MessageId).ReturnsNull();
            var manager = new AppealMessageManager(logger, appealRepository, appealMessageRepository, appealFileManager);
            
            Assert.Throws<NotFoundException>(() => manager.Update(command));
        }
        [Fact]
        public void Update_WhenAppealIdIsFound_Return()
        {
            var command = new UpdateAppealMessageCommand
            {
                MessageId = 1,
                Message = "update test",
            };
            appealMessageRepository.GetBy(command.MessageId).Returns(new Domain.Admins.AppealMessage { Id = command.MessageId, Message = "text" });
            var manager = new AppealMessageManager(logger, appealRepository, appealMessageRepository, appealFileManager);

            manager.Update(command);
        }
        [Fact]
        public void Delete_WhenIdIsNotFound_ThrowNotFoundException()
        {
            var command = new DeleteAppealMessageCommand
            {
                MessageId = 1
            };
            appealMessageRepository.GetBy(command.MessageId).ReturnsNull();
            var manager = new AppealMessageManager(logger, appealRepository, appealMessageRepository, appealFileManager);

            Assert.Throws<NotFoundException>(() => manager.Delete(command));
        }
        [Fact]
        public void Delete_WhenIdIsFound_Return()
        {
            var command = new DeleteAppealMessageCommand
            {
                MessageId = 1
            };
            appealMessageRepository.GetBy(command.MessageId).Returns(new Domain.Admins.AppealMessage { Id = command.MessageId, Message = "text" });
            var manager = new AppealMessageManager(logger, appealRepository, appealMessageRepository, appealFileManager);

            manager.Delete(command);
        }
    }
}
