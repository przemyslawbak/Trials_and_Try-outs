using BasicConfig.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace BasicConfig.Infrastructure
{
    //później używa się jako: [ServiceFilter(typeof(VerifyAdminActionFilter))]
    public class VerifyAdminActionFilter : ActionFilterAttribute
    {
        private UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public VerifyAdminActionFilter(UserManager<AppUser> userMgr, IConfiguration configuration)
        {
            _userManager = userMgr;
            _configuration = configuration;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            bool isOk = false;

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.GetUserAsync(filterContext.HttpContext.User);

                if (user.Email == _configuration["WebsiteSecret:Fckgwrhqq2"])
                {
                    isOk = true;
                }
                else
                {
                    isOk = false;
                }
            }

            if (isOk)
                await next();
            else
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "SomethingWrong" }));
        }
    }
}
