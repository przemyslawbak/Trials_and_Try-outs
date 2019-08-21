using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_.ViewModels;
using Wpf_.Views;
using Wpf_Services.Controls;
using Wpf_Services.Dialogs;

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

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<DialogWindow>().AsSelf();
            builder.RegisterType<DialogViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<DialogContainer>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
