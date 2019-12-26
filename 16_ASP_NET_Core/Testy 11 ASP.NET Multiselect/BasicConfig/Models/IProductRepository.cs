using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicConfig.Models.ViewModels;

namespace BasicConfig.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        void SaveProduct(Product product, ProductViewModel productVM, string tekst);

        Product DeleteProduct(int productID);
    }
}
