using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.AutoPosting
{
    [Table("AutoPostFiles")]
    public partial class AutoPostFile : BaseEntity
    {
        [ForeignKey("post")]
        public long PostId { get; set; }
        public required string Path { get; set; }
        public sbyte Order { get; set; }
        public bool Type { get; set; }
        public required string MediaId { get; set; }
        public required string VideoThumbnail { get; set; }
        public virtual required AutoPost post { get; set; }
    }
}