using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dependency_Injection_01_ksiazka.Models
{
    public class MemoryRepository : IRepository
    {
        private IModelStorage storage;
        public MemoryRepository(IModelStorage modelStore)
        {
            storage = modelStore;
            new List<Product> {
                new Product { Name = "Kajak", Price = 275M },
                new Product { Name = "Kamizelka ratunkowa", Price = 48.95M },
                new Product { Name = "Piłka nożna", Price = 19.50M }
            }.ForEach(p => AddProduct(p));
        }
        public IEnumerable<Product> Products => storage.Items;
        public Product this[string name] => storage[name];
        public void AddProduct(Product product) =>
        storage[product.Name] = product;
        public void DeleteProduct(Product product) =>
        storage.RemoveItem(product.Name);
    }
}
