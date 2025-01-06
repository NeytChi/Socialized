﻿using Domain.Packages;

namespace Infrastructure
{
    public class DiscountRepository : IDiscountRepository
    {
        private Context _context;

        public DiscountRepository(Context context) 
        {
            _context = context;
        }

        public DiscountPackage GetBy(int month)
        {
            return _context.DiscountPackages
                .Where(d => month >= d.Month)
                .OrderByDescending(d => d.Month)
                .FirstOrDefault();            
        }
    }
}