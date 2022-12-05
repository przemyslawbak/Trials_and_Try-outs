using Autofac;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Startup;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using System.Windows;
/// <summary>
/// Korzysta z Autofac przy odpaleniu aplikacji
/// </summary>
namespace FriendStorage.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootStrapper = new BootStrapper();
            var container = bootStrapper.BootStrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
