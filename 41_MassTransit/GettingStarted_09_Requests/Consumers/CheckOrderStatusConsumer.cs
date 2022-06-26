using GettingStarted.Contracts;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace GettingStarted.Consumers
{
    public class CheckOrderStatusConsumer : IConsumer<CheckOrderStatus>
    {
        readonly IOrderRepository _orderRepository;

        public CheckOrderStatusConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<CheckOrderStatus> context)
        {
            var order = await _orderRepository.Get(context.Message.OrderId);
            if (order == null)
                throw new InvalidOperationException("Order not found");

            await context.RespondAsync<OrderStatusResult>(new
            {
                OrderId = order.Id,
                order.Timestamp,
                order.StatusCode,
                order.StatusText
            });
        }
    }
}
