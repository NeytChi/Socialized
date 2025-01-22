using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.InstagramAccounts
{
    [Table("States")]
    public partial class SessionState : BaseEntity
    {
        [ForeignKey("Account")]
        public long AccountId { get; set; }
        public required string SessionSave { get; set; }
        public bool Usable { get; set; }
        public bool Challenger { get; set; }
        public bool Relogin { get; set; }
        public bool Spammed { get; set; }
        public DateTime SpammedStarted { get; set; }
        public DateTime SpammedEnd { get; set; }
        public virtual IGAccount Account { get; set; }
        public virtual TimeAction TimeAction { get; set; }
    }
}
