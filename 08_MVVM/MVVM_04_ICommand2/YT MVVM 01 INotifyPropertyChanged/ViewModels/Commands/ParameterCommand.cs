using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace YT_MVVM_01_INotifyPropertyChanged.ViewModels.Commands
{
    public class ParameterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (parameter != null)
            {
                var s = parameter as String;
                s = s.Trim();
                if (string.IsNullOrEmpty(s))
                    return false; //do not execute
                return true; //execute
            }
            return false; //do not execute
        }

        public void Execute(object parameter)
        {
            this.ViewModel.ParameterMethod(parameter as String);
        }
        public ViewModelBase ViewModel { get; set; }
        public ParameterCommand(ViewModelBase viewModel)
        {
            this.ViewModel = viewModel;
        }
    }
}
