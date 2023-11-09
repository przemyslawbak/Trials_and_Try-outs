using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Services;
using Sample.ViewModels;
using System;
using System.Windows;

namespace Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables().Build();

            _host = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(config);

                services.AddScoped<ISampleService, SampleService>();

                services.AddSingleton<MainViewModel>();

                services.AddTransient<MainWindow>();

                var urlBase = config.GetSection("UrlBase").Value;
                services.AddHttpClient("api_test", httpClient =>
                {
                    httpClient.BaseAddress = new Uri(urlBase);
                });

            }).Build();


            ServiceProvider = _host.Services;
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();

            var window = ServiceProvider.GetRequiredService<MainWindow>();

            window.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
