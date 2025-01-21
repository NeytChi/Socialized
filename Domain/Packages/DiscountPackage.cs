using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Packages
{
    [Table("DiscountPackages")]
    public class DiscountPackage : BaseEntity
    {
        public double Percent { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
    }
}