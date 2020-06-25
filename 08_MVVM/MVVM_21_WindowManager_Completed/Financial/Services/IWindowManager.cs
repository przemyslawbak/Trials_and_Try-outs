using System.Threading.Tasks;
using System.Windows;
using Financial.ViewModels;

namespace Financial.Services
{
    public interface IWindowManager
    {
        void OpenWindow<T>() where T : Window;
        void CloseWindow(object viewModel);
        Task<bool?> OpenModalDialogWindow<T>() where T : Window;
        Task<object> OpenResultWindow<T>() where T : Window;
        string OpenFileDialogWindow(string messageBoxTitle);
        bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle);
    }
}