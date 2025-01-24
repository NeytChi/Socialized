using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Admins
{
    [Table("AppealFiles")]
    public partial class AppealFile : BaseEntity
    {
        [ForeignKey("Message")]
        public long MessageId { get; set; }
        public required string RelativePath { get; set; }
        public virtual AppealMessage Message { get; set; }
    }
}