using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.GettingSubscribes
{
    [Table("TaskData")]
    public partial class TaskData : BaseEntity
    {
        public long TaskId { get; set; }
        public required string Names { get; set; }
        public double? Longitute { get; set; }
        public double? Latitute { get; set; }
        public required string Comment { get; set; }
        public bool Stopped { get; set; }
        public int NextPage { get; set; }
        public virtual required TaskGS Task { get; set; }
    }
}
