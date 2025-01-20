using Domain.Packages;
using UseCases.Packages.Command;

namespace UseCases.Packages
{
    public interface IPackageManager
    {
        ServiceAccess CreateDefaultServiceAccess(long userId);
        ICollection<PackageAccess> GetPackageAccess();
        ICollection<DiscountPackage> GetDiscountPackageAccess();
        string GetClientTokenForPay(GetClientTokenForPayCommand command);
        void PayForPackage(PayForPackageCommand command);
    }
}
