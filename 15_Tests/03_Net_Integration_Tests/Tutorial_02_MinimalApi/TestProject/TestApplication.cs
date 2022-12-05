using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample;

namespace TestProject
{
    // internal is important as it's the
    // same access level as `Program`
    internal class TestApplication : WebApplicationFactory<Program>
    {
        public string Message { get; set; }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(s => {
                s.AddScoped<IMessageService, TestMessageService>(
                    _ => new TestMessageService
                    {
                        Message = Message
                    });
            });

            return base.CreateHost(builder);
        }
    }

    public class TestMessageService : IMessageService
    {
        /// <summary>
        /// Allow us to set the message
        /// </summary>
        public string Message { get; set; } = "Hello, World!";

        public string SayHello() => Message;
    }
}
