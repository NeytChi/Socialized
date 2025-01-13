namespace UseCases.Packages.Command
{
    public class PayForPackageCommand
    {
        public required string UserToken { get; set; }
        public required string NonceToken { get; set; }
        public long PackageId { get; set; }
        public int MonthCount { get; set; }
    }
}
