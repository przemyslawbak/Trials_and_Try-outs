using Autofac;
using Wpf_.ViewModels;
using Wpf_.Views;
using Wpf_Services.Building;
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

            builder.RegisterType<MediumCarBuilder>()
              .As<IMediumCarBuilder>().SingleInstance();

            builder.RegisterType<NormalCarBuilder>()
              .As<INormalCarBuilder>().SingleInstance();

            builder.RegisterType<LuxaryCarBuilder>()
              .As<ILuxaryCarBuilder>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
