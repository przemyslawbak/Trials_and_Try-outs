using Autofac;
using MvvmDialogs;
using Wpf_.ViewModels;
using Wpf_.Views;

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

            builder.RegisterType<DialogService>()
              .As<IDialogService>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<AddTextDialog>().AsSelf();
            builder.RegisterType<AddTextDialogViewModel>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
