using Autofac;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Dialogs;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendStorage.UI.Startup
{
    /// <summary>
    /// rejestrowanie interfejsów przez Autofac
    /// </summary>
  public class BootStrapper
  {
    public IContainer BootStrap()
    {
      var builder = new ContainerBuilder(); //klasa Autofac

      builder.RegisterType<EventAggregator>()
        .As<IEventAggregator>().SingleInstance();

      builder.RegisterType<MessageDialogService>()
        .As<IMessageDialogService>();

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
