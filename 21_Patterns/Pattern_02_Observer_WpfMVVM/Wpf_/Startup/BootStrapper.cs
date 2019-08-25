using Autofac;
using Wpf_.ViewModels;
using Wpf_.Views;
using Wpf_Services.Controls;
using Wpf_Services.Observer;

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

            builder.RegisterType<ControlsService>()
              .As<IControlsService>().SingleInstance();

            builder.RegisterType<ObserverService>()
              .As<IObserverService>().WithParameter(new NamedParameter("name", ""));

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
