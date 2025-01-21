using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Packages
{
    [Table("PackageAccesses")]
    public class PackageAccess : BaseEntity
    {
        public required string Name { get; set; }
        public double Price { get; set; }
        public int IGAccounts { get; set; }
        public int Posts { get; set; }
        public int Stories { get; set; }
        public int AnalyticsDays { get; set; }
    }
}