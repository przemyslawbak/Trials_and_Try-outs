using Autofac;
using System.Windows;

namespace Financial.Services
{
    //https://stackoverflow.com/a/36642835
    public class WindowService: IWindowService
    {
        private IContainer _container;

        public void ShowWindow<T>() where T : Window
        {
            _container = Startup.BootStrap();

            _container.Resolve<T>().Show();
        }
    }
}
