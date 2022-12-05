using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Domain
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetFeaturedProducts();
    }
}
