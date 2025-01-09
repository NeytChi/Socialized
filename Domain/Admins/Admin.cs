using System.ComponentModel.DataAnnotations;

namespace Domain.Admins
{
    public partial class Admin : BaseEntity
    {
        [MaxLength(320)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Role { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        [MaxLength(100)]
        public string TokenForStart { get; set; }
        public DateTime LastLoginAt { get; set; }
        public int? RecoveryCode { get; set; }
    }
}
