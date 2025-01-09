using Serilog;
using Braintree;

namespace UseCases.Packages
{
    public class GatewayTransaction : IGatewayTransaction
    {
        private readonly ILogger _logger;
        public static BraintreeGateway gateway;

        public GatewayTransaction(ILogger logger, BrainTreeSettings treeSettings) 
        {
            _logger = logger;
            gateway = new BraintreeGateway
            {
                Environment = treeSettings.BraintreeEnvironment == 0 ?
                    Braintree.Environment.SANDBOX : Braintree.Environment.PRODUCTION,
                MerchantId = treeSettings.MerchantId,
                PublicKey = treeSettings.PublicKey,
                PrivateKey = treeSettings.PrivateKey
            };
        }
        public bool Sale(TransactionRequest request)
        {
            var responce = gateway.Transaction.Sale(request);

            if (!responce.IsSuccess())
            {
                _logger.Error("Помилка={result.Message}.");
            }
            return responce.IsSuccess();
        }
    }
}
