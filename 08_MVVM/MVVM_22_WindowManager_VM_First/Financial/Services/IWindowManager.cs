using System;
using System.Threading.Tasks;
using Financial.Models;
using Financial.ViewModels;

namespace Financial.Services
{
    public interface IWindowManager
    {
        bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle);
        string OpenFileDialogWindow(string messageBoxTitle);
        void OpenWindow(object vm);
        Task<bool?> OpenModalDialogWindow(object vm);
        void CloseWindow(object viewModel);
        Task<object> OpenResultWindow(object viewModel);
    }
}