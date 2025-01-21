using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.InstagramAccounts
{
    [Table("BusinessAccounts")]
    public partial class BusinessAccount : BaseEntity
    {
        [ForeignKey("Account")]
        public long AccountId { get; set; }
        public required string AccessToken { get; set; }
        public required string ProfilePicture { get; set; }
        public required string AccountUsername { get; set; }
        public required string LongLiveAccessToken { get; set; }
        public required string FacebookId { get; set; }
        public required string BusinessAccountId { get; set; }
        public long FollowersCount { get; set; }
        public int MediaCount { get; set; }
        public DateTime LongTokenExpiresIn { get; set; }
        public DateTime TokenCreated { get; set; }
        public bool Received { get; set; }
        public bool StartProcess { get; set; }
        public DateTime StartedProcess { get; set; }
        public virtual required IGAccount Account { get; set; }
    }
}