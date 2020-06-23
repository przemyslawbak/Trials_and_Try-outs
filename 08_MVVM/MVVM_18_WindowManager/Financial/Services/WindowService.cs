using System.Windows;

namespace Financial.Services
{
    public class WindowService : IWindowService
    {
        public void ShowWindow(object viewModel)
        {
            var win = new Window();
            win.DataContext = viewModel;
            win.Show();
        }
    }
}
