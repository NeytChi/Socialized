using Domain.InstagramAccounts;
using Domain.Admins;
using Domain.Packages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Users
{
    [Table("Users")]
    public partial class User : BaseEntity
    {
        public User()
        {
            IGAccounts = new HashSet<IGAccount>();
        }
        public string TokenForUse { get; set; }
        [MaxLength(320)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public DateTime LastLoginAt { get; set; }
        public string HashForActivate { get; set; }
        public bool Activate { get; set; }
        public int? RecoveryCode { get; set; }
        public string RecoveryToken { get; set; }
        public virtual ServiceAccess access { get; set; }
        public virtual UserProfile profile { get; set; }
        public virtual ICollection<IGAccount> IGAccounts { get; set; }
        public virtual ICollection<Appeal> Appeals { get; set; }
    }
}
