using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace React_01_Podrecznik.Controllers
{
    public class HomeController : Controller
    {
        IHttpContextAccessor _httpContextAccessor;
        public HomeController(IHttpContextAccessor httpCOntext)
        {
            _httpContextAccessor = httpCOntext;
        }

        [HttpGet]
        public IActionResult Index(string value = "no_value_yet")
        {
            string model = value;
            return View("Index", model);
        }

        [Route("{key}/{value}/{expireTime}")]
        public IActionResult AddCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult FindCookie()
        {
            string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies["xxx"];

            return RedirectToAction(nameof(Index), new { value = cookieValueFromContext });
        }
    }
}
