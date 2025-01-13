using Domain.InstagramAccounts;

namespace UseCases.InstagramApi.MockingApi
{
    public class GetStateData : IGetStateData
    {
        public string AsString(IGAccount account)
        {
            return string.Empty;
        }
    }
}
