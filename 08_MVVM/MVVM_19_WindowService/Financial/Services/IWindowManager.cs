using System.Windows;
using Financial.ViewModels;

namespace Financial.Services
{
    public interface IWindowManager
    {
        void OpenWindow<T>() where T : Window;
        void CloseWindow(object viewModel);
    }
}