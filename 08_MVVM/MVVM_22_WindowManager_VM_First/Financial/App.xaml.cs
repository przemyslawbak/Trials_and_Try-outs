using Autofac;
using Financial.Services;
using Financial.ViewModels;
using System.Windows;

namespace Financial
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IContainer _container;
        private readonly IWindowManager _winService;

        public App()
        {
            _container = Financial.Startup.BootStrap();
            _winService = new WindowManager();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _winService.OpenWindow(_container.Resolve<MainViewModel>());
        }
    }
}
