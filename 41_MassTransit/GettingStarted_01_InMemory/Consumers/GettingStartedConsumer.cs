namespace Company.Consumers
{
    using System.Threading.Tasks;
    using MassTransit;
    using Contracts;

    public class GettingStartedConsumer :
        IConsumer<GettingStarted>
    {
        public Task Consume(ConsumeContext<GettingStarted> context)
        {
            return Task.CompletedTask;
        }
    }
}