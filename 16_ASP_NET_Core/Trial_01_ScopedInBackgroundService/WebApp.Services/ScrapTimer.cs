using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApp.Services
{
    //source: https://cezarywalenciuk.pl/blog/programing/backgroundservice-w-aspnet-core-i-cykl-zycia-scoped
    public class ScrapTimer : BackgroundService
    {
        public ScrapTimer(IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);

                //execute every 1min
                if (DateTime.UtcNow.Second == 0)
                {
                    await DoWork(stoppingToken);
                }
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingServices =
                    scope.ServiceProvider
                        .GetServices<IScopedProcessingService>();

                for (int i = 0; i < 10; i++)
                {
                    foreach (var scopedProcessingService in scopedProcessingServices)
                    {
                        await scopedProcessingService.DoWork(stoppingToken);
                    }
                    await Task.Delay(1000);
                }

            }
        }
    }
}
