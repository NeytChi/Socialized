namespace Domain.Packages
{
    public interface IPackageAccessRepository
    {
        PackageAccess GetFirst();
        PackageAccess GetBy(long packageId);
    }
}
