using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.InstagramAccounts
{
    [Table("AccountProfiles")]
    public partial class AccountProfile : BaseEntity
    {
        [ForeignKey("Account")]
        public long AccountId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Username { get; set; }
        public long PostsCount { get; set; }
        public long FollowingCount { get; set; }
        public long SubscribersCount { get; set; }
        public required string AvatarUrl { get; set; }
        public long SubscribersGS { get; set; }
        public long SubscribersTodayGS { get; set; }
        public long ConversionGS { get; set; }
        public virtual required IGAccount Account { get; set; }
    }
}