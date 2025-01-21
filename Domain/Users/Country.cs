using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Users
{
    [Table("Countries")]
    public partial class Country : BaseEntity
    {
        [Column("name", TypeName="varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci")]
        public required string Name { get; set; }
		[Column("fullname", TypeName="varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci")]
        public required string Fullname { get; set; }
		[Column("english", TypeName="varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci")]
        public required string English { get; set; }
		[Column("location", TypeName="varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci")]
        public required string Location { get; set; }
		[Column("location_precise", TypeName="varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci")]
        public required string LocationPrecise { get; set; }
    }
}