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
            _host = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                /*services.Configure<AppSettings>(configuration
                    .GetSection(nameof(AppSettings)));*/
                services.AddScoped<ISampleService, SampleService>();

                // Register all ViewModels.
                services.AddSingleton<MainViewModel>();

                // Register all the Windows of the applications.
                services.AddTransient<MainWindow>();

            }).Build(); ;
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
