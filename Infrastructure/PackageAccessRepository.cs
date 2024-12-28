using Domain.Packages;

namespace Infrastructure
{
    public class PackageAccessRepository : IPackageAccessRepository
    {
        private Context context;

        public PackageAccessRepository(Context _context) 
        {
            context = _context;
        }
        public PackageAccess GetBy(long packageId)
        {
            return context.PackageAccess.Where(pa => pa.Id == packageId).FirstOrDefault();
        }
        public PackageAccess GetFirst()
        {
            return context.PackageAccess.First();
        }
    }
}
