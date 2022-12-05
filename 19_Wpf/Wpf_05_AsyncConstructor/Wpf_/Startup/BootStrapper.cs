using Autofac;
using Params_Logger;
using Wpf_.ViewModels;
using Wpf_.Views;
using Wpf_Services.Controls;

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

            builder.RegisterType<ParamsLogger>()
              .As<IParamsLogger>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
