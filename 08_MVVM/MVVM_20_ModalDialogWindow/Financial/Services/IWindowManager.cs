using System.Threading.Tasks;
using System.Windows;
using Financial.ViewModels;

namespace Financial.Services
{
    public interface IWindowManager
    {
        void OpenWindow<T>() where T : Window;
        void CloseWindow(object viewModel);
        Task<bool?> OpenDialogWindow<T>() where T : Window;
    }
}