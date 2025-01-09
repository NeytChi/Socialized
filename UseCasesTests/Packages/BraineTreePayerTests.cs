using Serilog;
using NSubstitute;
using Braintree;
using UseCases.Packages;

namespace UseCasesTests.Packages
{
    public class BraineTreePayerTests
    {
        private ILogger logger;
        private IGatewayTransaction gatewayTransaction;
        private BraineTreePayer braineTreePayer;

        public BraineTreePayerTests()
        {
            logger = Substitute.For<ILogger>();
            gatewayTransaction = Substitute.For<IGatewayTransaction>();
        }

        [Fact]
        public void PayForPackage_WhenTransactionIsSuccessful_LogsInformationAndReturnsTrue()
        {
            // Arrange
            var price = 100.00M;
            var nonceToken = "test_nonce";
            var deviceData = "test_device_data";
            string message = string.Empty;
            var request = new TransactionRequest
            {
                Amount = price,
                PaymentMethodNonce = nonceToken,
                DeviceData = deviceData,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };
            gatewayTransaction.Sale(request).ReturnsForAnyArgs(true);
            braineTreePayer = new BraineTreePayer(logger, gatewayTransaction);

            // Act
            var result = braineTreePayer.PayForPackage(price, nonceToken, deviceData, ref message);

            // Assert
            Assert.True(result);
            logger.Received().Information("Сплачено за пакет.");
        }

        [Fact]
        public void PayForPackage_WhenTransactionFails_LogsErrorAndReturnsFalse()
        {
            // Arrange
            var price = 100.00M;
            var nonceToken = "test_nonce";
            var deviceData = "test_device_data";
            string message = string.Empty;
            var request = new TransactionRequest
            {
                Amount = price,
                PaymentMethodNonce = nonceToken,
                DeviceData = deviceData,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };
            gatewayTransaction.Sale(request).ReturnsForAnyArgs(false);
            braineTreePayer = new BraineTreePayer(logger, gatewayTransaction);

            // Act
            var result = braineTreePayer.PayForPackage(price, nonceToken, deviceData, ref message);

            // Assert
            Assert.False(result);
            logger.Received().Error("Пакет не було сплачено.");
        }
    }
}
