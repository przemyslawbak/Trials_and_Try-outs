using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Startup;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using System.Windows;
using Autofac;

namespace FriendStorage.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootStrapper = new BootStrapper(); //zmienna dla interfejsów Autofac
            var container = bootStrapper.BootStrap(); //zmienna dla odpalenia kontenera Autofac
            var mainWindow = container.Resolve<MainWindow>(); //zmienna dla głównego okna
            mainWindow.Show(); //otwarcie głównego okna
        }
    }
}
