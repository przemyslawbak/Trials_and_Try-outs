using Microsoft.Extensions.Logging;

namespace WebApp.Services
{
    public class ScopedSimpleTextService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;

        public ScopedSimpleTextService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            await Task.Delay(100, stoppingToken);

            executionCount++;

            _logger.LogInformation(
                "Scoped Processing Service is working. Count: {Count}", executionCount);

        }
    }
}
