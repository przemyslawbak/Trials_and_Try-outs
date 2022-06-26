using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using GettingStarted.Consumers;

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
                        x.AddConsumer<SubmitOrderConsumer>(typeof(SubmitOrderConsumerDefinition))
                    .Endpoint(e =>
                    {
                        // override the default endpoint name
                        e.Name = "order-service-extreme";

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
                    });

                        x.SetKebabCaseEndpointNameFormatter();

                        //In the example, the bus is configured by the UsingRabbitMq method, which is passed two arguments
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h => {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            //ConfigureEndpoints uses an IEndpointNameFormatter to generate endpoint names, which by default uses a PascalCase formatter
                            //For the SubmitOrderConsumer, the endpoint names would be
                            //Default	SubmitOrder
                            //Snake Case	submit_order
                            //Kebab Case	submit-order

                            //All of the included formatters trim the Consumer, Saga, or Activity suffix from the end of the class name
                            cfg.ConfigureEndpoints(context);
                            //optional:
                            //cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);

                        });
                    });

                    services.AddHostedService<Worker>();

                });
    }
}
