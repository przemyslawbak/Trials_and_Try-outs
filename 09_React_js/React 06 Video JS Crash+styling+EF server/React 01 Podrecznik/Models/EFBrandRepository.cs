using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace React_01_Podrecznik.Models
{
    public class EFBrandRepository : IBrandRepository
    {
        private ApplicationDbContext context;
        public EFBrandRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Product> Products => context.Products;
    }
}
