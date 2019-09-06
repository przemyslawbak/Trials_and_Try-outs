using Autofac;
using Wpf_Services.Controls;
using Wpf_Services.Files;

namespace Wpf_Services.Main
{
    public class Booter
    {
        public static IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FilesService>()
              .As<IFilesService>().SingleInstance();

            builder.RegisterType<ControlsService>()
              .As<IControlsService>().SingleInstance();

            builder.RegisterType<Facade>()
              .AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
