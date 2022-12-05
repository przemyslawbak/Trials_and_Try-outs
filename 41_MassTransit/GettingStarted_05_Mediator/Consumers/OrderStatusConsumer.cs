using MassTransit;
using System;
using System.Threading.Tasks;

namespace GettingStarted.Consumers
{
    public interface GetOrderStatus
    {
        Guid OrderId { get; }
    }

    public interface OrderStatus
    {
        Guid OrderId { get; }
        string Status { get; }
    }

    class OrderStatusConsumer : IConsumer<GetOrderStatus>
    {
        public async Task Consume(ConsumeContext<GetOrderStatus> context)
        {
            await context.RespondAsync<OrderStatus>(new
            {
                context.Message.OrderId,
                Status = "Pending"
            });
        }
    }
}
