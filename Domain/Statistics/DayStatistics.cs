using Domain.InstagramAccounts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Statistics
{
    public partial class DayStatistics : BaseEntity
    {
        [ForeignKey("Account")]
        public long AccountId { get; set; }
        public int FollowerCount { get; set; }
        public int EmailContacts { get; set; }
        public long ProfileViews { get; set; }
        public int GetDirectionsClicks { get; set; }
        public int PhoneCallClicks { get; set; }
        public int TextMessageClicks { get; set; }
        public int WebsiteClicks { get; set; }
        public long Impressions { get; set; }
        public long Reach { get; set; }
        public DateTime EndTime { get; set; }
        public virtual BusinessAccount Account { get; set; }
    }
}