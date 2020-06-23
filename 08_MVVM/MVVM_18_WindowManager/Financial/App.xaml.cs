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
        private readonly IWindowService _win;

        public App(IWindowService win)
        {
            _win = win;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _win.ShowWindow(new ViewModelLocator().SecondViewModel);
        }
    }
}
