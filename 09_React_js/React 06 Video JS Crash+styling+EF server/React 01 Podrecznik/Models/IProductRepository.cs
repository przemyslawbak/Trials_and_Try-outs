using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace React_01_Podrecznik.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        Product DeleteProduct(int productID);
    }
}
