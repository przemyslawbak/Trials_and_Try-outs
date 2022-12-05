using BasicConfig.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Specialized;
using System.IO;

namespace BasicConfig.Infrastructure
{
    public class LogUserActionsFilter : ActionFilterAttribute
    {
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _context;
        protected DateTime start_time;

        public LogUserActionsFilter(IHttpContextAccessor accessor, ApplicationDbContext ctx)
        {
            _accessor = accessor;
            _context = ctx;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            start_time = DateTime.UtcNow;
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            RouteData route_data = filterContext.RouteData;
            int duration = (DateTime.UtcNow - start_time).Milliseconds;
            string controller = (string)route_data.Values["controller"];
            string action = (string)route_data.Values["action"];
            string path = Path.Combine(controller, action);
            DateTime created_at = DateTime.UtcNow;
            //Save all your required values, including user id and whatnot here.
            //The duration variable will allow you to see expensive page loads on the controller, this can be useful when clients complain about something being slow.
            var user = filterContext.HttpContext.User;
            // Stores the Request in an Accessible object
            var request = filterContext.HttpContext.Request;
            // Generate an audit
            Audit audit = new Audit()
            {
                // Our Username (if available)
                UserName = (user.Identity.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Guest",
                // The IP Address of the Request
                IpAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                // The URL that was accessed
                AreaAccessed = path,
                // Creates our Timestamp
                Timestamp = DateTime.UtcNow,
                ExecTime = duration,
                UserAgent = request.Headers["User-Agent"].ToString()
            };

            _context.AuditRecords.Add(audit);
            _context.SaveChanges();
        }
    }
}
