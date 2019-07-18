using Prism.Mvvm;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM_10_.ViewModels
{
    public class ViewModelBase : BindableBase, INotifyPropertyChanged //INotify
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}