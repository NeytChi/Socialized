using Serilog;
using Braintree;

namespace UseCases.Packages
{
    public class GatewayTransaction : IGatewayTransaction
    {
        private readonly ILogger Logger;
        private BraintreeGateway Gateway;

        public GatewayTransaction(ILogger logger, BrainTreeSettings treeSettings) 
        {
            Logger = logger;
            Gateway = new BraintreeGateway
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
            var responce = Gateway.Transaction.Sale(request);

            if (!responce.IsSuccess())
            {
                Logger.Error("Помилка={result.Message}.");
            }
            return responce.IsSuccess();
        }
    }
}
