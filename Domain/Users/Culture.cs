using System.ComponentModel.DataAnnotations;

namespace Domain.Users
{
    public partial class Culture : BaseEntity
    {
        [MaxLength(100)]
        public string Key { get; set; }
        [MaxLength(100)]
        public string Value { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}