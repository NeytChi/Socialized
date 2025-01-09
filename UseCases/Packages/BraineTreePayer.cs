using Serilog;
using Braintree;

namespace UseCases.Packages
{
    public class BraineTreePayer : BaseManager
    {
        private IGatewayTransaction GatewayTransaction;

        public BraineTreePayer(ILogger logger, IGatewayTransaction gatewayTransaction) : base(logger)
        {
            GatewayTransaction = gatewayTransaction;
        }
        public bool PayForPackage(decimal price, string nonceToken, string deviceData, ref string message)
        {
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
            var result = GatewayTransaction.Sale(request);
            if (result)
            {
                Logger.Information("Сплачено за пакет.");
                return true;
            }
            Logger.Error($"Пакет не було сплачено.");
            return false;
        }
    }
}
