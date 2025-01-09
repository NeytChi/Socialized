using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Admins
{
    public class AppealMessageReply : BaseEntity
    {
        [ForeignKey("Message")]
        public long AppealMessageId { get; set; }
        public string Reply {  get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual AppealMessage Message { get; set; }
    }
}
