using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dependency_Injection_01_ksiazka.Models
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }
        Product this[string name] { get; }
        void AddProduct(Product product);
        void DeleteProduct(Product product);
    }
}
