using Braintree;

namespace UseCases.Packages
{
    public interface IGatewayTransaction
    {
        public bool Sale(TransactionRequest request);
    }
}
