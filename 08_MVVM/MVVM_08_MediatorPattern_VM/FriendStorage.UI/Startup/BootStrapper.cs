using Autofac;
using FriendStorage.UI.Services;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;

namespace FriendStorage.UI.Startup
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SomeService>()
              .As<ISomeService>().SingleInstance();

            builder.RegisterType<PersonService>()
              .As<IPersonService>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<View1>().AsSelf();
            builder.RegisterType<View2>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<View1ViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<View2ViewModel>().AsSelf().SingleInstance();

            return builder.Build();
        }
    }
}
