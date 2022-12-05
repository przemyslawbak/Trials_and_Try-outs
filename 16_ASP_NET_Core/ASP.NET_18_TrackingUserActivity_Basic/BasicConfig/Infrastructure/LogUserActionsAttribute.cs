using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace BasicConfig.Infrastructure
{
    public class LogUserActionsAttribute : ActionFilterAttribute
    {
        protected DateTime start_time;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            start_time = DateTime.Now;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            RouteData route_data = filterContext.RouteData;
            TimeSpan duration = (DateTime.Now - start_time);
            string controller = (string)route_data.Values["controller"];
            string action = (string)route_data.Values["action"];
            DateTime created_at = DateTime.Now;
            //Save all your required values, including user id and whatnot here.
            //The duration variable will allow you to see expensive page loads on the controller, this can be useful when clients complain about something being slow.
        }
    }
}
