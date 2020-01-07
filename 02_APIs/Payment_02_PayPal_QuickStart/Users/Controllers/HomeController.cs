using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System.Collections.Generic;

namespace Users.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        //TODO: https://github.com/paypal/sdk-core-dotnet
        //2) https://github.com/paypal/PayPal-NET-SDK/wiki/Quick-Start
        [AllowAnonymous]
        public IActionResult Index()
        {
            var config = ConfigManager.Instance.GetProperties(); //paypal config
            var accessToken = new OAuthTokenCredential(config).GetAccessToken(); //OAuth access token
            var apiContext = new APIContext(accessToken);

            // Make an API call
            var payment = Payment.Create(apiContext, new Payment
            {
                intent = "sale",
                payer = new Payer
                {
                    payment_method = "paypal"
                },
                transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        description = "Transaction description.",
                        invoice_number = "001",
                        amount = new Amount
                        {
                            currency = "USD",
                            total = "100.00",
                            details = new Details
                            {
                                tax = "15",
                                shipping = "10",
                                subtotal = "75"
                            }
                        },
                        item_list = new ItemList
                        {
                            items = new List<Item>
                            {
                                new Item
                                {
                                    name = "Item Name",
                                    currency = "USD",
                                    price = "15",
                                    quantity = "5",
                                    sku = "sku"
                                }
                            }
                        }
                    }
                },
                redirect_urls = new RedirectUrls
                {
                    return_url = "http://mysite.com/return",
                    cancel_url = "http://mysite.com/cancel"
                }
            });

            var dupa = payment;

            return View();
        }
    }
}
