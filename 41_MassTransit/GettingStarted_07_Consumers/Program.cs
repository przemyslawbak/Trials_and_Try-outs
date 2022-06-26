using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using GettingStarted.Consumers;
using System;

namespace GettingStarted
{
    public class Program
    {
        /*
         * Consumers:
         * In MassTransit, a consumer consumes one or more message types.
         * MassTransit includes many consumer types, including consumers, sagas, saga state machines, routing slip activities,
         * handlers, and job consumers.
         * The interface has one method, Consume.
         */
        public static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.UsingInMemory((cxt, cfg) =>
                        {
                            cfg.ReceiveEndpoint("order-service", e =>
                            {
                                /*
                                 * Handler:
                                 * While creating a consumer is the preferred way to consume messages,
                                 * it is also possible to create a simple message handler. By specifying a method, anonymous method,
                                 * or lambda method, a message can be consumed on a receive endpoint.
                                 * The asynchronous handler method is called for each message delivered to the receive endpoint.
                                 */
                                e.Handler<SubmitOrder>(async context =>
                                {
                                    await Console.Out.WriteLineAsync($"Submit Order Received: {context.Message.OrderId}");
                                });
                            });
                        });
                    });
                })
                .Build()
                .RunAsync();
        }
    }
}
