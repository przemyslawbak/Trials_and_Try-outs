using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicConfig.Models.ViewModels;

namespace BasicConfig.Models
{
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;
        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Product> Products => context.Products;

        public Product DeleteProduct(int productID)
        {
            Product productDb = context.Products.FirstOrDefault(p => p.ProductID == productID);
            if (productDb != null)
            {
                context.Products.Remove(productDb);
                context.SaveChanges();
            }
            return productDb;
        }

        public void SaveProduct(Product product, ProductViewModel productVM, string tekst)
        {
            product.BradIds = tekst;
            product.Name = productVM.Name;
            product.Price = productVM.Price;
            product.Description = productVM.Description;
            product.Category = productVM.Category;
        }
    }

    
}
