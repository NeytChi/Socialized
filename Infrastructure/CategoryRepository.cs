using Domain.AutoPosting;

namespace Infrastructure
{
    public class CategoryRepository : ICategoryRepository
    {
        private Context _context;
        public CategoryRepository(Context context)
        {
            _context = context;
        }
        public Category GetBy(long accountId, long categoryId, bool categoryDeleted = false)
        {
            return _context.Categories
                .Where(c => c.AccountId == accountId 
                    && c.Id == categoryId 
                    && c.IsDeleted == categoryDeleted).FirstOrDefault();
        }
    }
}
