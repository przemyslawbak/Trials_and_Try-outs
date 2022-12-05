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

        //1) https://github.com/paypal/PayPal-NET-SDK/wiki/Make-Your-First-Call
        [AllowAnonymous]
        public IActionResult Index()
        {
            var config = ConfigManager.Instance.GetProperties(); //paypal config
            var accessToken = new OAuthTokenCredential(config).GetAccessToken(); //OAuth access token
            var apiContext = new APIContext(accessToken);
            // Initialize the apiContext's configuration with the default configuration for this application.
            apiContext.Config = ConfigManager.Instance.GetProperties();

            // Define any custom configuration settings for calls that will use this object.
            apiContext.Config["connectionTimeout"] = "1000"; // Quick timeout for testing purposes

            // Define any HTTP headers to be used in HTTP requests made with this APIContext object
            if (apiContext.HTTPHeaders == null)
            {
                apiContext.HTTPHeaders = new Dictionary<string, string>();
            }
            apiContext.HTTPHeaders["some-header-name"] = "some-value";
            var payment = Payment.Get(apiContext, "PAY-0XL713371A312273YKE2GCNI");
            return View();
        }
    }
}
