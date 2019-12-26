using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Users.Models;

namespace Users.Controllers
{
    //https://edi.wang/post/2017/10/16/get-client-ip-aspnet-20
    public class HomeController : Controller
    {
        private IHttpContextAccessor _accessor;

        public HomeController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public ViewResult Index()
        {
            ClientModel client = new ClientModel();

            client.IP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            client.UserAgent = Request.Headers["User-Agent"].ToString();

            return View(client);
        }
    }
}
