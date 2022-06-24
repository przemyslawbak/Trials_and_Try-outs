namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;
    using Microsoft.Extensions.Logging;
    using GettingStarted.Contracts;

    public class GettingStartedConsumer : IConsumer<GettingStartedConract>
    {
        readonly ILogger<GettingStartedConsumer> _logger;

        public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GettingStartedConract> context)
        {
            _logger.LogInformation("Received Text: {Text}", context.Message.Value);
            await context.Publish<ContractStarted>(new
            {
                context.Message.Value
            });

        }
    }
}