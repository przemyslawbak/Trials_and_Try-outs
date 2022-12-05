﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_DialogService.Utilities
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public DelegateCommand(
          Action<object> execute,
          Func<object, bool> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
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
