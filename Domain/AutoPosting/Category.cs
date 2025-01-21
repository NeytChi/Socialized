using Domain.InstagramAccounts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.AutoPosting
{
    [Table("Categories")]
    public partial class Category : BaseEntity
    {
        public Category()
        {
            Links = new HashSet<AutoPost>();
        }
        [ForeignKey("Account")]
        public long AccountId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool Deleted { get; set; }
        public virtual IGAccount account { get; set; }
        public virtual ICollection<AutoPost> Links { get; set; }
    }
}