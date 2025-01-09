using Serilog;
using NSubstitute;
using UseCases.AutoPosts;

namespace UseCasesTests.AutoPosts
{
    public class AutoPostConditionTests
    {
        private readonly ILogger logger;
        private readonly AutoPostCondition autoPostCondition;

        public AutoPostConditionTests()
        {
            logger = Substitute.For<ILogger>();
            autoPostCondition = new AutoPostCondition(logger);
        }

        [Fact]
        public void IsExecuteTimeTrue_WhenExecuteAtIsInFuture_ReturnsTrue()
        {
            var executeAt = DateTime.UtcNow.AddHours(4);
            var timezone = 3;

            var result = autoPostCondition.IsExecuteTimeTrue(executeAt, timezone);

            Assert.True(result);
        }

        [Fact]
        public void IsExecuteTimeTrue_WhenExecuteAtIsInPast_ReturnsFalseAndLogsError()
        {
            var executeAt = DateTime.UtcNow.AddHours(-2);
            var timezone = 3;

            var result = autoPostCondition.IsExecuteTimeTrue(executeAt, timezone);

            Assert.False(result);
            logger.Received().Error("Авто пост не може бути виконаний в минулому.");
        }
        [Fact]
        public void CheckFileType_WhenContentTypeIsVideo_ReturnsTrue()
        {
            // Arrange
            var contentType = "video/mp4";
            var message = string.Empty;

            // Act
            var result = autoPostCondition.CheckFileType(contentType, ref message);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckFileType_WhenContentTypeIsImage_ReturnsTrue()
        {
            // Arrange
            var contentType = "image/jpeg";
            var message = string.Empty;

            // Act
            var result = autoPostCondition.CheckFileType(contentType, ref message);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckFileType_WhenContentTypeIsApplicationOctetStream_ReturnsTrue()
        {
            // Arrange
            var contentType = "application/octet-stream";
            var message = string.Empty;

            // Act
            var result = autoPostCondition.CheckFileType(contentType, ref message);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckFileType_WhenContentTypeIsIncorrect_ReturnsFalseAndSetsMessage()
        {
            // Arrange
            var contentType = "text/plain";
            var message = string.Empty;

            // Act
            var result = autoPostCondition.CheckFileType(contentType, ref message);

            // Assert
            Assert.False(result);
            Assert.Equal("File has incorrect format. Required format -> 'image' or 'video'.", message);
        }
    }
}
