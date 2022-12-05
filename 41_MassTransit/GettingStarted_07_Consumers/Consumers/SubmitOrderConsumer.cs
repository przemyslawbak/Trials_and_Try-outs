using MassTransit;
using System.Threading.Tasks;

namespace GettingStarted.Consumers
{
    //An example class that consumes the SubmitOrder message type is shown below.
    //SubmitOrder - is a Commands
    //OrderSubmitted - is an Event
    class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            await context.Publish<OrderSubmitted>(new
            {
                context.Message.OrderId
            });
        }
    }
}
