using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Users
{
    [Table("Profiles")]
    public partial class UserProfile : BaseEntity
    {
        [ForeignKey("user")]
        public long UserId { get; set; }
        [MaxLength(100)]
        public required string CountryName { get; set; }
        public long TimeZone { get; set; }
        public virtual User user { get; set; }
    }
}