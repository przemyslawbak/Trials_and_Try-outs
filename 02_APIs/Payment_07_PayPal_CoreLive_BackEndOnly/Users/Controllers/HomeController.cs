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
    //https://github.com/paypal/PayPal-NET-SDK/issues/355
    //https://github.com/paypal/PayPal-NET-SDK/issues/336
    //https://ru.stackoverflow.com/a/779301
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private SandboxEnvironment _environmentPayPal;
        private PayPalHttpClient _clientPayPal;

        public HomeController(IConfiguration config)
        {
            _config = config;
            _environmentPayPal = new SandboxEnvironment(_config["paypal:clientId"], _config["paypal:secretId"]);
            _clientPayPal = new PayPalHttpClient(_environmentPayPal);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExecutePayment(string paymentId, string token, string PayerID)
        {
            PaymentExecuteRequest request = new PaymentExecuteRequest(paymentId);
            request.RequestBody(new PaymentExecution()
            {
                PayerId = PayerID
            });

            try
            {
                HttpResponse response = await _clientPayPal.Execute(request);
                HttpStatusCode statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();
                if (result.State == "approved")
                {
                    return RedirectToAction(nameof(Success));
                }
                else
                {
                    return RedirectToAction(nameof(Error));
                }
            }
            catch (HttpException httpException)
            {
                HttpStatusCode statusCode = httpException.StatusCode;
                string debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                return RedirectToAction(nameof(Error));
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> PaymentWithPayPal()
        {
            Payment payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<Transaction>()
                    {
                        new Transaction()
                        {
                            Amount = new Amount()
                            {
                                Total = "0.2",
                                Currency = "USD"
                            }
                        }
                    },
                RedirectUrls = new RedirectUrls()
                {
                    ReturnUrl = "https://localhost:44339/Home/ExecutePayment",
                    CancelUrl = "https://localhost:44339/Home/Cancel"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                },
                NoteToPayer = "Please pay or fuck off"
            };

            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                HttpResponse response = await _clientPayPal.Execute(request);
                HttpStatusCode statusCode = response.StatusCode;
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
                    return RedirectToAction(nameof(Error));
                }
                else
                {
                    return Redirect(redirectUrl);
                }
            }
            catch (HttpException ex)
            {
                HttpStatusCode statusCode = ex.StatusCode;
                string debugId = ex.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();
                return RedirectToAction(nameof(Error));
            }
        }

        public IActionResult Cancel()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
