using System;
using System.Windows.Input;

namespace Financial.Commands
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public DelegateCommand(
          Action<object> execute,
          Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged; //ICommand

        public bool CanExecute(object parameter) //ICommand
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter) //ICommand
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged() // dodatkowa metoda
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
