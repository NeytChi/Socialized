using Domain.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Packages
{
    [Table("ServiceAccesses")]
    public partial class ServiceAccess : BaseEntity
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public bool Available { get; set; }
        public long Type { get; set; }
        public bool Paid { get; set; }
        public DateTime PaidAt { get; set; }
        public DateTime DisableAt { get; set; }
        public virtual required User User { get; set; }
    }
}