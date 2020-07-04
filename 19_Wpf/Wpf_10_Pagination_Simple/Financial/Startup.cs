using Autofac;
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

            builder.RegisterType<MainView>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
