using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Xml.Linq;

namespace Sample.Controllers
{
    public class ProductsController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
                            IElasticClient elasticClient,
                            ILogger<ProductsController> logger)
        {
            _logger = logger;
            _elasticClient = elasticClient;
        }

        [HttpGet(Name = "GetAllProducts")]
        public async Task<IActionResult> Get(string keyword)
        {
            var result = await _elasticClient.SearchAsync<Product>(
                             s => s.Query(
                                 q => q.QueryString(
                                     d => d.Query('*' + keyword + '*')
                                 )).Size(5000));

            _logger.LogInformation("ProductsController Get - ", DateTime.UtcNow);
            return Ok(result.Documents.ToList());
        }

        [HttpPost(Name = "AddProduct")]
        public async Task<IActionResult> Post(Product product)
        {
            // Add product to ELS index
            var product1 = new Product
            {
                Description = "Product 1",
                Id = 1,
                Price = 200,
                Measurement = "2",
                Quantity = 90,
                ShowPrice = true,
                Title = "Nike Shoes",
                Unit = "10"
            };

            // Index product dto
            await _elasticClient.IndexDocumentAsync(product);

            _logger.LogInformation("ProductsController Get - ", DateTime.UtcNow);
            return Ok();
        }
    }
}
