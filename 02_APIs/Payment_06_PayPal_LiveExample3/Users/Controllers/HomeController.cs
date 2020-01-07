using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Users.Controllers
{
    //https://medium.com/@pmareke/using-paypal-sdk-with-net-core-full-explanation-66aab76cef66
    //core 2.2 not supported for this approach
    public class HomeController : Controller
    {
        string _accessToken;
        public HomeController()
        {
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Pay()
        {
            try
            {
                Payment payment = await CreatePayment();
                return View("ExecutePayment");
            }
            catch (Exception e)
            {
                //
                return View("Error");
            }
        }

        public async Task<Payment> CreatePayment()
        {
            Payment createdPayment = null;

            var _config = ConfigManager.Instance.GetProperties();
            _accessToken = new OAuthTokenCredential(_config).GetAccessToken();

            var apiContext = new APIContext(_accessToken)
            {
                Config = ConfigManager.Instance.GetProperties()
            };

            try
            {
                Payment payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            amount = new Amount { currency = "EUR", total = "0.1" }, description = "Test product"
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        cancel_url = "https://localhost:44339//Home/Cancel",
                        return_url = "https://localhost:44339//Home/ExecutePayment"
                    }
                };

                createdPayment = await Task.Run(() => payment.Create(apiContext));
            }
            catch (Exception e)
            {
                //
            }

            return createdPayment;
        }

        public async Task<IActionResult> ExecutePayment (string payerID, string paymentId)
        {
            var apiContext = new APIContext(_accessToken)
            {
                Config = ConfigManager.Instance.GetProperties()
            };

            PaymentExecution paymentExecution = new PaymentExecution() { payer_id = payerID };

            Payment payment = new Payment() { id = paymentId };

            Payment executedPayment = await Task.Run(() => payment.Execute(apiContext, paymentExecution));

            return View();
        }
    }
}




//- nie działa - NA POCZĄTEK: install i test SDK core https://github.com/paypal/PayPal-NET-SDK/tree/2.0-beta
//- payment config problem - przyklad: https://www.c-sharpcorner.com/article/paypal-payment-gateway-integration-in-asp-net-mvc/
//inny przykład: https://www.codeproject.com/Articles/870870/Using-Paypal-Rest-API-with-ASP-NET-MVC
//tutorial: https://www.ittutorialswithexample.com/2018/03/paypal-payment-gateway-integration-in-aspnet.html
//sandbox test: https://stackoverflow.com/questions/51124822/sandbox-test-second-payment-failed-paypal-integration-asp-net-mvc
//https://medium.com/@pmareke/using-paypal-sdk-with-net-core-full-explanation-66aab76cef66
//https://stackoverflow.com/questions/50588921/asp-net-core-paypal-implementation