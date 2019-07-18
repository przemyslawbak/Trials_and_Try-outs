using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Dialogs.DialogFacade;
using WpfApplication1.Dialogs.DialogService;
using WpfApplication1.ViewModels;
using WpfApplication1.Views;

namespace WpfApplication1.Startup
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DialogFacade>()
              .As<IDialogFacade>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<DialogYesNoView>().AsSelf();
            builder.RegisterType<DialogWindow>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<DialogYesNoViewModel>().AsSelf();

            return builder.Build();
        }
    }
}
