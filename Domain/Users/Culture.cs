using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Users
{
    [Table("Cultures")]
    public partial class Culture : BaseEntity
    {
        [MaxLength(100)]
        public required string Key { get; set; }
        [MaxLength(100)]
        public required string Value { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }
    }
}