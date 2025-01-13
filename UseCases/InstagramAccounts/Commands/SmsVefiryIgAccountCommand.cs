namespace UseCases.InstagramAccounts.Commands
{
    public class SmsVefiryIgAccountCommand
    {
        public long AccountId { get; set; }
        public required string UserToken { get; set; }
        public int VerifyCode { get; set; }
    }
}
