using System.Collections.Generic;

namespace _03_DI_Example_WebService.Models
{
    public class FeaturedProductsViewModel
    {
        public FeaturedProductsViewModel(IEnumerable<ProductViewModel> products)
        {
            this.Products = products;
        }

        public IEnumerable<ProductViewModel> Products { get; }
    }
}
