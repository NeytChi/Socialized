using Serilog;
using NSubstitute;
using Domain.Admins;
using UseCases.Exceptions;
using UseCases.Appeals.Replies.Commands;
using Domain.Appeals;
using Domain.Appeals.Replies;
using Xunit;
using UseCases.Appeals.Replies;
using UseCases.Appeals;

namespace UseCasesTests.Appeals.Replies
{
    public class AppealMessageReplyManagerTests
    {
        ILogger logger = Substitute.For<ILogger>();
        IAppealManager appealManager = Substitute.For<IAppealManager>();
        IAppealMessageRepository messageRepository = Substitute.For<IAppealMessageRepository>();
        IAppealMessageReplyRepository replyRepository = Substitute.For<IAppealMessageReplyRepository>();
        AppealMessageReplyManager manager;

        public AppealMessageReplyManagerTests()
        {
            manager = new AppealMessageReplyManager(logger, replyRepository, messageRepository, appealManager);
        }

        [Fact]
        public void Create_WhenMessageIsNotFound_ThrowsNotFoundException()
        {
            var command = new CreateAppealMessageReplyCommand { AppealMessageId = 1, Reply = "Test Reply" };
            messageRepository.GetBy(command.AppealMessageId).Returns((AppealMessage)null);

            Assert.Throws<NotFoundException>(() => manager.Create(command));
        }

        [Fact]
        public void Create_WhenMessageIsFound_CreatesReply()
        {
            var command = new CreateAppealMessageReplyCommand { AppealMessageId = 1, Reply = "Test Reply" };
            var message = new AppealMessage { Id = 1, AppealId = 1 };
            messageRepository.GetBy(command.AppealMessageId).Returns(message);

            var reply = manager.Create(command);

            appealManager.Received().UpdateAppealToAnswered(message.AppealId);
            replyRepository.Received().Create(reply);
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Було створенно відповідь на повідомлення")));
            Assert.Equal(message.Id, reply.AppealMessageId);
            Assert.Equal(command.Reply, reply.Reply);
            Assert.NotNull(reply.CreatedAt);
        }

        [Fact]
        public void Update_WhenReplyIsNotFound_ThrowsNotFoundException()
        {
            var command = new UpdateAppealMessageReplyCommand { ReplyId = 1, Reply = "Updated Reply" };
            replyRepository.Get(command.ReplyId).Returns((AppealMessageReply)null);

            Assert.Throws<NotFoundException>(() => manager.Update(command));
        }

        [Fact]
        public void Update_WhenReplyIsFound_UpdatesReply()
        {
            var command = new UpdateAppealMessageReplyCommand { ReplyId = 1, Reply = "Updated Reply" };
            var reply = new AppealMessageReply { Id = 1, Reply = "Old Reply" };
            replyRepository.Get(command.ReplyId).Returns(reply);

            manager.Update(command);

            Assert.Equal(command.Reply, reply.Reply);
            replyRepository.Received().Update(reply);
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Відповідь була оновленна")));
        }

        [Fact]
        public void Delete_WhenReplyIsNotFound_ThrowsNotFoundException()
        {
            var command = new DeleteAppealMessageReplyCommand { ReplyId = 1 };
            replyRepository.Get(command.ReplyId).Returns((AppealMessageReply)null);

            Assert.Throws<NotFoundException>(() => manager.Delete(command));
        }

        [Fact]
        public void Delete_WhenReplyIsFound_DeletesReply()
        {
            var command = new DeleteAppealMessageReplyCommand { ReplyId = 1 };
            var reply = new AppealMessageReply { Id = 1, IsDeleted = false };
            replyRepository.Get(command.ReplyId).Returns(reply);

            manager.Delete(command);

            Assert.True(reply.IsDeleted);
            replyRepository.Received().Update(reply);
            logger.Received().Information(Arg.Is<string>(str => str.Contains("Відповідь була видаленна")));
        }
    }
}
