using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.AutoPosting
{
    public partial class AutoPostFile : BaseEntity
    {
        [ForeignKey("post")]
        public long PostId { get; set; }
        public string Path { get; set; }
        public sbyte Order { get; set; }
        public bool Type { get; set; }
        public string MediaId { get; set; }
        public string VideoThumbnail { get; set; }
        public virtual AutoPost post { get; set; }
    }
}