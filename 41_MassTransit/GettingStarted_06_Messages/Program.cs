using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System;

namespace GettingStarted
{
    public class Program
    {
        /*
         * Commands:
         * A command tells a service to do something.
         * Commands are sent (using Send) to an endpoint, as it is expected that a single service instance performs the command action.
         * A command should never be published.
         * Example: UpdateCustomerAddress, UpgradeCustomerAccount, SubmitOrder
         */

        /*
         * Events:
         * An event signifies that something has happened.
         * Events are published (using Publish) via either IBus (standalone) or ConsumeContext (within a message consumer).
         * An event should not be sent directly to an endpoint.
         * Example Events: CustomerAddressUpdated, CustomerAccountUpgraded, OrderSubmitted
         */

        /*
         * Message Headers:
         * MassTransit encapsulates every sent or published message in a message envelope (described by the Envelope Wrapper (opens new window)pattern). 
         * The envelope adds a series of message headers
         */

        /*
         * Correlation:
         * Messages are usually part of a conversation and identifiers are used to connect messages to that conversation.
         */

        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();

            await busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
            try
            {
                var endpoint = await busControl.GetSendEndpoint(new Uri("queue:order-service"));

                // Set CorrelationId using SendContext<T>
                await endpoint.Send<SubmitOrder>(new { OrderId = InVar.Id }, context =>
                    context.CorrelationId = context.Message.OrderId);

                // Set CorrelationId using initializer header
                await endpoint.Send<SubmitOrder>(new
                {
                    OrderId = InVar.Id,
                    __CorrelationId = InVar.Id

                    // InVar.Id returns the same value within the message initializer
                });
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
