using Domain.Packages;

namespace Infrastructure
{
    public class ServiceAccessRepository : IServiceAccessRepository
    {
        private Context context;

        public ServiceAccessRepository(Context _context)
        {
            context = _context;
        }
        public void Create(ServiceAccess serviceAccess)
        {
            context.ServiceAccess.Add(serviceAccess);
            context.SaveChanges();
        }
        public void Update(ServiceAccess serviceAccess)
        {
            context.ServiceAccess.Update(serviceAccess);
            context.SaveChanges();
        }
        public ServiceAccess GetBy(long packageId)
        {
            return context.ServiceAccess.Where(sa => sa.Id == packageId).FirstOrDefault();
        }
        public ServiceAccess GetByUser(long userId)
        {
            return context.ServiceAccess.Where(sa => sa.UserId == userId).FirstOrDefault();
        }
    }
}
