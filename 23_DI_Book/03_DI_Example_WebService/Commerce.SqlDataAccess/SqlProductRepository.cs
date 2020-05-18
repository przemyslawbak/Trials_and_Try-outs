using Commerce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Commerce.SqlDataAccess
{
    public class SqlProductRepository : IProductRepository
    {
        private readonly CommerceContext context;

        public SqlProductRepository(CommerceContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            this.context = context;
        }

        public IEnumerable<Product> GetFeaturedProducts()
        {
            return
                from product in this.context.Products
                where product.IsFeatured
                select product;
        }
    }
}
