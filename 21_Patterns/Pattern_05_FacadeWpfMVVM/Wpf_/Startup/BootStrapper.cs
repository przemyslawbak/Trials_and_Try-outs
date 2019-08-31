using Autofac;
using Wpf_.ViewModels;
using Wpf_.Views;
using Wpf_Services.Main;

namespace Wpf_.Startup
{
    public class BootStrapper
    {
        /// <summary>
        /// IoC container
        /// </summary>
        /// <returns></returns>
        public static IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainService>()
              .As<IMainService>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
