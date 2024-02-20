using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Services;
using System.Configuration;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Program started - WebAPI.Historic_Parser");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables().Build();

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IApp, App>();
                    services.AddSingleton(config);
                    var connectionString = config.GetSection("ConnectionString").Value;

                    services.AddTransient<IServiceClass, ServiceClass>();
                    services.Configure<TransientFaultHandlingOptions>(config.GetSection("TransientFaultHandlingOptions"));
                })
                .Build();

            var app = host.Services.GetRequiredService<IApp>();
            await app.Start();

            Console.WriteLine("Program finished, press any key...");
            Console.ReadKey();
        }

    }
}



