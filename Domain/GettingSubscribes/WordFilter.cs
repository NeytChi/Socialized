using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.GettingSubscribes
{
    [Table("WordFilters")]
    public partial class WordFilter : BaseEntity
    {
        public long FilterId { get; set; }
        public required string Value { get; set; }
        public bool Use { get; set; }
        public virtual required TaskFilter Filter { get; set; }
    }
}