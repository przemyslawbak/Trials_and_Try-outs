using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _03_DI_Example_WebService.Models;
using Commerce.Domain;

namespace _03_DI_Example_WebService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService productService;

        public HomeController(IProductService productService)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        public ViewResult Index()
        {
            IEnumerable<DiscountedProduct> featuredProducts = this.productService.GetFeaturedProducts(); //tutaj powinno być podmienione

            var vm = new FeaturedProductsViewModel(from product in featuredProducts select new ProductViewModel(product));

            return this.View(vm);
        }

        public ViewResult About()
        {
            return this.View();
        }
    }
}
