using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using Company.Consumers;

namespace GettingStarted
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        // Add a single consumer
                        x.AddConsumer<GettingStartedConsumer>(typeof(GettingStartedConsumerDefinition));

                        // Add a single consumer by type
                        x.AddConsumer(typeof(GettingStartedConsumer), typeof(GettingStartedConsumerDefinition))
                        .Endpoint(e => //and several other ways
                        {
                            // override the default endpoint name
                            //e.Name = "order-service-extreme";

                            // specify the endpoint as temporary (may be non-durable, auto-delete, etc.)
                            e.Temporary = false;

                            // specify an optional concurrent message limit for the consumer
                            e.ConcurrentMessageLimit = 8;

                            // only use if needed, a sensible default is provided, and a reasonable
                            // value is automatically calculated based upon ConcurrentMessageLimit if 
                            // the transport supports it.
                            e.PrefetchCount = 16;

                            // set if each service instance should have its own endpoint for the consumer
                            // so that messages fan out to each instance.
                            e.InstanceId = "something-unique";
                        }); ;

                        // Add all consumers in the specified assembly
                        x.AddConsumers(typeof(GettingStartedConsumer).Assembly);

                        // Add all consumers in the namespace containing the specified type
                        x.AddConsumersFromNamespaceContaining<GettingStartedConsumer>();

                        //All of the included formatters trim the Consumer, Saga, or Activity suffix from the end of the class name.
                        //If the consumer name is generic, the generic parameter type is used instead of the generic type.
                        x.SetKebabCaseEndpointNameFormatter();
                        x.SetSnakeCaseEndpointNameFormatter();

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.ReceiveEndpoint("order-service", e =>
                            {
                                //To explicitly configure endpoints, use the ConfigureConsumer and/or ConfigureConsumers methods.
                                e.ConfigureConsumer<GettingStartedConsumer>(context);
                            });
                        });
                    });

                    // OPTIONAL, but can be used to configure the bus options
                    services.AddOptions<MassTransitHostOptions>()
                        .Configure(options =>
                        {
                    // if specified, waits until the bus is started before
                    // returning from IHostedService.StartAsync
                    // default is false
                            options.WaitUntilStarted = true;

                    // if specified, limits the wait time when starting the bus
                            options.StartTimeout = TimeSpan.FromSeconds(10);

                    // if specified, limits the wait time when stopping the bus
                            options.StopTimeout = TimeSpan.FromSeconds(30);
                        });

                });
    }
}
