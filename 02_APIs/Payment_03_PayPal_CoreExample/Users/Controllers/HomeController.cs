using BraintreeHttp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayPal.Core;
using PayPal.v1.Payments;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Users.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            //https://github.com/paypal/PayPal-NET-SDK/issues/355
            var environment = new SandboxEnvironment(_config["paypal:clinetId"], _config["paypal:secretId"]);
            var client = new PayPalHttpClient(environment);

            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                    {
                        new Transaction()
                        {
                            Amount = new Amount()
                            {
                                Total = "10",
                                Currency = "USD"
                            }
                        }
                    },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl = "https://localhost:44339/Home/Return",
                    CancelUrl = "https://localhost:44339/Home/Cancel"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };

            PaymentCreateRequest request = new PaymentCreateRequest();

            request.RequestBody(payment);

            HttpStatusCode statusCode;

            try
            {
                HttpResponse response = await client.Execute(request); //authentication fails
                statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                string redirectUrl = null;
                foreach (LinkDescriptionObject link in result.Links)
                {
                    if (link.Rel.Equals("approval_url"))
                    {
                        redirectUrl = link.Href;
                    }
                }

                if (redirectUrl == null)
                {
                    // Didn't find an approval_url in response.Links
                    //await context.Response.WriteAsync("Failed to find an approval_url in the response!");
                }
                else
                {
                    //await context.Response.WriteAsync("Now <a href=\"" + redirectUrl + "\">go to PayPal to approve the payment</a>.");
                }
            }
            catch (HttpException ex)
            {
                statusCode = ex.StatusCode;
                var debugId = ex.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                //await context.Response.WriteAsync("Request failed!  HTTP response code was " + statusCode + ", debug ID was " + debugId);
            }

            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }

        public IActionResult Return()
        {
            return View();
        }
    }
}
