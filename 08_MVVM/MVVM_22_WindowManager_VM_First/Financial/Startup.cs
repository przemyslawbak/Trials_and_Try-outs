using Autofac;
using Financial.Events;
using Financial.Services;
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

            builder.RegisterType<WindowManager>()
              .As<IWindowManager>().SingleInstance();

            builder.RegisterType<MainView>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<SecondView>().AsSelf();
            builder.RegisterType<SecondViewModel>().AsSelf();

            builder.RegisterType<DialogView>().AsSelf();
            builder.RegisterType<DialogViewModel>().AsSelf();

            return builder.Build();
        }
    }
}
