using System.Windows;

namespace Financial.Services
{
    public interface IWindowService
    {
        void ShowWindow<T>() where T : Window;
    }
}