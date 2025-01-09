using Domain.InstagramAccounts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Statistics
{
    public partial class OnlineFollowers : BaseEntity
    {
        [ForeignKey("Account")]
        public long AccountId { get; set; }
        public long Value { get; set; }
        public DateTime EndTime { get; set; }
        public virtual BusinessAccount Account { get; set; }
    }
}