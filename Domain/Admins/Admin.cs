using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Admins
{
    [Table("Admins")]
    public partial class Admin : BaseEntity
    {
        [MaxLength(320)]
        public required string Email { get; set; }
        [MaxLength(100)]
        public required string FirstName { get; set; }
        [MaxLength(100)]
        public required string LastName { get; set; }
        [MaxLength(100)]
        public required string Role { get; set; }
        [MaxLength(100)]
        public required string Password { get; set; }
        [MaxLength(100)]
        public required string TokenForStart { get; set; }
        public DateTime LastLoginAt { get; set; }
        public int? RecoveryCode { get; set; }
    }
}
