using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BasicConfig
{
    public class TimeHostedTrigger : IHostedService, IDisposable
    {
        private Timer _timerUpdates;
        private readonly IMemoryCache _cache;

        public TimeHostedTrigger(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timerUpdates = new Timer(InitReset, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void InitReset(object state)
        {
            _cache.Remove(CacheKeys.UserClicks);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timerUpdates?.Dispose();
        }
    }
}
