using Domain.Appeals;
using Domain.Appeals.Messages;
using Microsoft.AspNetCore.Http;
using NSubstitute;
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
        public void Create_WhenIdAndTokenIsNotValid_ThrowException()
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
        public void Create_WhenIdAndTokenIsValid_ReturnMessage()
        {

        }
        [Fact]
        public void Update_WhenIdIsNotValid_ThrowException()
        {

        }
        [Fact]
        public void Update_WhenIdIsValid_Return()
        {

        }
        [Fact]
        public void Delete_WhenIdIsNotValid_ThrowException()
        {

        }
        [Fact]
        public void Deletete_WhenIdIsValid_Return()
        {

        }
    }
}
