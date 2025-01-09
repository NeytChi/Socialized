using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Admins
{
    public partial class AppealFile : BaseEntity
    {
        [ForeignKey("Message")]
        public long MessageId { get; set; }
        public string RelativePath { get; set; }
        public virtual AppealMessage Message { get; set; }
    }
}