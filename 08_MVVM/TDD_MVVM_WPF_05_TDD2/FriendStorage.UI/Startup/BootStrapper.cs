using Autofac;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Rejestruje widoki i modele widoków
/// </summary>
namespace FriendStorage.UI.Startup
{
    public class BootStrapper
    {
        public IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>() //dla Prism
              .As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<FriendEditViewModel>()
              .As<IFriendEditViewModel>();

            builder.RegisterType<NavigationViewModel>()
              .As<INavigationViewModel>();

            builder.RegisterType<FriendDataProvider>()
              .As<IFriendDataProvider>();

            builder.RegisterType<NavigationDataProvider>()
              .As<INavigationDataProvider>();

            builder.RegisterType<FileDataService>()
              .As<IDataService>();

            return builder.Build();
        }
    }
}
