using Autofac;
using Financial.Events;
using Financial.ViewModels;
using Financial.Views;

namespace Financial
{
    public class Startup
    {
        /// <summary>
        /// IoC container
        /// </summary>
        /// <returns></returns>
        public static IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>()
              .As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MainView>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<SecondView>().AsSelf();
            builder.RegisterType<SecondViewModel>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
