using System.ComponentModel.DataAnnotations;

namespace UseCases.Packages.Command
{
    public class PayForPackageCommand
    {
        [Required(ErrorMessage = "User token is required")]
        public required string UserToken { get; set; }

        [Required(ErrorMessage = "Nonce token is required")]
        public required string NonceToken { get; set; }

        [Required(ErrorMessage = "Package ID is required")]
        public long PackageId { get; set; }

        [Required(ErrorMessage = "Month count is required")]
        [Range(1, 12, ErrorMessage = "Month count should be between 1 and 12")]
        public int MonthCount { get; set; }
    }
}
