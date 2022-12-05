using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options =>
                options.ValidateScopes = false)
                .Build();
        }
    }
}
