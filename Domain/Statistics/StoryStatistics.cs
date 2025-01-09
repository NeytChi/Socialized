using Domain.InstagramAccounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Statistics
{
    public partial class StoryStatistics : BaseEntity
    {
        [ForeignKey("Account")]
        public long AccountId { get; set; }
        public string MediaId { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public int Replies { get; set; }
        public bool Exists { get; set; }
        public long Impressions { get; set; }
        public long Reach { get; set; }
        public DateTime Timestamp { get; set; }
        public virtual BusinessAccount Account { get; set; }
    }
}